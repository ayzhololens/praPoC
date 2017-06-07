using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;
using System.Linq;
using RenderHeads.Media.AVProVideo;

//this script populates the violations items in the 2D offsite version reading from JU_databaseMan instance
//this also populates the comments contained within these violations prefabs
public class violationsParentSpawner : Singleton<violationsParentSpawner> {

    public int expandSize;
    public RectTransform VioBox;
    public GameObject violationPrefab;
    public GameObject parentObj;

    //for managing other 2D layouts 
    public collapsableManager bigBox { get; set; }
    public GameObject childItem;

    //for adding comments
    public GameObject addNewCommentBox;
    public InputField field;
    public Button addNoteDone;
    public MediaPlayer videoPlayer;
    public Button exButton;

    //for video only
    public GameObject hideThis;

    [System.Serializable]
    public class vioListItem
    {
        public GameObject vioPrefab;
        public List<GameObject> violationMedias = new List<GameObject>();
    }

    public List<vioListItem> spawnedVioPrefabs = new List<vioListItem>();

    //cam
    public CameraControlOffsite vioCam;

    //mediaPLayback
    public GameObject offsiteMediaWindow;
    public GameObject mediaPlane;
    public Material videoMaterial;
    public Material videoMaterialUI;
    public Material photoMaterial;
    public Material photoMaterialUI;
    public GameObject playButton;
    public cameraZoomOverTime guidedTargetObj;
    public GameObject simpleText;
    public GameObject diffBG;

    //minimapPlanePLayback
    public Material vioCamMat;
    public GameObject minimapPlane;

