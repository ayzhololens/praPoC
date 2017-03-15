﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using RenderHeads.Media.AVProVideo;

namespace HoloToolkit.Unity
{
    public class formFieldController : MonoBehaviour
    {

        public GameObject linkedNode;
        public Transform thumbPos;
        public Transform attachmentParent;
        public float thumbOffset;
        public Text DisplayName;
        public string trueName;
        public InputField Value;
        public Text previousValue;
        public int nodeIndex;

        public GameObject videoThumbPrefab;
        public List<GameObject> activeVideos;
        public List<string> videoFilePaths;
        string activeVideoPath;
        public MediaPlayer VideoPlayer;
        public videoRecorder vidRecorder;
        int videoCounter;
        public GameObject simpleNotePrefab;
        public List<GameObject> activeSimpleNotes;
        public GameObject photoThumbPrefab;
        public List<string> photoFilePaths;
        public List<GameObject> activePhotos;
        string activePhotoPath;
        Texture2D photoTexture;
        photoRecorder photoRecorder;



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void spawnNode()
        {
            if (linkedNode == null)
            {
                annotationSpawner.Instance.spawnFieldAnnotation();
                annotationManager.Instance.activeField = this.gameObject;
            }
            else
            {
                enableAttachmentCapture();
            }
        }

        public void repositionThumb()
        {
            thumbPos.position = new Vector3(thumbPos.position.x, thumbPos.position.y - thumbOffset, thumbPos.position.z);
        }




        public void enableAttachmentCapture()
        {
            GetComponent<subMenu>().turnOnSubButtons();
            attachmentParent.gameObject.SetActive(false);
            for(int i = 0; i<transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).gameObject != this.gameObject)
                {

                    transform.parent.GetChild(i).gameObject.GetComponent<subMenu>().turnOffCounter();
                    transform.parent.GetChild(i).gameObject.GetComponent<formFieldController>().attachmentParent.gameObject.SetActive(false);
                }
            }
            if(linkedNode.GetComponent<selectEvent>().enabled == false)
            {
                linkedNode.GetComponent<selectEvent>().enabled = true;
            }
        }

        public void revealAttachments()
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).gameObject != this.gameObject)
                {

                    transform.parent.GetChild(i).gameObject.GetComponent<subMenu>().turnOffCounter();
                    transform.parent.GetChild(i).gameObject.GetComponent<formFieldController>().attachmentParent.gameObject.SetActive(false);
                }
            }

            attachmentParent.gameObject.SetActive(true);
        }


        public void enableVideoCapture()
        {
            annotationManager.Instance.enableVideoRecording();
            annotationManager.Instance.currentAnnotation = this.gameObject;
            GetComponent<subMenu>().turnOffCounter();
            attachmentParent.gameObject.SetActive(true);
        }

        public void enablePhotoCapture()
        {
            annotationManager.Instance.enablePhotoCapture();
            annotationManager.Instance.currentAnnotation = this.gameObject;
            GetComponent<subMenu>().turnOffCounter();
            attachmentParent.gameObject.SetActive(true);

        }

        public void loadVideoMedia()
        {
            if (vidRecorder == null)
            {
                vidRecorder = GameObject.Find("VideoManager").GetComponent<videoRecorder>();
                VideoPlayer = GameObject.Find("VideoPlayer").GetComponent<MediaPlayer>();
            }
            //
            attachmentParent.gameObject.SetActive(false);
            //
            activeVideoPath = vidRecorder.filepath;
            VideoPlayer.m_VideoPath = activeVideoPath;
            videoFilePaths.Add(activeVideoPath);
            spawnVideoPane();
        }


        public void spawnVideoPane()
        {
            attachmentParent.gameObject.SetActive(true);
            GameObject spawnedVideo = Instantiate(videoThumbPrefab, transform.position, Quaternion.identity);
            activeVideos.Add(spawnedVideo);
            spawnedVideo.transform.SetParent(attachmentParent);
            spawnedVideo.transform.localPosition = thumbPos.localPosition;
            repositionThumb();

            spawnedVideo.GetComponent<commentContents>().isVideo = true;
            spawnedVideo.GetComponent<commentContents>().Date = System.DateTime.Now.ToString();
            spawnedVideo.GetComponent<commentContents>().user = metaManager.Instance.user;
            spawnedVideo.GetComponent<commentContents>().commentMeta.text = (metaManager.Instance.user + " " + System.DateTime.Now);
            spawnedVideo.GetComponent<commentContents>().filepath = activeVideoPath;
            spawnedVideo.GetComponent<commentContents>().linkedComponent = this.gameObject;
            VideoPlayer.LoadVideoPlayer();
        }

        public void spawnSimpleComment()
        {
            GetComponent<subMenu>().turnOffCounter();
            attachmentParent.gameObject.SetActive(true);
            GameObject spawnedComment = Instantiate(simpleNotePrefab, transform.position, Quaternion.identity);
            activeSimpleNotes.Add(spawnedComment);
            spawnedComment.transform.SetParent(attachmentParent);
            spawnedComment.transform.localPosition = thumbPos.localPosition;
            repositionThumb();
            spawnedComment.GetComponent<commentContents>().isSimple = true;
            spawnedComment.GetComponent<inputFieldManager>().activateField();
            spawnedComment.GetComponent<commentContents>().Date = System.DateTime.Now.ToString();
            spawnedComment.GetComponent<commentContents>().user = metaManager.Instance.user;
            spawnedComment.GetComponent<commentContents>().commentMeta.text = (metaManager.Instance.user + " " + System.DateTime.Now);
            spawnedComment.GetComponent<commentContents>().linkedComponent = this.gameObject;
        }


        public void loadPhotoMedia()
        {
            if (photoRecorder == null)
            {
                photoRecorder = GameObject.Find("PhotoManager").GetComponent<photoRecorder>();
            }
            //
            attachmentParent.gameObject.SetActive(false);
            //
            activePhotoPath = photoRecorder.filePath;
            photoFilePaths.Add(activePhotoPath);
            spawnPhotoPane();
        }

        public void spawnPhotoPane()
        {
            attachmentParent.gameObject.SetActive(true);
            GameObject spawnedPhoto = Instantiate(photoThumbPrefab, transform.position, Quaternion.identity);
            activePhotos.Add(spawnedPhoto);
            spawnedPhoto.transform.SetParent(attachmentParent);
            spawnedPhoto.transform.localPosition = thumbPos.localPosition;
            repositionThumb();
            spawnedPhoto.GetComponent<commentContents>().isPhoto = true;
            spawnedPhoto.GetComponent<commentContents>().filepath = activePhotoPath;
            spawnedPhoto.GetComponent<commentContents>().linkedComponent = this.gameObject;
            spawnedPhoto.GetComponent<commentContents>().Date = System.DateTime.Now.ToString();
            spawnedPhoto.GetComponent<commentContents>().user = metaManager.Instance.user;
            spawnedPhoto.GetComponent<commentContents>().commentMeta.text = (metaManager.Instance.user + " " + System.DateTime.Now);
            photoTexture = photoRecorder.targetTexture;
            spawnedPhoto.GetComponent<Renderer>().material.mainTexture = photoTexture;

        }


    }
}