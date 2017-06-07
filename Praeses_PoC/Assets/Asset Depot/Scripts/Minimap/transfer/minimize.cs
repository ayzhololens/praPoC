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
        //copy the real life size avatar to a mini
        miniCopy = Instantiate(gameObject);
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
                //allow mini avatar to be able to activate minimap tumbler
                childObj.gameObject.tag = "miniMapMesh";
                }
                if (childObj.gameObject.name == "headPivot")
                {
                    //make the paper plane arrow point at this avatar
                    miniRot = childObj.gameObject;
                    lookAtAvatar.Instance.avatarObj = miniCopy.transform;
                }
                if (childObj.gameObject.name == "body_geo")
                {
                    //make paper plane visibility the opposite of avatar
                    childObj.gameObject.GetComponent<followViz>().opposite = paperPlane;
                }
            }
        }
        minimapTransferObject.Instance.transferObject(miniCopy);
        done = true;
        foreach(GameObject mesh in meshesHide)
        {
            //destroy previously hidden meshes on llife size model because we dont need to see it
            Destroy(mesh.GetComponent<Collider>());
            Destroy(mesh.GetComponent<MeshRenderer>());
        }
        gameObject.GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        //depending on the bool turned on, it may include or not include the rotation
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

    //for manual testing to run manually
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!done)
        {
            miniThis();
        }
    }




}
