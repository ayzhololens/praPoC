using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class parentOnSpawn : NetworkBehaviour {

    private NetworkStartPosition[] spawnPoints;

    // Use this for initialization
    void Start () {
        if (isServer) { return; };
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        transform.SetParent(spawnPoints[0].transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
