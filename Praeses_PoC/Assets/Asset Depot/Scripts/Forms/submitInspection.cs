using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class submitInspection : MonoBehaviour {

    [Header ("Violations")]
    [Tooltip("Prefab for submitted violation")]
    public GameObject submittedVioPrefab;
    [Tooltip ("Position for submitted violation")]
    public Transform vioPos;
    GameObject spawnedVioPreview;
    int vioPreviewCount;
    violationController activeController;
    [Tooltip ("Distance between submitted violations")]
    public float vioOffsetDist;

    [Header ("Changed Values")]
    [Tooltip ( "Prefab for changed value field")]
    public GameObject changedValuePrefab;
    [Tooltip ("Positon for changed value field")]
    public Transform changedValuePos;
    GameObject spawnedChangedValue;
    int changedValueCount;
    [Tooltip ("Distance between changed value fields")]
    public float changesOffsetDist;

    [Tooltip ("Position for Issue Cert")]
    public Transform issuePos;
    [Tooltip ("Section parents that will get moved down as fields are added")]
    public Transform[] contents;

    
    /// <summary>
    /// Visual feedback for submitted inspection and syncing to JSON
    /// </summary>
    public void submitInspect()
    {
        submitState.Instance.startUpload();
        databaseMan.Instance.saveCmd();
    }
    

    /// <summary>
    /// This spawns a view violation preview when a violation is submitted
    /// </summary>
    /// <param name="index"></param>
    /// <param name="vioCont"></param>
    /// <param name="linkedPrev"></param>
    public void addVioPreview(violationController vioCont, vioPreviewComponent linkedPrev )
    {

        //Set transform values
        Vector3 offset = new Vector3(vioPos.localPosition.x, vioPos.localPosition.y - (vioPreviewCount * vioOffsetDist), vioPos.localPosition.z);
        spawnedVioPreview = Instantiate(submittedVioPrefab, transform.position, Quaternion.identity);
        spawnedVioPreview.transform.SetParent(vioPos.parent);
        spawnedVioPreview.transform.localPosition = offset;
        spawnedVioPreview.transform.localScale = submittedVioPrefab.transform.localScale;
        spawnedVioPreview.transform.localRotation = submittedVioPrefab.transform.localRotation;

        //add this linked preview to the violation preview
        linkedPrev.linkedPreviews.Add(spawnedVioPreview);

        //move all other sections down
        for (int i = 1; i < contents.Length; i++)
        {
            contents[i].localPosition = new Vector3(contents[i].localPosition.x, contents[i].localPosition.y - vioOffsetDist, contents[i].localPosition.z);
        }


        vioPreviewComponent vioPreview = spawnedVioPreview.GetComponent<vioPreviewComponent>();
        activeController = vioCont;
        vioPreview.vioController = vioCont;

        //Sets the violation value instead of the user
        vioPreview.user.text = 
            vioCont.violationIndices[0] + "." +
            vioCont.violationIndices[1] + "." +
            vioCont.violationIndices[2] + " " + vioCont.violationData[2];
        //Dont attach text for the date
        vioPreview.date.text = " ";


        vioPreviewCount += 1;
    }



   
    /// <summary>
    /// Creates and links a changed value field to an inspection field 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="preVal"></param>
    /// <param name="curVal"></param>
    /// <param name="linkedField"></param>
    public void addChangedValue(string name, string preVal, string curVal, formFieldController linkedField)
    {

        //set transforms 
        Vector3 offset = new Vector3(changedValuePos.localPosition.x, changedValuePos.localPosition.y - (changedValueCount * changesOffsetDist), changedValuePos.localPosition.z);
        spawnedChangedValue = Instantiate(changedValuePrefab, transform.position, Quaternion.identity);
        spawnedChangedValue.transform.SetParent(changedValuePos.parent);
        spawnedChangedValue.transform.localPosition = offset;
        spawnedChangedValue.transform.localScale = changedValuePrefab.transform.localScale;
        spawnedChangedValue.transform.localRotation = changedValuePrefab.transform.localRotation;

        linkedField.deltaField = spawnedChangedValue;

        changedValueCount += 1;


        //set values
        fieldChangedValueComponent changedVal = spawnedChangedValue.GetComponent<fieldChangedValueComponent>();
        changedVal.fieldName.text = name;
        changedVal.currValue.text = curVal;
        changedVal.previousValue.text = preVal;


        //move other sections down
        for (int i = 2; i < contents.Length; i++)
        {
            contents[i].localPosition = new Vector3(contents[i].localPosition.x, contents[i].localPosition.y - vioOffsetDist, contents[i].localPosition.z);
        }

    }
}
