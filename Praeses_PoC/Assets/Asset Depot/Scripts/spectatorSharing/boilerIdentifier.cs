using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class boilerIdentifier : MonoBehaviour {

    public bool isServer;
    public ActorBehavior Actor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void sendBoilerPosition()
    {
        if (isServer)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            Actor.RpcUpdateBoilerPosition(pos, rot);
        }
    }
}
