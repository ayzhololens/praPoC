using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using RenderHeads.Media.AVProVideo.Demos;

public class commentManager : MonoBehaviour {

    public List<GameObject> activeComments;
    public int commentCount;
    public Transform commentParent;
    public Transform CommmentStartPos;
    public Transform expandPos;
    public GameObject simpleCommentPrefab;
    public GameObject videoCommentPrefab;
    public GameObject photoCommentPrefab;
    GameObject spawnedComment;
    public float offsetDist;
    public bool xOffset;
    InputField activeInputField;
    public bool capturingPhoto { get; set; }
    public bool capturingVideo { get; set; }
    bool recordingEnabled;
    bool recordingInProgress;
    bool photoCaptureEnabled;

    public commentManager managerOverride;


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        if (capturingVideo || capturingPhoto)
        {
            stopCapturing();
        }

    }

    public void spawnSimpleComment()
    {
        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(simpleCommentPrefab, transform.position, Quaternion.identity);
        spawnedComment.transform.localEulerAngles = new Vector3(0, 0, 0);



        activeComments.Add(spawnedComment);
        if (managerOverride != null)
        {
            managerOverride.activeComments.Add(spawnedComment);
        }

        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = simpleCommentPrefab.transform.localScale;

        //define the comment type and open the keyboard
        spawnedComment.GetComponent<commentContents>().isSimple = true;
        spawnedComment.GetComponent<commentContents>().commentMain.GetComponent<inputFieldManager>().activateField();

        //is node
        if (GetComponent<nodeMediaHolder>() != null)
        {
            spawnedComment.GetComponent<commentContents>().commentMain.GetComponent<inputFieldManager>().commentNode = GetComponent<nodeMediaHolder>();
        }

        //is field
        if (GetComponent<formFieldController>() != null)
        {
            spawnedComment.GetComponent<commentContents>().commentMain.GetComponent<inputFieldManager>().commentNode = GetComponent<formFieldController>().linkedNode.GetComponent<nodeMediaHolder>();
        }

        //is violation
        if (GetComponent<violationController>() != null)
        {
            spawnedComment.GetComponent<commentContents>().commentMain.GetComponent<inputFieldManager>().commentNode = GetComponent<violationController>().linkedNode.GetComponent<nodeMediaHolder>();

        }

    }

    public virtual GameObject spawnSimpleCommentFromJSON()
    {        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(simpleCommentPrefab, transform.position, Quaternion.identity);
        spawnedComment.transform.localEulerAngles = new Vector3(0, 0, 0);

        activeComments.Add(spawnedComment);



        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = simpleCommentPrefab.transform.localScale;


        //define the comment type and open the keyboard
        spawnedComment.GetComponent<commentContents>().isSimple = true;

        return spawnedComment;

    }

    void commentSetup(commentContents newComment)
    {
        //set position and parenting
        newComment.transform.SetParent(commentParent);
        newComment.transform.localPosition = CommmentStartPos.localPosition;

        //define comment metas
        newComment.linkedComManager = this;
        newComment.Date = metaManager.Instance.date();
        newComment.user = metaManager.Instance.user;
        newComment.commentMetaUser.text = newComment.user;
        newComment.commentMetaDate.text = newComment.Date;

        //link comment to gameObject
        newComment.linkedComponent = this.gameObject;

    }
    
    void repositionComments()
    {

        for (int i = 0; i < activeComments.Count; i++)
        {
            if (!xOffset)
            {
                activeComments[i].transform.localPosition = new Vector3(activeComments[i].transform.localPosition.x,
                                                                    activeComments[i].transform.localPosition.y - offsetDist,
                                                                    activeComments[i].transform.localPosition.z);
            }
            else
            {
                activeComments[i].transform.localPosition = new Vector3(activeComments[i].transform.localPosition.x- offsetDist,
                                                                    activeComments[i].transform.localPosition.y,
                                                                    activeComments[i].transform.localPosition.z);
            }
        }
    }


    public void enableVideoCapture()
    {
        capturingVideo = true;
        mediaManager.Instance.setStatusIndicator("Tap to start recording video");

        //clear source manager
        sourceManager.Instance.sourcePressed = false;
        recordingEnabled = true;

        fovHider.Instance.toggleFOVHider(true, 0);
    }

    void startVideoCapture()
    {
        mediaManager.Instance.vidRecorder.startRecordingVideo();
        recordingEnabled = false;
        mediaManager.Instance.commentManager = GetComponent<commentManager>();
        mediaManager.Instance.setStatusIndicator("Recording in progress. Tap to stop");
        mediaManager.Instance.recordingIndicator.SetActive(true);

        //clear source manager
        sourceManager.Instance.sourcePressed = false;
        recordingInProgress = true;
    }

    void stopVideoRecording()
    {
        //stop recording, finish encoding then spawn video frame when done
        mediaManager.Instance.vidRecorder.StopRecordingVideo(false);
        Debug.Log("before status disable");
        mediaManager.Instance.setStatusIndicator("Video capture complete!");
        mediaManager.Instance.invokeStatusDisable(2.0f);
        audioManager.Instance.setAndPlayAudio(1);
        mediaManager.Instance.recordingIndicator.SetActive(false);
        recordingInProgress = false;

        //mediaManager.Instance.activateComment();

    }


    public virtual GameObject spawnVideoComment()
    {

        fovHider.Instance.toggleFOVHider(false, 1);

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(videoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);
        if (managerOverride != null)
        {
            managerOverride.activeComments.Add(spawnedComment);
        }



        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = videoCommentPrefab.transform.localScale;



        //define the comment type
        commentContents videoContent = spawnedComment.GetComponent<commentContents>();
        videoContent.isVideo = true;
        capturingVideo = false;

        videoContent.filepath = mediaManager.Instance.vidRecorder.filename;
        videoContent.LoadVideo();
        return spawnedComment;

    }

    public virtual GameObject spawnVideoCommentFromJSON(string filePath)
    {

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(videoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);



        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = videoCommentPrefab.transform.localScale;



        //define the comment type
        commentContents videoContent = spawnedComment.GetComponent<commentContents>();
        videoContent.isVideo = true;
        capturingVideo = false;
        videoContent.filepath = filePath;
        videoContent.LoadVideo();

        return spawnedComment;



    }

    public void enablePhotoCapture()
    {
        capturingPhoto = true;
        mediaManager.Instance.commentManager = GetComponent<commentManager>();
        mediaManager.Instance.setStatusIndicator("Tap to capture photo");
        mediaManager.Instance.recordingIndicator.SetActive(true);

        //clear source manager
        sourceManager.Instance.sourcePressed = false;
        photoCaptureEnabled = true;


        fovHider.Instance.toggleFOVHider(true, 0);
    }

    void capturePhoto()
    {
        photoCaptureEnabled = false;

        //capture photo, save it, activeMedia() when done
        mediaManager.Instance.photoRecorder.CapturePhoto();
        mediaManager.Instance.recordingIndicator.SetActive(false);
        mediaManager.Instance.setStatusIndicator("Photo capture complete!");
        mediaManager.Instance.invokeStatusDisable(2.0f);
        audioManager.Instance.setAndPlayAudio(1);
    }

    public void spawnPComment()
    {
        fovHider.Instance.toggleFOVHider(false, 1);

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(photoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);


        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = photoCommentPrefab.transform.localScale;

        //define the comment type
        commentContents photoContent = spawnedComment.GetComponent<commentContents>();
        photoContent.isPhoto = true;
        capturingPhoto = false;

        //photoContent.filepath = mediaManager.Instance.photoRecorder.filePath;
        //photoContent.loadPhoto();
    }

    public virtual GameObject spawnPhotoComment()
    {
        fovHider.Instance.toggleFOVHider(false,1 );

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(photoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);
        if (managerOverride != null)
        {
            managerOverride.activeComments.Add(spawnedComment);
        }

  

        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = photoCommentPrefab.transform.localScale;

        //define the comment type
        commentContents photoContent = spawnedComment.GetComponent<commentContents>();
        photoContent.isPhoto = true;
        capturingPhoto = false;

        photoContent.filepath = mediaManager.Instance.photoRecorder.filePath;
        photoContent.loadPhoto();
        return spawnedComment;
    }

    public virtual GameObject spawnPhotoCommentFromJSON()
    {
        
        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(photoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);


        commentSetup(spawnedComment.GetComponent<commentContents>());
        spawnedComment.transform.localScale = photoCommentPrefab.transform.localScale;

        //define the comment type
        commentContents photoContent = spawnedComment.GetComponent<commentContents>();
        photoContent.isPhoto = true;
        capturingPhoto = false;

        //print(photoContent.filepath);

        if (System.IO.File.Exists(photoContent.filepath))
        {
            //photoContent.loadPhoto();

        }



        return spawnedComment;
    }

    void stopCapturing()
    {
        if (sourceManager.Instance.sourcePressed)
        {
            if (recordingEnabled)
            {
                startVideoCapture();
            }
            if (recordingInProgress)
            {

                stopVideoRecording();
            }
            if (photoCaptureEnabled)
            {
                capturePhoto();
            }
        }

    }




    void fieldActivator()
    {

        activeComments[commentCount].GetComponent<inputFieldManager>().activateField();
        commentCount += 1;


    }
}
