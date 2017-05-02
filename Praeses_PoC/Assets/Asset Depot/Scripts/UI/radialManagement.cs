using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Examples.GazeRuler;

namespace HoloToolkit.Unity
{
    public class radialManagement : Singleton<radialManagement>
    {

        public GameObject RadialMenu;

        //public GameObject RadialAlt;
        //public GameObject RadialAlt2;
        //public GameObject RadialAlt3;
        //public int radialCounter;
        //public int radialMode;

        public Transform RadialHolder;
        sourceManager sourceManager;
        GazeManager gazeManager;
        public bool isActive;
        public GameObject focusedButton;
        public bool radialOpenNotClicked;
        public GameObject lineCenter;
        public GameObject radialCountIndicator;
        public float radialCounter;
        public bool counting;
        public bool hands;
        radialHands radHands;
        public GameObject Cursor;



        //// Use this for initialization
        void Start()
        {
            sourceManager = sourceManager.Instance;
            gazeManager = GazeManager.Instance;
            radHands = GetComponent<radialHands>();
        }




        // Update is called once per frame
        void FixedUpdate()
        {

            //radial turn on counter
            if (sourceManager.sourcePressed && !isActive)
            {
                if (gazeManager.HitObject != null)
                {
                    if (gazeManager.HitObject.tag == "SpatialMapping" || gazeManager.HitObject.tag == "boilerPrefab")
                    {
                        timerManager.Instance.radialCountDown();
                    }
                }
                else
                {
                    timerManager.Instance.radialCountDown();
                }
                //radialCounter -= Time.deltaTime;
                //Debug.Log("shoulddaa");
                //if (!counting)
                //{
                //    startRadialCounter();
                //    counting = true;
                //}


                //if (radialCounter < .1f)
                //{

                //}

                //if (radialCounter < 0)
                //{
                //    turnOnRadialMenu();
                //    //radialCountIndicator.transform.GetChild(0).GetComponent<Animator>().SetTrigger("radialStop");
                //}

            }else if (!sourceManager.sourcePressed)
            {
                timerManager.Instance.CountInterrupt();

                //if (counting)
                //{
                //    radialCounter = countMax;
                //    radialCountIndicator.GetComponent<tumblerRadialCounter>().radialCounterInterrupt();
                //    //radialCountIndicator.GetComponent<tumblerRadialCounter>().toggleAnim();
                    
                //    counting = false;
                //}

                //radialCountIndicator.transform.GetChild(0).GetComponent<Animator>().SetTrigger("radialStop");

            }

            if (!hands)
            {
                GazeRadial();
            }


            if (hands)
            {
                HandRadial();
            }

        }

        void GazeRadial()
        {


            //get focused object
            if (gazeManager.HitObject != null && gazeManager.HitObject.tag == "Button")
            {
                focusedButton = gazeManager.HitObject;
            }

            //clear focused object
            else if (gazeManager.HitObject == null || gazeManager.HitObject.tag != "Button")
            {
                focusedButton = null;
            }

            //released pinch and radial is still active so hide the line or hide line if not looking at menu
            if ((gazeManager.HitObject.tag != "Button" && gazeManager.HitObject.tag != "Backplate") || radialOpenNotClicked)
            {
                lineCenter.GetComponent<LineTest>().line.SetActive(false);
                lineCenter.SetActive(false);
                Debug.Log("line off");
            }

            //looking at menu so dont hide the line
            else if (!lineCenter.activeSelf && (gazeManager.HitObject.tag == "Button" || gazeManager.HitObject.tag == "Backplate") && !radialOpenNotClicked)
            {
                lineCenter.SetActive(true);
                lineCenter.GetComponent<LineTest>().line.SetActive(true);
                Debug.Log("line on");

            }
            //released so keep it open
            if (!sourceManager.sourcePressed && isActive && gazeManager.HitObject.tag != "Button")
            {
                radialOpenNotClicked = true;

            }

            //tapping off radial menu so turn it off
            if (sourceManager.sourcePressed && isActive && radialOpenNotClicked && gazeManager.HitObject.tag != "Button")
            {
                turnOffRadialMenu();
                radialOpenNotClicked = false;
            }


            //tapping on radial menu so turn it off
            if (sourceManager.sourcePressed && isActive && radialOpenNotClicked && gazeManager.HitObject.tag == "Button")
            {
                turnOffRadialMenu();
                radialOpenNotClicked = false;
            }

            //released over button 
            if (!sourceManager.sourcePressed && isActive && gazeManager.HitObject.tag == "Button" && !radialOpenNotClicked)
            {
                turnOffRadialMenu();
            }
        }

