using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.IO;

namespace HoloToolkit.Unity
{

    public class boilerSpawner : Singleton<boilerSpawner>
    {

        public GameObject boilerParent;
        public Color darkColor;
        bool tapToPlaceBoiler;
        Vector3 initBoilerPos;
        public GameObject[] popUpBoiler;
        public GameObject normalBoiler;

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
                for (int i = 0; i < boilerParent.transform.childCount; i++)
                {
                    boilerParent.transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
                    boilerParent.transform.GetChild(i).GetComponent<Renderer>().material.color = darkColor;
                }

                //clear out the source manager
                sourceManager.Instance.sourcePressed = false;
                tapToPlaceBoiler = true;
            }
            
            if(tapToPlaceBoiler)
            {
                //get gazemanager pos and set the boiler position to be there
                Vector3 pos = GazeManager.Instance.HitPosition;
                boilerParent.transform.position = pos;
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
            for (int i = 0; i < boilerParent.transform.childCount; i++)
            {
                boilerParent.transform.GetChild(i).GetComponent<MeshCollider>().enabled = true;
                boilerParent.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.white;
            }

        }



        //switch the visible boiler and associated JSON
        public void switchBoiler()
        {
            if (popUpBoiler[0].activeSelf)
            {

                databaseMan.Instance.popUp = 0;
                normalBoiler.SetActive(true);
                for (int i = 0; i < popUpBoiler.Length; i++)
                {

                    popUpBoiler[i].SetActive(false);

                    if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, "JO_JJ_values.json")))
                    {
                        databaseMan.Instance.valuesDir = Path.Combine(Application.persistentDataPath, "JO_JJ_values.json");
                    }


                }
            }
            else if (normalBoiler.activeSelf)
            {
                normalBoiler.SetActive(false);
                for (int i = 0; i < popUpBoiler.Length; i++)
                {

                    databaseMan.Instance.popUp = 1;
                    popUpBoiler[i].SetActive(true);

                    if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, "JO_JJ_valuesPopUp.json")))
                    {
                        databaseMan.Instance.valuesDir = Path.Combine(Application.persistentDataPath, "JO_JJ_valuesPopUp.json");
                    }
                }
            }
        }


    }
}
