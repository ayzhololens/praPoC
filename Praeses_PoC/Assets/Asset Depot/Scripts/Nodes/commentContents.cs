using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RenderHeads.Media.AVProVideo.Demos;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;


namespace HoloToolkit.Unity
{
    public class commentContents : MonoBehaviour {

        [Header ("Classification")]
        public bool isSimple;
        public bool isPhoto;
        public bool isVideo;

        [Header ("Data Related")]
        [Tooltip ("Input field for simple comments")]
        public InputField commentMain;
        [Tooltip("Display to show user")]
        public Text commentMetaUser;
        [Tooltip ("Display to show date")]
        public Text commentMetaDate;

        public string Date { get; set; }
        public string user { get; set; }
        public string filepath { get; set; }
        //filename
        public string fileName { get; set; }
        //Comment manager that made this
        public commentManager linkedComManager { get; set; }
        //What the comment is attached to-- like a violation or form field
        public GameObject linkedComponent { get; set; }

        [Header ("Photo/Video Components")]
        public GameObject playIcon;
        public GameObject pauseIcon;

        //main media player
        public MediaPlayer mediaPlayer { get; set; }
        //Texture thats set by the frame extractor
        public Texture vidThumbnail { get; set; }
        [Tooltip ("Material that video is rendered to")]
        public Material vidMat;
        [Tooltip ("Thumb mat base that will be duplicated")]
        public Material vidThumbMat;
        //Instanced copy of the thumbnail mat
        public Material thumbMat { get; set; }
        bool startedVideo;

        [Header ("Expand Controls")]
        [Tooltip ("Visual expansion indicator")]
        public GameObject expandIndicator;
        [Tooltip("Expansion exit button")]
        public GameObject exit;
        [Tooltip ("Amount to expand by")]
        public float expandScale;
        Vector3 initPos;
        bool expanded;
        


        void Update() {

            if (startedVideo)
            {
                videoChecker();
            }

            ///Check to see if thumbnail texture is ready
            if (vidThumbnail != null && thumbMat.mainTexture != vidThumbnail)
            {

                thumbMat.mainTexture = vidThumbnail;
                GetComponent<Renderer>().material = thumbMat;
            }
            

            ///Adjust to face forward
            transform.localEulerAngles = new Vector3(0, 0, 0);

        }



        /// <summary>
        /// Set the video path to the media player.  Queue video up to get a thumbnail from the frame extractor.  
        /// The frame extractor is from a 3rd party AVPro video plugin and is very unstable.  Theyve since released a dedicated
        /// thumbnail plugin that might be worth checking out
        /// </summary>
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
                mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().activeComment = this.gameObject;
                mediaManager.Instance.vidRecorder.GetComponent<FrameExtract>().addThumbnail(filepath, this.gameObject);
            }

            ///again check to see if thumb texture is ready to be applied
            if (vidThumbnail != null && thumbMat.mainTexture != vidThumbnail)
            {
                thumbMat.mainTexture = vidThumbnail;
            }

        }


        /// <summary>
        /// Load photo bytes from local storage
        /// </summary>
        public void loadPhoto()
        {

            Texture2D targetTexture = new Texture2D(2048, 1152);

            var bytesRead = System.IO.File.ReadAllBytes(filepath);
            targetTexture.LoadImage(bytesRead);
            GetComponent<Renderer>().material.mainTexture = targetTexture;

        }


        /// <summary>
        /// Set filepath and start playing
        /// </summary>
        public void PlayVideo()
        {

            if (mediaPlayer == null)
            {
                mediaPlayer = mediaManager.Instance.videoPlayer;
            }
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


        /// <summary>
        /// Pause current video
        /// </summary>
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


        /// <summary>
        /// Reset components when video is done
        /// </summary>
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


        /// <summary>
        /// Control state of visual indicator
        /// </summary>
        public void revealExpansion()
        {
            if (!expanded)
            {
                expandIndicator.SetActive(true);

            }
        }
        /// <summary>
        /// Control state of visual indicator
        /// </summary>
        public void hideExpansion()
        {
            if (!expanded)
            {
                expandIndicator.SetActive(false);
            }
            
        }


        /// <summary>
        /// Hide all other comments and enlarge comment
        /// </summary>
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


        /// <summary>
        /// Unhide all comments
        /// </summary>
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
