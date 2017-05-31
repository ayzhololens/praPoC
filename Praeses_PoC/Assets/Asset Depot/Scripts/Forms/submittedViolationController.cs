using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class submittedViolationController : MonoBehaviour {

    public violationController vioController;
    public GameObject submittedVioPrefab;
    public Transform submittedVioPos;
    public Transform goToResolveVioButton;
    public GameObject resolutionField;
    GameObject spawnedPreview;
    commentManager vioComManager;
    public int tempIndex { get; set; }
    public string resName { get; set; }




    public float offsetDist;
    
    int previewCount;

	// Use this for initialization
	void Start () {
        vioComManager = vioController.gameObject.GetComponent<commentManager>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setIndex(int index)
    {
        tempIndex = index;
    }

    public void submit()
    {

        vioController.violationData[7] = resName;
        vioController.violationIndices[7] = tempIndex;
        violatoinSpawner.Instance.successContentHolder.SetActive(true);
        violatoinSpawner.Instance.successContentHolder.transform.position = this.transform.position;
        databaseMan.Instance.updateVio(vioController);
    }

    public void addPreview(int index)
    {
            if(index == 10)
            {
                index = tempIndex;
            }

        Vector3 offset = new Vector3(submittedVioPos.localPosition.x, submittedVioPos.localPosition.y - (previewCount * offsetDist), submittedVioPos.localPosition.z);
        spawnedPreview = Instantiate(submittedVioPrefab, transform.position, Quaternion.identity);
        spawnedPreview.transform.SetParent(submittedVioPos.parent);
        spawnedPreview.transform.localPosition = offset;
        spawnedPreview.transform.localScale = submittedVioPrefab.transform.localScale;
        spawnedPreview.transform.localRotation = submittedVioPrefab.transform.localRotation;
        vioPreviewComponent vioPreview = spawnedPreview.GetComponent<vioPreviewComponent>();
        vioPreview.setResolution(index);
        vioPreview.user.text = metaManager.Instance.user;
        vioPreview.date.text = metaManager.Instance.date();


        goToResolveVioButton.localPosition = new Vector3(goToResolveVioButton.localPosition.x, goToResolveVioButton.localPosition.y-offsetDist, goToResolveVioButton.localPosition.z);
        previewCount += 1;

        //setPreviewComments();
        Invoke("setPreviewComments", .5f);

        formController.Instance.submitInspection.addVioPreview(index, vioController);
        viewViolationController.Instance.addVioPreview(index, vioController,vioController.fromJson);

    }

    public void setPreviewComments()
    {

        int sCounter = 0;
        int pCounter = 0;
        int vCounter = 0;


            foreach (GameObject activeComment in vioController.gameObject.GetComponent<commentManager>().activeComments)
            {
                if (activeComment.GetComponent<commentContents>().isSimple)
                {

                    sCounter += 1;
                    GameObject simpleComment = spawnedPreview.GetComponent<commentManager>().spawnSimpleCommentFromJSON();
                    simpleComment.GetComponent<commentContents>().commentMain.text = activeComment.GetComponent<commentContents>().commentMain.text;
                    simpleComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                    simpleComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;

                }
                if (activeComment.GetComponent<commentContents>().isVideo)
                {
                    vCounter += 1;
                    GameObject videoComment = spawnedPreview.GetComponent<commentManager>().spawnVideoCommentFromJSON(activeComment.GetComponent<commentContents>().filepath);
                    videoComment.GetComponent<commentContents>().vidThumbnail = activeComment.GetComponent<commentContents>().vidThumbnail;
                    videoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                    videoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;
                }
                if (activeComment.GetComponent<commentContents>().isPhoto)
                {

                    pCounter += 1;
                    GameObject photoComment = spawnedPreview.GetComponent<commentManager>().spawnPhotoCommentFromJSON();
                    photoComment.GetComponent<Renderer>().material.mainTexture = activeComment.GetComponent<Renderer>().material.mainTexture;
                    photoComment.GetComponent<commentContents>().filepath = activeComment.GetComponent<commentContents>().filepath;
                    photoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                    photoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;
                }


            }
        

     



        vioPreviewComponent preview = spawnedPreview.GetComponent<vioPreviewComponent>();
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


    public void updateComManager()
    {
        foreach (GameObject activeComment in vioComManager.activeComments)
        {
            Destroy(activeComment);
        }
        vioComManager.activeComments.Clear();

        foreach (GameObject activeComment in resolutionField.GetComponent<commentManager>().activeComments)
        {
            if (activeComment.GetComponent<commentContents>().isSimple)
            {
                GameObject simpleComment = vioComManager.spawnSimpleCommentFromJSON();
                simpleComment.GetComponent<commentContents>().commentMain.text = activeComment.GetComponent<commentContents>().commentMain.text;
                simpleComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                simpleComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;

                // resolveComs.Add(simpleComment);
            }
            if (activeComment.GetComponent<commentContents>().isVideo)
            {
                GameObject videoComment = vioComManager.spawnVideoCommentFromJSON(activeComment.GetComponent<commentContents>().filepath);
                videoComment.GetComponent<commentContents>().vidThumbnail = activeComment.GetComponent<commentContents>().vidThumbnail;
                videoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                videoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;
                // resolveComs.Add(videoComment);
            }
            if (activeComment.GetComponent<commentContents>().isPhoto)
            {
                GameObject photoComment = vioComManager.spawnPhotoCommentFromJSON();
                photoComment.GetComponent<Renderer>().material.mainTexture = activeComment.GetComponent<Renderer>().material.mainTexture;
                photoComment.GetComponent<commentContents>().filepath = activeComment.GetComponent<commentContents>().filepath;
                photoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                photoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;

                //resolveComs.Add(photoComment);
            }
        }
    }

    public void setResolveFieldComments()
    {



            List<GameObject> resolveComs = new List<GameObject>();
        int sCounter = 0;
        int pCounter = 0;
        int vCounter = 0;
        foreach (GameObject activeComment in resolutionField.GetComponent<commentManager>().activeComments)
        {
            resolutionField.GetComponent<commentManager>().activeComments.Remove(activeComment);
            Destroy(activeComment);
        }




            foreach (GameObject activeComment in vioController.gameObject.GetComponent<commentManager>().activeComments)
        {
            if (activeComment.GetComponent<commentContents>().isSimple)
            {
                sCounter += 1;
                GameObject simpleComment = resolutionField.GetComponent<commentManager>().spawnSimpleCommentFromJSON();
                simpleComment.GetComponent<commentContents>().commentMain.text = activeComment.GetComponent<commentContents>().commentMain.text;
                simpleComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                simpleComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;

               // resolveComs.Add(simpleComment);
            }
            if (activeComment.GetComponent<commentContents>().isVideo)
            {
                vCounter += 1;
                GameObject videoComment = resolutionField.GetComponent<commentManager>().spawnVideoCommentFromJSON(activeComment.GetComponent<commentContents>().filepath);
                videoComment.GetComponent<commentContents>().vidThumbnail = activeComment.GetComponent<commentContents>().vidThumbnail;
                videoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                videoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;
               // resolveComs.Add(videoComment);
            }
            if (activeComment.GetComponent<commentContents>().isPhoto)
            {
                pCounter += 1;
                GameObject photoComment = resolutionField.GetComponent<commentManager>().spawnPhotoCommentFromJSON();
                photoComment.GetComponent<Renderer>().material.mainTexture = activeComment.GetComponent<Renderer>().material.mainTexture;
                photoComment.GetComponent<commentContents>().filepath = activeComment.GetComponent<commentContents>().filepath;
                photoComment.GetComponent<commentContents>().commentMetaDate.text = activeComment.GetComponent<commentContents>().commentMetaDate.text;
                photoComment.GetComponent<commentContents>().commentMetaUser.text = activeComment.GetComponent<commentContents>().commentMetaUser.text;

                //resolveComs.Add(photoComment);
            }


        }


    }
}
