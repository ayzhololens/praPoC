using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popForward : MonoBehaviour {

    Vector3 startScale;
    Vector3 startPos;
    Vector3 movePos;

    [Tooltip("Amount to scale by")]
    public float scaleMult;
    [Tooltip("Amount to move")]
    public float moveDist;
    [Tooltip("Enable Scaling")]
    public bool scale;
    [Tooltip("Enable Moving")]
    public bool move;

	// Use this for initialization
	void Start () {
        startScale = transform.localScale;
        startPos = transform.localPosition;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void moveForward()
    {
        if (scale)
        {
            transform.localScale *= scaleMult;
        }


        if (move)
        {
            movePos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveDist);
            transform.localPosition = movePos;
        }
    }

    public void moveBackward()
    {
        if (scale)
        {
            transform.localScale = startScale;
        }


        if (move)
        {
            transform.localPosition = startPos;
        }
    }
}
