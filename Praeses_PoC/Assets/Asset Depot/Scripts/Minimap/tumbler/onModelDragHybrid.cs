using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class onModelDragHybrid : Singleton<onModelDragHybrid>
{
    public GameObject cursorOri;
    public GameObject cursorHand;
    Vector3 initHandPos;

    public GameObject buttonsGrp;

    Transform oriParent;
    public bool editState;
    float xPos;
    float yPos;

    float tempDist;

    public GameObject handPosLocal;

    public followCursorScript followCur;

    public bool navigating;

    public List<radialOperationsHybrid> operations;

    public float sensitivity;

    public GameObject moveModeMeshes;

    void Start()
    {
        initHandPos = new Vector3(0, 0, 0);
        navigating = false;
        oriParent = buttonsGrp.transform.parent;
        editState = false;
        adjustWithEdit();

        tempDist = 0.0f;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sourceManager.Instance.sourcePressed && GazeManager.Instance.HitObject)
        {
            if (GazeManager.Instance.HitObject.tag == "miniMapMesh") {
                //timerManager.Instance.tumbleCountDown();
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

    private void menuOn()
    {
        if (!moveModeMeshes.activeSelf)
        {
            if (GazeManager.Instance.HitObject == gameObject || navigating)
            {
                Debug.Log("looking");
                if (sourceManager.Instance.sourcePressed)
                {
                    Debug.Log("pressing...");
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
            buttonsGrp.transform.SetParent(oriParent);
        }
        else
        {
            followCur.iconIndex = 0;
            buttonsGrp.SetActive(false);
            cursorOri.SetActive(true);
            foreach (radialOperationsHybrid oper in operations)
            {
                oper.rotationFactor = 0;
            }

        }

    }

}

