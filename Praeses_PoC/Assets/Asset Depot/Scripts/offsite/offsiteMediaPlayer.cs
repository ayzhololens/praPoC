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
    public Material photoMaterialUI;
    public Material videoMaterial;
    public Material videoMaterialUI;
    public Texture vidThumbnail;
    public Material thumbMat;
    public GameObject thumbPlane;
    public GameObject playerPlane;

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
            stopVideo();
            mediaWindow.SetActive(false);
        }else
        {

        }

    }

    private void OnMouseUp()
    {
        if (closer)
        { }
        else
        {
            scrollerCheck scrollScript;
            JU_databaseMan.tempComment nullComment = new JU_databaseMan.tempComment();
            int nodeIndex = gameObject.GetComponent<offsiteFieldItemValueHolder>().nodeIndex;
            foreach (JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
            {
                if (node.indexNum == nodeIndex)
                {
                    currentNode = node;
                }
            }

            if (currentNode.type == 3)
            {
                scrollScript = transform.parent.parent.parent.gameObject.GetComponent<scrollerCheck>();

                if (!scrollScript.clickBlocker)
                {
                    if (gameObject.GetComponent<offsiteFieldItemValueHolder>().comment.type == 1)
                    {
                        mediaPlane.SetActive(true);
                        mediaPlane.GetComponent<Renderer>().material = thumbMat;
                        playButton.SetActive(false);
                    }
                    else if (gameObject.GetComponent<offsiteFieldItemValueHolder>().comment.type == 2)
                    {
                        mediaPlane.SetActive(true);
                        mediaPlane.GetComponent<Renderer>().material = videoMaterial;
                        loadVideo();
                        playButton.SetActive(true);
                    }

                    nullComment = gameObject.GetComponent<offsiteFieldItemValueHolder>().comment;
                    violationsParentSpawner.Instance.minimapPlane.GetComponent<Image>().material = violationsParentSpawner.Instance.vioCamMat;

                    guidedTargetObj.targetObject = offsiteJSonLoader.Instance.nodes3DList[currentNode.indexNum];
                    guidedTargetObj.smoothZoom();
                    mediaPlayerWindowPopulator.Instance.populateMediaPlayerWindow(currentNode, nullComment);
                    annotationsCollapseableBox.Instance.mediaPlaybackMinimapPlaneCol.enabled = true;
                    mediaWindow.SetActive(true);
                }

            }
            else
            {
                offsiteJSonLoader.Instance.populateComments(currentNode);
                annotationsCollapseableBox.Instance.minimapPlane.GetComponent<Image>().material = annotationsCollapseableBox.Instance.nodesCamMat;
                scrollScript = transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<scrollerCheck>();
                print(scrollScript.gameObject.name);
                if (!scrollScript.clickBlocker)
                {
                    if (currentNode.type == 0)
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
                    guidedTargetObj.targetObject = offsiteJSonLoader.Instance.nodes3DList[currentNode.indexNum];
                    guidedTargetObj.smoothZoom();
                    mediaPlayerWindowPopulator.Instance.populateMediaPlayerWindow(currentNode, nullComment);
                    annotationsCollapseableBox.Instance.mediaPlaybackMinimapPlaneCol.enabled = true;
                    mediaWindow.SetActive(true);
                }
            }
        }

        

    }

    public void loadVideo()
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

    public void stopVideo()
    {
        if (videoPlayer.Control.IsPlaying())
        {
            videoPlayer.Control.Stop();
        }
    }
}
