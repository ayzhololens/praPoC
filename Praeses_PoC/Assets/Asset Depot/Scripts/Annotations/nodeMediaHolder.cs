using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

namespace HoloToolkit.Unity
{

    public class nodeMediaHolder : MonoBehaviour
    {

        [Header("Node Identifier")]
        public bool videoNode;
        public bool photoNode;
        public bool simpleNode;
        public bool fieldNode;
        public bool violationNode;

        [Header("JSON Linked Components")]
        public List<GameObject> activeComments;
        public InputField Title;
        public InputField Description;
        public string activeFilepath { get; set; }
        public string fileName { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public string audioPath { get; set; }
        public int type { get; set; }
        public int NodeIndex { get; set; }

        [Header("Photo and Video Components")]
        public GameObject playIcon;
        public GameObject pauseIcon;
        MediaPlayer videoPlayer;
        bool startedVideo;
        Texture2D photoTexture;
        public GameObject photoVideoPane;



        


        // Update is called once per frame
        void Update()
        {
            if (startedVideo)
            {
                videoChecker();
            }


        }
        
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