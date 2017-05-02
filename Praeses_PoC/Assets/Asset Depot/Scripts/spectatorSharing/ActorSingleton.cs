using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class ActorSingleton : Singleton<ActorSingleton> {

    public bool isServer = false;

    private Camera _actor;

    public Camera Actor
    {
        get
        {
            if (isServer)
            {
                return Camera.main;
            }

            if (_actor == null)
            {
                GameObject actor = GameObject.Find("Actor(Clone)");
                if (actor != null)
                {
                    _actor = actor.GetComponent<Camera>();
                }
            }
            
            return _actor;
        }

        set
        {
            _actor = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
