using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

using RenderHeads.Media.AVProVideo;

public class offsiteMediaPlayer : MonoBehaviour {

    public GameObject mediaWindow;
    public GameObject mediaPlane;
    public cameraZoomOverTime guidedTargetObj;
    public GameObject mainWindow;

    public bool closer;
    public Material photoMaterial;
    public Material videoMaterial;
    public Texture vidThumbnail;
    public Material thumbMat;
    public GameObject thumbPlane;

    JU_databaseMan.nodeItem currentNode;

    public MediaPlayer videoPlayer;
    public GameObject playButton;

    private void Start()
    {

    }

    private void OnMouseDown()
    {
        if (closer)
        {
            annotationsCollapseableBox.Instance.mediaPlaybackMinimapPlaneCol.enabled = false;
            mediaWindow.SetActive(false);
        }
        else
        {
            int nodeIndex = gameObject.GetComponent<offsiteFieldItemValueHolder>().nodeIndex;
            foreach(JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
            {
                if (node.indexNum == nodeIndex)
                {
                    currentNode = node;
                }
            }

            if(currentNode.type == 0)
            {
                mediaPlane.SetActive(false);
                playButton.SetActive(false);
                //print("simple");
            }
            else if (currentNode.type == 1)
            {
                mediaPlane.SetActive(true);
                mediaPlane.GetComponent<Renderer>().material = photoMaterial;
                playButton.SetActive(false);
                //print("photo");
            }
            else if (currentNode.type == 4)
            {
                mediaPlane.SetActive(true);
                mediaPlane.GetComponent<Renderer>().material = videoMaterial;
                loadVideo();
                playButton.SetActive(true);
                //print("video");
            }

            offsiteJSonLoader.Instance.populateComments(currentNode);
            guidedTargetObj.targetObject = offsiteJSonLoader.Instance.nodes3DList[currentNode.indexNum];
            guidedTargetObj.smoothZoom(offsiteJSonLoader.Instance.nodes3DList[currentNode.indexNum].transform);
            mediaPlayerWindowPopulator.Instance.populateMediaPlayerWindow(currentNode);
            annotationsCollapseableBox.Instance.mediaPlaybackMinimapPlaneCol.enabled = true;
            mediaWindow.SetActive(true);
        }
    }   

    void loadVideo()
    {
        videoPlayer.m_VideoPath = gameObject.GetComponent<offsiteFieldItemValueHolder>().path.text;
        videoPlayer.LoadVideoPlayer();
    }

    public void playVideo()
    {
        if (videoPlayer.m_VideoPath != gameObject.GetComponent<offsiteFieldItemValueHolder>().path.text)
        {
            loadVideo();
        }
        videoPlayer.Control.Play();
    }

    public void pauseVideo()
    {
        if (videoPlayer.Control.IsPlaying())
        {
            videoPlayer.Control.Pause();
        }
    }
}
