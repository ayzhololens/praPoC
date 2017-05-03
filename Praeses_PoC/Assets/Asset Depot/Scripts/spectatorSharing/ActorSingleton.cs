using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class ActorSingleton : MonoBehaviour {

    private static bool _isServer = true;

    private static Camera _actor;

    public static Camera Actor
    {
        get
        {
            if (_isServer)
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

    public static bool isServer
    {
        get
        {
            return _isServer;
        }

        set
        {
            _isServer = value;
        }
    }

}
