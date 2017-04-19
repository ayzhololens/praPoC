using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

//#if WINDOWS_UWP
//using WindowsInput;
//#endif

namespace HoloToolkit.Unity
{
    public class keyboardScript : Singleton<keyboardScript>
    {

        public GameObject keyboardActivator;
        public bool onOff;
        public InputField currentField;
        public InputField keyboardField;
        public TextMesh previousValue;
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

        public Text actualText;
        public int textLength;

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

        public void editChangeSync()
        {
            if (currentField.GetComponent<inputFieldManager>().commentNode != null)
            {
                currentField.gameObject.GetComponent<inputFieldManager>().onEditChangeAddComment(currentField.gameObject.transform.parent.gameObject.GetComponent<commentContents>());
            }else
            {
                currentField.gameObject.GetComponent<inputFieldManager>().onEditChangeUpdateJSon();
            }

            print("done is pressed");
        }

        void textSync()
        {
            if (keyboardField.text.Length > textLength)
            {
                //print(keyboardField.text.Length + " is bigger than " + textLength);
                actualText.text = keyboardField.text;
                actualText.text = keyboardField.text.Remove(0, keyboardField.text.Length - textLength);
            }else
            {
                actualText.text = keyboardField.text;
            }
        }

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


            //canvasOriPos = canvasObj.GetComponent<RectTransform>().position;
            //canvasOffset = canvasOriPos + new Vector3(0, .2f, 0);

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
        

        public void adjustCaret()
        {
            keyboardField.caretPosition = currentField.text.Length;
        }

        void cameraParent()
        {
            Quaternion oldRot = transform.rotation;
            transform.SetParent(Camera.main.transform);
            transform.localPosition = new Vector3(0, 0, 1);
            transform.rotation = new Quaternion(oldRot.x, 0, oldRot.z, oldRot.w);
            transform.SetParent(null);
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
            //canvasObj.transform.position = Vector3.MoveTowards(canvasOriPos,canvasOffset, animMult);
        }

        void cleartext()
        {
            keyboardField.text = "";
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

        public void typeObject()
        {
            keyboardField.text = keyboardField.text.Insert(keyboardField.caretPosition, processUnderScore(GazeManager.Instance.HitObject.name));
            keyboardField.caretPosition++;
        }

        public void typeSuggestion()
        {
            cleartext();
            keyboardField.text = keyboardField.text.Insert(keyboardField.caretPosition, processParentheses(previousValue.text));
            keyboardField.caretPosition = keyboardField.caretPosition + processParentheses(previousValue.text).Length;
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
            }else
            {
                capsLock = false;
            }
        }
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

        public void caretBack()
        {
            keyboardField.caretPosition--;
        }

        public void caretForward()
        {
            keyboardField.caretPosition++;
        }



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

        public void startRecording()
        {
            keypad.SetActive(false);
            micOn.SetActive(true);
            isRecording = true;
        }

        public void finishRecording()
        {
            keypad.SetActive(true);
            micOn.SetActive(false);
            isRecording = false;
        }
    }

}

