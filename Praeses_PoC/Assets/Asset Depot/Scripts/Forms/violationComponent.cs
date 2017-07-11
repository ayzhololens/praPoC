using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class violationComponent : MonoBehaviour
    {
        [Tooltip ("Value that gets stored on the violation controller")]
        public string value;
        [Tooltip ("Text diplay")]
        public Text displayText;
        public InputField[] optionContent;
        [Tooltip ("Top level violation controller that gets udpated")]
        public violationController linkedViolation;
        [Tooltip ("Index that gets stored in the violation controller")]
        public int Index;

        [Tooltip ("Check to set the date value")]
        public bool setDate;

        // Use this for initialization
        void Start()
        {
            //Set the future date of when the violation is due.
            if (setDate)
            {
                optionContent[0].placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString("MM");
                optionContent[1].placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString("dd");
                optionContent[2].placeholder.GetComponent<Text>().text = System.DateTime.Now.AddDays(180).ToString("yyyy");

            }

        }
        /// <summary>
        /// Add value and index to the violation controller then spawn the next section of the violation workflow
        /// </summary>
        public void setCategory()
        {
            if (linkedViolation.violationData.Count==0)
            {
                linkedViolation.violationData.Add(value);
                linkedViolation.violationIndices.Add(Index);
                vioControl.Instance.populateSubCategories(linkedViolation.violationIndices[0]);
            }
            else
            {
                linkedViolation.violationData[0] = value;
                linkedViolation.violationIndices[0] = Index;
            }
            Text linkedText = linkedViolation.violationSectionTitles[1];
            linkedText.text = Index + ") " + value;
            linkedViolation.goToTab(1);
        }


        /// <summary>
        /// Add the subcategory value then spawn the next section of the violation workflor
        /// </summary>
        public void setSubCategory()
        {
            if (linkedViolation.violationData.Count == 1)
            {
                linkedViolation.violationData.Add(value);
                linkedViolation.violationIndices.Add(Index);
                vioControl.Instance.populateViolations(linkedViolation.violationIndices[1]);
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
            linkedViolation.goToTab(2);
        }

        /// <summary>
        /// Add the violation value and index
        /// </summary>
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

            linkedViolation.goToTab(3);

        }

        /// <summary>
        /// Add classification value and index
        /// </summary>
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
            linkedViolation.goToTab(4);

        }

        /// <summary>
        /// Parse the date from the three content pieces.  Add that to the violation controller
        /// </summary>
        public void setDueDate()
        { 
            Text linkedText = linkedViolation.violationSectionTitles[5];
            string tempDate = "";
            for (int i = 0; i < optionContent.Length; i++)
            {
                if (i != optionContent.Length - 1)
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

            tempDate = System.DateTime.Now.AddDays(180).ToString("MM/dd/yyyy");

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

            linkedViolation.goToTab(5);
        }


        /// <summary>
        /// Set conditions and requirements
        /// </summary>
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
            linkedViolation.goToTab(6);
        }

        public void setComments()
        {

            linkedViolation.goToTab(7);

        }
        

    }
}