using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class videoPlayButton : MonoBehaviour {

    public MediaPlayer videoPlayer;
    public GameObject pauseButton;

    private void OnMouseDown()
    {
        OnMouseDownProxy();
    }

    private void Update()
    {
        if (videoPlayer.Control.IsFinished())
        {
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            pauseButton.GetComponent<Collider>().enabled = false;
        }
    }

    public void playVideo()
    {
        videoPlayer.Control.Play();
    }

    public void OnMouseDownProxy()
    {
        playVideo();
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        pauseButton.GetComponent<Collider>().enabled = true;
    }
}
