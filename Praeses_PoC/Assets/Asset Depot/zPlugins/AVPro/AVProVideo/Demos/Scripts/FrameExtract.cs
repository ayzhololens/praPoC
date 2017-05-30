#if !UNITY_WEBPLAYER
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
#define AVPRO_FILESYSTEM_SUPPORT
#endif
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class FrameExtract : MonoBehaviour
	{
		private const int NumFrames = 1;
		public MediaPlayer _mediaPlayer;
		public bool _accurateSeek = false;
		public int _timeoutMs = 250;
        
		private float _timeStepSeconds;
		private int _frameIndex = 0;
		public  Texture2D _texture;
        public List<Texture2D> thumbTexts;
        public GameObject activeComment;
        public string filePath;

        public List<string> queuedThumbs;
        public List<GameObject> queuedComments;
        int index;
        public float invokeTime;


        private void Start()
        {

        }
        void listCheck()
        {
            if (index == queuedThumbs.Count){
                makeThumbnail();
            }
        }


        public void addThumbnail(string filePath, GameObject comment)
        {

            CancelInvoke();
            queuedComments.Add(comment);
            queuedThumbs.Add(filePath);
            index += 1;
            Invoke("listCheck", 1);

        }

		public void makeThumbnail()
        {
            if (queuedThumbs.Count > 0)
            {

                _mediaPlayer.m_VideoPath = queuedThumbs[0];
                _mediaPlayer.LoadVideoPlayer();
                OnNewMediaReady();
                _mediaPlayer.Events.AddListener(OnMediaPlayerEvent);

            }
            else
            {
               loadThumbs();
            }



        }

        public void loadThumbs()
        {


            for (int i = 0; i < queuedComments.Count; i++)
            {
                if (queuedComments[i].GetComponent<commentContents>() != null)
                {
                    queuedComments[i].GetComponent<commentContents>().vidThumbnail = thumbTexts[i];


                }
                if (activeComment.GetComponent<offsiteMediaPlayer>() != null)
                {
                    queuedComments[i].GetComponent<offsiteMediaPlayer>().vidThumbnail = thumbTexts[i];

                }

            }

            index = 0;
            thumbTexts.Clear();
            queuedThumbs.Clear();
            queuedComments.Clear();
        }

		public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
		{
			switch (et)
			{
				case MediaPlayerEvent.EventType.MetaDataReady:
					// Android platform doesn't display its first frame until poked
					mp.Play();
					mp.Pause();
					break;
				case MediaPlayerEvent.EventType.FirstFrameReady:
					OnNewMediaReady();
					break;
			}
		}

		private void OnNewMediaReady()
		{
			IMediaInfo info = _mediaPlayer.Info;

			// Create a texture the same resolution as our video
			if (_texture != null)
			{
				//Texture2D.Destroy(_texture);
				_texture = null;
			}
			_texture = new Texture2D(info.GetVideoWidth(), info.GetVideoHeight(), TextureFormat.ARGB32, false);

			_timeStepSeconds = (_mediaPlayer.Info.GetDurationMs() / 1000f) / (float)NumFrames;
            
            ExtractNextFrame();
        }

		void OnDestroy()
		{
			if (_texture != null)
			{
				Texture2D.Destroy(_texture);
				_texture = null;
			}
		}

		void Update()
		{
		}

        private void ExtractNextFrame()
        {
            // Extract the frame to Texture2D
            float timeSeconds = _frameIndex * _timeStepSeconds;
            _texture = _mediaPlayer.ExtractFrame(_texture, timeSeconds, _accurateSeek, _timeoutMs);
            
            if (_texture != null)
            {
                //if (activeComment.GetComponent<commentContents>() != null)
                //{

                //    activeComment.GetComponent<commentContents>().vidThumbnail = _texture;
                //    activeComment.GetComponent<commentContents>().thumbMat.mainTexture = activeComment.GetComponent<commentContents>().vidThumbnail;
                //    activeComment.GetComponent<Renderer>().material = activeComment.GetComponent<commentContents>().thumbMat;
                //}

                //if (activeComment.GetComponent<offsiteMediaPlayer>() != null)
                //{

                //    activeComment.GetComponent<offsiteMediaPlayer>().vidThumbnail = _texture;
                //    activeComment.GetComponent<offsiteMediaPlayer>().thumbMat.mainTexture = activeComment.GetComponent<offsiteMediaPlayer>().vidThumbnail;
                //    activeComment.GetComponent<offsiteMediaPlayer>().thumbPlane.GetComponent<Renderer>().material = activeComment.GetComponent<offsiteMediaPlayer>().thumbMat;
                //}
                Invoke("clear", invokeTime);

            }
            else
            {
                print("no texture");
            }
        }

        void clear()
        {
            if (_texture != null)
            {
                thumbTexts.Add(_texture);
                //thumbTexts.Add(_texture);
                _texture = null;
                if (queuedThumbs.Count > 0)
                {
                    queuedThumbs.RemoveAt(0);
                    Invoke("makeThumbnail", invokeTime);
                }

                _mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent); 
            }

        }
    }
}