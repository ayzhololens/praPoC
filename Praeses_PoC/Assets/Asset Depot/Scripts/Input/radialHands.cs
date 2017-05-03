using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class radialHands : Singleton<radialHands> {

    public HandsManager handsManager;
    public GameObject invisCursor;
    public GameObject navCursor;
    Vector3 startPos;
    public bool canManipulate;
    public bool manipulating;
    Vector3 offset;
    public float sensitivity;
    Vector3 handStartPos;
    public GameObject focusedObj;
    Vector3 handPos;
    public Vector3 actorHandPos;
    public bool isClient;


    // Use this for initialization
    void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (canManipulate)
        {
            Vector3 tempHandPos = handsManager.ManipulationHandPosition;
            if (isClient)
            {
                handPos = actorHandPos;
            }
            else
            {
                handPos = tempHandPos;
            }


            if (sourceManager.Instance.sourcePressed && !manipulating)
            {
                navCursor.transform.position = startPos;
                handStartPos = (handPos);
                manipulating = true;
            }

            if (manipulating && sourceManager.Instance.sourcePressed)
            {
                invisCursor.transform.position = (handPos - handStartPos);
                navCursor.transform.localPosition = (new Vector3(invisCursor.transform.localPosition.x * -1, invisCursor.transform.localPosition.y, 1)) * sensitivity;

            }
        }
            if (manipulating && !sourceManager.Instance.sourcePressed)
            {
                manipulating = false;
            //navCursor.transform.position = startPos;

            }

            if (navCursor.GetComponent<cursorListening>().focusedObj != focusedObj)
            {
                focusedObj = navCursor.GetComponent<cursorListening>().focusedObj;

            }
        
        
	}

    public void startManipulating()
    {

    }


}
