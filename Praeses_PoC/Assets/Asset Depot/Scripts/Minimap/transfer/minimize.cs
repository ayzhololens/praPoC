using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

public class minimize : MonoBehaviour, IInputClickHandler {

    public GameObject miniCopy;
    public bool posUpdate;
    public bool rotUpdate;
    public GameObject miniRot;
    bool done;
    public GameObject bigHeadGeo;
    public List<GameObject> meshesHide;
    Vector3 anchDist;
    public bool useAvatar;

    public GameObject paperPlane;

    public void miniThis()
    {
        miniCopy = Instantiate(gameObject);
        //print("miniThis:" + miniCopy);
        Destroy(miniCopy.GetComponent<minimize>());
        if (miniCopy.GetComponent<followCam>())
        {
            Destroy(miniCopy.GetComponent<followCam>());
        }
        if (rotUpdate)
        {
            foreach (Transform childObj in miniCopy.GetComponentsInChildren<Transform>())
            {
                if (childObj.gameObject.GetComponent<MeshRenderer>() != null)
                { 
                childObj.gameObject.tag = "miniMapMesh";
                }
                if (childObj.gameObject.name == "headPivot")
                {
                    miniRot = childObj.gameObject;
                    lookAtAvatar.Instance.avatarObj = miniCopy.transform;
                }
                if (childObj.gameObject.name == "body_geo")
                {
                    childObj.gameObject.GetComponent<followViz>().opposite = paperPlane;
                }
            }
        }

        minimapTransferObject.Instance.transferObject(miniCopy);
        done = true;
        foreach(GameObject mesh in meshesHide)
        {
            Destroy(mesh.GetComponent<Collider>());
            Destroy(mesh.GetComponent<MeshRenderer>());
        }
    }

    private void Update()
    {

        if (miniCopy != null && posUpdate)
        {

            miniCopy.transform.localPosition = transform.localPosition;
            miniCopy.transform.localRotation = transform.rotation;
        }

        if (miniCopy != null && rotUpdate && done)
        {
            miniRot.transform.localRotation = bigHeadGeo.transform.localRotation;
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!done)
        {
            miniThis();
        }
    }




}
