using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

//script for putting objects from real space in the minimap
public class minimapTransferObject : Singleton<minimapTransferObject> {

    Vector3 initPos;
    Quaternion initRot;
    Vector3 initSca;

    public void transferObject(GameObject processObj)
    {   
        //record the initial local position of object
        initPos = minimapSpawn.Instance.miniMapHolder.transform.localPosition;
        initRot = minimapSpawn.Instance.miniMapHolder.transform.localRotation;
        initSca = minimapSpawn.Instance.miniMapHolder.transform.localScale;

        //resize the minimap holder back to full size
        resetHolder();

        //parent object to the minimap holder
        processObj.transform.SetParent(minimapSpawn.Instance.miniMapHolder.transform);
        minimapSpawn.Instance.miniMapHolder.transform.SetParent(minimapSpawn.Instance.MiniMapHolderParent.transform);
        //put minimap holder back to its initial position
        minimapSpawn.Instance.miniMapHolder.transform.localPosition = initPos;
        minimapSpawn.Instance.miniMapHolder.transform.localRotation = initRot;
        minimapSpawn.Instance.miniMapHolder.transform.localScale = initSca;
    }

    public void resetHolder()
    {
        minimapSpawn.Instance.miniMapHolder.transform.SetParent(null);
        minimapSpawn.Instance.miniMapHolder.transform.position = Vector3.zero;
        minimapSpawn.Instance.miniMapHolder.transform.rotation = new Quaternion(0,0,0,0);
        minimapSpawn.Instance.miniMapHolder.transform.localScale = Vector3.one;
    }
}
