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
        public GameObject activeComment;
        public string filePath;

		public void makeThumbnail()
		{
            OnNewMediaReady();
            _mediaPlayer.Events.AddListener(OnMediaPlayerEvent);



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
                activeComment.GetComponent<commentContents>().vidThumbnail = _texture;
                activeComment.GetComponent<commentContents>().thumbMat.mainTexture = activeComment.GetComponent<commentContents>().vidThumbnail;
                activeComment.GetComponent<Renderer>().material = activeComment.GetComponent<commentContents>().thumbMat;
                Invoke("clear", 1);
                print("ip");
            }
            else
            {
                print("no texture");
            }

        }

        void clear()
        {
            _mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent);
            activeComment = null;
        }


    }
}