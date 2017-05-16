using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;


public class moveContent : MonoBehaviour,IManipulationHandler
{

    public Transform content;
    Vector3 handStartPos;
    bool manipulating;
    Vector3 contentStartPos;
    public float sensitivity;
    public float dampening;

    public float lerp;
    bool canManipulate;
    public GameObject BoundingBox;
    public GameObject MoveButton;
    Vector3 prevDelta;
    Vector3 startDelta;
    Vector3 curDelta;

    // Use this for initialization
    void Start () {
        canManipulate = true;

		
	}
	
	// Update is called once per frame
	void Update () {


        if (canManipulate)
        {
            if (sourceManager.Instance.sourcePressed)
            {

                if (GazeManager.Instance.HitObject == MoveButton)
                {
                    if (!manipulating)
                    {
                        contentStartPos = content.position;
                        handStartPos = HandsManager.Instance.ManipulationHandPosition;
                        transform.InverseTransformDirection(handStartPos);
                        manipulating = true;


                        radialManagement.Instance.canOpen = false;
                        BoundingBox.transform.GetChild(0).gameObject.SetActive(true);
                        BoundingBox.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                if (manipulating)
                { 

                    //handStartPos = contentStartPos + (sensitivity * (handsManager.ManipulationHandPosition));
                    //content.localPosition = new Vector3(contentStartPos.x - (sensitivity * (HandsManager.Instance.ManipulationHandPosition.x - handStartPos.x)), contentStartPos.y + (sensitivity * (HandsManager.Instance.ManipulationHandPosition.y - handStartPos.y)), contentStartPos.z - (sensitivity * (HandsManager.Instance.ManipulationHandPosition.z - handStartPos.z)));
                    //content.localPosition = contentStartPos + (sensitivity * (handsManager.ManipulationHandPosition - handStartPos));
                }

            }
            else if (manipulating && !sourceManager.Instance.sourcePressed)
            {
                //manipulating = false;
                //radialManagement.Instance.canOpen = true;
                //BoundingBox.transform.GetChild(0).gameObject.SetActive(false);
                //BoundingBox.GetComponent<BoxCollider>().enabled = false;
            }

        }


    }

    public void toggleManipulating()
    {
        //canManipulate = !canManipulate;
        //radialManagement.Instance.canOpen = !canManipulate;
        //BoundingBox.transform.GetChild(0).gameObject.SetActive(canManipulate);
        //BoundingBox.GetComponent<BoxCollider>().enabled = canManipulate;
    }

    public void OnManipulationStarted (ManipulationEventData delta)

    {

        curDelta = Vector3.zero;
        prevDelta = Vector3.zero;
        //print(delta.CumulativeDelta);
        startDelta = delta.CumulativeDelta;
        //content.position += (delta.CumulativeDelta * sensitivity);

        content.position = Vector3.Lerp(content.position, (content.position + (curDelta / dampening) * sensitivity), lerp);
        //content.position = Vector3.Lerp(content.position, (content.position), lerp);
        //content.position = (content.position + (delta.CumulativeDelta / dampening) * sensitivity);
    }

    public void OnManipulationUpdated(ManipulationEventData delta)
    {
        if (delta.CumulativeDelta.normalized.magnitude > (prevDelta.normalized.magnitude*2))
        {
            //content.position = Vector3.Lerp(content.position, (content.position + (delta.CumulativeDelta / dampening) * sensitivity), lerp);

           // print("d");
        }
        else
        {
            //print("y");
        }

        //print(delta.CumulativeDelta);
        //content.position += (delta.CumulativeDelta * sensitivity);
        content.position = Vector3.Lerp(content.position, (content.position + (curDelta / dampening) * sensitivity), lerp);
        curDelta = delta.CumulativeDelta - prevDelta;
        prevDelta = delta.CumulativeDelta;
        print(curDelta);
        //content.position = (content.position + (delta.CumulativeDelta / dampening) * sensitivity);

    }

    public void OnManipulationCompleted(ManipulationEventData delta)
    {
        //print(delta.CumulativeDelta);
        manipulating = false;
        radialManagement.Instance.canOpen = true;
        BoundingBox.transform.GetChild(0).gameObject.SetActive(false);
        BoundingBox.GetComponent<BoxCollider>().enabled = false;
        curDelta = Vector3.zero;
        prevDelta = Vector3.zero;

    }

    public void OnManipulationCanceled(ManipulationEventData delta)
    {
        manipulating = false;
        radialManagement.Instance.canOpen = true;
        BoundingBox.transform.GetChild(0).gameObject.SetActive(false);
        BoundingBox.GetComponent<BoxCollider>().enabled = false;
        curDelta = Vector3.zero;
        prevDelta = Vector3.zero;


    }



}
