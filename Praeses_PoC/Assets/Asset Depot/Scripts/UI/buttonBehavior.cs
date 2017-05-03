using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    public class buttonBehavior : MonoBehaviour
    {
        public bool subButton;
        public GameObject subButtonParent;
        public GameObject nextButtonHolder;
        public GameObject currentButtonHolder;
        public Transform backHolder;
        public Transform frontPosHolder;
        public Transform downHolder;
        public Transform upHolder;
        Vector3 curTargetPos;
        Vector3 nextTargetPos;
        public float speed;
        public bool moving;


        // Use this for initialization
        void Start()
        {
            moving = false;

        }

        // Update is called once per frame
        void Update()
        {

            if (moving)
            {

                nextButtonHolder.transform.position = Vector3.MoveTowards(nextButtonHolder.transform.position, nextTargetPos, speed * Time.deltaTime);
                currentButtonHolder.transform.position = Vector3.MoveTowards(currentButtonHolder.transform.position, curTargetPos, speed * Time.deltaTime * 1.1f);
                Invoke("turnOffHolder", 1f);

                if (nextButtonHolder.transform.position == nextTargetPos)
                {
                    moving = false;
                    currentButtonHolder.transform.position = backHolder.position;
                    Debug.Log("done");
                    GetComponent<gazeLeaveEvent>().enabled = true;
                    currentButtonHolder.SetActive(false);

                    //if (subButtonParent != null)
                    //{
                    //    subButtonParent.SetActive(false);
                    //}

                    //if (subButtonParent != null)
                    //{
                    //    subButtonParent.SetActive(false);
                    //}
                }
            }
        }

        public void keepSubButtonsOn()
        {
            subButtonParent.GetComponent<subMenu>().subButtonsOn = true;

        }

        public void goToNextButton()
        {
            currentButtonHolder.SetActive(false);
            //nextButtonHolder.transform.LookAt(ActorSingleton.Actor.transform);
            nextButtonHolder.SetActive(true);
        }

        void turnOffHolder()
        {
            moving = false;
            currentButtonHolder.transform.position = backHolder.position;
            Debug.Log("done");
            GetComponent<gazeLeaveEvent>().enabled = true;
            currentButtonHolder.SetActive(false);
        }

        public void acceptInput()
        {
            nextButtonHolder.SetActive(true);
            moving = true;
            nextButtonHolder.transform.position = backHolder.transform.position;
            curTargetPos = upHolder.position;
            nextTargetPos = frontPosHolder.position;
            // nextButtonHolder.transform.position = Vector3.MoveTowards(nextButtonHolder.transform.position, frontPosHolder.transform.position, speed * Time.deltaTime);
            //currentButtonHolder.transform.position = Vector3.MoveTowards(currentButtonHolder.transform.position, downHolder.transform.position, speed * Time.deltaTime);
            //currentButtonHolder.SetActive(false);
        }

        public void denyInput()
        {
            nextButtonHolder.SetActive(true);
            moving = true;
            nextButtonHolder.transform.position = upHolder.transform.position;
            curTargetPos = backHolder.position;
            nextTargetPos = frontPosHolder.position;
        }
    }
}
