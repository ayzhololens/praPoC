using System.Collections;
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
        public Text DisplayName;
        public string trueName;
        public InputField Value;
        public Text previousValue;
        public int nodeIndex;
        public GameObject fieldButton;
        public Transform buttonPos;
        public float buttonXOffset;
        public float buttonYOffset;
        public Transform buttonParent;
        public List<GameObject> curButtons;
        int currCommentType;
        public bool showUpdate;



        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void spawnNode(int commentType)
        {
            currCommentType = commentType;
            if (linkedNode == null)
            {

                nodeSpawner.Instance.spawnNode(4);
                nodeSpawner.Instance.getLinkedField(gameObject.GetComponent<formFieldController>());
            }
            else
            {
                enableAttachmentCapture();
            }



        }

        //public void repositionThumb()
        //{
        //    thumbPos.position = new Vector3(thumbPos.position.x, thumbPos.position.y - thumbOffset, thumbPos.position.z);
        //}




        public void enableAttachmentCapture()
        {

            //GetComponent<subMenu>().turnOnSubButtons();
            ////attachmentParent.gameObject.SetActive(false);
            //for(int i = 0; i<transform.parent.childCount; i++)
            //{
            //    if (transform.parent.GetChild(i).gameObject != this.gameObject && transform.parent.GetChild(i).gameObject.GetComponent<subMenu>() != null)
            //    {

            //        transform.parent.GetChild(i).gameObject.GetComponent<subMenu>().turnOffCounter();
            //        //transform.parent.GetChild(i).gameObject.GetComponent<formFieldController>().attachmentParent.gameObject.SetActive(false);
            //    }
            //}

            print(currCommentType);
            if (linkedNode.GetComponent<selectEvent>().enabled == false)
            {
                linkedNode.GetComponent<selectEvent>().enabled = true;
            }

            if(currCommentType == 1)
            {
                GetComponent<commentManager>().spawnSimpleComment();
            }
            if (currCommentType == 2)
            {
                GetComponent<commentManager>().enablePhotoCapture();
            }
            if (currCommentType == 3)
            {
                GetComponent<commentManager>().enableVideoCapture();
            }

            currCommentType = 0;
        }

        public void populateButtons(int buttonAmount)
        {
            Vector3 buttonLoc = buttonPos.position;
            for (int i = 0; i<buttonAmount; i++)
            {
                if (i == 2)
                {
                    //buttonLoc = new Vector3(buttonPos.position.x, buttonPos.position.y + buttonYOffset, buttonLoc.z); ;
                }

                curButtons.Add( Instantiate(fieldButton, buttonLoc, Quaternion.identity));
                curButtons[i].GetComponent<formButtonController>().field = this;
                curButtons[i].transform.SetParent(buttonParent);
                curButtons[i].transform.localScale = fieldButton.transform.localScale;
                curButtons[i].transform.localRotation = fieldButton.transform.localRotation;

                buttonLoc = new Vector3(buttonLoc.x + buttonXOffset, buttonLoc.y, buttonLoc.z) ;

            }
        }

        public void setStatus()
        {
            
            if (showUpdate)
            {

                formController.Instance.updateFieldStatus(1);
                showUpdate = false;
            }
        }

        //public void revealAttachments()
        //{
        //    if (!GetComponent<subMenu>().subButtonsOn && linkedNode!=null)
        //    {
        //        for (int i = 0; i < transform.parent.childCount; i++)
        //        {
        //            if (transform.parent.GetChild(i).gameObject != this.gameObject && transform.parent.GetChild(i).gameObject.GetComponent<subMenu>() != null)
        //            {

        //                transform.parent.GetChild(i).gameObject.GetComponent<subMenu>().turnOffCounter();
        //                transform.parent.GetChild(i).gameObject.GetComponent<formFieldController>().attachmentParent.gameObject.SetActive(false);
        //            }
        //        }
        //    }


        //    attachmentParent.gameObject.SetActive(true);
        //}


    //    public void enableVideoCapture()
    //    {
    //        annotationManager.Instance.enableVideoRecording();
    //        annotationManager.Instance.currentAnnotation = linkedNode;

    //        annotationManager.Instance.activeField = this.gameObject;
    //        GetComponent<subMenu>().turnOffCounter();
    //        attachmentParent.gameObject.SetActive(true);
    //        capturingVideo = true;
    //    }

    //    public void enablePhotoCapture()
    //    {
    //        annotationManager.Instance.enablePhotoCapture();
    //        annotationManager.Instance.currentAnnotation = linkedNode;
    //        annotationManager.Instance.activeField = this.gameObject;
    //        GetComponent<subMenu>().turnOffCounter();
    //        attachmentParent.gameObject.SetActive(true);
    //        capturingPhoto = true;

    //    }

    //    public void loadVideoMedia()
    //    {
    //        if (vidRecorder == null)
    //        {
    //            vidRecorder = GameObject.Find("VideoManager").GetComponent<videoRecorder>();
    //            VideoPlayer = GameObject.Find("VideoPlayer").GetComponent<MediaPlayer>();
    //        }
    //        //
    //        attachmentParent.gameObject.SetActive(false);
    //        //
    //        activeVideoPath = vidRecorder.filepath;
    //        VideoPlayer.m_VideoPath = activeVideoPath;
    //        linkedNode.GetComponent<nodeMediaHolder>().filepath.Add(activeVideoPath);
    //        videoFilePaths.Add(activeVideoPath);
    //        spawnVideoPane();

    //    }


    //    public void spawnVideoPane()
    //    {
    //        attachmentParent.gameObject.SetActive(true);
    //        GameObject spawnedVideo = Instantiate(videoThumbPrefab, transform.position, Quaternion.identity);
    //        activeVideos.Add(spawnedVideo);
    //        spawnedVideo.transform.SetParent(attachmentParent);
    //        spawnedVideo.transform.localPosition = thumbPos.localPosition;
    //        repositionThumb();

    //        spawnedVideo.GetComponent<commentContents>().isVideo = true;
    //        linkedNode.GetComponent<nodeMediaHolder>().activeComments.Add(spawnedVideo);
    //        spawnedVideo.GetComponent<commentContents>().Date = System.DateTime.Now.ToString();
    //        spawnedVideo.GetComponent<commentContents>().user = metaManager.Instance.user;
    //        spawnedVideo.GetComponent<commentContents>().commentMeta.text = (metaManager.Instance.user + " " + System.DateTime.Now);
    //        spawnedVideo.GetComponent<commentContents>().filepath = activeVideoPath;
    //        spawnedVideo.GetComponent<commentContents>().linkedComponent = this.gameObject;
    //        capturingVideo = false;
    //    }

    //    public virtual GameObject spawnVideoPaneFromJSon()
    //    {
    //        if (VideoPlayer == null)
    //        {
    //            VideoPlayer = GameObject.Find("VideoPlayer").GetComponent<MediaPlayer>();
    //        }

    //        GameObject spawnedVideo = Instantiate(videoThumbPrefab, transform.position, Quaternion.identity);
    //        activeVideos.Add(spawnedVideo);
    //        spawnedVideo.transform.SetParent(attachmentParent);
    //        spawnedVideo.transform.localPosition = thumbPos.localPosition;
    //        repositionThumb();

    //        spawnedVideo.GetComponent<commentContents>().isVideo = true;
    //        linkedNode.GetComponent<nodeMediaHolder>().activeComments.Add(spawnedVideo);
    //        spawnedVideo.GetComponent<commentContents>().linkedComponent = this.gameObject;
    //        return spawnedVideo;
    //    }


    //    public void spawnSimpleComment()
    //    {
    //        GetComponent<subMenu>().turnOffCounter();
    //        attachmentParent.gameObject.SetActive(true);
    //        GameObject spawnedComment = Instantiate(simpleNotePrefab, transform.position, Quaternion.identity);
    //        activeSimpleNotes.Add(spawnedComment);
    //        spawnedComment.transform.SetParent(attachmentParent);
    //        spawnedComment.transform.position = thumbPos.position;
    //        repositionThumb();
    //        spawnedComment.GetComponent<commentContents>().isSimple = true;
    //        spawnedComment.GetComponent<inputFieldManager>().activateField();
    //        linkedNode.GetComponent<nodeMediaHolder>().activeComments.Add(spawnedComment);
    //        spawnedComment.GetComponent<commentContents>().Date = System.DateTime.Now.ToString();
    //        spawnedComment.GetComponent<commentContents>().user = metaManager.Instance.user;
    //        spawnedComment.GetComponent<commentContents>().commentMeta.text = (metaManager.Instance.user + " " + System.DateTime.Now);
    //        spawnedComment.GetComponent<commentContents>().linkedComponent = this.gameObject;
    //    }

    //    public virtual GameObject spawnSimpleCommentFromJSon()
    //    {
    //        GameObject spawnedComment = Instantiate(simpleNotePrefab, transform.position, Quaternion.identity);
    //        activeSimpleNotes.Add(spawnedComment);
    //        spawnedComment.transform.SetParent(attachmentParent);
    //        spawnedComment.transform.localPosition = thumbPos.localPosition;
    //        repositionThumb();
    //        spawnedComment.GetComponent<commentContents>().isSimple = true;
    //        linkedNode.GetComponent<nodeMediaHolder>().activeComments.Add(spawnedComment);
    //        spawnedComment.GetComponent<commentContents>().linkedComponent = this.gameObject;
    //        return spawnedComment;
    //    }

    //    public void loadPhotoMedia()
    //    {
    //        if (photoRecorder == null)
    //        {
    //            photoRecorder = GameObject.Find("PhotoManager").GetComponent<photoRecorder>();
    //        }
    //        //
    //        attachmentParent.gameObject.SetActive(false);
    //        //
    //        activePhotoPath = photoRecorder.filePath;
    //        photoFilePaths.Add(activePhotoPath);
    //        linkedNode.GetComponent<nodeMediaHolder>().filepath.Add(activePhotoPath);
    //        spawnPhotoPane();
    //    }

    //    public void spawnPhotoPane()
    //    {
    //        attachmentParent.gameObject.SetActive(true);
    //        GameObject spawnedPhoto = Instantiate(photoThumbPrefab, transform.position, Quaternion.identity);
    //        activePhotos.Add(spawnedPhoto);
    //        spawnedPhoto.transform.SetParent(attachmentParent);
    //        spawnedPhoto.transform.localPosition = thumbPos.localPosition;
    //        repositionThumb();
    //        spawnedPhoto.GetComponent<commentContents>().isPhoto = true;
    //        spawnedPhoto.GetComponent<commentContents>().filepath = activePhotoPath;
    //        spawnedPhoto.GetComponent<commentContents>().linkedComponent = this.gameObject;
    //        linkedNode.GetComponent<nodeMediaHolder>().activeComments.Add(spawnedPhoto);
    //        spawnedPhoto.GetComponent<commentContents>().Date = System.DateTime.Now.ToString();
    //        spawnedPhoto.GetComponent<commentContents>().user = metaManager.Instance.user;
    //        spawnedPhoto.GetComponent<commentContents>().commentMeta.text = (metaManager.Instance.user + " " + System.DateTime.Now);
    //        photoTexture = photoRecorder.targetTexture;
    //        spawnedPhoto.GetComponent<Renderer>().material.mainTexture = photoTexture;
    //        capturingPhoto = false;

    //    }

    //    public virtual GameObject spawnPhotoPaneFromJSon()
    //    {
    //        GameObject spawnedPhoto = Instantiate(photoThumbPrefab, transform.position, Quaternion.identity);
    //        activePhotos.Add(spawnedPhoto);
    //        spawnedPhoto.transform.SetParent(attachmentParent);
    //        spawnedPhoto.transform.localPosition = thumbPos.localPosition;
    //        repositionThumb();
    //        spawnedPhoto.GetComponent<commentContents>().isPhoto = true;
    //        spawnedPhoto.GetComponent<commentContents>().linkedComponent = this.gameObject;
    //        linkedNode.GetComponent<nodeMediaHolder>().activeComments.Add(spawnedPhoto);


    //        return spawnedPhoto;

    //    }

    }
}
