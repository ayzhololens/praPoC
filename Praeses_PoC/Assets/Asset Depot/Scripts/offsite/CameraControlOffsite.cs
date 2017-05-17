
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraControlOffsite : MonoBehaviour
{
    public Vector2 PanSensitivity = new Vector2(60.0f, 60.0f);
    public Vector2 RotateSensitivity = new Vector2(120.0f, 120.0f);
    public float ZoomSensititity = 450.0f;
    public GameObject rotatorGrp;
    public GameObject rotateCam;
    Vector3 initPos;
    int tempInt;

    private void Start()
    {
        initPos = rotateCam.transform.position;
        tempInt = 0;

#if UNITY_EDITOR
            PanSensitivity = new Vector2(20, 20);
            RotateSensitivity = new Vector2(600, 600);
#elif WINDOWS_UWP
            PanSensitivity = new Vector2(1, 1);
            RotateSensitivity = new Vector2(20, 20);
#endif

    }

    void Update()
    {
        //float sensitivityScale = Input.GetKey(KeyCode.LeftShift) ? 0.1f : 1.0f;
        float sensitivityScale = 1.0f;
        float finalScale = Time.deltaTime * sensitivityScale;

        transform.Translate(new Vector3(0.0f, 0.0f, Input.GetAxis("Mouse ScrollWheel") * ZoomSensititity * finalScale));
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * 2.5f * Time.deltaTime,
                0,
                                            Input.GetAxis("Vertical") * 2.5f * Time.deltaTime
                                            ));
        
        //pan
        if (((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetMouseButton(0)) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
        {
            float y = Input.GetKey(KeyCode.F) ? -.01f : 0;
            y += Input.GetKey(KeyCode.R) ? .01f : 0;
            y += Input.GetAxis("Mouse Y");

            transform.Translate(new Vector3(Input.GetAxis("Mouse X") * -PanSensitivity.x * finalScale,
                                            y * -PanSensitivity.y * finalScale,
                                            0.0f));
        }
        //rotate
        else if ((!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) && Input.GetMouseButton(0)) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            float y = Input.GetKey(KeyCode.Q) ? -.5f : 0;
            y += Input.GetKey(KeyCode.E) ? .5f : 0;
            y += Input.GetAxis("Mouse X");

            rotateCam.transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y") * RotateSensitivity.y * finalScale,
                                                    y * RotateSensitivity.x * finalScale,
                                                    0.0f);

        }
    }

    public void lockCam()
    {
        PanSensitivity = new Vector2(0, 0);
        RotateSensitivity = new Vector2(0, 0);
        ZoomSensititity = 0;
    }

    public void focus(int nodeIndex)
    {
        float rotY;

        //angling the cam for focus
        Vector3 point = new Vector3(offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.position.x,
            offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.position.y + 2,
            offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.position.z);
        Vector3 rotatedPoint = RotatePointAroundPivot(point, 
            offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.position,
            offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.eulerAngles);
        GameObject fakeObj = new GameObject();
        GameObject fakeObj2 = new GameObject();
        fakeObj.transform.position = rotatedPoint;
        fakeObj2.transform.position = offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.position;
        fakeObj.transform.SetParent(fakeObj2.transform);
        rotY = Mathf.Rad2Deg * Mathf.Atan2(fakeObj.transform.localPosition.x, fakeObj.transform.localPosition.z);

        rotateCam.transform.position = offsiteJSonLoader.Instance.nodes3DList[nodeIndex].transform.position;

        rotateCam.transform.eulerAngles = new Vector3(rotateCam.transform.eulerAngles.x,
                                                    rotY + 180,
                                                    rotateCam.transform.eulerAngles.z);
        transform.localPosition = new Vector3(transform.localPosition.x,
                                              .3f,
                                              -3);

    }

    public virtual Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
       Vector3 dir = point - pivot; // get point direction relative to pivot
       dir = Quaternion.Euler(angles) * dir; // rotate it
       point = dir + pivot; // calculate rotated point
       return point; // return it
    }
}
