using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
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

    [SyncVar]
    public Vector3 syncHandPos;

    // Use this for initialization
    void Start()
    {

        holoCollection = GameObject.Find("HologramCollection").transform; 
        transform.SetParent(holoCollection);

        if (isServer)
        {
            Debug.Log("i am a server");
            ActorSingleton.isServer = true;
            
        }
        else
        {
            Debug.Log("i am a client");
            ActorSingleton.isServer = false;
            GazeManager.Instance.GazeTransform = this.gameObject.transform;
            radialHands.Instance.isClient = true;
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
            transform.position = ActorSingleton.Actor.transform.position;
            transform.rotation = ActorSingleton.Actor.transform.rotation;
    }


    [ClientRpc]
    public void RpcOnInputDown()
    {
        if (GazeManager.Instance.HitObject != null)
        {
            if (GazeManager.Instance.HitObject.GetComponent<selectEvent>() != null)
            {
                GazeManager.Instance.HitObject.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        }


    }

    [ClientRpc]
    public void RpcUpdateSource(bool sourcePressed)
    {
        sourceManager.Instance.sourcePressed = sourcePressed;
    }

    [ClientRpc]
    public void RpcUpdateHandPos(Vector3 handPos)
    {
        syncHandPos = handPos;
        radialHands.Instance.actorHandPos = syncHandPos;

    }


}
