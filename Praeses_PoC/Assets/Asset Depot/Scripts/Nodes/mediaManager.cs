using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class mediaManager : Singleton<mediaManager>
    {
        //video 
        public videoRecorder vidRecorder;
        public MediaPlayer videoPlayer;
        bool recordingEnabled;
        bool recordingInProgress;

        //photo
        public photoRecorder photoRecorder;
        bool photoCaptureEnabled;


        public GameObject stateIndicator;
        public GameObject recordingIndicator;
        public GameObject currentNode { get; set; }
        public GameObject activeField { get; set; }
        public List<GameObject> activeNodes;
        public bool isCapturing { get; set; }
        public int nodeIndex { get; set; }
        public commentManager commentManager { get; set; }

        int nodeInd;



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (isCapturing)
            {
                stopCapturing();
            }

        }

        public void activateMedia()
        {
            
            nodeMediaHolder nodeMedia = currentNode.GetComponent<nodeMediaHolder>();

            if (nodeMedia.photoNode)
            {
                //send photo file path to be stored and loaded on node
                nodeMedia.activeFilepath = photoRecorder.filePath;
                nodeMedia.fileName = photoRecorder.filename;
                nodeMedia.loadPhoto(photoRecorder.filePath);
                fovHider.Instance.toggleFOVHider(false);

            }
            if (nodeMedia.videoNode)
            {
                //send video name to node and load it
                //using filename instead of path because the media player is set to persistent data path
                nodeMedia.activeFilepath = vidRecorder.filename;
                nodeMedia.fileName = vidRecorder.filename;
                nodeMedia.LoadVideo();

            }
            
            //reenable selection so it's clickable
            currentNode.GetComponent<BoxCollider>().enabled = true;

            //set user and date
            nodeMedia.User = metaManager.Instance.user;
            nodeMedia.Date = metaManager.Instance.date();
            currentNode.GetComponent<nodeController>().setUpNode();

            print(currentNode);
            databaseMan.Instance.addAnnotation(currentNode);

        }

        public void activateComment()
        {
            GameObject spawnedComment = new GameObject(); ;
            int nodeInd =0;
            commentContents com = new commentContents();
            databaseMan.tempComment commentClass = new databaseMan.tempComment();

            if (commentManager.capturingPhoto)
            {
                spawnedComment = commentManager.spawnPhotoComment();
                com = spawnedComment.GetComponent<commentContents>();
                spawnedComment.GetComponent<commentContents>().fileName = photoRecorder.filename;
                commentClass.type = 2;

            }
            else if (commentManager.capturingVideo)
            {

                spawnedComment = commentManager.spawnVideoComment();
                com = spawnedComment.GetComponent<commentContents>();
                spawnedComment.GetComponent<commentContents>().fileName = vidRecorder.filename;

                commentClass.type = 3;
            }

            if (com.linkedComponent.GetComponent<violationController>() != null)
            {
                nodeInd = com.linkedComponent.GetComponent<violationController>().linkedNode.GetComponent<nodeMediaHolder>().NodeIndex;
            }
            if (com.linkedComponent.GetComponent<formFieldController>() != null)
            {
                nodeInd = com.linkedComponent.GetComponent<formFieldController>().linkedNode.GetComponent<nodeMediaHolder>().NodeIndex;
            }

            commentClass.user = com.user;
            commentClass.date = com.Date;
            commentClass.path = com.filepath;

            databaseMan.Instance.commentToClassValueSync(nodeInd, commentClass);
        }
        
        public void enablePhotoCapture()
        {
            isCapturing = true;
            photoCaptureEnabled = true;
            setStatusIndicator("Tap to capture photo");
            recordingIndicator.SetActive(true);

            //clear source manager
            sourceManager.Instance.sourcePressed = false;

            fovHider.Instance.toggleFOVHider(true);

        }
        
        void capturePhoto()
        {
            photoCaptureEnabled = false;
            isCapturing = false;

            //capture photo, save it, activeMedia() when done
            photoRecorder.activateMedia = true;
            photoRecorder.CapturePhoto();
            recordingIndicator.SetActive(false);
            setStatusIndicator("Photo capture complete!");
            invokeStatusDisable(2.0f);

            audioManager.Instance.setAndPlayAudio(1);
        } 

        public void enableVideoRecording()
        {
            isCapturing = true;
            setStatusIndicator("Tap to start recording video");
            recordingEnabled = true;

            //clear source manager
            sourceManager.Instance.sourcePressed = false;

            fovHider.Instance.toggleFOVHider(true);
        }

        void startVideoRecording()
        {
            vidRecorder.startRecordingVideo();
            recordingEnabled = false;
            recordingInProgress = true;
            setStatusIndicator("Recording in progress. Tap to stop");
            recordingIndicator.SetActive(true);

            //clear source manager
            sourceManager.Instance.sourcePressed = false;
        }

        void stopVideoRecording()
        {   
            //stop recording, finish encoding then calling activateMedia() when done
            vidRecorder.StopRecordingVideo(true);
            setStatusIndicator("Video capture complete!");
            invokeStatusDisable(2.0f);
            audioManager.Instance.setAndPlayAudio(1);
            recordingIndicator.SetActive(false);
            recordingInProgress = false;
            isCapturing = false;
            fovHider.Instance.toggleFOVHider(false);
        }

        public void setStatusIndicator(string curStatus)
        {
            if (!stateIndicator.activeSelf)
            {
                stateIndicator.SetActive(true);
            }
            stateIndicator.GetComponent<TextMesh>().text = curStatus;
        }

        public void disableStatusIndicator()
        {
            if (stateIndicator.activeSelf)
            {
                stateIndicator.SetActive(false);
            }
            stateIndicator.GetComponent<TextMesh>().text = null;
        }

        public void invokeStatusDisable (float delay)
        {
            Invoke("disableStatusIndicator", delay);
        }

        void stopCapturing()
        {
            if (sourceManager.Instance.sourcePressed)
            {
                if (recordingEnabled)
                {
                    startVideoRecording();
                }
                if (recordingInProgress)
                {
                    stopVideoRecording();
                }
                if (photoCaptureEnabled)
                {
                    capturePhoto();
                }

            }

        }
    }
}