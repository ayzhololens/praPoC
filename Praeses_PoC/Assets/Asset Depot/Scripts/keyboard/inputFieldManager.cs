﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class inputFieldManager : MonoBehaviour, IInputClickHandler
    {

        public InputField mainInputField;

        bool engaged;
        public bool panelResize;
        public int recCharSize;
        public GameObject charPanel;
        public float scaleMult;
        public bool useKeypad;
        public bool useNumpad;
        
        private void Update()
        {
            if (engaged && sourceManager.Instance.sourcePressed)
            {
                if (GazeManager.Instance.HitObject != null)
                {
                    if ( GazeManager.Instance.HitObject.tag != "inputField" && GazeManager.Instance.HitObject.tag != "keyboard" && GazeManager.Instance.HitObject.tag != "keyboardBG")
                    {
                        keyboardScript.Instance.turnOff();
                        deactivateField();
                    }
                }else
                {
                    keyboardScript.Instance.turnOff();
                    deactivateField();
                }
            }
        }

        private void Start()
        {
            engaged = false;
            if (panelResize)
            {
                float offset = recCharSize * scaleMult;
                Vector3 panelResizeScale = new Vector3(charPanel.transform.localScale.x+offset, charPanel.transform.localScale.y, charPanel.transform.localScale.z);
                charPanel.transform.localScale = panelResizeScale;
                
            }

        }

        public void activateField()
        {
            mainInputField.ActivateInputField();
            if (useKeypad)
            {
                Invoke("turnOnKeyboard", .1f);
            }
            if (useNumpad)
            {
                Invoke("turnOnNumpad", .1f);
            }
            
            engaged = true;
        }




        void turnOnKeyboard()
        {
            keyboardScript.Instance.currentField = mainInputField;
            keyboardScript.Instance.useKeypad = true;
            keyboardScript.Instance.keyboardToggle();
        }

        void turnOnNumpad()
        {
            keyboardScript.Instance.currentField = mainInputField;
            keyboardScript.Instance.useNumpad = true;
            keyboardScript.Instance.keyboardToggle();

        }

        public void deactivateField()
        {
            
            mainInputField.DeactivateInputField();
            keyboardScript.Instance.currentField = null;
            keyboardScript.Instance.turnOff();
            engaged = false;
        }

        public void toggleField()
        {
            Debug.Log("toggled");
            if (engaged)
            {
                deactivateField();
                
            }
            else
            {
                activateField();
            }
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            //toggleField();
        }

        public void onEditChangeUpdateJSon()
        {
            print("dip");
            string keyword;
            keyword = transform.parent.gameObject.GetComponent<formFieldController>().trueName;
            databaseMan.Instance.formToClassValueSync(keyword, mainInputField.text);
            print("sup");
        }
    }
}
