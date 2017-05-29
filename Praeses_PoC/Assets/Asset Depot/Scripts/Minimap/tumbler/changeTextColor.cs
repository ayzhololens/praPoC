using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTextColor : MonoBehaviour {

    public Color col;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate() {
        gameObject.GetComponent<Renderer>().material.color = col;
	}

}
