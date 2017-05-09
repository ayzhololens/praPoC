using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class violationComponent : MonoBehaviour
    {

        public string value;
        public Text displayText;
        public InputField[] optionContent;
        public violationController linkedViolation;
        public int Index;
        public bool setDate;

        // Use this for initialization
        void Start()
        {
            if (setDate)
            {
                optionContent[0].placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString("MM");
                optionContent[1].placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString("dd");
                optionContent[2].placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString("yyyy");

                //optionContent.placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString();
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setCategory()
        {
            if (linkedViolation.violationData.Count==0)
            {
                linkedViolation.violationData.Add(value);
                linkedViolation.violationIndices.Add(Index);
                violatoinSpawner.Instance.populateSubCategories(linkedViolation.violationIndices[0]);
            }
            else
            {
                linkedViolation.violationData[0] = value;
                linkedViolation.violationIndices[0] = Index;
            }
            Text linkedText = linkedViolation.violationSectionTitles[1];
            linkedText.text = Index + ") " + value;
            linkedViolation.violationTabButtons[0].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[0].SetActive(false);
            linkedViolation.violationTabs[1].SetActive(true);
        }

        public void setSubCategory()
        {
            if (linkedViolation.violationData.Count == 1)
            {
                linkedViolation.violationData.Add(value);
                linkedViolation.violationIndices.Add(Index);
                violatoinSpawner.Instance.populateViolations(linkedViolation.violationIndices[1]);
            }
            else
            {
                linkedViolation.violationData[1] = value;
                linkedViolation.violationIndices[1] = Index;
            }
            Text linkedText = linkedViolation.violationSectionTitles[2];
            linkedText.text = linkedViolation.violationIndices[0]
                + "." + linkedViolation.violationIndices[1] + " "
                + linkedViolation.violationData[0] + " > " +
                value;
            linkedViolation.violationTabButtons[1].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[1].SetActive(false);
            linkedViolation.violationTabs[2].SetActive(true);
        }

        public void setViolation()
        {
            if (linkedViolation.violationData.Count == 2)
            {
                linkedViolation.violationData.Add(value);
                linkedViolation.violationIndices.Add(Index);
            }
            else
            {
                linkedViolation.violationData[2] = value;
                linkedViolation.violationIndices[2] = Index;
            }
            Text linkedText = linkedViolation.violationSectionTitles[3];
            linkedText.text = linkedViolation.violationIndices[0]
                + "." + linkedViolation.violationIndices[1] + "."
                + linkedViolation.violationIndices[2] + " "
                + linkedViolation.violationData[2];
            
            linkedViolation.violationTabButtons[2].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[2].SetActive(false);
            linkedViolation.violationTabs[3].SetActive(true);

        }

        public void setClassification()
        {
            if (linkedViolation.violationData.Count == 3)
            {
                linkedViolation.violationData.Add(value);
                linkedViolation.violationIndices.Add(Index);
            }
            else
            {
                linkedViolation.violationData[3] = value;
                linkedViolation.violationIndices[3] = Index;
            }
            Text linkedText = linkedViolation.violationSectionTitles[4];
            linkedText.text = linkedViolation.violationIndices[0]
                + "." + linkedViolation.violationIndices[1] + "."
                + linkedViolation.violationIndices[2] + " "
                + linkedViolation.violationData[2];
            linkedViolation.violationTabButtons[3].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[3].SetActive(false);
            linkedViolation.violationTabs[4].SetActive(true);

        }

        public void setDueDate()
        { 
            Text linkedText = linkedViolation.violationSectionTitles[5];
            string tempDate = "";
            for (int i = 0; i < optionContent.Length; i++)
            {
                if(i!= optionContent.Length-1)
                {
                    if (optionContent[i].text.Length == 0)
                    {
                        tempDate = tempDate + optionContent[i].placeholder.GetComponent<Text>().text + "/";
                    }
                    else
                    {
                        tempDate = tempDate + optionContent[i].text + "/";
                    }
                }
                else
                {
                    if (optionContent[i].text.Length == 0)
                    {
                        tempDate = tempDate + optionContent[i].placeholder.GetComponent<Text>().text;
                    }
                    else
                    {
                        tempDate = tempDate + optionContent[i].text;
                    }
                }


            }

            if (linkedViolation.violationData.Count == 4)
            {
                linkedViolation.violationData.Add(tempDate);

                linkedViolation.violationIndices.Add(Index);
            }
            else
            {
                linkedViolation.violationData[4] = (tempDate);
                linkedViolation.violationIndices[4] = Index;
            }


            linkedText.text = linkedViolation.violationIndices[0]
                + "." + linkedViolation.violationIndices[1] + "."
                + linkedViolation.violationIndices[2] + " "
                + linkedViolation.violationData[2];
            
            linkedViolation.violationTabButtons[4].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[4].SetActive(false);
            linkedViolation.violationTabs[5].SetActive(true);
        }


        public void setConditions()
        {

            Text linkedText = linkedViolation.violationSectionTitles[6];

            if (linkedViolation.violationData.Count == 5)
            {

                linkedViolation.violationData.Add(optionContent[0].text);
                linkedViolation.violationData.Add(optionContent[1].text);
                linkedViolation.violationIndices.Add(Index);
                linkedViolation.violationIndices.Add(Index);
            }
            else
            {

                linkedViolation.violationData[5] = optionContent[0].text;
                linkedViolation.violationData[6] = optionContent[1].text;
                linkedViolation.violationIndices[5] = Index;
                linkedViolation.violationIndices[6] = Index;
            }
            linkedText.text = linkedViolation.violationIndices[0]
                + "." + linkedViolation.violationIndices[1] + "."
                + linkedViolation.violationIndices[2] + " "
                + linkedViolation.violationData[2];
            linkedViolation.violationIndices.Add(Index);
            
            linkedViolation.violationTabButtons[5].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[5].SetActive(false);
            linkedViolation.violationTabs[6].SetActive(true);
        }

        public void setComments()
        {

            linkedViolation.violationTabButtons[6].GetComponent<buttonHightlight>().updateMat();
            linkedViolation.violationTabs[6].SetActive(false);
            linkedViolation.violationTabs[7].SetActive(true);

        }

        public void setRequirements()
        {
            //Text linkedText = linkedViolation.violationTabButtons[6].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
            //if (linkedViolation.violationData.Count == 6)
            //{
            //    if (optionContent.text.Length == 0)
            //    {
            //        linkedViolation.violationData.Add(optionContent.placeholder.GetComponent<Text>().text);
            //        linkedText.text = optionContent.placeholder.GetComponent<Text>().text;
            //    }
            //    else
            //    {

            //        linkedViolation.violationData.Add(optionContent.text);
            //        linkedText.text = optionContent.text;
            //    }
            //    linkedViolation.violationIndices.Add(Index);
            //}
            //else
            //{

            //    if (optionContent.text.Length == 0)
            //    {
            //        linkedViolation.violationData[6] = optionContent.placeholder.GetComponent<Text>().text;
            //        linkedText.text = optionContent.placeholder.GetComponent<Text>().text;
            //    }
            //    else
            //    {

            //        linkedViolation.violationData[6] = (optionContent.text);
            //        linkedText.text = optionContent.text;
            //    }


            //    linkedViolation.violationIndices[6] = Index;
            //}
            //linkedText.color = Color.white;
            //linkedViolation.violationIndices.Add(Index);
            //linkedViolation.violationTabs[6].SetActive(false);
            //linkedViolation.violationTabs[7].SetActive(true);
        }


    }
}