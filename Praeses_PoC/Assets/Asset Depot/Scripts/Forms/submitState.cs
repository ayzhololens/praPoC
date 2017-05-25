using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class submitState : Singleton<submitState>

{
    public GameObject successContent;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startUpload()
    {
        formController.Instance.closeForm();
        mediaManager.Instance.setStatusIndicator("Compiling Inspection Data...");
        Invoke("success", 2f);

    }

    void success()
    {
        audioManager.Instance.setAndPlayAudio(1);
        successContent.SetActive(true);
        successContent.transform.position = frontHolderInstance.Instance.setFrontHolder(1.5f).transform.position;
        mediaManager.Instance.disableStatusIndicator();
    }
}
