using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class miniMapToggle : Singleton<miniMapToggle>
    {
        [Tooltip("From Minimap Container.  We use this for positioning")]
        public GameObject MiniMapTagAlong;

        //The direct parent of all the minimap meshes.  Defined from spawnMiniMap
        public GameObject MiniMapHolder { get; set; }

        //Minimap state
        public bool active { get; set; }

        [Tooltip ("Indicator of where the avatar is when not visible on the map.  We use it here to hide it when the map is toggled off")]
        public GameObject paperPlane;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        //This will place the minimap by the boiler.  Useful so users can easily find it their first time
        public void placeMapByBoiler()
        {
            MiniMapTagAlong.transform.position = GameObject.Find("minimapPlacement").transform.position;
        }


        //grabs all the children of minimap holder and switch their active state
        public void toggleMiniMap()
        {

            for (int i = 0; i < MiniMapHolder.transform.childCount; i++)
            {
                MiniMapHolder.transform.GetChild(i).gameObject.SetActive(!MiniMapHolder.transform.GetChild(i).gameObject.activeSelf);
            }
            paperPlane.SetActive(!paperPlane.activeSelf);
            active = MiniMapHolder.transform.GetChild(0).gameObject.activeSelf;


            MiniMapTagAlong.transform.position = frontHolderInstance.Instance.setFrontHolder(1.5f).transform.position;



        }
    }
}
