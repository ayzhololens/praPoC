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
        public string fileName { get; set; }
        public commentManager linkedComManager { get; set; }
        public GameObject linkedComponent;
        bool startedVideo;
        public GameObject playIcon;
        public GameObject pauseIcon;
        public MediaPlayer mediaPlayer;
        public Texture vidThumbnail;
        public Material vidMat;
        public Material vidThumbMat;
        public Material thumbMat;
        Vector3 initPos;
        public GameObject expandIndicator;
        public GameObject exit;
        public float expandScale;
        bool expanded;


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

            if (startedVideo)
            {
                videoChecker();
            }

            if (vidThumbnail != null && thumbMat.mainTexture != vidThumbnail)
            {

                thumbMat.mainTexture = vidThumbnail;
                GetComponent<Renderer>().material = thumbMat;
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
                GetComponent<Renderer>().material = thumbMat;
                //vidThumbMat = GetComponent<Renderer>().material;
                mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().activeComment = this.gameObject;
                mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().addThumbnail(filepath, this.gameObject);

                //mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().makeThumbnail();
                //vidThumbnail = mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>()._texture;
                //vidThumbMat.mainTexture = vidThumbnail;
                //GetComponent<Renderer>().material = vidThumbMat;

            }

            if (vidThumbnail != null && thumbMat.mainTexture != vidThumbnail)
            {
                thumbMat.mainTexture = vidThumbnail;
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


            if (mediaPlayer == null)
            {
                mediaPlayer = mediaManager.Instance.videoPlayer;
            }
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
                mediaPlayer.m_VideoPath = filepath;
                mediaPlayer.LoadVideoPlayer();

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

        public void revealExpansion()
        {
            if (!expanded)
            {
                expandIndicator.SetActive(true);

            }
        }
        public void hideExpansion()
        {
            if (!expanded)
            {
                expandIndicator.SetActive(false);
            }
            
        }

        public void expandComment()
        {
            if (!expanded)
            {
                initPos = transform.localPosition;

                for (int i = 0; i < linkedComManager.activeComments.Count; i++)
                {
                    if (linkedComManager.activeComments[i] != this.gameObject)
                    {
                        linkedComManager.activeComments[i].SetActive(false);

                    }
                }

                if (isVideo)
                {
                    playIcon.SetActive(true);
                }

                transform.localPosition = linkedComManager.expandPos.localPosition;
                transform.localScale *= expandScale;


                expanded = true;
                expandIndicator.SetActive(false);
                exit.SetActive(true);
            }


  


        }

        public void contractComment()
        {
            for (int i = 0; i < linkedComManager.activeComments.Count; i++)
            {
                if (linkedComManager.activeComments[i] != this.gameObject)
                {
                    linkedComManager.activeComments[i].SetActive(true);

                }
            }

            if (isVideo)
            {
                playIcon.SetActive(false);
            }
            expanded = false;
            exit.SetActive(false);
            transform.localPosition = initPos;
            transform.localScale /= expandScale;


        }
    }
}
