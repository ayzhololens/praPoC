using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using RenderHeads.Media.AVProVideo;


namespace HoloToolkit.Unity
{
    public class violationController : MonoBehaviour
    {
        [Header ("Data Storage")]
        [Tooltip ("List of the string values of each violation section")]
        public List<string> violationData;
        [Tooltip ("List of the int values of each violation section")]
        public List<int> violationIndices;
        [Tooltip ("Violation Review Component.  Located in Adding Violations > Review")]
        public violationReview vioReview;

        [Header ("Content Components")]
        [Tooltip ("Tagalong holder")]
        public GameObject contentHolder;
        [Tooltip ("Section Parents")]
        public GameObject[] violationTabs;
        [Tooltip ("Visual buttons associated with section parents")]
        public GameObject[] violationTabButtons;
        [Header ("Layout")]
        public Transform boxStartPos;
        public Text[] violationSectionTitles;
        public InputField violationHeader;
        public Transform fieldStartPos;


        public GameObject linkedNode { get; set; }
        public GameObject linkedPreview { get; set; }
        public bool fromJson { get; set; }



        /// <summary>
        /// Find current tab
        /// </summary>
        public void checkTab()
        {
            for (int i = 0; i < violationTabs.Length; i++)
            {
                if (violationTabs[i].activeSelf)
                {
                    goToTab(i);
                }
            }
        }

        /// <summary>
        /// Visual feedback on current tab and enable it
        /// </summary>
        /// <param name="tab"></param> 
        public void goToTab(int tab)
        {

            for (int i = 0; i < violationTabs.Length; i++)
            {
                if (i != tab)
                {
                    violationTabButtons[i].GetComponent<buttonHightlight>().revertMat();
                    if (violationTabs[i].activeSelf)
                    {

                        violationTabs[i].SetActive(false);
                    }

                }
                else
                {
                    violationTabButtons[i].GetComponent<buttonHightlight>().updateMat();
                    violationTabs[i].SetActive(true);
                }
            }
            
        }
        
        //Tell the associated node to close
        public void closeViolation()
        {
            linkedNode.GetComponent<nodeController>().closeNode();
        }
        
        


    }
}