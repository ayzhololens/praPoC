using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Examples.GazeRuler;

namespace HoloToolkit.Unity
{
    public class radialManagement : Singleton<radialManagement>
    {

        sourceManager sourceManager;
        GazeManager gazeManager;
        radialHands radHands;


        public GameObject RadialMenu;
        public Transform radialHolder;
        public GameObject radialLine;
        public GameObject Cursor;


        public GameObject focusedButton;
        public float radialCounter;
        public bool canOpen { get; set; }
        public bool isActive { get; set; }


        //// Use this for initialization
        void Start()
        {
            canOpen = true;
            sourceManager = sourceManager.Instance;
            gazeManager = GazeManager.Instance;
            radHands = GetComponent<radialHands>();
        }





        // Update is called once per frame

        //listen for sourcepressed state and countdown to enable radial menu
        void FixedUpdate()
        {
            if (canOpen)
            {
                //radial turn on counter
                if (sourceManager.sourcePressed && !isActive)
                {
                    //check to see if youre looking at the boiler or spatial mapping
                    if (gazeManager.HitObject != null)
                    {
                        if (gazeManager.HitObject.tag == "SpatialMapping" || gazeManager.HitObject.tag == "boilerPrefab")
                        {
                            timerManager.Instance.radialCountDown();
                        }
                    }
                    //check to see if youre looking at nothing
                    else
                    {
                        timerManager.Instance.radialCountDown();
                    }

                }

                //interrupt the countdown
                else if (!sourceManager.sourcePressed)
                {
                    timerManager.Instance.CountInterrupt();
                }

                if (isActive)
                {
                    HandRadial();
                }

            }
        }


        //check radial hands to find focuesd hand object and sourcepressed state
        void HandRadial()
        {
            //released over button 
            if (!sourceManager.sourcePressed)
            {
                turnOffRadialMenu();
            }
            else
            {
                if (radHands.focusedObj != null)
                {
                    if (radHands.focusedObj.tag == "Button")
                    {
                        focusedButton = radHands.focusedObj;
                    }
                    if (radHands.focusedObj.tag != "Button")
                    {
                        focusedButton = null;
                    }
                }
                else
                {
                    focusedButton = null;
                }

            }
            
        }
        public void turnOnRadialMenu()
        {
            //enable radial menu and position it
            radHands.canManipulate = true;
            RadialMenu.SetActive(true);
            RadialMenu.transform.position = radialHolder.position;
            RadialMenu.transform.LookAt(Camera.main.transform);
            RadialMenu.GetComponent<BoxCollider>().enabled = false;
            isActive = true;

            //turn on the hand line
            radialLine.SetActive(true);
            radialLine.GetComponent<LineTest>().line.SetActive(true);


        }


        //send message on release to execute function and then reset the radial menu
        public void turnOffRadialMenu()
        {

            float lineScale = radialLine.GetComponent<LineTest>().scale;

            if (focusedButton == null)
            {
                BroadcastMessage("OnFocusExit", SendMessageOptions.DontRequireReceiver);
            }

            if (focusedButton != null)
            {
                focusedButton.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("OnFocusExit");


            }
            RadialMenu.SetActive(false);
            RadialMenu.transform.position = radialHolder.position;
            RadialMenu.transform.LookAt(Camera.main.transform);
            focusedButton = null;
            isActive = false;
            RadialMenu.GetComponent<BoxCollider>().enabled = true;
            radHands.canManipulate = false;
            Cursor.SetActive(true);


            //reset the line
            radialLine.GetComponent<LineTest>().line.transform.localScale = new Vector3(lineScale, lineScale, lineScale);
            radialLine.SetActive(false);
            radialLine.GetComponent<LineTest>().line.SetActive(false);




        }


        //since we instantiate the form and view violation sections, we have to link their fucntions to the onSelectEvents here
        public void openForm()
        {
            formController.Instance.openForm();
        }

        public void openReview()
        {
            formController.Instance.openForm();
            formController.Instance.goToTab(3);
        }

        public void toggleViewVios()
        {
            viewViolationController.Instance.toggleContent();
        }
    }
}