        void HandRadial()
        {


            //released so keep it open
            //if (!sourceManager.sourcePressed && isActive && !annotManager.annotating && focusedButton == null)
            //{
            //    radialOpenNotClicked = true;

            //}

            ////released pinch and radial is still active so hide the line or hide line if not looking at menu
            //if ((focusedButton!=null && radHands.focusedObj.tag != "Backplate") || radialOpenNotClicked)
            //{
            //    line.GetComponent<LineTest>().line.SetActive(false);
            //    line.SetActive(false);
            //    Debug.Log("line off");
            //}

            ////looking at menu so dont hide the line
            //else if (!line.activeSelf && (focusedButton != null || radHands.focusedObj.tag == "Backplate") && !radialOpenNotClicked)
            //{
            //    line.SetActive(true);
            //    line.GetComponent<LineTest>().line.SetActive(true);
            //    Debug.Log("line on");

            //}
            ////tapping off radial menu so turn it off
            //if (sourceManager.sourcePressed && isActive && gazeManager.HitObject.tag != "Button")
            //{
            //    turnOffRadialMenu();
            //    radialOpenNotClicked = false;
            //}

            ////tapping on radial menu so turn it off
            //if (sourceManager.sourcePressed && isActive && radialOpenNotClicked && gazeManager.HitObject.tag == "Button")
            //{
            //    turnOffRadialMenu();
            //    radialOpenNotClicked = false;
            //}

            //released over button 
            if (!sourceManager.sourcePressed && isActive)
            {
                turnOffRadialMenu();
            }

            if (isActive)
            {
                //gazeCursor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
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
            if (!isActive)
            {
                //gazeCursor.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
            }

        }

        public void turnOnRadialMenu()
        {
            //radialCounter = countMax;
            //counting = false;
            radHands.canManipulate = true;
            //Cursor.SetActive(false);
            RadialMenu.SetActive(true);
            RadialMenu.transform.position = RadialHolder.position;
            RadialMenu.transform.LookAt(ActorSingleton.Actor.transform);
            RadialMenu.GetComponent<BoxCollider>().enabled = false;
            isActive = true;
            lineCenter.SetActive(true);
            lineCenter.GetComponent<LineTest>().line.SetActive(true);
            //radialCountIndicator.SetActive(false);


        }

        public void startRadialCounter()
        {
            radialCountIndicator.SetActive(true);
            //radialCountIndicator.transform.position = Cursor.transform.position;
            //radialCountIndicator.transform.LookAt(ActorSingleton.Actor.transform);
            //radialCountIndicator.GetComponent<tumblerRadialCounter>().toggleAnim();
            radialCountIndicator.GetComponent<tumblerRadialCounter>().radialCounterOn();
            //counting = true;
            //radialCountIndicator.transform.GetChild(0).GetComponent<Animator>().SetTrigger("radialStart");


        }

        public void turnOffRadialMenu()
        {

            float lineScale = lineCenter.GetComponent<LineTest>().scale;

            if (focusedButton == null)
            {
                BroadcastMessage("OnFocusExit", SendMessageOptions.DontRequireReceiver);
                RadialMenu.SetActive(false);
                RadialMenu.transform.position = RadialHolder.position;
                RadialMenu.transform.LookAt(ActorSingleton.Actor.transform);
                isActive = false;
                lineCenter.GetComponent<LineTest>().line.transform.localScale = new Vector3(lineScale, lineScale, lineScale);
                lineCenter.SetActive(false);
                lineCenter.GetComponent<LineTest>().line.SetActive(false);

            }

            if (focusedButton != null)
            {


                BroadcastMessage("OnFocusExit");
                focusedButton.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
                Debug.Log("sent");
                RadialMenu.SetActive(false);
                RadialMenu.transform.position = RadialHolder.position;
                RadialMenu.transform.LookAt(ActorSingleton.Actor.transform);
                focusedButton = null;
                isActive = false;
                lineCenter.GetComponent<LineTest>().line.transform.localScale = new Vector3(lineScale, lineScale, lineScale);
                lineCenter.SetActive(false);
                lineCenter.GetComponent<LineTest>().line.SetActive(false);



            }
            RadialMenu.GetComponent<BoxCollider>().enabled = true;
            radHands.canManipulate = false;
            Cursor.SetActive(true);



        }

    }
}