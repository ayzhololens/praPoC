using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class submitState : Singleton<submitState>

{
    public GameObject inspectionSucess;


    //enable feedback to convey the inspection has completed
    public void startUpload()
    {
        formController.Instance.closeForm();
        mediaManager.Instance.setStatusIndicator("Compiling Inspection Data...");
        Invoke("success", 2f);

    }

    void success()
    {
        audioManager.Instance.setAndPlayAudio(1);
        inspectionSucess.SetActive(true);
        inspectionSucess.transform.position = frontHolderInstance.Instance.setFrontHolder(1.5f).transform.position;
        mediaManager.Instance.disableStatusIndicator();
    }
}
