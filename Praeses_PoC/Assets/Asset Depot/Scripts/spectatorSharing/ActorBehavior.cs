using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorBehavior : NetworkBehaviour
{

    [SyncVar]
    public Vector3 tra;

    [SyncVar]
    public Quaternion rot;

    [SyncVar]
    public Vector3 sca;

    Transform holoCollection;

    // Use this for initialization
    void Start()
    {

        holoCollection = GameObject.Find("HologramCollection").transform; 
        transform.SetParent(holoCollection);

        //holoCollection.GetComponent<NetworkTransformChild>().target = gameObject.transform;

        if (isServer)
        {
            Debug.Log("i am a server");
            ActorSingleton.Instance.isServer = true;
        }
        else
        {
            Debug.Log("i am a client");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //server defines sync values
        if (isServer)
        {
            mainCameraSync();
            tra = transform.localPosition;
            rot = transform.localRotation;
            sca = transform.localScale;
        }
        else

        //client picks up sync values
        {
            transform.localPosition = tra;
            transform.localRotation = rot;
            transform.localScale = sca;
        }
    }

    public void mainCameraSync()
    {
            transform.position = Camera.main.transform.position;
            transform.localRotation = Camera.main.transform.rotation;
    }

}
