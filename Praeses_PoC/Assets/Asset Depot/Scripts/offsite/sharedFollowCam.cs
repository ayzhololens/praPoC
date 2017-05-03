using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class sharedFollowCam : NetworkBehaviour {

    float initHeight;
    public Transform cameraTra{get;set;}
    public GameObject headRot;

    public List<GameObject> meshViz;

    // Use this for initialization
    void Start () {
        initHeight = transform.position.y;
        cameraTra = ActorSingleton.Actor.transform;
        if (isServer)
        {
            if (isLocalPlayer)
            {
                //Destroy(gameObject);
            }
        }else
        {
            if (NetworkServer.connections.Count < 2)
            {
                foreach (GameObject obj in meshViz)
                {
                    if (obj.GetComponent<MeshRenderer>() != null)
                    {
                        obj.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isServer) { return; };
        if (isLocalPlayer)
        {
            transform.localPosition = new Vector3(cameraTra.position.x,
                                initHeight,
                                cameraTra.position.z);
            transform.localRotation = new Quaternion(transform.rotation.x,
                                    cameraTra.rotation.y,
                                    transform.rotation.z, transform.rotation.w);
            headRot.transform.localRotation = cameraTra.rotation;
        }


    }
}
