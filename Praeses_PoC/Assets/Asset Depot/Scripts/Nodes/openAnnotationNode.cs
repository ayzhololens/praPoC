using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class openAnnotationNode : MonoBehaviour {

        public GameObject contentHoler;
        public Transform contenLoc;
        public bool contentOpen;
        public annotationManager annotManager;
        public bool isMiniNode;
        public GameObject miniNode;
        public GameObject parentNode;
        Transform miniMapTagAlong;
        bool miniNodeOpen;
        float camDistance;
        float contentDistance;
        public float distanceThreshold;
        public int openNodeCounter;
        //Vector3 startPos;
        public float speed;
        bool opening;
        Vector3 miniMapPos;
        public bool reviewState;
        public GameObject reviewButtons;



        // Use this for initialization
        void Start() {
            miniMapTagAlong = GameObject.Find("SpatialMapping").GetComponent<miniMapToggle>().MiniMapHolder.transform;
            annotManager = GameObject.Find("AnnotationManager").GetComponent<annotationManager>();
            if (isMiniNode)
            {
                closeContent();
            }
            
                        

        }

        // Update is called once per frame
        void Update() {

            camDistance = Vector3.Distance(transform.position, ActorSingleton.Actor.transform.position);
            //Debug.Log(camDistance);

            if (!reviewState)
            {
                if (openNodeCounter == 2 && contentHoler.activeSelf && !miniNodeOpen && contentOpen)
                {
                    if (camDistance > distanceThreshold)
                    {
                        contentDistance = Vector3.Distance(contentHoler.transform.position, ActorSingleton.Actor.transform.position);
                        if (contentDistance > 2 && contentHoler.GetComponent<SimpleTagalong>().enabled != true)
                        {
                            contentHoler.transform.position = Vector3.MoveTowards(contentHoler.transform.position, ActorSingleton.Actor.transform.position, speed / 1.5f);
                        }

                        if (contentDistance < 2 && contentHoler.GetComponent<SimpleTagalong>().enabled != true)
                        {
                            contentHoler.GetComponent<SimpleTagalong>().enabled = true;
                            //contentHoler.GetComponent<Interpolator>().enabled = true;
                        }



                    }

                    if (camDistance < distanceThreshold)
                    {
                        if (contentHoler.GetComponent<SimpleTagalong>().enabled == true)
                        {
                            contentHoler.GetComponent<SimpleTagalong>().enabled = false;
                            contentHoler.GetComponent<Interpolator>().enabled = false;
                        }

                        contentHoler.transform.position = Vector3.MoveTowards(contentHoler.transform.position, contenLoc.position, speed);


                    }

                }

                if (openNodeCounter == 2 && contentHoler.activeSelf && miniNodeOpen)
                {
                    miniMapPos = new Vector3(miniMapTagAlong.position.x, miniMapTagAlong.position.y + .18f, miniMapTagAlong.position.z);
                    contentDistance = Vector3.Distance(contentHoler.transform.position, miniMapTagAlong.position);

                    if (contentDistance > .1f)
                    {
                        contentHoler.transform.position = Vector3.MoveTowards(contentHoler.transform.position, miniMapPos, speed);
                    }
                    else
                    {
                        contentHoler.transform.position = miniMapPos;
                    }

                }
            }








        }

        public void enableReview()
        {
            if (!isMiniNode)
            {
                reviewButtons.SetActive(true);
                reviewState = true;
                if (contentHoler.GetComponent<SimpleTagalong>().enabled != true)
                {
                    contentHoler.GetComponent<SimpleTagalong>().enabled = true;

                }
                
                for (int i = 0; i < GetComponent<commentManager>().activeComments.Count; i++)
                {
                    GetComponent<commentManager>().activeComments[i].GetComponent<commentContents>().editButton.SetActive(true);
                }
            }

        }

        public void completeReview()
        {
            if (!isMiniNode)
            {
                reviewButtons.SetActive(false);
                reviewState = false;
                for (int i = 0; i < GetComponent<commentManager>().activeComments.Count; i++)
                {
                    GetComponent<commentManager>().activeComments[i].GetComponent<commentContents>().editButton.SetActive(false);
                }
                BroadcastMessage("OnFocusExit", SendMessageOptions.DontRequireReceiver);


            }
        }




        public void recapture()
        {
            if (GetComponent<nodeMediaHolder>().photoNode)
            {
                annotationManager.Instance.currentAnnotation = this.gameObject;
                annotationManager.Instance.enablePhotoCapture();
                closeContent();
            }
            if (GetComponent<nodeMediaHolder>().videoNode)
            {
                annotationManager.Instance.currentAnnotation = this.gameObject;
                annotationManager.Instance.enableVideoRecording();
                closeContent();
            }
        }
        

        public void openContent()
        {



            if (isMiniNode)
            {
                parentNode.GetComponent<openAnnotationNode>().openContent();
                parentNode.GetComponent<openAnnotationNode>().miniNodeOpen = true;
            }

            if (!isMiniNode)
            {
                contentHoler.transform.position = contenLoc.position;
                if (annotManager != null)
                {

                    foreach (GameObject annots in annotManager.activeAnnotations)
                    {
                        if (annots.GetComponent<openAnnotationNode>() != null)
                        {
                            annots.GetComponent<openAnnotationNode>().closeContent();
                        }


                    }
                }

                contentHoler.SetActive(true);

                if (GetComponent<nodeMediaHolder>().videoNode)
                {
                    GetComponent<nodeMediaHolder>().LoadVideo();
                }

                if (GetComponent<nodeMediaHolder>().photoNode)
                {
                    //GetComponent<nodeMediaHolder>().loadPhotoMedia();
                }
                contentOpen = true;



            }


            





        }

        public void closeContent()
        {
            //contentHoler.transform.position = startPos;

            BroadcastMessage("GazeLeave", SendMessageOptions.DontRequireReceiver);


            if (contentHoler != null)
            {
                contentHoler.SetActive(false);
            }
            

            if (!isMiniNode)
            {
                //contentHoler.GetComponent<SimpleTagalong>().enabled = false;
                contentHoler.GetComponent<Interpolator>().enabled = false;
                contentHoler.transform.position = contenLoc.position;
                miniNodeOpen = false;
                contentOpen = false;
                //completeReview();
            }
            
        }
    }
}
