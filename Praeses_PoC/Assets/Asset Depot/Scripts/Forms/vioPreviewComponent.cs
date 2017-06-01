using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class vioPreviewComponent : MonoBehaviour
{

    public Color[] resolutionColors;
    public GameObject resolutionBox;
    public violationController vioController { get; set; }
    public string[] resolutionOptions;
    public Text resolutionText;
    public Text user;
    public Text date;
    public GameObject[] commentText;
    public float commentOffset;
    public List<GameObject> linkedPreviews;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setResolution(int index)
    {
        resolutionBox.GetComponent<Renderer>().material.color = resolutionColors[index];
        resolutionText.text = resolutionOptions[index];
        Invoke("setPreviewComments", .5f);
    }

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

