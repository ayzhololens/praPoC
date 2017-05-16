using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraViewRectangle : MonoBehaviour {

    public GameObject mainScroller;
    public GameObject miniCamera;
    public GameObject shooterCamera;
    public GameObject lightHolder;

    bool hold;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        miniCamera.GetComponent<CameraControlOffsite>().enabled = false;
    }

    private void OnMouseEnter()
    {
        if (hold || scrollOverrider.Instance.isScrolling) { return; };
        cameraModeOn();
    }

    private void OnMouseExit()
    {
        if (hold) { return; };
        cameraModeOff();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            hold = false;
            ray = shooterCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "Button")
                {
                    cameraModeOn();
                }else
                {
                    cameraModeOff();
                }
                //Debug.Log(hit.collider.name + " is the same as "+ gameObject.GetComponent<Collider>().name);
            }else
            {
                //print("nothing hit");
                cameraModeOff();
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            hold = true;
        }
    }

    void cameraModeOn()
    {
        miniCamera.GetComponent<CameraControlOffsite>().enabled = true;
        mainScroller.GetComponent<ScrollRect>().enabled = false;

        faceLookAtToThis(true);
    }

    void cameraModeOff()
    {
        miniCamera.GetComponent<CameraControlOffsite>().enabled = false;
        mainScroller.GetComponent<ScrollRect>().enabled = true;
        //faceLookAtToThis(false);
    }

    void faceLookAtToThis(bool isOverridden)
    {
        foreach (GameObject node in offsiteJSonLoader.Instance.nodes3DList.Values)
        {
            foreach (Transform child in node.transform)
            {
                if (child.name == "Mesh (1)")
                {
                    foreach (Transform gChild in child.transform)
                    {
                        if (gChild.name == "Coin")
                        {
                            GameObject coinObj;
                            coinObj = gChild.gameObject;
                            if (isOverridden)
                            {
                                coinObj.GetComponent<lookAt>().mainCamera = miniCamera.transform;
                            }else
                            {
                                coinObj.GetComponent<lookAt>().mainCamera = Camera.main.transform;
                            }

                        }
                    }
                }
            }
        }
        if (isOverridden)
        {
            lightHolder.GetComponent<orientCameraScript>().mainCamera = miniCamera.transform;
        }
        else
        {
            lightHolder.GetComponent<orientCameraScript>().mainCamera = Camera.main.transform;
        }
    }
}