    // Use this for initialization
    void Start () {
        bigBox = collapsableManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void expandBox(int totalLines)
    {
        VioBox.sizeDelta = new Vector2(VioBox.rect.width, expandSize * totalLines);
        childItem.GetComponent<RectTransform>().localPosition = new Vector3(childItem.GetComponent<RectTransform>().localPosition.x,
                                                                        childItem.GetComponent<RectTransform>().localPosition.y - (expandSize*1),
                                                                        childItem.GetComponent<RectTransform>().localPosition.z);
        bigBox.startCollapse += expandSize;
        bigBox.readjustBox();
    }

    public void populateViolations()
    {
        int totalVio = 0;

        foreach(JU_databaseMan.ViolationsItem vio in JU_databaseMan.Instance.violationsManager.violations)
        {
            totalVio++;
            GameObject newItem;
            newItem = Instantiate(violationPrefab);
            newItem.transform.SetParent(parentObj.transform);
            newItem.GetComponent<RectTransform>().localPosition = new Vector3(0, (totalVio - 1) * -175, 0);
            newItem.GetComponent<RectTransform>().localScale = Vector3.one;
            newItem.GetComponent<violationsCollapseableBox>().vioBox = VioBox.gameObject;

            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addNewCommentWindow = addNewCommentBox;
            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().field = field;
            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addNoteDone = addNoteDone;
            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().exButton = exButton;

            newItem.GetComponent<violationsCollapseableBox>().childItems.Add(childItem);
            foreach (vioListItem vioPre in spawnedVioPrefabs)
            {
                vioPre.vioPrefab.GetComponent<violationsCollapseableBox>().childItems.Add(newItem);
            }
            expandBox(totalVio);

            newItem.GetComponent<violationsCollapseableBox>().vioInt = totalVio - 1;
            newItem.GetComponent<violationsCollapseableBox>().updateTitleContents();
            newItem.GetComponent<violationsCollapseableBox>().updateCollapseableContents();

            vioCam.lockCam();

            vioListItem newVio = new vioListItem();
            newVio.vioPrefab = newItem;
            populateVioComments(vio, newVio);
            spawnedVioPrefabs.Add(newVio);
        }
        bigBox.startCollapse += 25;
        bigBox.readjustBox();
    }

    public void populateVioComments(JU_databaseMan.ViolationsItem vio, vioListItem newVio)
    {
        List<JU_databaseMan.tempComment> commentsList = reorderCommentsByDate(vio);
        foreach(JU_databaseMan.tempComment comment in commentsList)
        {
            if (comment.type == 0)
            {
                GameObject newCom = newVio.vioPrefab.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addOneSimple(comment);
                newCom.GetComponent<offsiteMediaPlayer>().hideThis = hideThis;
                newCom.GetComponent<offsiteMediaPlayer>().mediaPlane = mediaPlane;

                newCom.GetComponent<offsiteFieldItemValueHolder>().meta.text = (comment.date
                                                                                + " - "
                                                                                + comment.user);
                newCom.GetComponent<offsiteFieldItemValueHolder>().content.text = comment.content;
                newCom.GetComponent<offsiteFieldItemValueHolder>().user = comment.user;
                newCom.GetComponent<offsiteFieldItemValueHolder>().date = comment.date;
                newCom.GetComponent<offsiteFieldItemValueHolder>().nodeIndex = vio.nodeIndex;
                newCom.GetComponent<offsiteFieldItemValueHolder>().comment = comment;

                newCom.GetComponent<offsiteMediaPlayer>().photoMaterial = photoMaterial;
                newCom.GetComponent<offsiteMediaPlayer>().mediaWindow = offsiteMediaWindow;
                newCom.GetComponent<offsiteMediaPlayer>().mediaPlane = mediaPlane;
                newCom.GetComponent<offsiteMediaPlayer>().guidedTargetObj = guidedTargetObj;
                newCom.GetComponent<offsiteMediaPlayer>().videoPlayer = videoPlayer;
                newCom.GetComponent<offsiteMediaPlayer>().playButton = playButton;
                newCom.GetComponent<offsiteMediaPlayer>().hideThis = hideThis;
                newCom.GetComponent<offsiteMediaPlayer>().simpleText = simpleText;
                newCom.GetComponent<offsiteMediaPlayer>().diffBG = diffBG;
                newCom.GetComponent<offsiteMediaPlayer>().commentType = 0;

                newVio.violationMedias.Add(newCom);
            }
            else if (comment.type == 1)
            {
                violationsCollapseableBox box = newVio.vioPrefab.GetComponent<violationsCollapseableBox>();
                GameObject newCom = box.addObject.GetComponent<addCommentButton>().addOnePhoto(comment);

                newCom.GetComponent<offsiteFieldItemValueHolder>().meta.text = (comment.date
                                                                                + " - "
                                                                                + comment.user);
                newCom.GetComponent<offsiteFieldItemValueHolder>().content.text = comment.content;
                newCom.GetComponent<offsiteFieldItemValueHolder>().user = comment.user;
                newCom.GetComponent<offsiteFieldItemValueHolder>().date = comment.date;
                newCom.GetComponent<offsiteFieldItemValueHolder>().nodeIndex = vio.nodeIndex;
                newCom.GetComponent<offsiteFieldItemValueHolder>().comment = comment;

                newCom.GetComponent<offsiteMediaPlayer>().photoMaterial = photoMaterial;
                newCom.GetComponent<offsiteMediaPlayer>().mediaWindow = offsiteMediaWindow;
                newCom.GetComponent<offsiteMediaPlayer>().mediaPlane = mediaPlane;
                newCom.GetComponent<offsiteMediaPlayer>().guidedTargetObj = guidedTargetObj;
                newCom.GetComponent<offsiteMediaPlayer>().videoPlayer = videoPlayer;
                newCom.GetComponent<offsiteMediaPlayer>().playButton = playButton;
                newCom.GetComponent<offsiteMediaPlayer>().hideThis = hideThis;
                newCom.GetComponent<offsiteMediaPlayer>().simpleText = simpleText;
                newCom.GetComponent<offsiteMediaPlayer>().diffBG = diffBG;
                newCom.GetComponent<offsiteMediaPlayer>().commentType = 1;

                newVio.violationMedias.Add(newCom);
            }
            else if (comment.type == 2)
            {
                newVio.vioPrefab.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().videoPlayer = videoPlayer;
                GameObject newCom =  newVio.vioPrefab.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addOneVideo(comment);

                newCom.GetComponent<offsiteFieldItemValueHolder>().meta.text = (comment.date
                                                                + " - "
                                                                + comment.user);
                newCom.GetComponent<offsiteFieldItemValueHolder>().content.text = comment.content;
                newCom.GetComponent<offsiteFieldItemValueHolder>().user = comment.user;
                newCom.GetComponent<offsiteFieldItemValueHolder>().date = comment.date;
                newCom.GetComponent<offsiteFieldItemValueHolder>().nodeIndex = vio.nodeIndex;
                newCom.GetComponent<offsiteFieldItemValueHolder>().comment = comment;

                newCom.GetComponent<offsiteMediaPlayer>().videoMaterial = videoMaterial;
                newCom.GetComponent<offsiteMediaPlayer>().mediaWindow = offsiteMediaWindow;
                newCom.GetComponent<offsiteMediaPlayer>().mediaPlane = mediaPlane;
                newCom.GetComponent<offsiteMediaPlayer>().guidedTargetObj = guidedTargetObj;
                newCom.GetComponent<offsiteMediaPlayer>().videoPlayer = videoPlayer;
                newCom.GetComponent<offsiteMediaPlayer>().playButton = playButton;
                newCom.GetComponent<offsiteMediaPlayer>().hideThis = hideThis;
                newCom.GetComponent<offsiteMediaPlayer>().simpleText = simpleText;
                newCom.GetComponent<offsiteMediaPlayer>().diffBG = diffBG;
                newCom.GetComponent<offsiteMediaPlayer>().commentType = 2;

                newVio.violationMedias.Add(newCom);
            }
        }

    }

    public virtual List<JU_databaseMan.tempComment> reorderCommentsByDate(JU_databaseMan.ViolationsItem vio)
    {
        List<JU_databaseMan.tempComment> tempVioCommentsList = new List<JU_databaseMan.tempComment>();
        List<JU_databaseMan.tempComment> vioCommentsList = new List<JU_databaseMan.tempComment>();
        List<JU_databaseMan.tempComment> returnValue = new List<JU_databaseMan.tempComment>();

        foreach (JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
        {
            if (node.indexNum == vio.nodeIndex)
            {
                foreach (JU_databaseMan.media media in node.videos)
                {
                    JU_databaseMan.tempComment tempComment = new JU_databaseMan.tempComment();
                    tempComment.date = media.date;
                    tempComment.user = media.user;
                    tempComment.path = media.path;
                    tempComment.type = 2;
                    tempVioCommentsList.Add(tempComment);
                }
                foreach (JU_databaseMan.media media in node.photos)
                {
                    JU_databaseMan.tempComment tempComment = new JU_databaseMan.tempComment();
                    tempComment.date = media.date;
                    tempComment.user = media.user;
                    tempComment.path = media.path;
                    tempComment.type = 1;
                    tempVioCommentsList.Add(tempComment);
                }
                foreach (JU_databaseMan.comment comment in node.comments)
                {
                    JU_databaseMan.tempComment tempComment = new JU_databaseMan.tempComment();
                    tempComment.date = comment.date;
                    tempComment.user = comment.user;
                    tempComment.content = comment.content;
                    tempComment.type = 0;
                    tempVioCommentsList.Add(tempComment);
                }
                vioCommentsList = tempVioCommentsList.OrderBy(si => si.date).ToList();

            }
        }
        foreach (JU_databaseMan.tempComment comment in vioCommentsList.Reverse<JU_databaseMan.tempComment>())
        {
            returnValue.Add(comment);
        }
        return returnValue;
    }
}
