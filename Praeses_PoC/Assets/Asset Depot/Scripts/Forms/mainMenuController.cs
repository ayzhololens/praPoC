using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using RenderHeads.Media.AVProVideo;

namespace HoloToolkit.Unity
{
    public class mainMenuController : Singleton<mainMenuController> {

        public GameObject[] tabs;
        public formContent[] preloadedDataFields;
        public GameObject contentHolder;
        public GameObject alignerIndicator;
        bool startedAlignment;
        int curTab;
        public MediaPlayer radAnim;
        public GameObject tapToContinue;
        bool videoStarted;

        // Use this for initialization
        void Start() {
            //openMainMenu();
        }

        // Update is called once per frame
        void Update() {
            if (startedAlignment)
            {
                findZone();
            }
            if (videoStarted)
            {
                if (radAnim.Control.IsFinished())
                {
                    tapToContinue.SetActive(true);
                    radAnim.Control.Play();

                }
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
            curTab = tabIndex;
            tabs[tabIndex].SetActive(true);
            print(curTab);
        }

        public void preloadData()
        {

            for (int i = 0; i < preloadedDataFields.Length; i++)
            {
                preloadedDataFields[i].loadDetails();
            }
        }

        public void closeMainMenu()
        {
            contentHolder.SetActive(false);
        }

        public void openMainMenu()
        {

            contentHolder.SetActive(true);
            contentHolder.transform.position = frontHolderInstance.Instance.setFrontHolder(1.5f).transform.position;
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
            if(GazeManager.Instance.HitObject.name == "AlignmentZone")
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
            audioManager.Instance.setAndPlayAudio(1);
            minimapSpawn.Instance.gameObject.GetComponent<spatialRadiate>().spatRadiate();
            alignerIndicator.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .2f);
            alignerIndicator.SetActive(false);
            Invoke("turnOffAligner", 2);
        }

        void turnOffAligner()
        {
            openMainMenu();
            goToTab(5);
            mediaManager.Instance.disableStatusIndicator();
        }

        public void completeMainMenu()
        {
            goToTab(7);
            videoStarted = true;
            radAnim.Control.Play();
        }

        public void stopRad()
        {
            radAnim.Control.Stop();
            videoStarted = false;
            tapToContinue.SetActive(false);
            mediaManager.Instance.setStatusIndicator("Say 'Help' for more information");
            mediaManager.Instance.invokeStatusDisable(4);

        }


        void turnOffInd()
        {
            mediaManager.Instance.disableStatusIndicator();
        }

        public void goBackTab()
        {
            if (curTab == 3 || curTab == 4)
            {

                goToTab(curTab - 2);
            }
            else if (curTab != 0)
            {
                goToTab(curTab - 1);

            }
        }

    }
}