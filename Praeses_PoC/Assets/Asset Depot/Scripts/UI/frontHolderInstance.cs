using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class frontHolderInstance : Singleton<frontHolderInstance> {

    public GameObject holder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual GameObject setFrontHolder(float dist)
    {
        GameObject frontHolder = holder;
        frontHolder.transform.localPosition = new Vector3(0, 0, dist);
            //Camera.main.transform.forward * dist;

        return frontHolder;
    }
}
