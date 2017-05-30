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

    //for video only in vio
    public GameObject hideThis;
    Vector3 initPos;
    Vector3 initScale;  

    private void Start()
    {
        initPos = new Vector3(1442.008f, 110.0012f, -118.0077f);
        initScale = new Vector3(189.6336f, 189.6336f, 105.3446f); 
    }

    void videoOnly()
    {
        hideThis.SetActive(false);
        mediaPlane.transform.localPosition = new Vector3(843, -180 , -118.0077f);
        mediaPlane.transform.localScale = new Vector3(295.3811f, 766.8397f, 164.089f);
    }

    void allVisible()
    {
        hideThis.SetActive(true);
        mediaPlane.transform.localPosition = initPos;
        mediaPlane.transform.localScale = initScale;
    }

    private void OnMouseDown()
    {
        if (closer)
        {
            annotationsCollapseableBox.Instance.mediaPlaybackMinimapPlaneCol.enabled = false;
            stopVideo();
            mediaWindow.SetActive(false);
            allVisible();
        }
        else
        {

        }

    }

   private void Update()
    {
        if (vidThumbnail != null && thumbMat.mainTexture != vidThumbnail)
        {

            thumbMat.mainTexture = vidThumbnail;
            thumbPlane.GetComponent<Renderer>().material = thumbMat;
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

                    videoOnly();
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
