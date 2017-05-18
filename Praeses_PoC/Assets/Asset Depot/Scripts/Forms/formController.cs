using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class formController : Singleton<formController> {


        public GameObject[] fieldTabs;
        public GameObject[] fieldTabButtons;
        public Transform fieldStartPos;
        public GameObject contentHolder;
        public GameObject Sumbit;
        public submitInspection submitInspection;
        public Text fieldStatus;
        public int totalFields { get; set; }
        public int curFields { get; set; }
        public Transform frontHolder;
        public List<GameObject> fieldNodes;


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }



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



        public void toggleForm()
        {
            contentHolder.SetActive(!contentHolder.activeSelf);
        }

        public void closeForm()
        {
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

        public void openForm()
        {
            contentHolder.transform.position = frontHolder.position;
            checkTab();
            contentHolder.SetActive(true);
        }

        public void updateFieldStatus(int amount)
        {
            curFields += amount;
            fieldStatus.text = "Total Fields Completed: " + curFields + "/" + totalFields;
        }
    }
}