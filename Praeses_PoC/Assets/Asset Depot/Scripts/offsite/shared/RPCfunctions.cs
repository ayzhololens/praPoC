using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RPCfunctions : NetworkBehaviour {

    public GameObject focusGuidedTarget;

    public GameObject mediaNode;
    public JU_databaseMan.nodeItem currentNode;

    public GameObject playSymbol;
    public GameObject pauseSymbol;

    private void Start()
    {   //for scripted focus
        GameObject connectObj = GameObject.Find("connectScript");
        connectObj.GetComponent<sharedConnectButton>().avatarObj = gameObject;
        focusGuidedTarget = connectObj.GetComponent<sharedConnectButton>().focusGuidedTarget;

        //for scripted open panel
        int vioCount;
        int commentCount;
        int vioNodeNum;
        vioCount = violationsParentSpawner.Instance.spawnedVioPrefabs.Count -1;
        commentCount = violationsParentSpawner.Instance.spawnedVioPrefabs[vioCount].violationMedias.Count -1;
        mediaNode = violationsParentSpawner.Instance.spawnedVioPrefabs[vioCount].violationMedias[commentCount];
        vioNodeNum = JU_databaseMan.Instance.violationsManager.violations[vioCount].nodeIndex;
        foreach (JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
        {
            if (node.indexNum == vioNodeNum)
            {
                currentNode = node;
            }
        }

        //for scripted play video
        playSymbol = violationsParentSpawner.Instance.playButton;
        pauseSymbol = violationsParentSpawner.Instance.mediaPlane;
    }

    public void focus()
    {
        Rpcfocus();
    }

    [ClientRpc]
    public void Rpcfocus()
    {
        if (!isServer) { 
        print("focus");
        focusGuidedTarget.GetComponent<cameraZoomOverTime>().smoothZoom();
        }
    }

    [ClientRpc]
    public void RpcOpen()
    {
        print("open");
        JU_databaseMan.tempComment nullComment = new JU_databaseMan.tempComment();

        if (mediaNode.GetComponent<offsiteFieldItemValueHolder>().comment.type == 1)
        {
            mediaNode.GetComponent<offsiteMediaPlayer>().mediaPlane.SetActive(true);
            mediaNode.GetComponent<offsiteMediaPlayer>().mediaPlane.GetComponent<Renderer>().material = mediaNode.GetComponent<offsiteMediaPlayer>().thumbMat;
            mediaNode.GetComponent<offsiteMediaPlayer>().playButton.SetActive(false);
        }
        else if (mediaNode.GetComponent<offsiteFieldItemValueHolder>().comment.type == 2)
        {
            mediaNode.GetComponent<offsiteMediaPlayer>().mediaPlane.SetActive(true);
            mediaNode.GetComponent<offsiteMediaPlayer>().mediaPlane.GetComponent<Renderer>().material = mediaNode.GetComponent<offsiteMediaPlayer>().videoMaterial;
            mediaNode.GetComponent<offsiteMediaPlayer>().loadVideo();
            mediaNode.GetComponent<offsiteMediaPlayer>().playButton.SetActive(true);
        }

        nullComment = mediaNode.GetComponent<offsiteFieldItemValueHolder>().comment;
        violationsParentSpawner.Instance.minimapPlane.GetComponent<Image>().material = violationsParentSpawner.Instance.vioCamMat;

        mediaNode.GetComponent<offsiteMediaPlayer>().guidedTargetObj.targetObject = offsiteJSonLoader.Instance.nodes3DList[currentNode.indexNum];
        mediaNode.GetComponent<offsiteMediaPlayer>().guidedTargetObj.smoothZoom();
        mediaPlayerWindowPopulator.Instance.populateMediaPlayerWindow(currentNode, nullComment);
        annotationsCollapseableBox.Instance.mediaPlaybackMinimapPlaneCol.enabled = true;
        mediaNode.GetComponent<offsiteMediaPlayer>().mediaWindow.SetActive(true);


    }

    [ClientRpc]
    public void RpcPlay()
    {
        if (pauseSymbol.GetComponent<videoPauseButton>().videoPlayer.Control.IsPlaying())
        {
            print("pause");
            pauseSymbol.GetComponent<videoPauseButton>().pauseVideo();
        }else
        {
            print("play");
            playSymbol.GetComponent<videoPlayButton>().OnMouseDownProxy();
        }

    }
}
