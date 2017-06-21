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

    [Tooltip("Overall sensitivity control")]
    public float sensitivity;

    [Tooltip("Dampens the distance your hand moved before its applied to the transform")]
    public float dampening;

    //links the content to the node so it'll open where you left it
    public nodeController nodeCont { get; set; }

    [Tooltip("Lerp Time")]
    public float lerp;

    [Tooltip("Button that must be gazed in order to start moving")]
    public GameObject MoveButton;

    Vector3 prevDelta;
    Vector3 curDelta;

    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {


        //check to see if we should turn it on
        if (sourceManager.Instance.sourcePressed && GazeManager.Instance.HitObject == MoveButton)
        {
            if (!manipulating)
            {
                manipulating = true;
                radialManagement.Instance.canOpen = false;

                //turn on visual bounding box 
                transform.GetChild(0).gameObject.SetActive(true);

                //turn on collider.
                //unless the collider is on, the manipulation listeners below wont be looking for manipulation inpu
                GetComponent<BoxCollider>().enabled = true;
            }
        }
        


    }


    //when manipulation is started on the collider.
    public void OnManipulationStarted (ManipulationEventData delta)

    {

        curDelta = Vector3.zero;
        prevDelta = Vector3.zero;

        content.position = Vector3.Lerp(content.position, (content.position + (curDelta / dampening) * sensitivity), lerp);
    }

    //The Manipulation listener has the delta argument. delta.CumlativeDelta is how much youve manipulated in one frame.
    //We compare that to how much we moved last frame then lerp to the new position
    public void OnManipulationUpdated(ManipulationEventData delta)
    {

        content.position = Vector3.Lerp(content.position, (content.position + (curDelta / dampening) * sensitivity), lerp);
        curDelta = delta.CumulativeDelta - prevDelta;
        prevDelta = delta.CumulativeDelta;

    }


    //When finger is released
    public void OnManipulationCompleted(ManipulationEventData delta)
    {
        completeMoving();
    }


    //When finger tracking is lost
    public void OnManipulationCanceled(ManipulationEventData delta)
    {
        completeMoving();
    }

    //reset delta values
    //turn off collider and visual bounding box
    //store where the content should open 
    void completeMoving()
    {
        curDelta = Vector3.zero;
        prevDelta = Vector3.zero;

        manipulating = false;
        radialManagement.Instance.canOpen = true;

        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;


        if (nodeCont != null)
        {
            nodeCont.contentStartLoc = content;
        }
    }



}
