using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class testCameraTraPrint : MonoBehaviour{

    public GameObject holoCol;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void printCam()
    {
        //Debug.Log("main camera tx is " + Camera.main.transform.position.x);
        //Debug.Log("main camera ty is " + Camera.main.transform.position.y);
        //Debug.Log("main camera tz is " + Camera.main.transform.position.z);
        //Debug.Log("main camera rx is " + Camera.main.transform.rotation.x);
        //Debug.Log("main camera ry is " + Camera.main.transform.rotation.y);
        //Debug.Log("main camera rz is " + Camera.main.transform.rotation.z);

        //Debug.Log("actor camera tx is " + ActorSingleton.Actor.transform.position.x);
        //Debug.Log("actor camera ty is " + ActorSingleton.Actor.transform.position.y);
        //Debug.Log("actor camera tz is " + ActorSingleton.Actor.transform.position.z);
        //Debug.Log("actor camera rx is " + ActorSingleton.Actor.transform.rotation.x);
        //Debug.Log("actor camera ry is " + ActorSingleton.Actor.transform.rotation.y);
        //Debug.Log("actor camera rz is " + ActorSingleton.Actor.transform.rotation.z);
        //Debug.Log(ActorSingleton.Actor.name);

        //Debug.Log(" tx is " + holoCol.transform.position.x);
        //Debug.Log(" ty is " + holoCol.transform.position.y);
        //Debug.Log(" tz is " + holoCol.transform.position.z);
        //Debug.Log(" rx is " + holoCol.transform.rotation.x);
        //Debug.Log(" ry is " + holoCol.transform.rotation.y);
        //Debug.Log(" rz is " + holoCol.transform.rotation.z);

        //Debug.Log(" tx is " + boilerIdentifier.Instance.gameObject.transform.localPosition.x);
    }
}
