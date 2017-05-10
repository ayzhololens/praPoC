using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RenderHeads.Media.AVProVideo;

public class offsiteMediaPlayer : MonoBehaviour {

    public GameObject mediaWindow;
    public GameObject mediaPlane;
    public CameraControlOffsite nodesMinimapCam;
    public GameObject mainWindow;

    public bool closer;
    public Material photoMaterial;
    public Material videoMaterial;
    public GameObject minimapGrp;
    public GameObject commentBoxObject;
    public GameObject descObject;
    public GameObject metaobject;

    Vector3 initPosMap;
    Vector3 initScaMap;
    Vector3 initPosCommentBox;
    Vector2 initSizeCommentBox;

    JU_databaseMan.nodeItem currentNode;

    public MediaPlayer videoPlayer;
    public GameObject playButton;

    private void Start()
    {
        if (!closer)
        {
            initPosMap = minimapGrp.GetComponent<RectTransform>().localPosition;
            initScaMap = minimapGrp.GetComponent<RectTransform>().localScale;

            initPosCommentBox = commentBoxObject.GetComponent<RectTransform>().localPosition;
            initSizeCommentBox = commentBoxObject.GetComponent<RectTransform>().sizeDelta;
        }

    }

    private void OnMouseDown()
    {
        if (closer)
        {
            mediaWindow.SetActive(false);
            //mainWindow.SetActive(true);
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
                offsetMinimap();
                playButton.SetActive(false);
                //print("simple");
            }
            else if (currentNode.type == 1)
            {
                mediaPlane.SetActive(true);
                resetMinimap();
                mediaPlane.GetComponent<Renderer>().material = photoMaterial;
                playButton.SetActive(false);
                //print("photo");
            }
            else if (currentNode.type == 4)
            {
                mediaPlane.SetActive(true);
                resetMinimap();
                mediaPlane.GetComponent<Renderer>().material = videoMaterial;
                loadVideo();
                playButton.SetActive(true);
                //print("video");
            }

            offsiteJSonLoader.Instance.populateComments(currentNode);
            descObject.GetComponent<Text>().text = currentNode.description;
            metaobject.GetComponent<Text>().text = (currentNode.date + " - " + currentNode.user);
            nodesMinimapCam.focus(currentNode.indexNum);
            mediaWindow.SetActive(true);
            //mainWindow.SetActive(false);
        }
    }
    
    void offsetMinimap()
    {
        minimapGrp.GetComponent<RectTransform>().localPosition = new Vector3(-2226, 1161, -25.789f);
        minimapGrp.GetComponent<RectTransform>().localScale = new Vector3(1.536f, 1.536f, 1.536f);

        commentBoxObject.GetComponent<RectTransform>().sizeDelta = new Vector2(commentBoxObject.GetComponent<RectTransform>().rect.width,
                                                                               commentBoxObject.GetComponent<RectTransform>().rect.height + 600);
        commentBoxObject.GetComponent<RectTransform>().localPosition = new Vector3(initPosCommentBox.x, initPosCommentBox.y + 600, initPosCommentBox.z);
    }

    void resetMinimap()
    {
        minimapGrp.GetComponent<RectTransform>().localPosition = initPosMap;
        minimapGrp.GetComponent<RectTransform>().localScale = initScaMap;

        commentBoxObject.GetComponent<RectTransform>().sizeDelta = initSizeCommentBox;
        commentBoxObject.GetComponent<RectTransform>().localPosition = initPosCommentBox;
    }

    void loadVideo()
    {
        videoPlayer.m_VideoPath = gameObject.GetComponent<offsiteFieldItemValueHolder>().path;
        videoPlayer.LoadVideoPlayer();
    }

    public void playVideo()
    {
        if (videoPlayer.m_VideoPath != gameObject.GetComponent<offsiteFieldItemValueHolder>().path)
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
