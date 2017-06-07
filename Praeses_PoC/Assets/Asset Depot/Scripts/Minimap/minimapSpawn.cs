using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

namespace HoloToolkit.Unity
{
    public class minimapSpawn : Singleton<minimapSpawn>
    {

        public GameObject miniMapHolder { get; set; }
        public List<GameObject> miniMapMeshes;
        public GameObject MiniMapHolderParent;
        public Material occlusionMat;
        public Material miniMapMat;
        public float scaleOffset;
        GameObject desk;
        GameObject boiler;
        int switchCounter;

        public Vector3 boilerPivot { get; set; }
        public GameObject avatar;
        public bool useAvatar;

        // Use this for initialization
        void Start()
        {
            miniMapHolder = new GameObject();
            miniMapHolder.name = "MiniMapHolder";
            GetComponent<miniMapToggle>().MiniMapHolder = miniMapHolder;


        }

        //this zeros out the node holder if we need to place anything in the minimap at a later time
        void repositionNodeHolder()
        {
            Transform initParent = mediaManager.Instance.gameObject.transform.parent;
            mediaManager.Instance.gameObject.transform.SetParent(boiler.transform);
            mediaManager.Instance.gameObject.transform.localPosition = Vector3.zero;
            mediaManager.Instance.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            mediaManager.Instance.gameObject.transform.localScale = Vector3.one;
            mediaManager.Instance.gameObject.transform.SetParent(initParent);
        }



        public void spawnMiniMap()
        {
            //puts the minimap holder at the transform position of the boiler as its pivot
            boiler = GameObject.Find("boiler");
            boiler.transform.SetParent(transform);
            repositionNodeHolder();

            for (int i = 0; i < transform.childCount; i++)
            {
                miniMapMeshes.Add((GameObject)Instantiate(transform.GetChild(i).gameObject, transform.GetChild(i).position, transform.GetChild(i).localRotation));
                miniMapMeshes[i].transform.SetParent(miniMapHolder.transform);
                if (miniMapMeshes[i].tag == "boilerPrefab")
                {
                    boilerPivot = miniMapMeshes[i].transform.position;
                    miniMapMeshes[i].tag = "miniMapMesh";
                    miniMapMeshes[i].layer = 0;
                    miniMapMeshes[i].GetComponent<Collider>().enabled = true;
                    foreach (Transform child in miniMapMeshes[i].GetComponentsInChildren<Transform>())
                    {
                        if(child.gameObject.name == "initialShadingGroup_boiler_model")
                        {
                            child.gameObject.GetComponent<Renderer>().material.renderQueue = 1000;
                        }
                        child.gameObject.tag = "miniMapMesh";
                        child.gameObject.layer = 0;
                    }
                        
                }
                else
                {
                    transform.GetChild(i).gameObject.tag = "SpatialMapping";
                    miniMapMeshes[i].tag = "miniMapMesh";
                    miniMapMeshes[i].GetComponent<MeshRenderer>().enabled = false;
                    miniMapMeshes[i].layer = 2;
                }

                //override the materials 
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

            //set the minimap holder back to its original position after we reset it
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
}