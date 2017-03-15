﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sumbitState : MonoBehaviour {
    public GameObject Indicator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startUpload()
    {
        Indicator.SetActive(true);
        Indicator.GetComponent<TextMesh>().text = "Compiling Inspection Data...";
        Invoke("success", 2.5f);

    }

    void success()
    {
        Indicator.GetComponent<TextMesh>().text = "Success!";
        Invoke("turnOff", 5);
    }

    void turnOff()
    {
        Indicator.SetActive(false);
    }
}
