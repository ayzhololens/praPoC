using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using RenderHeads.Media.AVProVideo;

public class helpMenuController : Singleton<helpMenuController> {

    public GameObject contentHolder;
    public MediaPlayer[] gestureAnims;
    public MediaPlayer gestureAnim;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openHelp()
    {
        contentHolder.SetActive(true);
        contentHolder.transform.position = frontHolderInstance.Instance.setFrontHolder(1.5f).transform.position;

        for (int i = 0; i<gestureAnims.Length; i++)
        {
            gestureAnims[i].LoadVideoPlayer();
            gestureAnims[i].Control.Play();
        }
    }

    public void closeHelp()
    {
        for (int i = 0; i < gestureAnims.Length; i++)
        {
            gestureAnims[i].Control.Stop();
        }
        contentHolder.SetActive(false);
    }
}
