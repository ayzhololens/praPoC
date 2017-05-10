using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class submitInspection : MonoBehaviour {
    public Transform vioPos;
    public GameObject submittedVioPrefab;
    public GameObject changedValuePrefab;
    public Transform changedValuePos;
    public Transform issuePos;
    int changedValueCount;
    int vioPreviewCount;
    GameObject spawnedVioPreview;
    GameObject spawnedChangedValue;
    violationController activeController;
    public float vioOffsetDist;
    public float changesOffsetDist;
    public Transform[] contents;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

    public void addVioPreview(int index, violationController vioCont)
    {
        Vector3 offset = new Vector3(vioPos.position.x, vioPos.position.y - (vioPreviewCount * vioOffsetDist), vioPos.position.z);
        spawnedVioPreview = Instantiate(submittedVioPrefab, offset, Quaternion.identity);
        spawnedVioPreview.transform.SetParent(vioPos.parent);
        spawnedVioPreview.transform.localScale = submittedVioPrefab.transform.localScale;
        spawnedVioPreview.transform.localRotation = submittedVioPrefab.transform.localRotation;
        vioPreviewComponent vioPreview = spawnedVioPreview.GetComponent<vioPreviewComponent>();
        activeController = vioCont;
        vioPreview.setResolution(index);
        vioPreview.user.text = 
            vioCont.violationIndices[0] + "." +
            vioCont.violationIndices[1] + "." +
            vioCont.violationIndices[2] + " " + vioCont.violationData[2];
        vioPreview.date.text = " ";

        for(int i =1; i<contents.Length; i++)
        {
            contents[i].position = new Vector3(contents[i].position.x, contents[i].position.y - vioOffsetDist, contents[i].position.z);
        }
        vioPreviewCount += 1;
        Invoke("setPreviewComments", 1);
        //setPreviewComments();
    }
    public void setPreviewComments()
    {
        int sCounter = 0;
        int pCounter = 0;
        int vCounter = 0;

        foreach (GameObject activeComment in activeController.gameObject.GetComponent<commentManager>().activeComments)
        {
            if (activeComment.GetComponent<commentContents>().isSimple)
            {

                sCounter += 1;
                GameObject simpleComment = spawnedVioPreview.GetComponent<commentManager>().spawnSimpleCommentFromJSON();
                simpleComment.GetComponent<commentContents>().commentMain.text = activeComment.GetComponent<commentContents>().commentMain.text;
                simpleComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                simpleComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;


            }
            if (activeComment.GetComponent<commentContents>().isVideo)
            {
                vCounter += 1;
                GameObject videoComment = spawnedVioPreview.GetComponent<commentManager>().spawnVideoCommentFromJSON();
                videoComment.GetComponent<commentContents>().filepath = activeComment.GetComponent<commentContents>().filepath;
                videoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                videoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;


            }
            if (activeComment.GetComponent<commentContents>().isPhoto)
            {
                pCounter += 1;
                GameObject photoComment = spawnedVioPreview.GetComponent<commentManager>().spawnPhotoCommentFromJSON();
                photoComment.GetComponent<Renderer>().material.mainTexture = activeComment.GetComponent<Renderer>().material.mainTexture;
                photoComment.GetComponent<commentContents>().filepath = activeComment.GetComponent<commentContents>().filepath;
                photoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                photoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;


            }
        }

        vioPreviewComponent preview = spawnedVioPreview.GetComponent<vioPreviewComponent>();
        if (pCounter > 0)
        {
            preview.commentText[1].SetActive(true);
            preview.commentText[1].transform.localPosition = preview.commentPos.localPosition;
            preview.commentText[1].GetComponent<Text>().text = pCounter.ToString() + "x";
            if (sCounter > 0)
            {
                preview.commentPos.localPosition = new Vector3(preview.commentPos.localPosition.x + preview.commentOffset, preview.commentPos.localPosition.y, preview.commentPos.localPosition.z);

            }
            if (vCounter > 0)
            {
                preview.commentPos.localPosition = new Vector3(preview.commentPos.localPosition.x + preview.commentOffset, preview.commentPos.localPosition.y, preview.commentPos.localPosition.z);

            }
        }
        if (sCounter > 0)
        {

            preview.commentText[0].SetActive(true);
            preview.commentText[0].transform.localPosition = preview.commentPos.localPosition;
            preview.commentText[0].GetComponent<Text>().text = sCounter.ToString() + "x";
            if (vCounter > 0)
            {
                preview.commentPos.localPosition = new Vector3(preview.commentPos.position.x + preview.commentOffset, preview.commentPos.localPosition.y, preview.commentPos.localPosition.z);

            }
        }
        if (vCounter > 0)
        {

            preview.commentText[2].SetActive(true);
            preview.commentText[2].transform.localPosition = preview.commentPos.localPosition;
            preview.commentText[2].GetComponent<Text>().text = vCounter.ToString() + "x";
        }



    }
   

    public void addChangedValue(string name, string preVal, string curVal)
    {
        Vector3 offset = new Vector3(changedValuePos.position.x, changedValuePos.position.y - (changedValueCount * changesOffsetDist), changedValuePos.position.z);
        spawnedChangedValue = Instantiate(changedValuePrefab, offset, Quaternion.identity);
        spawnedChangedValue.transform.SetParent(changedValuePos.parent);
        spawnedChangedValue.transform.localScale = changedValuePrefab.transform.localScale;
        spawnedChangedValue.transform.localRotation = changedValuePrefab.transform.localRotation;
        changedValueCount += 1;

        fieldChangedValueComponent changedVal = spawnedChangedValue.GetComponent<fieldChangedValueComponent>();
        changedVal.fieldName.text = name;
        changedVal.currValue.text = curVal;
        changedVal.previousValue.text = preVal;

        for (int i = 2; i < contents.Length; i++)
        {
            contents[i].position = new Vector3(contents[i].position.x, contents[i].position.y - vioOffsetDist, contents[i].position.z);
        }

    }
}
