using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

namespace HoloToolkit.Unity
{

    public class nodeMediaHolder : MonoBehaviour
    {


        public List<string> filepath;
        public string activeFilepath;
        public string fileName;
        MediaPlayer videoPlayer;
        public List<GameObject> activeComments;
        public GameObject playIcon;
        public GameObject pauseIcon;
        bool startedVideo;
        Texture2D photoTexture;
        public GameObject photoVideoPane;
        public bool videoNode;
        public bool photoNode;
        public bool simpleNode;
        public bool fieldNode;
        public bool violationNode;
        public InputField Title;
        public InputField Description;
        public string Date { get; set; }
        public string User { get; set; }
        public string audioPath { get; set; }
        public int type;
        public int NodeIndex;

        // Use this for initialization
        void Start()
        {



        }


        // Update is called once per frame
        void Update()
        {
            if (startedVideo)
            {
                videoChecker();
            }


        }

        //public void loadVideoMedia()
        //{


        //    activeFileName = vidRecorder.filename;
        //    filepath.Add(vidRecorder.filepath);
        //    activeFilepath = vidRecorder.filepath;
        //    VideoPlayer.m_VideoPath = activeFileName;
        //    VideoPlayer.LoadVideoPlayer();
        //}

        //public void loadPhotoMedia()
        //{

        //    if (photoRecorder.targetTexture != null && photoVideoPane != null)
        //    {
        //        filepath.Add(photoRecorder.filePath);
        //        photoTexture = photoRecorder.targetTexture;
        //        photoVideoPane.GetComponent<Renderer>().material.mainTexture = photoTexture;
        //    }
        //}

        public void loadPhoto(string filepath)
        {

            Texture2D targetTexture = new Texture2D(2048, 1152);
            var bytesRead = System.IO.File.ReadAllBytes(filepath);
            targetTexture.LoadImage(bytesRead);
            photoTexture = targetTexture;
            photoVideoPane.GetComponent<Renderer>().material.mainTexture = photoTexture;
        }



        public void LoadVideo()
        {
            if(videoPlayer == null)
            {
                videoPlayer = GameObject.Find("VideoPlayer").GetComponent<MediaPlayer>();
            }
            videoPlayer.m_VideoPath = activeFilepath;
            videoPlayer.LoadVideoPlayer();

        }


        public void PlayVideo()
        {
            if (!startedVideo)
            {
                videoPlayer.Control.Play();
                startedVideo = true;
                playIcon.SetActive(false);
                pauseIcon.SetActive(true);
            }

        }

        public void PauseVideo()
        {
            if (startedVideo)
            {
                videoPlayer.Control.Pause();
                startedVideo = false;
                playIcon.SetActive(true);
                pauseIcon.SetActive(false);
            }
        }

        void videoChecker()
        {
            if (videoPlayer.Control.IsFinished())
            {
                playIcon.SetActive(true);
                pauseIcon.SetActive(false);
                startedVideo = false;
            }
        }

        public void reCapture()
        {
            if (videoNode)
            {
                mediaManager.Instance.currentNode = this.gameObject;
                mediaManager.Instance.enableVideoRecording();
                GetComponent<nodeController>().closeNode();
            }
            if (photoNode)
            {
                mediaManager.Instance.currentNode = this.gameObject;
                mediaManager.Instance.enablePhotoCapture();
                GetComponent<nodeController>().closeNode();
            }
        }


    }
}