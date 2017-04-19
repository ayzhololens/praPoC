using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using RenderHeads.Media.AVProVideo.Demos;

public class commentManager : MonoBehaviour {

    public List<GameObject> activeComments;
    public List<GameObject> activeSimpleComments;
    public int commentCount;
    public Transform commentParent;
    public Transform CommmentStartPos;
    Vector3 startPos;
    public GameObject simpleCommentPrefab;
    public GameObject videoCommentPrefab;
    public GameObject photoCommentPrefab;
    GameObject spawnedComment;
    public float offsetDist;
    InputField activeInputField;
    public bool capturingPhoto { get; set; }
    public bool capturingVideo { get; set; }
    bool recordingEnabled;
    bool recordingInProgress;
    bool photoCaptureEnabled;


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
        activeComments.Add(spawnedComment);

        commentSetup(spawnedComment.GetComponent<commentContents>());

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
    {
        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(simpleCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);

        commentSetup(spawnedComment.GetComponent<commentContents>());

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
        newComment.Date = System.DateTime.Now.ToString();
        newComment.user = metaManager.Instance.user;
        newComment.commentMeta.text = (newComment.user + " " + newComment.Date);
        
        //link comment to gameObject
        newComment.linkedComponent = this.gameObject;

    }
    
    void repositionComments()
    {

        for (int i = 0; i < activeComments.Count; i++)
        {
            activeComments[i].transform.position = new Vector3(activeComments[i].transform.position.x,
                                                                activeComments[i].transform.position.y - offsetDist,
                                                                activeComments[i].transform.position.z);
        }
    }


    public void enableVideoCapture()
    {
        capturingVideo = true;
        mediaManager.Instance.setStatusIndicator("Tap to start recording video");

        //clear source manager
        sourceManager.Instance.sourcePressed = false;
        recordingEnabled = true;

        fovHider.Instance.toggleFOVHider(true);
    }

    void startVideoCapture()
    {
        //mediaManager.Instance.vidRecorder.startRecordingVideo();
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
        //mediaManager.Instance.vidRecorder.StopRecordingVideo(false);
        Debug.Log("before status disable");
        mediaManager.Instance.disableStatusIndicator();
        mediaManager.Instance.recordingIndicator.SetActive(false);
        recordingInProgress = false;

        mediaManager.Instance.activateComment();

    }

    public virtual GameObject spawnVideoComment()
    {

        fovHider.Instance.toggleFOVHider(false);

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(videoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);

        commentSetup(spawnedComment.GetComponent<commentContents>());



        //define the comment type
        commentContents videoContent = spawnedComment.GetComponent<commentContents>();
        videoContent.isVideo = true;
        capturingVideo = false;

        videoContent.filepath = mediaManager.Instance.vidRecorder.filename;
        videoContent.LoadVideo();
        return spawnedComment;

    }

    public virtual GameObject spawnVideoCommentFromJSON()
    {

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(videoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);

        commentSetup(spawnedComment.GetComponent<commentContents>());



        //define the comment type
        commentContents videoContent = spawnedComment.GetComponent<commentContents>();
        videoContent.isVideo = true;
        capturingVideo = false;
        videoContent.LoadVideo();

        return spawnedComment;



    }

    public void enablePhotoCapture()
    {
        capturingPhoto = true;
        mediaManager.Instance.commentManager = GetComponent<commentManager>();
        mediaManager.Instance.setStatusIndicator("Tap to capture photo");

        //clear source manager
        sourceManager.Instance.sourcePressed = false;
        photoCaptureEnabled = true;


        fovHider.Instance.toggleFOVHider(true);
    }

    void capturePhoto()
    {
        mediaManager.Instance.disableStatusIndicator();
        photoCaptureEnabled = false;

        //capture photo, save it, activeMedia() when done
        mediaManager.Instance.photoRecorder.CapturePhoto();
    }

    public virtual GameObject spawnPhotoComment()
    {
        fovHider.Instance.toggleFOVHider(false);

        //shift all comments down
        repositionComments();

        //spawn simple comment
        spawnedComment = Instantiate(photoCommentPrefab, transform.position, Quaternion.identity);
        activeComments.Add(spawnedComment);

        commentSetup(spawnedComment.GetComponent<commentContents>());

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

        //define the comment type
        commentContents photoContent = spawnedComment.GetComponent<commentContents>();
        photoContent.isPhoto = true;
        capturingPhoto = false;
        


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


    public virtual  GameObject spawnNewCommentFromJSon()
    {
        GameObject output;

        startPos = CommmentStartPos.position;
        if (commentCount == 0)
        {
            commentParent.parent.gameObject.SetActive(true);
        }

        if (commentCount == 2)
        {
            //scrollBoxCollider.enabled = true;
        }

        for (int i = 0; i < activeComments.Count; i++)
        {
            activeComments[i].transform.position = new Vector3(activeComments[i].transform.position.x,
                                                                activeComments[i].transform.position.y - offsetDist,
                                                                activeComments[i].transform.position.z);
        }

        activeComments.Add((GameObject)Instantiate(simpleCommentPrefab, transform.position, Quaternion.identity));
        activeComments[commentCount].transform.SetParent(commentParent);
        activeComments[commentCount].transform.localScale = simpleCommentPrefab.transform.localScale;
        activeComments[commentCount].transform.position = startPos;
        activeComments[commentCount].transform.localRotation = CommmentStartPos.localRotation;
        output = activeComments[commentCount];
        GetComponent<nodeMediaHolder>().activeComments.Add(activeComments[commentCount]);
        activeComments[commentCount].GetComponent<commentContents>().linkedComponent = this.gameObject;
        commentCount += 1;
        return output;
    }

    void fieldActivator()
    {

        activeComments[commentCount].GetComponent<inputFieldManager>().activateField();
        commentCount += 1;


    }
}
