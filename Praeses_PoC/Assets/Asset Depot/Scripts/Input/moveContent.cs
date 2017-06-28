using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;


public class moveContent : MonoBehaviour,IManipulationHandler
{
    [Tooltip("Content to be moved.  Usually a ContentHolder")]
    public Transform content;
    bool manipulating;
    bool canManipulate;

    [Tooltip("Overall sensitivity")]
    public float sensitivity;
    [Tooltip("Dampens value of the distance your hand moved before its applied to the transform")]
    public float dampening;
    [Tooltip("Lerp Time")]
    public float lerp;

    public nodeController nodeCont { get; set; }
    public GameObject BoundingBox;
    public GameObject MoveButton;


    Vector3 prevDelta;
    Vector3 curDelta;

    // Use this for initialization
    void Start () {
        canManipulate = true;

		
	}
	
    //Check to see if we should start moving content
    //Turn on visual box and a large box collider to drag on
	void Update () {


        if (canManipulate)
        {
            if (sourceManager.Instance.sourcePressed)
            {

                if (GazeManager.Instance.HitObject == MoveButton)
                {
                    if (!manipulating)
                    {
                        manipulating = true;
                        radialManagement.Instance.canOpen = false;
                        BoundingBox.transform.GetChild(0).gameObject.SetActive(true);
                        BoundingBox.GetComponent<BoxCollider>().enabled = true;
                    }
                }

            }
        }


    }


    //when manipulating starts
    public void OnManipulationStarted (ManipulationEventData delta)

    {
        if (manipulating)
        {
            curDelta = Vector3.zero;
            prevDelta = Vector3.zero;
            content.position = Vector3.Lerp(content.position, (content.position + (curDelta / dampening) * sensitivity), lerp);

        }
    }

    //Compare how much your hand moved this frame with how much your hand moved last frame.  Lerp bewteen these values
    public void OnManipulationUpdated(ManipulationEventData delta)
    {
        if (manipulating)
        {
            content.position = Vector3.Lerp(content.position, (content.position + (curDelta / dampening) * sensitivity), lerp);
            curDelta = delta.CumulativeDelta - prevDelta;
            prevDelta = delta.CumulativeDelta;

        }

    }


    //On finger release
    public void OnManipulationCompleted(ManipulationEventData delta)
    {
        completeMoving();

    }

    //On hand tracking lost
    public void OnManipulationCanceled(ManipulationEventData delta)
    {


        completeMoving();


    }


    /// <summary>
    /// reset delta values 
    /// turn off visual bounding box and large collide
    /// store where the content should open
    /// </summary>
    void completeMoving()
    {
        manipulating = false;
        radialManagement.Instance.canOpen = true;
        BoundingBox.transform.GetChild(0).gameObject.SetActive(false);
        BoundingBox.GetComponent<BoxCollider>().enabled = false;
        curDelta = Vector3.zero;
        prevDelta = Vector3.zero;

        if (nodeCont != null)
        {
            nodeCont.contentStartLoc = content;
        }
    }



}
