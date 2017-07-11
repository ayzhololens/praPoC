using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class viewViolationController : Singleton<viewViolationController>
{ 
    public GameObject contentHolder;

    [Tooltip ("Prefab for a submitted violation")]
    public GameObject submittedVioPrefab;
    int vioPreviewCount;
    GameObject spawnedVioPreview;
    violationController activeController;

    [Tooltip ("Sections to move down")]
    public Transform[] contents;

    public Transform pastVioPos;
    public Transform newVioPos;
    public float vioOffsetDist;
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vioCont"></param>
    /// <param name="fromJson"></param>
    /// <param name="linkedPrev"></param>
    public void addVioPreview(violationController vioCont, bool fromJson, vioPreviewComponent linkedPrev)
    {

        Transform vioPos;

        //determine where to put it based on where its coming from
        if (fromJson)
        {
            vioPos = pastVioPos;
        }
        else
        {
            vioPos = newVioPos;
        }

        //set transforms
        Vector3 offset = new Vector3(vioPos.localPosition.x, vioPos.localPosition.y - (vioPreviewCount * vioOffsetDist), vioPos.localPosition.z);
        spawnedVioPreview = Instantiate(submittedVioPrefab, transform.position, Quaternion.identity);
        spawnedVioPreview.transform.SetParent(vioPos.parent);
        spawnedVioPreview.transform.localPosition = offset;
        spawnedVioPreview.transform.localScale = submittedVioPrefab.transform.localScale;
        spawnedVioPreview.transform.localRotation = submittedVioPrefab.transform.localRotation;

        linkedPrev.linkedPreviews.Add(spawnedVioPreview);


        //move other contents down
        if (!fromJson)
        {
            for (int i = 0; i < 1; i++)
            {

                contents[i].localPosition = new Vector3(contents[i].localPosition.x, contents[i].localPosition.y - (4*vioOffsetDist), contents[i].localPosition.z);
            }
        }
        else
        {
            for (int i = 1; i < contents.Length; i++)
            {
                contents[i].localPosition = new Vector3(contents[i].localPosition.x, contents[i].localPosition.y - vioOffsetDist, contents[i].localPosition.z);
            }
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




    public void toggleContent()
    {
        contentHolder.SetActive(!contentHolder.activeSelf);
    }
}
