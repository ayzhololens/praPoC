using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class nodeController : MonoBehaviour
    {

        [Header("Content and Location")]
        public GameObject contentHolder;
        public Transform contentStartLoc;
        public GameObject parentNode;
        public GameObject miniNode { get; set; }
        public bool isMiniNode;
        SimpleTagalong nodeTagalong;
        bool contentOpen;
        [Header("Follow Controls")]
        public float distanceThreshold;
        public float moveSpeed;
        public bool fromJSON { get; set; }
        public GameObject linkedField;
        public GameObject reviewButtons;



        void Start()
        {
            // set initial location of the node content

        }

        public void setUpNode()
        {
            //essentially our start function but called when node is placed
            if (!isMiniNode)
            {
                nodeTagalong = contentHolder.GetComponent<SimpleTagalong>();
                openNode();
            }
        }
        
        void Update()
        {
            if (contentOpen && !nodeTagalong.locked)
            {
                //distanceChecker();
            }

        }

        

        //Open node's content
        public void openNode()
        {   
            //close every node that could be open
            closeAllNodes(parentNode);

            // if mini node, open the parent
            if (isMiniNode)
            {
                nodeController parentOpener = parentNode.GetComponent<nodeController>();
                parentOpener.openNode();


            }
            else
            {
                //close every node that could be open
                //closeAllNodes(gameObject);

                //open node content
                contentOpen = true;
                contentHolder.SetActive(true);
                contentHolder.GetComponent<DirectionIndicator>().enabled = true;
                contentHolder.GetComponent<DirectionIndicator>().hasGazed = false;
                contentHolder.transform.position = contentStartLoc.position;
                GetComponent<AudioSource>().PlayClip(audioManager.Instance.openSound);
                print("open " + gameObject.name);

                if (GetComponent<nodeMediaHolder>().videoNode)
                {
                    GetComponent<nodeMediaHolder>().LoadVideo();
                }

                if (GetComponent<nodeMediaHolder>().fieldNode || GetComponent<nodeMediaHolder>().violationNode)
                {
                    contentHolder.transform.parent.gameObject.SendMessage("checkTab", SendMessageOptions.DontRequireReceiver);
                }
                if (GetComponent<nodeMediaHolder>().fieldNode)
                {
                    formController.Instance.goToTab(2);
                }

            }


        }

        void closeAllNodes(GameObject thisNode)
        {

            foreach (GameObject nodes in mediaManager.Instance.activeNodes)
            {
                if (nodes != thisNode)
                {
                    nodeController nodeControl = nodes.GetComponent<nodeController>();
                    nodeControl.closeNode();
                    nodeControl.contentHolder.GetComponent<DirectionIndicator>().enabled = false;
                }
            }
        }

        public void closeNode()
        {
            //close node content
            contentHolder.SetActive(false);
            contentOpen = false;
        }

        public void repos()
        {
            nodeSpawner.Instance.repositionNode(gameObject);
        }

        //public void enableReview()
        //{
        //    if (!isMiniNode)
        //    {
        //        reviewButtons.SetActive(true);
        //        for (int i = 0; i < GetComponent<commentManager>().activeComments.Count; i++)
        //        {
        //            //GetComponent<commentManager>().activeComments[i].GetComponent<commentContents>().editButton.SetActive(true);
        //        }
        //    }

        //}

        //public void completeReview()
        //{
        //    if (!isMiniNode)
        //    {
        //        reviewButtons.SetActive(false);
        //        for (int i = 0; i < GetComponent<commentManager>().activeComments.Count; i++)
        //        {
        //            //GetComponent<commentManager>().activeComments[i].GetComponent<commentContents>().editButton.SetActive(false);
        //        }
        //        BroadcastMessage("OnFocusExit", SendMessageOptions.DontRequireReceiver);


        //    }
        //}


        // determine whether or not to tag along
        void distanceChecker()
        {
            //get user's distance from the node
            Vector3 contentPos = contentHolder.transform.position;
            Vector3 camPos = Camera.main.transform.position;
            float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);

            if (camDistance > distanceThreshold )
            {
                //check if the node is close enough to user to enable tagalong, if not move it closer
                float contentDistance = Vector3.Distance(contentPos, camPos);

                if (contentDistance > nodeTagalong.TagalongDistance)
                {
                    if (!nodeTagalong.enabled)
                    {
                        contentHolder.transform.position = Vector3.MoveTowards(contentPos, camPos, moveSpeed);
                    }
                }
                else
                {
                    if (!nodeTagalong.enabled)
                    {
                        nodeTagalong.enabled = true;
                    }
                }

            }
            else
            {
                // turn off tagalong and move content back to the node
                if (nodeTagalong.enabled)
                {
                    nodeTagalong.enabled = false;
                    contentHolder.GetComponent<Interpolator>().enabled = false;
                }
                if (contentPos != contentStartLoc.position)
                {
                    contentHolder.transform.position = Vector3.MoveTowards(contentPos, contentStartLoc.position, moveSpeed);
                }
            }
        }


    }
}