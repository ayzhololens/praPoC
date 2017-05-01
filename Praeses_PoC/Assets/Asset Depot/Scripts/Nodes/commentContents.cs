using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RenderHeads.Media.AVProVideo.Demos;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;


namespace HoloToolkit.Unity
{
    public class commentContents : MonoBehaviour {

        public bool isSimple;
        public bool isPhoto;
        public bool isVideo;
        public InputField commentMain;
        public Text commentMetaUser;
        public Text commentMetaDate;
        public string Date;
        public string user;
        public GameObject editButton;
        public InputField inputField;
        public string filepath;
        public GameObject linkedComponent;
        bool startedVideo;
        public GameObject playIcon;
        public GameObject pauseIcon;
        public MediaPlayer mediaPlayer;
        public Texture vidThumbnail;
        public Material vidMat;
        public Material vidThumbMat;
        public Material thumbMat;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

            if (startedVideo)
            {
                videoChecker();
            }
            
            transform.localEulerAngles = new Vector3(0, 0, 0);

        }


        public void LoadVideo()
        {

            if (mediaPlayer == null)
            {
                mediaPlayer = mediaManager.Instance.videoPlayer;
            }
            mediaPlayer.m_VideoPath = filepath;
            mediaPlayer.LoadVideoPlayer();
            if (vidThumbnail == null)
            {
                thumbMat = Instantiate(vidThumbMat);
                Debug.Log("oh no");
                //vidThumbMat = GetComponent<Renderer>().material;
                mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().activeComment = this.gameObject;
                mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().makeThumbnail();
                //vidThumbnail = mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>()._texture;
                //vidThumbMat.mainTexture = vidThumbnail;
                //GetComponent<Renderer>().material = vidThumbMat;
                Debug.Log("oh no2");

            }

        }

        public void loadPhoto()
        {

            Texture2D targetTexture = new Texture2D(2048, 1152);

            var bytesRead = System.IO.File.ReadAllBytes(filepath);
            targetTexture.LoadImage(bytesRead);
            GetComponent<Renderer>().material.mainTexture = targetTexture;

        }

        public void PlayVideo()
        {
            print(startedVideo);
            if (mediaPlayer.m_VideoPath != filepath)
            {
                mediaPlayer.m_VideoPath = filepath;
            }
            if (!mediaPlayer.Control.IsPlaying() && !mediaPlayer.Control.IsPaused())
            {
                mediaPlayer.LoadVideoPlayer();

            }
            if(GetComponent<Renderer>().material != vidMat)
            {
                GetComponent<Renderer>().material = vidMat;
            }
            if (!startedVideo)
            {
                mediaPlayer.Control.Play();

                startedVideo = true;
                playIcon.SetActive(false);
                pauseIcon.SetActive(true);
            }


        }

        public void PauseVideo()
        {
            if (startedVideo)
            {
                mediaPlayer.Control.Pause();
                GetComponent<Renderer>().material = thumbMat;
                startedVideo = false;
                playIcon.SetActive(true);
                pauseIcon.SetActive(false);
            }

        }

        void videoChecker()
        {
            if (mediaPlayer.Control.IsFinished())
            {
                playIcon.SetActive(true);
                pauseIcon.SetActive(false);
                startedVideo = false;
                GetComponent<Renderer>().material = thumbMat;

            }
        }
    }
}
