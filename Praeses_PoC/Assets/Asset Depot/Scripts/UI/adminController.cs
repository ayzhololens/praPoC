﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloLensXboxController;

public class adminController : MonoBehaviour {

    private ControllerInput controllerInput;
    public GameObject contentHolder;

    // Use this for initialization
    void Start () {

        controllerInput = new ControllerInput(0, 0.19f);

    }
	
	// Update is called once per frame
	void Update () {


        if (controllerInput.GetButtonDown(ControllerButton.Menu))
        {
            toggleMenu();
        }

    }

    void toggleMenu()
    {
        contentHolder.SetActive(!contentHolder.activeSelf);
    }
}
