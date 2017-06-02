using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dismissContent : MonoBehaviour {

    public GameObject contentHolder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void dismissContentHolder()
    {
        contentHolder.SetActive(false);
    }
}
