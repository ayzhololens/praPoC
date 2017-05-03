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
    Transform person;
    Transform boiler;
    Transform actorWorld;

    [SyncVar]
    public Vector3 syncHandPos;

    public Vector3 startCamPos;
    public Quaternion startCamRot;

    // Use this for initialization
    void Start()
    {

        holoCollection = GameObject.Find("HologramCollection").transform; 
        transform.SetParent(holoCollection);
        boiler = GameObject.Find("boiler").transform;
        boiler.GetComponent<boilerIdentifier>().Actor = this;
        actorWorld = GameObject.Find("ActorWorld").transform;
        actorWorld.SetParent(null);

        if (isServer)
        {
            Debug.Log("i am a server");
            ActorSingleton.isServer = true;
            startCamPos = Camera.main.transform.position;
            startCamRot = Camera.main.transform.rotation;
            boiler.GetComponent<boilerIdentifier>().isServer = true;
        }
        else
        {
            ActorSingleton.Actor = this.gameObject.GetComponent<Camera>();
            Debug.Log("i am a client");
            ActorSingleton.isServer = false;
            GazeManager.Instance.GazeTransform = this.gameObject.transform;

            //radial menu
            radialHands.Instance.isClient = true;
            radialManagement.Instance.RadialHolder = transform.Find("RadialHolderActor").transform;
            
                 
            //minimap network setup
            person = GameObject.Find("person").transform;
            person.GetComponent<followCam>().cameraTra = this.gameObject.transform;
            onModelDragHybrid.Instance.isClient = true;

            //boiler
            boiler.gameObject.GetComponent<boilerContoller>().useController = false;
            boilerSpawner.Instance.frontHolder = transform.Find("Front Holder Actor").gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        actorWorld.localPosition = tra;
        actorWorld.localRotation = rot;
        actorWorld.localScale = sca;

        //server defines sync values
        if (isServer)
        {
            mainCameraSync();
            tra = transform.localPosition;
            rot = transform.localRotation;
            sca = transform.localScale;
        }
        else if(!isLocalPlayer)

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
            transform.rotation = Camera.main.transform.rotation;
    }


    //boiler position snap
    [ClientRpc]
    public void RpcUpdateBoilerPosition(Vector3 pos, Quaternion rot)
    {
        if (!isServer)
        {
            boiler.localPosition = pos;
            boiler.localRotation = rot;
        }
            
    }

    [ClientRpc]
    public void RpcOnInputDown()
    {
        if (!isServer)
        {
            if (GazeManager.Instance.HitObject != null)
            {
                if (GazeManager.Instance.HitObject.GetComponent<selectEvent>() != null)
                {
                    GazeManager.Instance.HitObject.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }

    [ClientRpc]
    public void RpcUpdateSource(bool sourcePressed)
    {

        if (!isServer)
        {
            sourceManager.Instance.sourcePressed = sourcePressed;
        }
    }

    [ClientRpc]
    public void RpcUpdateHandPos(Vector3 handPos)
    {
        if (!isServer)
        {
            syncHandPos = handPos;
            radialHands.Instance.actorHandPos = syncHandPos;
            onModelDragHybrid.Instance.actorHandPos = syncHandPos;
        }
    }


}
