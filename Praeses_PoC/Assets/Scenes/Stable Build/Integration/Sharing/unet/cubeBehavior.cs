using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cubeBehavior : NetworkBehaviour {

    [SyncVar]
    public Vector3 tra;

    [SyncVar]
    public Quaternion rot;

    [SyncVar]
    public Vector3 sca;

    [SyncVar]
    public bool followHead;

    // Use this for initialization
    void Start () {
        //transform.SetParent(parentIdentifier.Instance.gameObject.transform);
		if (isServer)
        {
            Debug.Log("i am a server");
        }
        else
        {
            Debug.Log("i am a client");
        }
	}
	
	// Update is called once per frame
	void Update () {

        ////server defines sync values
        //if (isServer)
        //{
        //    mainCameraSync();
        //    tra = transform.localPosition;
        //    rot = transform.localRotation;
        //    sca = transform.localScale;
        //}else

        ////client picks up sync values
        //{
        //    transform.localPosition = tra;
        //    transform.localRotation = rot;
        //    transform.localScale = sca;
        //}
    }

    public void mainCameraSync()
    {
        if (followHead)
        {
            transform.position = Camera.main.transform.position;
            transform.localRotation = Camera.main.transform.rotation;
        }
    }

    public void tapActivate()
    {
        if (isServer)
        {
            if (!followHead)
            {
                followHead = true;
                Debug.Log("cube is following device on server");
            }else
            {
                followHead = false;
                Debug.Log("cube stopped following device on server");
            }
        }
    }
}
