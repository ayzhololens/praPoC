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

    public void submitInspect()
    {
        submitState.Instance.startUpload();
        databaseMan.Instance.saveCmd();
    }
    

    public void addVioPreview(int index, violationController vioCont, vioPreviewComponent linkedPrev )
    {

        Vector3 offset = new Vector3(vioPos.localPosition.x, vioPos.localPosition.y - (vioPreviewCount * vioOffsetDist), vioPos.localPosition.z);
        spawnedVioPreview = Instantiate(submittedVioPrefab, transform.position, Quaternion.identity);
        spawnedVioPreview.transform.SetParent(vioPos.parent);
        spawnedVioPreview.transform.localPosition = offset;
        spawnedVioPreview.transform.localScale = submittedVioPrefab.transform.localScale;
        spawnedVioPreview.transform.localRotation = submittedVioPrefab.transform.localRotation;

        linkedPrev.linkedPreviews.Add(spawnedVioPreview);

        for (int i = 1; i < contents.Length; i++)
        {
            contents[i].localPosition = new Vector3(contents[i].localPosition.x, contents[i].localPosition.y - vioOffsetDist, contents[i].localPosition.z);
        }


        vioPreviewComponent vioPreview = spawnedVioPreview.GetComponent<vioPreviewComponent>();
        activeController = vioCont;
        vioPreview.vioController = vioCont;
        //vioPreview.setResolution(index);
        vioPreview.user.text = 
            vioCont.violationIndices[0] + "." +
            vioCont.violationIndices[1] + "." +
            vioCont.violationIndices[2] + " " + vioCont.violationData[2];
        vioPreview.date.text = " ";


        vioPreviewCount += 1;
        //Invoke("setPreviewComments", 1);
        //setPreviewComments();
    }



   

    public void addChangedValue(string name, string preVal, string curVal, formFieldController linkedField)
    {
        Vector3 offset = new Vector3(changedValuePos.localPosition.x, changedValuePos.localPosition.y - (changedValueCount * changesOffsetDist), changedValuePos.localPosition.z);
        spawnedChangedValue = Instantiate(changedValuePrefab, transform.position, Quaternion.identity);
        spawnedChangedValue.transform.SetParent(changedValuePos.parent);
        spawnedChangedValue.transform.localPosition = offset;
        spawnedChangedValue.transform.localScale = changedValuePrefab.transform.localScale;
        spawnedChangedValue.transform.localRotation = changedValuePrefab.transform.localRotation;

        linkedField.deltaField = spawnedChangedValue;

        changedValueCount += 1;

        fieldChangedValueComponent changedVal = spawnedChangedValue.GetComponent<fieldChangedValueComponent>();
        changedVal.fieldName.text = name;
        changedVal.currValue.text = curVal;
        changedVal.previousValue.text = preVal;

        for (int i = 2; i < contents.Length; i++)
        {
            contents[i].localPosition = new Vector3(contents[i].localPosition.x, contents[i].localPosition.y - vioOffsetDist, contents[i].localPosition.z);
        }

    }
}
