using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class annotationManager : Singleton<annotationManager>
    {

        //public GameObject videoRecordTextIndicator;
        //public GameObject tapToPlaceIndicator;
        //public GameObject videoRecordingInProgressIndicator;
        //public GameObject photoCaptureTextIndicator;
        //public GameObject dictationInProgressIndicator;
        public GameObject stateIndicator;
        public bool videoReordingEnabled;
        public bool videoRecordingInProgress;
        public bool photoCaptureEnabled;
        public bool dictationInProgress;
        public List<GameObject> activeAnnotations;
        public GameObject currentAnnotation;
        public GameObject vidManager;
        public GameObject photoManager;
        videoRecorder vidRecorder;
        photoRecorder photoRecorder;
        annotationSpawner annotSpawner;
        public GameObject activeDictationBox;
        public GameObject activeField;
        public int nodeIndex;

        public bool annotating;

        // Use this for initialization
        void Start()
        {
            vidRecorder = vidManager.GetComponent<videoRecorder>();
            photoRecorder = photoManager.GetComponent<photoRecorder>();
            annotSpawner = GetComponent<annotationSpawner>();

        }

        // Update is called once per frame
        void Update()
        {

            if (sourceManager.Instance.sourcePressed && videoReordingEnabled)
            {
                StartVideoRecording();
            }
            if (sourceManager.Instance.sourcePressed && videoRecordingInProgress)
            {
                StopVideoRecording();
            }
            if(sourceManager.Instance.sourcePressed && photoCaptureEnabled)
            {
                CapturePhoto();
            }
            if(sourceManager.Instance.sourcePressed && dictationInProgress)
            {
                StopDictation();
            }


        }

        public void activateMedia()
        {

            if (currentAnnotation.GetComponent<nodeMediaHolder>().photoNode)
            {
                currentAnnotation.GetComponent<selectEvent>().enabled = true;

                //currentAnnotation.GetComponent<nodeMediaHolder>().loadPhotoMedia();
                currentAnnotation.GetComponent<openAnnotationNode>().openContent();
                currentAnnotation.GetComponent<openAnnotationNode>().enableReview();
            }

            if (currentAnnotation.GetComponent<nodeMediaHolder>().videoNode)
            {
                currentAnnotation.GetComponent<selectEvent>().enabled = true;

                //currentAnnotation.GetComponent<nodeMediaHolder>().loadVideoMedia();
                currentAnnotation.GetComponent<openAnnotationNode>().openContent();
                currentAnnotation.GetComponent<openAnnotationNode>().enableReview();
            }

            if (currentAnnotation.GetComponent<nodeMediaHolder>().simpleNode)
            {
                currentAnnotation.GetComponent<selectEvent>().enabled = true;
                
                currentAnnotation.GetComponent<openAnnotationNode>().openContent();
                currentAnnotation.GetComponent<openAnnotationNode>().enableReview();
            }
            if (currentAnnotation.GetComponent<nodeMediaHolder>().violationNode)
            {


                if (currentAnnotation.GetComponent<selectEvent>().enabled == false)
                {

                    currentAnnotation.GetComponent<selectEvent>().enabled = true;
                }

                if (activeField != null)
                {
                    //if (activeField.GetComponent<violationController>().capturingPhoto)
                    //{
                    //    activeField.GetComponent<violationController>().loadPhotoMedia();
                    //}
                    //if (activeField.GetComponent<violationController>().capturingVideo)
                    //{
                    //    activeField.GetComponent<violationController>().loadVideoMedia();
                    //}
                }
            }
            if (activeField != null)
            {
                if (currentAnnotation.GetComponent<nodeMediaHolder>().fieldNode)
                {
                    //if (activeField.GetComponent<formFieldController>().capturingPhoto)
                    //{
                    //    //activeField.GetComponent<formFieldController>().loadPhotoMedia();
                    //}
                    //if (activeField.GetComponent<formFieldController>().capturingVideo)
                    //{
                    //    //activeField.GetComponent<formFieldController>().loadVideoMedia();
                    //}
                }



                    //currentAnnotation.GetComponent<nodeMediaHolder>().Title = activeField.GetComponent<formFieldController>().DisplayName.text;
                }

            stateIndicator.SetActive(false);
            currentAnnotation.GetComponent<nodeMediaHolder>().User = metaManager.Instance.user;
            currentAnnotation.GetComponent<nodeMediaHolder>().Date = System.DateTime.Now.ToString();





            //annotSpawner.spawnedAnnotation.GetComponent<selectEvent>().enabled = true;

            //annotSpawner.spawnedAnnotation.GetComponent<annotationMediaHolder>().loadMedia();
            //annotSpawner.spawnedAnnotation.GetComponent<openAnnotationNode>().openContent();
            //annotSpawner.spawnedAnnotation.GetComponent<openAnnotationNode>().enableReview();
        }


        public void enableVideoRecording()
        {
            annotating = true;
            if (videoReordingEnabled == false)
            {
                stateIndicator.SetActive(true);
                videoReordingEnabled = true;
                sourceManager.Instance.sourcePressed = false;
                stateIndicator.GetComponent<TextMesh>().text = "Tap to start recording video.";
            }

            Debug.Log("enabled");

        }

        public void StartVideoRecording()
        {
            stateIndicator.GetComponent<TextMesh>().text = "Recoding in progress.  Tap to stop";

            vidRecorder.startRecordingVideo();
            videoRecordingInProgress = true;
            videoReordingEnabled = false;
            Debug.Log("started");

        }

        public void StopVideoRecording()
        {
            stateIndicator.SetActive(false);
            vidRecorder.StopRecordingVideo(true);
            videoRecordingInProgress = false;
            annotating = false;
            Debug.Log("stopped");

        }


        public void enableSimpleCapture()
        {
            annotating = true;

            Debug.Log("enabled");
        }

        public void enablePhotoCapture()
        {
            annotating = true;
            if (photoCaptureEnabled == false)
            {
                photoCaptureEnabled = true;
                stateIndicator.SetActive(true);
                stateIndicator.GetComponent<TextMesh>().text = "Tap to start capture photo.";
                sourceManager.Instance.sourcePressed = false;
            }
        }

        

        public void CapturePhoto()
        {
            if (photoCaptureEnabled)
            {
                photoRecorder.CapturePhoto();
                photoCaptureEnabled = false;
                stateIndicator.SetActive(false);
                //activateMedia();
                annotating = false;
            }
        }

        public void FinishFakeCapture()
        {
            photoCaptureEnabled = false;
            stateIndicator.SetActive(false);
            annotating = false;
            videoRecordingInProgress = false;

        }

        public void StartDictation()
        {
            annotating = true;
            dictationInProgress = true;
            stateIndicator.SetActive(true);
            stateIndicator.GetComponent<TextMesh>().text = "Dictation in progress.  Tap to stop";
        }

        public void StopDictation()
        {
            annotating = false;
            dictationInProgress = false;
            stateIndicator.SetActive(false);
            //activeDictationBox.GetComponent<Dictationizer>().stopDiction();
        }




    }
}
