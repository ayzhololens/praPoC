using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoomOverTime : MonoBehaviour {

    GameObject rotateCam;
    public GameObject targetObject;
    public GameObject cameraToMove;
    public GameObject parentToMove;
    public float speed = 1.5f;

    //public KeyCode zoom = KeyCode.L;

    // Use this for initialization
    void Start () {
        rotateCam = transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //if (Input.GetKeyUp(zoom))
        //{
        //   }
    }

    public IEnumerator smoothMove(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, Vector3 pStartPos, Vector3 pEndPos, float seconds)
    {
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            cameraToMove.transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0.0f, 1.0f, t));
            parentToMove.transform.rotation = Quaternion.Lerp(startRot, endRot, Mathf.SmoothStep(0.0f, 1.0f, t));
            parentToMove.transform.position = Vector3.Lerp(pStartPos, pEndPos, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return true;
        }
    }

    public void resetCam()
    {
        parentToMove.transform.localPosition = Vector3.zero;
        parentToMove.transform.eulerAngles = Vector3.zero;
        cameraToMove.transform.localPosition = new Vector3(0,.8f,-5);
    }

    public void smoothZoom(Transform target)
    {
        resetCam();
        focusObj(targetObject.transform);
        StartCoroutine(smoothMove(cameraToMove.transform.position, transform.position, 
            parentToMove.transform.rotation, rotateCam.transform.rotation,
            parentToMove.transform.position, rotateCam.transform.position,
            speed));
    }

    public void focusObj(Transform target)
    {
        float rotY;

        //angling the cam for focus
        Vector3 point = new Vector3(target.position.x,
            target.position.y + 2,
            target.position.z);
        Vector3 rotatedPoint = RotatePointAroundPivot(point,
            target.position,
            target.eulerAngles);
        GameObject fakeObj = new GameObject();
        GameObject fakeObj2 = new GameObject();
        fakeObj.transform.position = rotatedPoint;
        fakeObj2.transform.position = target.position;
        fakeObj.transform.SetParent(fakeObj2.transform);
        rotY = Mathf.Rad2Deg * Mathf.Atan2(fakeObj.transform.localPosition.x, fakeObj.transform.localPosition.z);

        rotateCam.transform.position = target.position;

        rotateCam.transform.eulerAngles = new Vector3(rotateCam.transform.eulerAngles.x,
                                                    rotY + 180,
                                                    rotateCam.transform.eulerAngles.z);
        transform.localPosition = new Vector3(transform.localPosition.x,
                                              0.1f,
                                              -1);

        Destroy(fakeObj);
        Destroy(fakeObj2);
    }

    public virtual Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
