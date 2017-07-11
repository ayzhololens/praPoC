using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class violationReview : MonoBehaviour {

        [Tooltip ("List of the violation data thats pulled from violationControl")]
        public Text[] violationData;
        public Text[] violationSubmittedData;
        public violationController violationControl;
        public GameObject submittedViolationHolder;
        public GameObject addingViolationHolder;



        public void loadReview()
        {
            for(int i=0; i < violationControl.violationData.Count+1; i++)
            {
                if(i!= violationControl.violationData.Count)
                {
                    violationData[i].text = violationControl.violationData[i];
                }
                else
                {
                    int sCounter = 0;
                    int pCounter = 0;
                    int vCounter = 0;
                    string tempString = "";
                    foreach(GameObject activeComment in violationControl.gameObject.GetComponent<commentManager>().activeComments)
                    {
                        if (activeComment.GetComponent<commentContents>().isSimple)
                        {
                            sCounter += 1;
                        }
                        if (activeComment.GetComponent<commentContents>().isVideo)
                        {
                            vCounter += 1;
                        }
                        if (activeComment.GetComponent<commentContents>().isPhoto)
                        {
                            pCounter += 1;
                        }

                    }
                    if (pCounter > 0)
                    {
                        tempString = tempString + "Photo Notes" + "(" + pCounter + ")";
                        if (sCounter > 0)
                        {
                            tempString = tempString + ", ";
                        }
                        if (vCounter > 0)
                        {
                            tempString = tempString + ", ";
                        }
                    }
                    if (sCounter > 0)
                    {
                        tempString = tempString + "Simple Notes" + "(" + sCounter + ")";
                        if (vCounter > 0)
                        {
                            tempString = tempString + ", ";
                        }
                    }
                    if (vCounter > 0)
                    {
                        tempString = tempString + "Video Notes" + "(" + vCounter + ")";
                    }
                    violationData[i].text = tempString;
                }
            }
        }

        public void submitReview(bool fromJson)
        {


            for (int i = 0; i < violationControl.violationData.Count + 1; i++)
            {
                violationSubmittedData[i].text = violationData[i].text;
            }

            violationSubmittedData[8].text = metaManager.Instance.user;
            violationSubmittedData[9].text = metaManager.Instance.date();


            violationControl.goToTab(8);
            violationControl.violationHeader.text = 
                ("Violation " + violationControl.violationIndices[0].ToString() +"."
                + violationControl.violationIndices[1].ToString() +"."
                + violationControl.violationIndices[2].ToString());
            submittedViolationHolder.SetActive(true);
            violationControl.violationData.Add("New");
            violationControl.violationIndices.Add(0);
            submittedViolationHolder.GetComponent<submittedViolationController>().addPreview(0);


            submittedViolationHolder.GetComponent<submittedViolationController>().tempIndex = JU_databaseMan.Instance.violationsManager.violations[0].status;
            submittedViolationHolder.GetComponent<submittedViolationController>().resName = violationsLib.Instance.violationsStatus[JU_databaseMan.Instance.violationsManager.violations[0].status];
            addingViolationHolder.SetActive(false);


            violationControl.closeViolation();
            
            if (!fromJson)
            {
                vioControl.Instance.successContentHolder.SetActive(true);
                vioControl.Instance.successContentHolder.transform.position = this.transform.position;
                databaseMan.Instance.syncViolation(violationControl);

            }
        }




    }
}