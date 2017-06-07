using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class keyboardScript : Singleton<keyboardScript>
    {
        [Tooltip("gameobject to hide or unhide when keyboard turns on")]
        public GameObject keyboardActivator;

        public bool onOff { get; set; }
        public InputField currentField { get; set; }

        [Tooltip("Keyboard Input field")]
        public InputField keyboardField;

        [Tooltip("this gets filled in depending on deltas")]
        public TextMesh previousValue;

        [Header("CapslockItems")]
        public GameObject numbers;
        public GameObject symbols;
        bool symbolsOn;
        public GameObject lower;
        public GameObject upper;
        bool shift;
        bool capsLock;

        public float doubleClickSpeed;
        float initDoubleClick;
        public GameObject LcapsLockBG;
        public GameObject RcapsLockBG;

        [Header("Text Scrolling Items")]
        public Text actualText;
        [Tooltip("limit how long the characters can go on the field before it starts scrolling to the right")]
        public int textLength;

        [Header("QWERTY Numpad Switcher")]
        public GameObject keypad;
        public GameObject numpad;
        public GameObject micOn;
        bool isRecording;

        public bool useKeypad;
        public bool useNumpad;
        public bool useEnum;


        private void Start()
        {
            initDoubleClick = doubleClickSpeed;
        }

        private void FixedUpdate()
        {
            doubleClick();
            textSync();
            if (currentField)
            {
                currentField.text = keyboardField.text;
            }

        }

        //This is called on update so the double click registers for capslock
        void doubleClick()
        {
            if (shift)
            { 
            doubleClickSpeed -= Time.deltaTime;
            }
            else
            {
                doubleClickSpeed = initDoubleClick;
            }

        }

        //this is called whenever a comment is created/added or when a form is edited
        public void editChangeSync()
        {
            if (currentField.GetComponent<inputFieldManager>().commentNode != null)
            {
                currentField.gameObject.GetComponent<inputFieldManager>().onEditChangeAddComment(currentField.gameObject.transform.parent.parent.gameObject.GetComponent<commentContents>());

            }else
            {
                currentField.gameObject.GetComponent<inputFieldManager>().onEditChangeUpdateJSon();
            }
        }

        //this is so the field and carat can scroll to the right if the number of characters hits the limit
        void textSync()
        {
            if (keyboardField.text.Length > textLength)
            {
                actualText.text = keyboardField.text;
                actualText.text = keyboardField.text.Remove(0, keyboardField.text.Length - textLength);
            }else
            {
                actualText.text = keyboardField.text;
            }
        }

        //on off state
        #region
        public void turnOn()
        {
            getText();
            nestedOn();
            Invoke("adjustCaret",.1f);
        }

        void getText()
        {
            keyboardField.text = currentField.text;
        }

        void nestedOn()
        {
            if (useKeypad)
            {
                keypad.SetActive(true);
                numpad.SetActive(false);
            }
            else if (useNumpad)
            {
                keypad.SetActive(false);
                numpad.SetActive(true);
            }
            keyboardActivator.SetActive(true);

            keyboardField.ActivateInputField();
            onOff = true;
            cameraParent();
            
            //symbols=============================================================
            numbers.SetActive(true);
            symbols.SetActive(false);
            symbolsOn = false;
            //======================================================================
            //shift================================================================
            lower.SetActive(true);
            upper.SetActive(false);
            shift = false;
            //====================================================================
        }

        public void turnOff()
        {
            keyboardActivator.SetActive(false);
            keyboardField.DeactivateInputField();
            onOff = false;
            currentField = null;
            keyboardField.text = "";
            useNumpad = false;
            useKeypad = false;
        }

        public void keyboardToggle()
        {
            if (onOff)
            {
                turnOff();

            }
            else
            {
                turnOn();

            }
        }

        #endregion

        //this is for positioning the keyboard when turned on
        void cameraParent()
        {
            Quaternion oldRot = transform.rotation;
            transform.SetParent(Camera.main.transform);
            transform.localPosition = new Vector3(0, 0, 1);
            transform.rotation = new Quaternion(oldRot.x, 0, oldRot.z, oldRot.w);
            transform.SetParent(null);
        }

        //typing and clearing fields
        #region
        void cleartext()
        {
            keyboardField.text = "";
        }

        //will type a character based on the processed gameobject.name hit by gaze
        public void typeObject()
        {
            keyboardField.text = keyboardField.text.Insert(keyboardField.caretPosition, processUnderScore(GazeManager.Instance.HitObject.name));
            keyboardField.caretPosition++;
        }

        //types the values of the previous value 
        public void typeSuggestion()
        {
            cleartext();
            keyboardField.text = keyboardField.text.Insert(keyboardField.caretPosition, processParentheses(previousValue.text));
            keyboardField.caretPosition = keyboardField.caretPosition + processParentheses(previousValue.text).Length;
        }

        //processes the naming convention key_# to configure what character is going to be typed
        public virtual string processUnderScore(string inputString)
        {
            string outputString;
            outputString = inputString.Split('_')[1];
            if (inputString.Substring(inputString.Length - 1, 1) == "_")
            {
                outputString = "_";
            }
            return outputString;
        }

        //processes historical value without the parentheses
        public virtual string processParentheses(string inputString)
        {
            string outputString;
            outputString = inputString.Split('(')[1];
            if (inputString.Substring(inputString.Length - 1, 1) == "_")
            {
                outputString = "_";
            }

            outputString = outputString.Split(')')[0];
            if (inputString.Substring(inputString.Length - 1, 1) == "_")
            {
                outputString = "_";
            }
            return outputString;
        }

        public void deleteField()
        {
            string tempText = "";
            bool notEnd = false;

            if (keyboardField.caretPosition < keyboardField.text.Length)
            {
                notEnd = true;
            }

            if (keyboardField.caretPosition > 0)
            {
                tempText = keyboardField.text;
                tempText = tempText.Remove(keyboardField.caretPosition - 1, 1);
                keyboardField.text = tempText;
                if (notEnd)
                {
                    keyboardField.caretPosition--;
                }
            }
            textSync();
        }

        public void clearAllField()
        {
            keyboardField.text = "";
            textSync();
        }

        #endregion

        //carat states
        #region
        public void caretBack()
        {
            keyboardField.caretPosition--;
        }

        public void caretForward()
        {
            keyboardField.caretPosition++;
        }

        public void adjustCaret()
        {
            keyboardField.caretPosition = currentField.text.Length;
        }
        #endregion

        //shift and capslock
        #region
        public void symbolsToggle()
        {
            if (symbolsOn)
            {
                numbers.SetActive(true);
                symbols.SetActive(false);
                symbolsOn = false;
            }
            else
            {
                numbers.SetActive(false);
                symbols.SetActive(true);
                symbolsOn =true;
            }
        }

        void capsLockOnFunc()
        {
            if (!capsLock)
            {
                LcapsLockBG.SetActive(true);
                RcapsLockBG.SetActive(true);
                capsLock = true;
            }
        }

        void capsLockOffFunc()
        {
            if (capsLock)
            {
                LcapsLockBG.SetActive(false);
                RcapsLockBG.SetActive(false);
                capsLock = false;
            }
        }

        public void shiftToggle()
        {
            if (shift)
            {
                if (doubleClickSpeed < initDoubleClick && doubleClickSpeed > 0)
                {
                    capsLockOnFunc();
                } else {
                    capsLockOffFunc();
                    lower.SetActive(true);
                    upper.SetActive(false);
                    shift = false;
                }
            }
            else
            {
                lower.SetActive(false);
                upper.SetActive(true);
                shift = true;
            }
        }

        public void typeCapitalCheck()
        {
            typeObject();
            if (!capsLock)
            {
                shiftToggle();
            }
        }

        public void capsToggle()
        {
            if (!capsLock)
            {
                capsLock = true;
            }
            else
            {
                capsLock = false;
            }
        }
        #endregion

        //dictation
        #region
        public void startRecording()
        {
            numpad.SetActive(false);
            keypad.SetActive(false);
            micOn.SetActive(true);
            isRecording = true;
        }

        public void finishRecording()
        {
            if (useKeypad)
            {
                keypad.SetActive(true);
            }
            else if (useNumpad)
            {
                numpad.SetActive(true);
            }
            micOn.SetActive(false);
            isRecording = false;
        }
        #endregion
    }

}

