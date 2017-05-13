using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

using RenderHeads.Media.AVProVideo;

public class offsiteJSonLoader : Singleton<offsiteJSonLoader> {

    public GameObject fieldItemPrefab;
    public GameObject equipmentDataParent;
    public equipmentDataCollapseble equipmentCollapse;
    Dictionary<string, GameObject> fieldItemCollection = new Dictionary<string, GameObject>();

    int nodeRow;
    int nodeColumn;
    public GameObject simpleNodePrefab;
    public GameObject photoNodePrefab;
    public GameObject videoNodePrefab;
    public GameObject nodesParent;
    public collapseableBox nodesCollapsable;

    public Dictionary<int, GameObject> nodes3DList = new Dictionary<int, GameObject>();

    public GameObject commentSimplePrefab;
    public GameObject commentParent;
    public List<GameObject> commentHolder;
    public GameObject commentBox;

    //for media playing
    public GameObject offsiteMediaWindow;
    public GameObject mainWindow;
    public GameObject mediaPlane;
    public Material videoMaterial;
    public MediaPlayer videoPlayer;
    public GameObject playButton;
    public GameObject minimapGrp;
    public GameObject descObject;
    public CameraControlOffsite nodesMinimapCam;
    public GameObject metaObject;

    public void populateEquipment()
    {
        //definitions
        float yOffset = -23.11f;
        foreach (JU_databaseMan.fieldItem fieldItem in JU_databaseMan.Instance.definitions.EquipmentData.fields)
        {
            addOneField(equipmentDataParent, yOffset,fieldItem);
            yOffset += -94;
        }
        //values
        foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.equipmentData)
        {
            insertBasicValues(valueItem);
        }
    }

    void addOneField(GameObject parentObj, float yOffset, JU_databaseMan.fieldItem fieldItem)
    {
        GameObject newItem;
        float initExpandSize = equipmentCollapse.expandSize;
        newItem = Instantiate(fieldItemPrefab);
        newItem.transform.SetParent(parentObj.transform);
        newItem.GetComponent<RectTransform>().localPosition = new Vector3(9.88f, yOffset, 0);
        newItem.GetComponent<RectTransform>().localScale = new Vector3(.36f, .072f, .241f);
        equipmentCollapse.expandSize += 94.625f;
        equipmentCollapse.openCollapseable(-initExpandSize);
        newItem.name = fieldItem.Name;
        newItem.GetComponent<offsiteFieldItemValueHolder>().name.text = fieldItem.DisplayName;
        if (fieldItem.DisplayName.Length > 16)
        {
            newItem.GetComponent<offsiteFieldItemValueHolder>().name.resizeTextForBestFit = true;
        }
        fieldItemCollection.Add(fieldItem.Name, newItem);
    }

    void insertBasicValues(JU_databaseMan.valueItem valueItem)
    {

            if (fieldItemCollection.ContainsKey(valueItem.name))
            {
                fieldItemCollection[valueItem.name].GetComponent<offsiteFieldItemValueHolder>().value.text = valueItem.value;
            }
            else
            {
                //print(valueItem.name + " does not exist");
            }
    }

    public void populateNodes()
    {
        List<JU_databaseMan.nodeItem> nodesList = new List<JU_databaseMan.nodeItem>();
        foreach(JU_databaseMan.nodeItem nodeItem in JU_databaseMan.Instance.nodesManager.nodes)
        {
            //print("hi: "+ nodeItem.title);
            if(nodeItem.type == 2 || nodeItem.type == 3) { }
            else
            {
                nodesList.Add(nodeItem);
            }
        }
        nodeRow = 0;
        nodeColumn = 0;
        int max = nodesList.Count;

        for(int en=0; en < max; en++)
        {
            int currentItem;
            int currentRow;
            int currentColumn;

            currentItem = en + 1;
            currentRow = nodeRow + 1;

            nodeRow = Mathf.FloorToInt((en+1) / 3);
            nodeColumn = ((en%3) +1);

            currentColumn = nodeColumn;

            addOneNode(nodesParent, currentColumn, currentRow, nodesList[en]);
        }

        //print("num of items: " + max);
        //print("num of rows: " + (nodeRow + 1));
        //print("num of columns: " +nodeColumn);
    }

    void addOneNode(GameObject parentObj, int currentColumn, int currentRow, JU_databaseMan.nodeItem nodeItem)
    {
        GameObject newItem;
        float xpos = (580 * (currentColumn-1)) + 72;
        float ypos = (-408 * (currentRow-1)) -37;
        float initExpandSize = nodesCollapsable.expandSize;

        if (nodeItem.type == 0)
        {
            newItem = Instantiate(simpleNodePrefab);
        }
        else if(nodeItem.type == 1)
        {
            newItem = Instantiate(photoNodePrefab);
            newItem.GetComponent<offsiteFieldItemValueHolder>().path = nodeItem.photos[0].path;
            //newItem.GetComponent<offsiteMediaPlayer>().photoMaterial = photoMaterial;
        }
        else if(nodeItem.type == 4)
        {
            newItem = Instantiate(videoNodePrefab);
            newItem.GetComponent<offsiteFieldItemValueHolder>().path = nodeItem.videos[0].path;
            newItem.GetComponent<offsiteMediaPlayer>().videoMaterial = videoMaterial;
        }
        else
        {
            newItem = new GameObject();
        }
        
        if(newItem == null)
        {
            Destroy(newItem);
            print("no add node created");
        }else
        {
            newItem.transform.SetParent(parentObj.transform);
            newItem.GetComponent<RectTransform>().localPosition = new Vector3(xpos, ypos, 0);
            newItem.GetComponent<RectTransform>().localScale = Vector3.one;
            nodesCollapsable.expandSize = 430 * currentRow;
            nodesCollapsable.openCollapseable(-initExpandSize);
            newItem.name = nodeItem.title;
            newItem.GetComponent<offsiteFieldItemValueHolder>().meta.text = (nodeItem.date + " - " + nodeItem.user);
            newItem.GetComponent<offsiteFieldItemValueHolder>().content.text = nodeItem.description;
            newItem.GetComponent<offsiteFieldItemValueHolder>().user = nodeItem.user;
            newItem.GetComponent<offsiteFieldItemValueHolder>().date = nodeItem.date;
            newItem.GetComponent<offsiteFieldItemValueHolder>().nodeIndex = nodeItem.indexNum;
            newItem.GetComponent<offsiteMediaPlayer>().mediaWindow = offsiteMediaWindow;
            newItem.GetComponent<offsiteMediaPlayer>().mediaPlane = mediaPlane;
            newItem.GetComponent<offsiteMediaPlayer>().nodesMinimapCam = nodesMinimapCam;
            newItem.GetComponent<offsiteMediaPlayer>().minimapGrp = minimapGrp;
            newItem.GetComponent<offsiteMediaPlayer>().descObject = descObject;
            newItem.GetComponent<offsiteMediaPlayer>().metaobject = metaObject;
            newItem.GetComponent<offsiteMediaPlayer>().mainWindow = mainWindow;
            newItem.GetComponent<offsiteMediaPlayer>().commentBoxObject = commentBox;
            newItem.GetComponent<offsiteMediaPlayer>().videoPlayer = videoPlayer;
            newItem.GetComponent<offsiteMediaPlayer>().playButton = playButton;
            //fieldItemCollection.Add(fieldItem.Name, newItem);
        }

    }

    public void populateComments(JU_databaseMan.nodeItem nodeItem)
    {
        if(commentHolder.Count > 0)
        {
            foreach(GameObject obj in commentHolder)
            {
                Destroy(obj);
            }
            commentHolder.Clear();
        }

        float yOffset = 0;
        for (int iteration = 0; iteration < nodeItem.comments.Count; iteration++)
        {
            addOneComment(commentParent, yOffset, nodeItem.comments[iteration], iteration + 1);
            yOffset += -600;
        }
    }

    void addOneComment(GameObject parentObj, float yOffset, JU_databaseMan.comment commentItem, int iteration)
    {
        GameObject newItem;
        newItem = Instantiate(commentSimplePrefab);

        newItem.transform.SetParent(parentObj.transform);
        newItem.GetComponent<RectTransform>().localPosition = new Vector3(25, yOffset, 0);
        newItem.GetComponent<RectTransform>().localScale = new Vector3(2.300118f, 2.300118f, 2.300118f);
        commentParent.GetComponent<RectTransform>().sizeDelta = new Vector2(commentParent.GetComponent<RectTransform>().rect.width,
                                                                            iteration * 600);
        newItem.GetComponent<offsiteFieldItemValueHolder>().content.text = commentItem.content;
        newItem.GetComponent<offsiteFieldItemValueHolder>().user = commentItem.user;
        newItem.GetComponent<offsiteFieldItemValueHolder>().date = commentItem.date;

        commentHolder.Add(newItem);
    }

    public void loadPhoto(GameObject newItem)
    {
        string filepath = newItem.GetComponent<offsiteFieldItemValueHolder>().path;
        //Debug.Log(filepath);
        Texture2D targetTexture = new Texture2D(2048, 1152);

        var bytesRead = System.IO.File.ReadAllBytes(filepath);
        targetTexture.LoadImage(bytesRead);
        //newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Renderer>().material.mainTexture = targetTexture;
        //newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Image>().material = newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Renderer>().material;
        //newItem.GetComponent<offsiteMediaPlayer>().photoMaterial = newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Image>().material;
    }

}
