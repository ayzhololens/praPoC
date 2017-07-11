using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR.WSA.WebCam;

using System.Linq;
#if !UNITY_EDITOR
    using Windows.Storage;
    using Windows.System;
    using System.Collections.Generic;
using System;
using System.IO;
#endif

namespace HoloToolkit.Unity
{
    public class videoRecorder : MonoBehaviour
    {


        VideoCapture m_VideoCapture = null;
        public bool recording { get; set; }
        [Tooltip ("Video name")]
        public string filename;
        [Tooltip ("Video name plus filepath")]
        public string filepath;
        [Tooltip ("Number of videos recorded")]
        public int vidCounter;
        [Tooltip ("Videos that have been recorded")]
        public List<string> fileList;


        // Use this for initialization
        void Start()
        {
            
            vidCounter = 0;
#if !UNITY_EDITOR
            m_VideoCapture.Dispose();
            m_VideoCapture = null;
#endif



        }

        // Update is called once per frame
        void Update()
        {

        }

        public void startRecordingVideo()
        {
            ///Change argument to false to hide holograms
#if !UNITY_EDITOR
            VideoCapture.CreateAsync(false, OnVideoCaptureCreated);
#endif
            recording = true;
        }

        
        void OnVideoCaptureCreated(VideoCapture videoCapture)
        {
            if (videoCapture != null)
            {
                m_VideoCapture = videoCapture;

                Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Last();
                float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();

                CameraParameters cameraParameters = new CameraParameters();

                ///Set hologram opacity to 0 to hide holograms
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.frameRate = cameraFramerate;
                cameraParameters.cameraResolutionWidth = cameraResolution.width;
                cameraParameters.cameraResolutionHeight = cameraResolution.height;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                m_VideoCapture.StartVideoModeAsync(cameraParameters,
                                                    VideoCapture.AudioState.ApplicationAndMicAudio,
                                                    OnStartedVideoCaptureMode);
            }
            else
            {
                Debug.LogError("Failed to create VideoCapture Instance!");
            }

        }

        void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
        {
            if (result.success)
            {
                filename = string.Format("pocVideo_" + System.DateTime.Now.ToString("MMddyy") + "_" + vidCounter + ".mp4", Time.time);
                filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                fileList.Add((string)filepath);
                m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
            }
        }

        void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
        {


            Debug.Log("Started Recording Video!");
            // We will stop the video from recording via other input such as a timer or a tap, etc.
        }

        public void StopRecordingVideo(bool activateMedia)
        {
#if !UNITY_EDITOR
            m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
#endif
            if (activateMedia)
            {
                mediaManager.Instance.activateMedia();
            }
            else
            {
                mediaManager.Instance.activateComment();
            }
            
        }

        void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)

        {

            Debug.Log("Stopped Recording Video!");
            m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
            vidCounter += 1;




        }

        void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
        {
            recording = false;
            m_VideoCapture.Dispose();
            m_VideoCapture = null;
        }
    }


}
