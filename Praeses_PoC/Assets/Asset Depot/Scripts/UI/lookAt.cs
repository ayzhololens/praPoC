using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour {

    public Transform mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(mainCamera);

    }
}
