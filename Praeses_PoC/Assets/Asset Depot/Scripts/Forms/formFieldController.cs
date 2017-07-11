using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using RenderHeads.Media.AVProVideo;

namespace HoloToolkit.Unity
{
    public class formFieldController : MonoBehaviour
    {

        //linked node that is created when leaving a comment
        public GameObject linkedNode { get; set; }

        [Header ("Value Components")]
        [Tooltip ("Text for displayed name")]
        public Text DisplayName;
        [Tooltip ("Input field that holds the values")]
        public InputField Value;
        [Tooltip ("Text for previous value")]
        public Text previousValue;
        [Tooltip("Controls whether or not to update the amount of fields completed")]
        public bool showUpdate;

        //hidden value components
        public string trueName { get; set; }
        public string buttonVal { get; set; }
        public int nodeIndex { get; set; }

        [Header("Field Button Control")]
        [Tooltip ("Prefab for the field button")]
        public GameObject fieldButton;
        [Tooltip ("Position for the buttons to spawn")]
        public Transform buttonPos;
        [Tooltip ("X Distance between buttons")]
        public float buttonXOffset;
        [Tooltip ("Parent for the field buttons")]
        public Transform buttonParent;
        [Tooltip("Spawned Buttons")]
        public List<GameObject> curButtons;
        int currCommentType;

        //Check to see if values are changed
        public bool ignoreDeltaCheck { get; set; }

        //Changed value field that is spawned either via formButtonController or InputFieldManager calling the checkDelta() function
        public  GameObject deltaField { get; set; }



        /// <summary>
        /// Check to see if theres an assoicated node, if not spawn one.
        /// </summary>
        /// <param name="commentType"></param>
        public void spawnNode(int commentType)
        {
            currCommentType = commentType;
            if (linkedNode == null)
            {

                nodeSpawner.Instance.spawnNode(4);
                nodeSpawner.Instance.getLinkedField(gameObject.GetComponent<formFieldController>());
            }
            else
            {
                enableAttachmentCapture();
            }



        }
        


        /// <summary>
        /// After node is spawned, take the appropriate comment action
        /// </summary>
        public void enableAttachmentCapture()
        {

            if (linkedNode.GetComponent<selectEvent>().enabled == false)
            {
                linkedNode.GetComponent<selectEvent>().enabled = true;
            }

            if(currCommentType == 1)
            {
                GetComponent<commentManager>().spawnSimpleComment();
            }
            if (currCommentType == 2)
            {
                GetComponent<commentManager>().enablePhotoCapture();
            }
            if (currCommentType == 3)
            {
                GetComponent<commentManager>().enableVideoCapture();
            }

            currCommentType = 0;
        }

        /// <summary>
        /// Spawns and positions field buttons
        /// </summary>
        /// <param name="buttonAmount"></param>
        public void populateButtons(int buttonAmount)
        {
            Vector3 buttonLoc = buttonPos.position;
            for (int i = 0; i<buttonAmount; i++)
            {

                curButtons.Add( Instantiate(fieldButton, buttonLoc, Quaternion.identity));
                curButtons[i].GetComponent<formButtonController>().field = this;
                curButtons[i].transform.SetParent(buttonParent);
                curButtons[i].transform.localScale = fieldButton.transform.localScale;
                curButtons[i].transform.localRotation = fieldButton.transform.localRotation;

                buttonLoc = new Vector3(buttonLoc.x + buttonXOffset, buttonLoc.y, buttonLoc.z) ;

            }
        }


        /// <summary>
        /// Updates the amount of fields completed.  This is called on the value inputfield OnValueChanged() Event
        /// </summary>
        public void setStatus()
        {
            
            if (showUpdate)
            {
                formController.Instance.updateFieldStatus(1);
                showUpdate = false;
            }
        }


        /// <summary>
        /// Compares previous value to current value and spawned a changed value field in the review inspection section.  Called either in inputFieldManager or formButtonController
        /// </summary>
        public void checkDelta()
        {
            if (!ignoreDeltaCheck)
            {
                if (deltaField == null)
                {
                    string tempVal = "(" + Value.text + ")";

                    if (tempVal != previousValue.text)
                    {
                        if (buttonVal == null)
                        {
                            formController.Instance.submitInspection.addChangedValue(DisplayName.text, previousValue.text, Value.text, this);

                        }
                        else
                        {
                            formController.Instance.submitInspection.addChangedValue(DisplayName.text, previousValue.text, buttonVal, this);
                        }
                    }
                }
                else
                {
                    if (buttonVal == null)
                    {
                        deltaField.GetComponent<fieldChangedValueComponent>().currValue.text = Value.text;
                    }
                    else
                    {
                        deltaField.GetComponent<fieldChangedValueComponent>().currValue.text = buttonVal;
                    }
                }
            }

          


            

        }

    }
}
