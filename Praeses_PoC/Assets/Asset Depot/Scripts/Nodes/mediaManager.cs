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
        [Header("Media Capture Components")]

        //video 
        [Tooltip("Records Video")]
        public videoRecorder vidRecorder;
        [Tooltip("Main Video Player that handles playback of user captured content")]
        public MediaPlayer videoPlayer;
        bool recordingEnabled;
        bool recordingInProgress;

        //photo
        [Tooltip("Captures Photos")]
        public photoRecorder photoRecorder;
        bool photoCaptureEnabled;

        [Tooltip("Text display for current status")]
        public GameObject stateIndicator;
        [Tooltip("Displays during photo and video capture")]
        public GameObject recordingIndicator;

        //The current node thats being set up
        public GameObject currentNode { get; set; }

        //If setting up a field node, store the field item here
        public GameObject activeField { get; set; }

        //List of nodes currently in scene
        public List<GameObject> activeNodes;

        public bool isCapturing { get; set; }

        //current comment manager 
        public commentManager commentManager { get; set; }

        int nodeInd;



        // Use this for initialization
        void Start()
        {

        }

        // If capturing check for when to complete capture
        void Update()
        {
            if (isCapturing)
            {
                captureControl();
            }

        }


        //This passes the captured media along to the nodes
        public void activateMedia()
        {
            
            nodeMediaHolder nodeMedia = currentNode.GetComponent<nodeMediaHolder>();

            if (nodeMedia.photoNode)
            {
                //send photo file path to be stored and loaded on node
                nodeMedia.activeFilepath = photoRecorder.filePath;
                nodeMedia.fileName = photoRecorder.filename;
                nodeMedia.loadPhoto(photoRecorder.filePath);
                fovHider.Instance.toggleFOVHider(false, 1);

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


            databaseMan.Instance.addAnnotation(currentNode);

        }


        //Sets up the comment so its stored to JSON
        public void activateComment()
        {
            GameObject spawnedComment = new GameObject(); ;
            commentContents com = new commentContents();
            databaseMan.tempComment commentClass = new databaseMan.tempComment();

            //check the current comment manager for what type its capturing
            if (commentManager.capturingPhoto)
            {
                
                spawnedComment = commentManager.spawnPhotoComment();
                com = spawnedComment.GetComponent<commentContents>();
                spawnedComment.GetComponent<commentContents>().fileName = photoRecorder.filename;
                
                //define the comment type and path
                commentClass.type = 2;
                commentClass.path = com.fileName;

            }

            //check the current comment manager for what type its capturing
            else if (commentManager.capturingVideo)
            {

                spawnedComment = commentManager.spawnVideoComment();
                com = spawnedComment.GetComponent<commentContents>();
                spawnedComment.GetComponent<commentContents>().fileName = vidRecorder.filename;

                //define the comment type and path
                commentClass.type = 3;
                commentClass.path = com.filepath;
            }


            //temp int for the node index that will get stored in JSON
            int nodeInd = 0;
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

            //pass then index and class into JSON
            databaseMan.Instance.commentToClassValueSync(nodeInd, commentClass);
        }
        

        public void enablePhotoCapture()
        {
            //set capture states
            isCapturing = true;
            photoCaptureEnabled = true;

            //capture feedback
            setStatusIndicator("Tap to capture photo");
            recordingIndicator.SetActive(true);

            //clear source manager
            sourceManager.Instance.sourcePressed = false;

            //toggle box to hide holograms from user's view
            //holograms are never captured to media this is purely for the in device user experience
            fovHider.Instance.toggleFOVHider(true, 0);

        }
        
        void capturePhoto()
        {
            //clear capture states
            photoCaptureEnabled = false;
            isCapturing = false;

            //capture photo, save it, activeMedia() when done
            photoRecorder.activateMedia = true;
            photoRecorder.CapturePhoto();

            //give capture sucess feedback
            recordingIndicator.SetActive(false);
            setStatusIndicator("Photo capture complete!");
            invokeStatusDisable(2.0f);
            audioManager.Instance.setAndPlayAudio(1);
        } 

        public void enableVideoRecording()
        {
            //set that you are about to capture
            isCapturing = true;
            recordingEnabled = true;

            //set state feedback
            setStatusIndicator("Tap to start recording video");

            //clear source manager
            sourceManager.Instance.sourcePressed = false;

            //toggle box to hide holograms from user's view
            //holograms are never captured to media this is purely for the in device user experience
            fovHider.Instance.toggleFOVHider(true, 0);
        }

        void startVideoRecording()
        {
            //capture states
            recordingEnabled = false;
            recordingInProgress = true;

            //start video capture
            vidRecorder.startRecordingVideo();

            //feedback
            setStatusIndicator("Recording in progress. Tap to stop");
            recordingIndicator.SetActive(true);

            //clear source manager
            sourceManager.Instance.sourcePressed = false;
        }

        void stopVideoRecording()
        {   
            //stop recording, finish encoding then calling activateMedia() when done
            vidRecorder.StopRecordingVideo(true);

            //capture states
            recordingInProgress = false;
            isCapturing = false;

            //feedback
            setStatusIndicator("Video capture complete!");
            invokeStatusDisable(2.0f);
            audioManager.Instance.setAndPlayAudio(1);
            recordingIndicator.SetActive(false);
            fovHider.Instance.toggleFOVHider(false, 1);
        }

        //controls status indicator which is under the hololens camera
        public void setStatusIndicator(string curStatus)
        {
            if (!stateIndicator.activeSelf)
            {
                stateIndicator.SetActive(true);
            }
            stateIndicator.GetComponent<TextMesh>().text = curStatus;
        }

        //immediately disable and clear the status indicator
        public void disableStatusIndicator()
        {
            if (stateIndicator.activeSelf)
            {
                stateIndicator.SetActive(false);
            }
            stateIndicator.GetComponent<TextMesh>().text = null;
        }

        //turns off status indicator after a delay
        public void invokeStatusDisable (float delay)
        {
            Invoke("disableStatusIndicator", delay);
        }

        //update checks this during capture modes
        void captureControl()
        {
            //decide what to do on a tap
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