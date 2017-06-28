using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorListening : MonoBehaviour {
    public GameObject focusedObj;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        focusedObj = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        focusedObj = null;
    }
}
