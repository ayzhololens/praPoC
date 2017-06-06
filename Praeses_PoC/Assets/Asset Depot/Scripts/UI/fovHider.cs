using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class fovHider : Singleton<fovHider> {
    public GameObject Hider;
    bool status;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //turn on/off the fovHide with a delay if necessary
    public void toggleFOVHider(bool on, float delay)
    {
        status = on;
        Invoke("toggle", delay);
    }

    void toggle()
    {

        Hider.SetActive(status);
    }
}
