using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnVideoFrame : MonoBehaviour {

    public GameObject videoPlayer;
    public GameObject videoFrame;
    public GameObject activeVideoFrame;

    Vector3 spawnPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnMediaPlayer()
    {
        spawnPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.x + 1);
        activeVideoFrame = Instantiate(videoFrame, spawnPos, videoFrame.transform.rotation) as GameObject;

    }
}
