using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;


namespace HoloToolkit.Unity
{
    public class mainMenuController : MonoBehaviour {

        public GameObject[] tabs;
        public formContent[] preloadedDataFields;
        public GameObject contentHolder;
        public GameObject aligner;
        public GameObject alignerIndicator;
        bool startedAlignment;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (startedAlignment)
            {
                findZone();
            }

        }

        public void goToTab(int tabIndex)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].activeSelf && i != tabIndex)
                {
                    tabs[i].SetActive(false);
                }
            }
            tabs[tabIndex].SetActive(true);
        }

        public void preloadData()
        {

            for (int i = 0; i < preloadedDataFields.Length; i++)
            {
                //preloadedDataFields[i].loadDetails();
            }
        }

        public void closeMainMenu()
        {
            contentHolder.SetActive(false);
        }

        public void openMainMenu()
        {
            contentHolder.SetActive(true);
        }


        public void beginAlignment()
        {
            mediaManager.Instance.setStatusIndicator("Please Locate Boiler Tag");
            closeMainMenu();
            alignerIndicator.SetActive(true);
            startedAlignment = true;
        }

        void findZone()
        {
            if(GazeManager.Instance.HitObject == aligner)
            {
                mediaManager.Instance.setStatusIndicator("Tag Located! Calibrating...");
                alignerIndicator.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .8f);
                startedAlignment = false;
                Invoke("finishAlignment", 3);
            }
        }

        void finishAlignment()
        {
            mediaManager.Instance.setStatusIndicator("Success!");
            minimapSpawn.Instance.gameObject.GetComponent<spatialRadiate>().spatRadiate();
            alignerIndicator.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .2f);
            alignerIndicator.SetActive(false);
            Invoke("turnOffAligner", 2);
        }

        void turnOffAligner()
        {
            openMainMenu();
            goToTab(6);
            mediaManager.Instance.disableStatusIndicator();
        }

    }
}