using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class materialMatcher : MonoBehaviour {

    public GameObject planeObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Image>().material = planeObj.GetComponent<Renderer>().material;

    }
}
