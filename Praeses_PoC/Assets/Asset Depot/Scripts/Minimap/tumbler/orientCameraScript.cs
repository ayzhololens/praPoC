using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orientCameraScript : MonoBehaviour {

    // Update is called once per frame
    void FixedUpdate () {
        transform.rotation = ActorSingleton.Actor.transform.localRotation;

    }

}
