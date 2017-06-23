using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

namespace HoloToolkit.Unity
{
    public class minimapSpawn : Singleton<minimapSpawn>
    {
        //List containing all the mini spatial meshes
        public List<GameObject> miniMapMeshes;
        //We'll instantiate a null game object into this, and use it as the parent for all the spatial meshes
        public GameObject miniMapHolder { get; set; }
        [Tooltip("Object to parent the spawned miniMap holder under.  Located within the Minimap Container")]
        public GameObject MiniMapHolderParent;

        [Tooltip("How much to scale the minimap.  Recommended .075")]
        public float scaleOffset;


        [Tooltip("Boiler's uppermost parent goes here.  Will use this to instantiate a miniature boiler")]
        public GameObject boiler;
        public Vector3 boilerPivot { get; set; }

        [Tooltip("Displayed Avatar.  Located within Minimap container")]
        public GameObject avatar;
        [Tooltip("Whether or not to use the avatar")]
        public bool useAvatar;

        [Tooltip("This is for changing the real sized spatial mesh to an invisible occlusion material")]
        public Material occlusionMat;
        [Tooltip("Material which the miniature meshes will get set to")]
        public Material miniMapMat;


        //Spawn and set up the parent of the minimap meshes
        void Start()
        {
            miniMapHolder = new GameObject();
            miniMapHolder.name = "MiniMapHolder";
            GetComponent<miniMapToggle>().MiniMapHolder = miniMapHolder;


        }

        //Set the node holder so all node position values are relative to the boiler.  This is useful for storing the pos values in JSON
        void repositionNodeHolder()
        {
            Transform nodeContainer = mediaManager.Instance.gameObject.transform;
            Transform initParent = nodeContainer.parent;
            nodeContainer.SetParent(boiler.transform);
            nodeContainer.localPosition = Vector3.zero;
            nodeContainer.localRotation = new Quaternion(0, 0, 0, 0);
            nodeContainer.localScale = Vector3.one;
            nodeContainer.SetParent(initParent);
        }


        //This spawns and sets up the minimap
        public void spawnMiniMap()
        {
            //Parent the boiler to Spatial Mapping.  It now lives in the same heirarchy as the real sized spatial meshes
            boiler.transform.SetParent(transform);

            repositionNodeHolder();

            //Go through each child
            for (int i = 0; i < transform.childCount; i++)
            {

                //spawn copies of each real sized mesh
                miniMapMeshes.Add((GameObject)Instantiate(transform.GetChild(i).gameObject, transform.GetChild(i).position, transform.GetChild(i).localRotation));
                miniMapMeshes[i].transform.SetParent(miniMapHolder.transform);

                //If we find the boiler we need to change its tag and layer so it can be tumbled on
                if (miniMapMeshes[i].tag == "boilerPrefab")
                {
                    boilerPivot = miniMapMeshes[i].transform.position;
                    miniMapMeshes[i].tag = "miniMapMesh";
                    miniMapMeshes[i].layer = 0;
                    miniMapMeshes[i].GetComponent<Collider>().enabled = true;

                    //Some boiler components have children that we need to properly tag
                    foreach (Transform child in miniMapMeshes[i].GetComponentsInChildren<Transform>())
                    {
                        if(child.gameObject.name == "regularBoiler")
                        {
                            child.gameObject.GetComponent<Renderer>().material.renderQueue = 1000;
                        }
                        child.gameObject.tag = "miniMapMesh";
                        child.gameObject.layer = 0;
                    }
                        
                }

                //
                else
                {
                    transform.GetChild(i).gameObject.tag = "SpatialMapping";
                    miniMapMeshes[i].tag = "miniMapMesh";
                    miniMapMeshes[i].GetComponent<MeshRenderer>().enabled = false;
                    miniMapMeshes[i].layer = 2;
                }
                if (miniMapMeshes[i].GetComponent<Renderer>() != null)
                {
                    miniMapMeshes[i].GetComponent<Renderer>().material = miniMapMat;
                    
                }

                if (miniMapMeshes[i].GetComponent<MeshRenderer>() != null)
                {
                    transform.GetChild(i).GetComponent<Renderer>().material = occlusionMat;
                }

                if (miniMapMeshes[i].GetComponent<WorldAnchor>() != null)
                {
                    Destroy(miniMapMeshes[i].GetComponent<WorldAnchor>());
                }

                if (miniMapMeshes[i].GetComponent<MeshFilter>() != null && miniMapMeshes[i].GetComponent<MeshFilter>().sharedMesh != null)
                {
                    miniMapMeshes[i].GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

                }
            }

            MiniMapHolderParent.transform.localScale = MiniMapHolderParent.transform.localScale / scaleOffset;
            MiniMapHolderParent.transform.position = boilerPivot;
            miniMapHolder.transform.SetParent(MiniMapHolderParent.transform);
            MiniMapHolderParent.transform.localPosition = Vector3.zero;
            MiniMapHolderParent.transform.localScale = Vector3.one;

            //turn on and parent lock buttons
            MiniMapHolderParent.transform.GetChild(0).gameObject.SetActive(true);
            MiniMapHolderParent.transform.GetChild(0).SetParent(miniMapHolder.transform);
            GetComponent<miniMapToggle>().active = true;



            if (useAvatar)
            {
                avatar.GetComponent<minimize>().miniThis();




            }

            for (int u = 0; u < boiler.transform.childCount; u++)
            {
                if (boiler.transform.GetChild(u).gameObject.activeSelf && boiler.transform.GetChild(u).gameObject.GetComponent<MeshRenderer>() != null)
                {
                    boiler.transform.GetChild(u).gameObject.GetComponent<Renderer>().material = occlusionMat;

                }
            }
        }
        

    }
}