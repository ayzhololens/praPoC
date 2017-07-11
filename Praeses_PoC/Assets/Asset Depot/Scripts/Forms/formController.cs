using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class formController : Singleton<formController> {
        
        [Tooltip("Content Parents")]
        public GameObject[] fieldTabs;
        [Tooltip ("Associated Tab Buttons")]
        public GameObject[] fieldTabButtons;
        [Tooltip ("Start Location of the fields, used in DataManager>Field Spawner")]
        public Transform fieldStartPos;
        [Tooltip("Tagalong holder")]
        public GameObject contentHolder;
        [Tooltip("Review button that we need to reposition at the end of all our spawned fields")]
        public GameObject Sumbit;

        [Tooltip ("Script that controls the review inspection components")]
        public submitInspection submitInspection;
        [Tooltip("Indicator of how many fields have been completed")]
        public Text fieldStatus;
        public int totalFields { get; set; }
        public int curFields { get; set; }
        [Tooltip ("List of associated field nodes") ]
        public List<GameObject> fieldNodes;

        [Tooltip ("For loading non editable data.  Same idea as whats happening in mainMenu controller")]
        public formContent[] preloadedData;


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }


        /// <summary>
        /// Find current tab
        /// </summary>
        public void checkTab()
        {
            for (int i = 0; i < fieldTabs.Length; i++)
            {
                if (fieldTabs[i].activeSelf)
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

            for (int i = 0; i < fieldTabs.Length; i++)
            {
                if (i != tab)
                {
                    fieldTabButtons[i].GetComponent<buttonHightlight>().revertMat();
                    if (fieldTabs[i].activeSelf)
                    {

                        fieldTabs[i].SetActive(false);
                    }

                }
                else
                {
                    fieldTabButtons[i].GetComponent<buttonHightlight>().updateMat();
                    fieldTabs[i].SetActive(true);
                }
            }

        }


        /// <summary>
        /// Toggle the content holder.  Used in the radial menu
        /// </summary>
        public void toggleForm()
        {
            contentHolder.SetActive(!contentHolder.activeSelf);
        }

        /// <summary>
        /// Close the form.  Used in the 'X'
        /// </summary>
        public void closeForm()
        {
            contentHolder.transform.localScale = contentHolder.GetComponent<contentScaler>().sScale;
            if (fieldNodes.Count != 0)
            {
                foreach (GameObject node in fieldNodes)
                {
                    node.GetComponent<nodeController>().closeNode();
                }
            }
            else
            {
                contentHolder.SetActive(false);
            }

            
        }

        /// <summary>
        /// open form via node
        /// </summary>
        public void openForm()
        {

            contentHolder.transform.position = frontHolderInstance.Instance.setFrontHolder(2f).transform.position;
            checkTab();
            contentHolder.SetActive(true);

        }


        /// <summary>
        /// update the amount of fields completed
        /// </summary>
        /// <param name="amount"></param>
        public void updateFieldStatus(int amount)
        {
            curFields += amount;
            fieldStatus.text = "Total Fields Completed: " + curFields + "/" + totalFields;
        }


        /// <summary>
        /// Loads non editable data
        /// </summary>
        public void preloadFormData()
        {

            for (int i = 0; i < preloadedData.Length; i++)
            {
                preloadedData[i].loadDetails();
            }
        }
    }
}