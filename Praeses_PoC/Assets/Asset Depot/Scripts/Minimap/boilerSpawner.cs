using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class boilerSpawner : Singleton<boilerSpawner>
    {

        public GameObject boiler;
        GameObject boilerClone;
        public GameObject desk;
        bool tapToPlaceBoiler;
        public GameObject SpatialMapping;
        public bool isObj;
        GameObject activeObj;

        public GameObject frontHolder;
        Vector3 initBoilerPos;

        // Use this for initialization
        void Start()
        {
            initBoilerPos = boiler.transform.GetChild(0).localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (tapToPlaceBoiler)
            {
                PlaceBoiler();
            }
        }

        public void SpawnBoiler()
        {
            isObj = false;
            Vector3 pos = GazeManager.Instance.HitPosition;
            boilerClone = Instantiate(boiler, pos, Quaternion.identity) as GameObject;
            activeObj = boilerClone;
            for (int i = 0; i < activeObj.transform.childCount; i++)
            {
                    activeObj.transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
            }
            tapToPlaceBoiler = true;
        }

        public void SpawnDesk()
        {
            isObj = true;
            Vector3 pos = GazeManager.Instance.HitPosition;

            GameObject deskClone = Instantiate(desk, pos, Quaternion.identity) as GameObject;
            activeObj = deskClone;
            for (int i = 0; i < activeObj.transform.childCount; i++)
            {
                activeObj.transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
            }
            tapToPlaceBoiler = true;
        }

        public void PlaceBoiler()
        {
            if (activeObj == null)
            {
                activeObj = boiler;
            }

            if (tapToPlaceBoiler == false)
            {
                activeObj.transform.GetChild(0).localPosition = initBoilerPos;
                activeObj.transform.GetChild(0).localRotation = new Quaternion(0, 0, 0, 0);
                for (int i = 0; i < activeObj.transform.childCount; i++)
                {
                    activeObj.transform.GetChild(i).GetComponent<MeshCollider>().enabled = false;
                }


            }
            if (!tapToPlaceBoiler)
            {
                sourceManager.Instance.sourcePressed = false;
                tapToPlaceBoiler = true;
            }


            if (isObj)
            {
                activeObj.transform.position = frontHolder.transform.position;
            }

            if (!isObj)
            {
                Vector3 pos = GazeManager.Instance.HitPosition;
                activeObj.transform.localPosition = pos;
            }

            if(sourceManager.Instance.sourcePressed && tapToPlaceBoiler)
            {
                LockBoiler();
            }

        }

        public void LockBoiler()
        {
            tapToPlaceBoiler = false;
            for (int i = 0; i < activeObj.transform.childCount; i++)
            {
                activeObj.transform.GetChild(i).GetComponent<MeshCollider>().enabled = true;
            }

            if (isObj)
            {
                activeObj.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            boiler.GetComponent<boilerIdentifier>().sendBoilerPosition();
        }


    }
}
