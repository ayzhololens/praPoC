using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contentScaler : MonoBehaviour {

    public float dist;
    public Vector3 sScale;
    public float scaleMult;
    public float floor;
    public float ceiling;

    // Use this for initialization
    void Awake () {
        sScale = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        //dist = Vector3.Distance(transform.position, Camera.main.transform.position);
        //transform.localScale = (startScale * (dist *scaleMult));

        //if(transform.localScale.x <= floor)
        //{
        //    transform.localScale = new Vector3(floor, floor, floor);
        //}

        //if (transform.localScale.x >= ceiling)
        //{
        //    transform.localScale = new Vector3(ceiling, ceiling, ceiling);
        //}

        //print(dist);
	}

    public void setScale()
    {
        transform.localScale = new Vector3(scaleMult, scaleMult, scaleMult); 
    }

    public void resetScale()
    {
        //transform.localScale = startScale;
    }
}
