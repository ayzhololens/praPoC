using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orientCameraScript : MonoBehaviour {

    public Transform mainCamera;
    public bool onlyY;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update () {

        if(mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        if (onlyY)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 
                mainCamera.transform.eulerAngles.y,
                transform.eulerAngles.z);

        }
        else
        {
            transform.rotation = mainCamera.transform.rotation;

        }

    }

}
