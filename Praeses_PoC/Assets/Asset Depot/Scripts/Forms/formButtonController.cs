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

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void setFormButtonValue()
        {
            foreach(GameObject formButton in transform.parent.gameObject.GetComponent<formFieldController>().curButtons)
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
            transform.parent.gameObject.GetComponent<formFieldController>().Value.text = buttonIndex.ToString();
            databaseMan.Instance.formToClassValueSync(transform.parent.gameObject.GetComponent<formFieldController>().trueName, transform.parent.gameObject.GetComponent<formFieldController>().Value.text);
            GetComponent<gazeLeaveEvent>().enabled = false;
        }
    }
}