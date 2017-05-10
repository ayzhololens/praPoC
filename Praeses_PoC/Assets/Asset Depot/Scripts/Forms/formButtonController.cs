using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class formButtonController : MonoBehaviour {
        public int buttonIndex { get; set; }
        public Text buttonText;
        public formFieldController field;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

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
            field.gameObject.GetComponent<buttonHightlight>().updateMat();
            field.Value.text = buttonIndex.ToString();
            field.checkDelta();
            databaseMan.Instance.formToClassValueSync(field.trueName, field.Value.text);
            GetComponent<gazeLeaveEvent>().enabled = false;
        }
    }
}