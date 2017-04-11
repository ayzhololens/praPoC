﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using RenderHeads.Media.AVProVideo;


namespace HoloToolkit.Unity
{
    public class violationController : MonoBehaviour
    {
        public GameObject contentHolder;
        public List<string> violationData;
        public List<int> violationIndices;
        public GameObject[] violationTabs;
        public GameObject[] violationTabButtons;
        public Text violationHeader;
        public Text violationContent;
        public Transform boxStartPos;
        public Transform fieldStartPos;
        public GameObject linkedNode;
        public violationReview vioReview;
        public GameObject linkedPreview;
        public Transform frontHolder;
        // Use this for initialization
        void Start()
        {
            frontHolder = Camera.main.transform.GetChild(0);
        }

        // Update is called once per frame
        void Update()
        {

        }


        //enablements here
        #region
        public void enableCategories()
        {
            if(violationData.Count > 0)
            {
                violationTabs[0].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 0)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }

            }
        }

        public void enableSubCategories()
        {
            if (violationData.Count > 1)
            {

                violationTabs[1].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 1)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }
            }
        }

        public void enableViolations()
        {

            if(violationData.Count > 2)
            {

                violationTabs[2].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 2)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }
            }
        }

        public void enableClassification()
        {

            if(violationData.Count > 3)
            {
                violationTabs[3].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 3)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }
            }
        }

        public void enableDueDate()
        {
            if (violationData.Count > 4)
            {

                violationTabs[4].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 4)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }
            }
        }

        public void enableConditions()
        {
            if(violationData.Count > 5)
            {

                violationTabs[5].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 5)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }
            }
        }

        public void enableRequirements()
        {
            if(violationData.Count > 6)
            {
                violationTabs[6].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 6)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }

            }
        }

        public void enableReview()
        {
            if (violationData.Count > 7)
            {

                violationTabs[7].SetActive(true);

                for (int i = 0; i < violationTabs.Length; i++)
                {
                    if (i != 7)
                    {
                        if (violationTabs[i].activeSelf)
                        {
                            violationTabs[i].SetActive(false);
                        }

                    }
                }
            }

        }
        #endregion 

        public void openViolation()
        {
            contentHolder.transform.position = frontHolder.position;
            linkedNode.GetComponent<nodeController>().openNode();
        }
        public void closeViolation()
        {
            linkedNode.GetComponent<nodeController>().closeNode();
        }

        public void showTabs(bool active)
        {
            for(int i=0; i< violationTabButtons.Length; i++)
            {
                violationTabButtons[i].SetActive(active);
            }
        }
        


    }
}