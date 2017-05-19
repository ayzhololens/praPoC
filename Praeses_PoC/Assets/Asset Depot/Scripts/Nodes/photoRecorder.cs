
using UnityEngine;
using System.Collections;
using System.IO;

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

    public class photoRecorder : MonoBehaviour
    {

        PhotoCapture photoCaptureObject = null;
        public Texture2D targetTexture;
        public string filePath;
        public string filename;
        int index;

        //activate media if a photo node
        public bool activateMedia { get; set; }


        // Use this for initialization
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {

        }

        public void CapturePhoto()
        {

#if !UNITY_EDITOR
            PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
#else
            Invoke("loadPhoto", 1);
#endif

        }


        public void OnPhotoCaptureCreated(PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Last();
            CameraParameters c = new CameraParameters();
            c.hologramOpacity = 0.0f;
            c.cameraResolutionWidth = cameraResolution.width;
            c.cameraResolutionHeight = cameraResolution.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;

            captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
        }

        void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Last();
                filename = string.Format(@"newPhoto"+ index+ ".jpg", Time.time);
                filePath = Path.Combine(Application.persistentDataPath , filename);
                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
                
                
                //photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                Invoke("loadPhoto", 1);

                index += 1;
            }
            else
            {
                Debug.Log("Unable to start photo mode");
            }
        }

        void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            if (result.success)
            {
                Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
                targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
                photoCaptureFrame.UploadImageDataToTexture(targetTexture);


            }

            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }


        void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                Debug.Log("Saved Photo to Disk");
                photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            }
            else
            {
                Debug.Log("failed to save Photo to disk");
            }
        }


        void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }

        void loadPhoto()
        {

            if (activateMedia)
            {
                mediaManager.Instance.activateMedia();
            }
            if (!activateMedia)
            {
                mediaManager.Instance.activateComment();
            }
            activateMedia = false;

        }


    }

}
