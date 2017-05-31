using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class onModelDragHybrid : Singleton<onModelDragHybrid>
{
    [Header("Cursor States and local space converters")]
    public GameObject cursorOri;
    public GameObject cursorHand;
    Vector3 initHandPos;

    public GameObject buttonsGrp;

    Transform oriParent;
    bool editState;
    float xPos;
    float yPos;

    float tempDist;

    [Tooltip("Object for world orient local space to camera")]
    public GameObject handPosLocal;

    public followCursorScript followCur;

    bool navigating;

    [Header("Colliders for tumbling operations")]
    public List<radialOperationsHybrid> operations;

    float sensitivity;

    [Header("Move mode checker")]
    public GameObject moveModeMeshes;

    void Start()
    {
        initHandPos = new Vector3(0, 0, 0);
        navigating = false;
        oriParent = buttonsGrp.transform.parent;
        editState = false;
        adjustWithEdit();
        sensitivity = 1.2f;
        tempDist = 0.0f;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sourceManager.Instance.sourcePressed && GazeManager.Instance.HitObject)
        {
            //check that only miniMapMesh tagged objects will react to this script
            if (GazeManager.Instance.HitObject.tag == "miniMapMesh") {
                colliderOn();
                menuOn();
            }
        }
        else if (!sourceManager.Instance.sourcePressed)
        {

            if (gameObject.GetComponent<Collider>().enabled == false) { return; }
            gameObject.GetComponent<Collider>().enabled = false;
            
        }

        menuOn();
        
    }

    public void colliderOn()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    //this function deals with hand manipulation and local positioning of the hand cursor
    private void menuOn()
    {
        //check if move mode is active, if it is not then allow manipulation menu
        if (!moveModeMeshes.activeSelf)
        {
            if (GazeManager.Instance.HitObject == gameObject || navigating)
            {
                if (sourceManager.Instance.sourcePressed)
                {
                    if (!navigating)
                    {

                        tempDist = Vector3.Distance(Camera.main.transform.position, GazeManager.Instance.HitPosition);

                        initHandPos = HandsManager.Instance.ManipulationHandPosition;
                        editState = true;
                        adjustWithEdit();
                        navigating = true;
                        radialManagement.Instance.isActive = true;
                    }
                    navigating = true;
                    cursorHand.SetActive(true);

                    handPosLocal.transform.position = HandsManager.Instance.ManipulationHandPosition - initHandPos;

                    handPosLocal.transform.localPosition = new Vector3(Mathf.Clamp(handPosLocal.transform.localPosition.x, -.1f, .1f),
                                                                        Mathf.Clamp(handPosLocal.transform.localPosition.y, -.1f, .1f),
                                                                        handPosLocal.transform.localPosition.z) * sensitivity;
                    xPos = handPosLocal.transform.localPosition.x;
                    yPos = handPosLocal.transform.localPosition.y;

                    cursorHand.transform.localPosition = new Vector3(xPos, yPos, tempDist / 100 - .025f);

                }
                else
                {
                    editState = false;
                    navigating = false;
                    adjustWithEdit();
                    cursorOri.SetActive(true);
                    cursorHand.SetActive(false);
                    initHandPos = new Vector3(0, 0, 0);
                    timerManager.Instance.CountInterrupt();
                    radialManagement.Instance.isActive = false;
                }
            }
        }

    }

    //this function repositions the hand draggable menu(buttons) at the position of where the gaze hits this object's collider
    public void adjustWithEdit()
    {
        if (editState)
        {
            Vector3 up = oriParent.up;
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, up).normalized;

            buttonsGrp.SetActive(true);
            buttonsGrp.transform.SetParent(Camera.main.transform);
            buttonsGrp.transform.localPosition = new Vector3(0, 0, tempDist);
            buttonsGrp.transform.rotation = Quaternion.LookRotation(forward, up);
            print(tempDist);
            buttonsGrp.transform.localScale = new Vector3(1.5f,1.5f,1.5f) * Mathf.Clamp(tempDist/1.19f, .2f, 1);
            buttonsGrp.transform.SetParent(oriParent);
        }
        else
        {
            followCur.iconIndex = 0;
            buttonsGrp.SetActive(false);
            cursorOri.SetActive(true);
            foreach (radialOperationsHybrid oper in operations)
            {
                //when not editing make sure any manipulation is non-existant by multiplying by 0
                oper.rotationFactor = 0;
            }

        }

    }

}

