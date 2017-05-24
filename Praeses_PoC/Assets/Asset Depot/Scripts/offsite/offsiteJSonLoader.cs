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

    //for address
    public Text Location;
    public Text IDNum;
    public Text Address;
    public Text CertType;

    public Text inspector;
    public Text date;

    string certCode;
    string optionsText;
    string expDate;

    public void populateAddress()
    {
        inspector.text = metaManager.Instance.user;
        date.text = metaManager.Instance.date();

        Location.text = JU_databaseMan.Instance.definitions.LocationFields.LocationName;
        IDNum.text = JU_databaseMan.Instance.definitions.LocationFields.LocationID.ToString();
        if (JU_databaseMan.Instance.definitions.LocationFields.address2 != null)
        {
            Address.text = JU_databaseMan.Instance.definitions.LocationFields.address1 + ", " + JU_databaseMan.Instance.definitions.LocationFields.address2;
        }
        else
        {
            Address.text = JU_databaseMan.Instance.definitions.LocationFields.address1;
        }
        foreach (JU_databaseMan.valueItem val in JU_databaseMan.Instance.values.equipmentData)
        {
            if (val.name == "intActivityTypeID")
            {
                certCode = val.value;
            }
        }
        foreach (JU_databaseMan.fieldItem field in JU_databaseMan.Instance.definitions.ExtraFields.fields)
        {
            if (field.Name == "intActivityTypeID")
            {
                optionsText = field.Options[certCode];
            }
        }

        foreach (JU_databaseMan.valueItem val in JU_databaseMan.Instance.values.equipmentData)
        {
            if (val.name == "dtCertExpire")
            {
                expDate = val.value;
            }
        }

        CertType.text = optionsText + "- Exp: " + expDate;
    }

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
        string filepath = newItem.GetComponent<offsiteFieldItemValueHolder>().path.text;
        //Debug.Log(filepath);
        Texture2D targetTexture = new Texture2D(2048, 1152);

        var bytesRead = System.IO.File.ReadAllBytes(filepath);
        targetTexture.LoadImage(bytesRead);
        //newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Renderer>().material.mainTexture = targetTexture;
        //newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Image>().material = newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Renderer>().material;
        //newItem.GetComponent<offsiteMediaPlayer>().photoMaterial = newItem.GetComponent<offsiteFieldItemValueHolder>().thumbnail.GetComponent<Image>().material;
    }

}
