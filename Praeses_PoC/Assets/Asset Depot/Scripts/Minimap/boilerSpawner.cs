using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class boilerSpawner : Singleton<boilerSpawner>
    {

        public GameObject boiler;
        public Color darkColor;
        bool tapToPlaceBoiler;
        Vector3 initBoilerPos;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (tapToPlaceBoiler)
            {
                PlaceBoiler();
            }
        }


        public void PlaceBoiler()
        {

            if (tapToPlaceBoiler == false)
            {

                //disable the mesh colliders and turn it darker so its easier to see
                //doing an int loop because the popup boiler is made up multiple parts
                for (int i = 0; i < boiler.transform.childCount; i++)
                {
                    boiler.transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
                    boiler.transform.GetChild(i).GetComponent<Renderer>().material.color = darkColor;
                }

                //clear out the source manager
                sourceManager.Instance.sourcePressed = false;
                tapToPlaceBoiler = true;
            }
            
            if(tapToPlaceBoiler)
            {
                //get gazemanager pos and set the boiler position to be there
                Vector3 pos = GazeManager.Instance.HitPosition;
                boiler.transform.position = pos;
                if (sourceManager.Instance.sourcePressed)
                {
                    LockBoiler();
                }


            }


        }

        public void LockBoiler()
        {
            //turn off bool to stop placing 
            //reenable it
            tapToPlaceBoiler = false;
            for (int i = 0; i < boiler.transform.childCount; i++)
            {
                boiler.transform.GetChild(i).GetComponent<MeshCollider>().enabled = true;
                boiler.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.white;
            }

        }


    }
}
