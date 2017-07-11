using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class submittedViolationController : MonoBehaviour {


    [Tooltip ("Top level violation controller")]
    public violationController vioController;
    [Tooltip ("Prefab for the submitted violation previews")]
    public GameObject submittedVioPrefab;
    [Tooltip ("Positon for the submitted violation")]
    public Transform submittedVioPos;
    [Tooltip ("Continue button that gets moved down as more vio previews are added")]
    public Transform goToResolveVioButton;


    vioPreviewComponent vioPreview;
    GameObject spawnedPreview;
    commentManager vioComManager;
    public int tempIndex { get; set; }
    public string resName { get; set; }




    public float offsetDist;
    

	// Use this for initialization
	void Start () {
        vioComManager = vioController.gameObject.GetComponent<commentManager>();

    }
	

    /// <summary>
    /// Called on the actual button that gets pressed when picking a violation resolution
    /// </summary>
    /// <param name="index"></param>
    public void setIndex(int index)
    {
        tempIndex = index;
    }


    /// <summary>
    /// Stores the index and resolution name in the violation controller.  Updates the JSON
    /// </summary>
    public void submit()
    {

        vioController.violationData[7] = resName;
        vioController.violationIndices[7] = tempIndex;
        vioControl.Instance.successContentHolder.SetActive(true);
        vioControl.Instance.successContentHolder.transform.position = this.transform.position;
        databaseMan.Instance.updateVio(vioController);
    }

    public void addPreview(int index)
    {
            if(index == 10)
            {
                index = tempIndex;
            }


        //check if there's already a viopreview.  If not, spawn new one
        if (spawnedPreview == null)
        {

            //Set transform values
            Vector3 offset = new Vector3(submittedVioPos.localPosition.x, submittedVioPos.localPosition.y, submittedVioPos.localPosition.z);
            spawnedPreview = Instantiate(submittedVioPrefab, transform.position, Quaternion.identity);
            spawnedPreview.transform.SetParent(submittedVioPos.parent);
            spawnedPreview.transform.localPosition = offset;
            spawnedPreview.transform.localScale = submittedVioPrefab.transform.localScale;
            spawnedPreview.transform.localRotation = submittedVioPrefab.transform.localRotation;

            //resposition the continue button
            goToResolveVioButton.localPosition = new Vector3(goToResolveVioButton.localPosition.x, goToResolveVioButton.localPosition.y - offsetDist, goToResolveVioButton.localPosition.z);

            //Spawn similar previews in the submit inspection page and the view violations section
            vioPreview = spawnedPreview.GetComponent<vioPreviewComponent>();
            formController.Instance.submitInspection.addVioPreview(vioController, vioPreview);
            viewViolationController.Instance.addVioPreview(vioController, vioController.fromJson, vioPreview);

            //set vio preview components
            vioPreview.vioController = vioController;
            vioPreview.setResolution(index);
            vioPreview.user.text = metaManager.Instance.user;
            vioPreview.date.text = metaManager.Instance.date();
        }

        //if yes then just update the data 
        else
        {
            vioPreview.setResolution(index);
            vioPreview.user.text = metaManager.Instance.user;
            vioPreview.date.text = metaManager.Instance.date();
            vioPreview.updateLinks(index);
        }

        
    }

    
}
