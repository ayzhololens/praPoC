using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class minimapTransferObject : Singleton<minimapTransferObject> {
    Vector3 initPos;
    Quaternion initRot;
    Vector3 initSca;

    GameObject holoCollection;

    public void transferObject(GameObject processObj)
    {
        initPos = minimapSpawn.Instance.miniMapHolder.transform.localPosition;
        initRot = minimapSpawn.Instance.miniMapHolder.transform.localRotation;
        initSca = minimapSpawn.Instance.miniMapHolder.transform.localScale;
        resetHolder();

        GameObject miniHoloCollection = new GameObject();
        miniHoloCollection.name = "miniHoloCollection";
        miniHoloCollection.transform.SetParent(holoCollection.transform);
        miniHoloCollection.transform.position = Vector3.zero;
        miniHoloCollection.transform.rotation = new Quaternion(0, 0, 0, 0);
        miniHoloCollection.transform.localScale = Vector3.one;

        miniHoloCollection.transform.SetParent(minimapSpawn.Instance.miniMapHolder.transform);
        processObj.transform.SetParent(miniHoloCollection.transform);
        minimapSpawn.Instance.miniMapHolder.transform.SetParent(minimapSpawn.Instance.MiniMapHolderParent.transform);
        minimapSpawn.Instance.miniMapHolder.transform.localPosition = initPos;
        minimapSpawn.Instance.miniMapHolder.transform.localRotation = initRot;
        minimapSpawn.Instance.miniMapHolder.transform.localScale = initSca;
    }

    public void resetHolder()
    {
        holoCollection = GameObject.Find("HologramCollection");
        minimapSpawn.Instance.miniMapHolder.transform.SetParent(holoCollection.transform);
        minimapSpawn.Instance.miniMapHolder.transform.position = Vector3.zero;
        minimapSpawn.Instance.miniMapHolder.transform.rotation = new Quaternion(0,0,0,0);
        minimapSpawn.Instance.miniMapHolder.transform.localScale = Vector3.one;
    }

}
