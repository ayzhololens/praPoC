using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCam : MonoBehaviour {

    float initHeight;
    public Transform cameraTra;
    public GameObject headRot;

	// Use this for initialization
	void Start () {
        initHeight = transform.position.y;
        cameraTra = ActorSingleton.Actor.transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(cameraTra.position.x,
                                        initHeight,
                                        cameraTra.position.z);
        transform.rotation = new Quaternion(transform.rotation.x,
                                cameraTra.rotation.y,
                                transform.rotation.z, transform.rotation.w);
        headRot.transform.rotation = cameraTra.rotation;

    }
}
