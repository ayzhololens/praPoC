using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using HoloToolkit.Unity;
using RenderHeads.Media.AVProVideo.Demos;
using System.IO;

public class annotationsCollapseableBox : Singleton<annotationsCollapseableBox> {

    int nodeRow;
    int nodeColumn;
    int totalRows;

    public GameObject simpleNodePrefab;
    public GameObject photoNodePrefab;
    public GameObject videoNodePrefab;
    public GameObject nodesParent;

    public float expandSize;
    public GameObject scrollBox;
    public collapsableManager bigBox;

    public Collider mediaPlaybackMinimapPlaneCol;

    //media playing
    public GameObject offsiteMediaWindow;
    public GameObject mediaPlane;
    public Material videoMaterial;
    public MediaPlayer videoPlayer;
    public GameObject playButton;
    public cameraZoomOverTime guidedTargetObj;

    public Material photoMaterial;

    //minimapPlanePLayback
    public Material nodesCamMat;
    public GameObject minimapPlane;

    //video only
    public GameObject hideThis;

    public void populateNodes()
    {
        List<JU_databaseMan.nodeItem> nodesList = new List<JU_databaseMan.nodeItem>();
        foreach (JU_databaseMan.nodeItem nodeItem in JU_databaseMan.Instance.nodesManager.nodes)
        {
            //print("hi: "+ nodeItem.title);
            if (nodeItem.type == 2 || nodeItem.type == 3) { }
            else
            {
                nodesList.Add(nodeItem);
            }
        }
        nodeRow = 0;
        nodeColumn = 0;
        int max = nodesList.Count;

        for (int en = 0; en < max; en++)
        {
            int currentItem;
            int currentRow;
            int currentColumn;

            currentItem = en + 1;
            currentRow = nodeRow + 1;

            nodeRow = Mathf.FloorToInt((en + 1) / 3);
            nodeColumn = ((en % 3) + 1);

            currentColumn = nodeColumn;

            addOneNode(nodesParent, currentColumn, currentRow, nodesList[en]);
            totalRows = currentRow;
        }

        expandSize = 430 * totalRows;
        bigBox.startCollapse += expandSize;
        bigBox.readjustBox();
        //print("num of items: " + max);
        //print("num of rows: " + (nodeRow + 1));
        //print("num of columns: " +nodeColumn);
    }

    void addOneNode(GameObject parentObj, int currentColumn, int currentRow, JU_databaseMan.nodeItem nodeItem)
    {
        GameObject newItem;
        float xpos = (580 * (currentColumn - 1)) + 72;
        float ypos = (-408 * (currentRow - 1)) - 37;
        float initExpandSize = expandSize;

        if (nodeItem.type == 0)
        {
            newItem = Instantiate(simpleNodePrefab);
        }
        else if (nodeItem.type == 1)
        {
            newItem = Instantiate(photoNodePrefab);
            newItem.GetComponent<offsiteFieldItemValueHolder>().path.text = nodeItem.photos[0].path;

            Material newMat = Instantiate(photoMaterial);
            Texture2D targetTexture = new Texture2D(2048, 1152);

            string pathAppend = Path.Combine(Application.persistentDataPath, newItem.GetComponent<offsiteFieldItemValueHolder>().path.text);
            //string pathAppend = ( "C:\\Users\\ayzhololens\\AppData\\Local\\Packages\\PraesesPoC_pzq3xp76mxafg\\LocalState\\" + newItem.GetComponent<offsiteFieldItemValueHolder>().path.text);

            var bytesRead = System.IO.File.ReadAllBytes(pathAppend);
            targetTexture.LoadImage(bytesRead);
            newMat.mainTexture = targetTexture;

            newItem.GetComponent<offsiteMediaPlayer>().photoMaterial = newMat;
            newItem.GetComponent<offsiteMediaPlayer>().thumbPlane.GetComponent<Renderer>().material = newMat;

        }
        else if (nodeItem.type == 4)
        {
            newItem = Instantiate(videoNodePrefab);
            newItem.GetComponent<offsiteFieldItemValueHolder>().path.text = nodeItem.videos[0].path;
            newItem.GetComponent<offsiteMediaPlayer>().videoMaterial = videoMaterial;

            videoPlayer.m_VideoPath = newItem.GetComponent<offsiteFieldItemValueHolder>().path.text;
            videoPlayer.LoadVideoPlayer();
            newItem.GetComponent<offsiteMediaPlayer>().thumbMat = Instantiate(videoMaterial);
            videoPlayer.gameObject.GetComponent<FrameExtract>().activeComment = newItem;
            videoPlayer.gameObject.GetComponent<FrameExtract>().makeThumbnail();
        }
        else
        {
            newItem = new GameObject();
        }

        if (newItem == null)
        {
            Destroy(newItem);
            print("no add node created");
        }
        else
        {
            newItem.transform.SetParent(parentObj.transform);
            newItem.GetComponent<RectTransform>().localPosition = new Vector3(xpos, ypos, 0);
            newItem.GetComponent<RectTransform>().localScale = Vector3.one;
            expandSize = 430 * currentRow;
            scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
                                      expandSize);
            newItem.name = nodeItem.title;
            newItem.GetComponent<offsiteFieldItemValueHolder>().meta.text = (nodeItem.date + " - " + nodeItem.user);
            newItem.GetComponent<offsiteFieldItemValueHolder>().content.text = nodeItem.description;
            newItem.GetComponent<offsiteFieldItemValueHolder>().user = nodeItem.user;
            newItem.GetComponent<offsiteFieldItemValueHolder>().date = nodeItem.date;
            newItem.GetComponent<offsiteFieldItemValueHolder>().nodeIndex = nodeItem.indexNum;
            newItem.GetComponent<offsiteMediaPlayer>().mediaWindow = offsiteMediaWindow;
            newItem.GetComponent<offsiteMediaPlayer>().mediaPlane = mediaPlane;
            newItem.GetComponent<offsiteMediaPlayer>().guidedTargetObj = guidedTargetObj;
            newItem.GetComponent<offsiteMediaPlayer>().videoPlayer = videoPlayer;
            newItem.GetComponent<offsiteMediaPlayer>().playButton = playButton;
            newItem.GetComponent<offsiteMediaPlayer>().hideThis = hideThis;
            //fieldItemCollection.Add(fieldItem.Name, newItem);
        }

    }

}
