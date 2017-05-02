using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orientCameraScript : MonoBehaviour {

    // Update is called once per frame
    void Update () {
        transform.rotation = ActorSingleton.Actor.transform.rotation;

    }

}
