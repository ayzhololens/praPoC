using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class vioPreviewComponent : MonoBehaviour
{

    public violationController vioController { get; set; }

    [Header("Layout")]
    [Tooltip ("Box for resolution color display")]
    public GameObject resolutionBox;
    [Tooltip ("Acceptable statuses")]
    public string[] resolutionOptions;
    [Tooltip ("Acceptable status colors")]
    public Color[] resolutionColors;
    [Tooltip("Status display")]
    public Text resolutionText;
    public Text user;
    public Text date;

    [Header ("Comments")]
    [Tooltip ("Contains the sprites and text components for displaying comment amount")]
    public GameObject[] commentText;
    [Tooltip ("Distance between comment texts")]
    public float commentOffset;
    public List<GameObject> linkedPreviews;


    /// <summary>
    /// Set the resolution based on the selected value.  Tell all the linked previews to do the same thing.  
    /// Only the preview on submitted violation actually has links so this wont create a loop
    /// </summary>
    /// <param name="index"></param>
    public void setResolution(int index)
    {
        resolutionBox.GetComponent<Renderer>().material.color = resolutionColors[index];
        resolutionText.text = resolutionOptions[index];
        Invoke("setPreviewComments", .5f);
        updateLinks(index);
    }


    /// <summary>
    /// View violation and submit inspection previews are told to update
    /// </summary>
    /// <param name="index"></param>
    public void updateLinks(int index)
    {
        if (linkedPreviews.Count > 0)
        {
            for (int i = 0; i < linkedPreviews.Count; i++)
            {
                linkedPreviews[i].GetComponent<vioPreviewComponent>().setResolution(index);

            }
        }
    }

    /// <summary>
    /// Grab all the comments from the main violation controller and then duplicate them and put them on the vio previews
    /// </summary>
    public void setPreviewComments()
    {

        int sCounter = 0;
        int pCounter = 0;
        int vCounter = 0;

        foreach (GameObject activeComment in GetComponent<commentManager>().activeComments)
        {
            Destroy(activeComment);
        }
        GetComponent<commentManager>().activeComments.Clear();


        foreach (GameObject activeComment in vioController.gameObject.GetComponent<commentManager>().activeComments)
        {
            if (activeComment.GetComponent<commentContents>().isSimple)
            {

                sCounter += 1;
                GameObject simpleComment = GetComponent<commentManager>().spawnSimpleCommentFromJSON();
                simpleComment.GetComponent<commentContents>().commentMain.text = activeComment.GetComponent<commentContents>().commentMain.text;
                simpleComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                simpleComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;

            }
            if (activeComment.GetComponent<commentContents>().isVideo)
            {
                vCounter += 1;
                GameObject videoComment = GetComponent<commentManager>().addVideoComment(activeComment);
                videoComment.GetComponent<commentContents>().thumbMat = activeComment.GetComponent<commentContents>().thumbMat;
                videoComment.GetComponent<commentContents>().vidThumbnail = activeComment.GetComponent<commentContents>().vidThumbnail; videoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                videoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;
            }
            if (activeComment.GetComponent<commentContents>().isPhoto)
            {

                pCounter += 1;
                GameObject photoComment = GetComponent<commentManager>().spawnPhotoCommentFromJSON();
                photoComment.GetComponent<Renderer>().material.mainTexture = activeComment.GetComponent<Renderer>().material.mainTexture;
                photoComment.GetComponent<commentContents>().filepath = activeComment.GetComponent<commentContents>().filepath;
                photoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                photoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;
            }


        }






        commentText[1].GetComponent<Text>().text = pCounter.ToString() + "x";
        commentText[0].GetComponent<Text>().text = sCounter.ToString() + "x";
        commentText[2].GetComponent<Text>().text = vCounter.ToString() + "x";
    }
}

