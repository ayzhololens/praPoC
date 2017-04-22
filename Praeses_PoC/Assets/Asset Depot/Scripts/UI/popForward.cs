using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popForward : MonoBehaviour {

    Vector3 startScale;
    Vector3 largeScale;
    Vector3 startPos;
    Vector3 movePos;
    public float scaleMult;
    public float moveDist;
    public bool scale;
    public bool move;

	// Use this for initialization
	void Start () {
        startScale = transform.localScale;
        largeScale = new Vector3(transform.localScale.x + scaleMult, transform.localScale.y + scaleMult, transform.localScale.z + scaleMult);
        startPos = transform.localPosition;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void moveForward()
    {
        if (scale)
        {
            transform.localScale = largeScale;
        }


        if (move)
        {
            movePos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveDist);
            transform.localPosition = movePos;
        }
    }

    public void moveBackward()
    {
        Debug.Log("gazeOff");

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
