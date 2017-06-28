using HoloToolkit;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.InputModule;


namespace HoloToolkit.Unity.InputModule
{



    public class Dictationizer : Singleton<Dictationizer>
    {
        private DictationRecognizer dictationRecognizer;

        
        public StringBuilder textSoFar;
        bool inProgress;


        // Use this for initialization
        void Start()
        {
            inProgress = false;

        }



        
        // Update is called once per frame
        void Update()
        {

        }
       


        /// <summary>
        /// Dispose of the keyword manager resources and set up dictation
        /// Start dictation
        /// </summary>
        public void setUpDictation()
        {
            dictationRecognizer = new DictationRecognizer();
            if (!inProgress)
            {
                PhraseRecognitionSystem.Shutdown();
                dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
                dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
                dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
                dictationRecognizer.DictationError += DictationRecognizer_DictationError;
                dictationRecognizer.Start();
                textSoFar = new StringBuilder();
                keyboardScript.Instance.keyboardField.text = "Speech to text started.  Begin dictating";
                inProgress = true;
            }
            
        }


        /// <summary>
        /// Dictation result after a short pause
        /// </summary>
        /// <param name="text"></param>
        /// <param name="confidence"></param>
        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
        {
            if (keyboardScript.Instance.useKeypad)
            {
                textSoFar.Append(text + ". ");
            }
            else
            {
                textSoFar.Append(text + "");
            }
            keyboardScript.Instance.keyboardField.text = textSoFar.ToString();
            keyboardScript.Instance.keyboardField.caretPosition = keyboardScript.Instance.keyboardField.caretPosition + textSoFar.ToString().Length;


        }


        /// <summary>
        /// Immediate guess as to what was just dictated
        /// </summary>
        /// <param name="text"></param>
        private void DictationRecognizer_DictationHypothesis(string text)
        {
            if (keyboardScript.Instance.useKeypad)
            {
                keyboardScript.Instance.keyboardField.text = textSoFar.ToString() + " " + text + "...";
            }
            else
            {
                keyboardScript.Instance.keyboardField.text = textSoFar.ToString() + "" + text + "";
            }
            keyboardScript.Instance.keyboardField.caretPosition = keyboardScript.Instance.keyboardField.caretPosition + textSoFar.ToString().Length;
        }


        /// <summary>
        /// We use stopDiction() instead of this but this is the out of the box method of stopping dictation
        /// </summary>
        /// <param name="cause"></param>
        public void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
        {
            dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
            dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
            dictationRecognizer.Dispose();
        }

        private void DictationRecognizer_DictationError(string error, int hresult)
        {
            Debug.LogError("Dictation Error");
        }


        /// <summary>
        /// shut down dictation recognizer and free up resources
        /// start the keyword manager
        /// </summary>
        public void stopDiction()
        {
            if (inProgress)
            {
                dictationRecognizer.Stop();
                dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
                dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
                dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
                dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
                dictationRecognizer.Dispose();
                PhraseRecognitionSystem.Restart();

                inProgress = false;
            }



        }




    }
}
