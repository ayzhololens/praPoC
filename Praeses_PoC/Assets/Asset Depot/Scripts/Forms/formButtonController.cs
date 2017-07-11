using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class formButtonController : MonoBehaviour {

        public int buttonIndex { get; set; }
        [Tooltip ("Display Name")]
        public Text buttonText;
        public formFieldController field { get; set; }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }


        /// <summary>
        /// Is set when a button is selected
        /// </summary>
        public void setFormButtonValue()
        {
            foreach(GameObject formButton in field.curButtons)
            {
                if (formButton != this.gameObject)
                {
                    if (formButton.GetComponent<gazeLeaveEvent>().enabled == false)
                    {
                        formButton.GetComponent<gazeLeaveEvent>().enabled = true;
                    }
                    formButton.SendMessage("OnFocusExit", SendMessageOptions.DontRequireReceiver);

                }
            }
            //Visual feedback.  Disable highlight because its already selected
            field.gameObject.GetComponent<buttonHightlight>().updateMat();
            GetComponent<gazeLeaveEvent>().enabled = false;

            //send values to the field
            field.Value.text = buttonIndex.ToString();
            field.buttonVal = buttonText.text;
            field.checkDelta();

            //sync with JSON
            databaseMan.Instance.formToClassValueSync(field.trueName, field.Value.text);
        }
    }
}