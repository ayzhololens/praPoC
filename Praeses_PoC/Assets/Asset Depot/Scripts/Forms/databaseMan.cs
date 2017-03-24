#if WINDOWS_UWP
using Newtonsoft.Json;
using System.Collections;
#endif

using Newtonsoft.Json;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;
using HoloToolkit.Unity;

public class databaseMan : Singleton<databaseMan>
{
    public string saveDir; 
    public string definitionsDir;
    public string valuesDir;
    public string defJsonText;
    public string valJsonText;
    public MainForm definitions;
    public ValuesClass values;

    public GameObject testItem;

    public Dictionary<string, GameObject> formPairs = new Dictionary<string, GameObject>();

    private void Start()
    {
        definitionsDir = Path.Combine(Application.persistentDataPath, "JO_JJ.json");
        valuesDir = Path.Combine(Application.persistentDataPath, "JO_JJ_values.json");
        saveDir = Path.Combine(Application.persistentDataPath, "savedJson.json");
    }
    
    [System.Serializable]
    public class MainForm
    {
        public EquipmentFieldsClass EquipmentFields = new EquipmentFieldsClass();
        public EquipmentFieldsClass EquipmentInspectionFields = new EquipmentFieldsClass();
    }

    [System.Serializable]
    public class EquipmentFieldsClass
    {
        public List<fieldItem> threeNine = new List<fieldItem>();
    }

    [System.Serializable]
    public class fieldItem
    {
        public string DisplayName;
        public int FieldType;
        public string Name;
        public bool Required;
        public Dictionary<string,string> Options = new Dictionary<string, string>();
    }

    [System.Serializable]
    public class ValuesClass
    {
        public LocationsClass Location = new LocationsClass();
    }

    [System.Serializable]
    public class LocationsClass
    {
        public int LocationID;
        public string LocationName;
        public AddressClass Address = new AddressClass();
        public List<ObjectsClass> Equipment = new List<ObjectsClass>();
    }

    [System.Serializable]
    public class AddressClass
    {
        public string address1;
        public string address2;
        public string City;
        public string County;
        public string Country;
        public string State;
        public string Zip;
    }

    [System.Serializable]
    public class ObjectsClass
    {
        public List<ItemClass> EquipmentData = new List<ItemClass>();
        public List<PreviousInspectionClass> PreviousInspection = new List<PreviousInspectionClass>();
        public List<ItemClass> CurrentInspection = new List<ItemClass>();
        public List<ViolationsClass> Violations = new List<ViolationsClass>();
        public List<NodeClass> Nodes = new List<NodeClass>();
    }

    [System.Serializable]
    public class PreviousInspectionClass
    {
        public List<ItemClass> InspectionData = new List<ItemClass>();
    }

    [System.Serializable]
    public class ViolationsClass
    {
        public Dictionary<string, string> ViolationData = new Dictionary<string, string>();
        public int nodeIndex;
    }

    [System.Serializable]
    public class ItemClass
    {
        public string name;
        public string value;
        public int nodeIndex;
    }

    [System.Serializable]
    public class NodeClass
    {
        public List<float> transform = new List<float>();
        public string title;
        public string user;
        public string date;
        public string description;
        public string audioPath;
        public List<comment> comments = new List<comment>();
        public List<media> medias = new List<media>();
        public int indexNum;
        //1=generic,2=form, 3=violation
        public int type;
    }

    [System.Serializable]
    public class comment
    {
        public string content;
        public string user;
        public string date;
    }

    [System.Serializable]
    public class media
    {
        //1=simple,2=photo,3=video
        public int type;
        public string path;
        public string user;
        public string date;
    }

    [System.Serializable]
    public class tempComment
    {
        //1=simple,2=photo,3=video
        public int type;
        public string path;
        public string content;
        public string user;
        public string date;
    }

    public void saveCmd()
    {
#if WINDOWS_UWP
        string json = JsonConvert.SerializeObject(values, Formatting.Indented);
        System.IO.File.WriteAllText(saveDir, json);
#endif

        string json = JsonConvert.SerializeObject(values, Formatting.Indented);
        System.IO.File.WriteAllText(saveDir, json);

        print("jsonSaved");
    }

    public void loadDefCmd()
    {
#if WINDOWS_UWP
        defJsonText = File.ReadAllText(definitionsDir);
        definitions = JsonConvert.DeserializeObject<MainForm>(defJsonText);
#endif

        defJsonText = File.ReadAllText(definitionsDir);
        definitions = JsonConvert.DeserializeObject<MainForm>(defJsonText);

        print("jsonDefinitionsLoaded");
        loadValCmd();
        JU_databaseMan.Instance.loadDefCmd();
}

    public void loadValCmd()
    {
#if WINDOWS_UWP
        Debug.Log("reading from: " + valuesDir);
        valJsonText = File.ReadAllText(valuesDir);
        values = JsonConvert.DeserializeObject<ValuesClass>(valJsonText);
#endif

        valJsonText = File.ReadAllText(valuesDir);
        values = JsonConvert.DeserializeObject<ValuesClass>(valJsonText);

        print("jsonValuesLoaded");
    }

    public void storeNodesList()
    {
        values.Location.Equipment[0].Nodes.Clear();
        foreach (GameObject node in annotationManager.Instance.activeAnnotations)
        {
            addAnnotation(node);
        }
        saveCmd();
    }

    public void addAnnotation(GameObject nodeObj)
    {
        NodeClass newNode = new NodeClass();
        Vector3 pos = nodeObj.transform.localPosition;
        Quaternion rot = nodeObj.transform.localRotation;
        Vector3 sca = nodeObj.transform.localScale;

        float[] floats = new float[] { pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w, sca.x, sca.y, sca.z };
        foreach (float flo in floats)
        {
            newNode.transform.Add(flo);
        };

        //0=simple, 1=photo,2=form, 3=violation, 4=video
        if (nodeObj.GetComponent<nodeMediaHolder>().fieldNode)
        {
            newNode.type = 2;
        }
        else if (nodeObj.GetComponent<nodeMediaHolder>().violationNode)
        {
            newNode.type = 3;
        }
        else if (nodeObj.GetComponent<nodeMediaHolder>().simpleNode)
        {
            newNode.type = 0;
        }
        else if (nodeObj.GetComponent<nodeMediaHolder>().videoNode)
        {
            newNode.type = 4;
        }
        else if (nodeObj.GetComponent<nodeMediaHolder>().photoNode)
        {
            newNode.type = 1;
        }
        newNode.user = nodeObj.GetComponent<nodeMediaHolder>().User;
        newNode.date = nodeObj.GetComponent<nodeMediaHolder>().Date;

        if (newNode.type == 4 || newNode.type == 1)
        {
            foreach (GameObject comment in nodeObj.GetComponent<nodeMediaHolder>().activeComments)
            {
                comment newComment = new comment();
                newComment.content = comment.GetComponent<commentContents>().commentMain.text;
                newComment.user = comment.GetComponent<commentContents>().user;
                newComment.date = comment.GetComponent<commentContents>().Date;
                newNode.comments.Add(newComment);
            }
            media newMedia = new media();
            if (newNode.type == 1)
            {
                newMedia.type = 2;
            }
            else if(newNode.type == 4)
            {
                newMedia.type = 3;
            }
            newMedia.path = nodeObj.GetComponent<nodeMediaHolder>().activeFilepath;
            newMedia.user = nodeObj.GetComponent<nodeMediaHolder>().User;
            newMedia.date = nodeObj.GetComponent<nodeMediaHolder>().Date;
            newNode.medias.Add(newMedia);
        }
        else
        {
            foreach (GameObject comment in nodeObj.GetComponent<nodeMediaHolder>().activeComments)
            {
                if (comment.GetComponent<commentContents>().isSimple)
                {
                    comment newComment = new comment();
                    newComment.content = comment.GetComponent<commentContents>().commentMain.text;
                    newComment.user = comment.GetComponent<commentContents>().user;
                    newComment.date = comment.GetComponent<commentContents>().Date;
                    newNode.comments.Add(newComment);
                }
                else
                {
                    media newMedia = new media();
                    newMedia.path = comment.GetComponent<commentContents>().filepath;
                    if (comment.GetComponent<commentContents>().isPhoto)
                    {
                        newMedia.type = 2;
                    }
                    else
                    {
                        newMedia.type = 3;
                    }
                    newMedia.user = comment.GetComponent<commentContents>().user;
                    newMedia.date = comment.GetComponent<commentContents>().Date;
                    newNode.medias.Add(newMedia);
                }

            }
        }
        

        if (newNode.type == 2)
        {
            newNode.title = nodeObj.GetComponent<formNodeController>().linkedField.GetComponent<formFieldController>().DisplayName.text;
            newNode.description = nodeObj.GetComponent<formNodeController>().linkedField.GetComponent<formFieldController>().Value.text;
            newNode.audioPath = "";
        }
        else
        { 
        newNode.title = nodeObj.GetComponent<nodeMediaHolder>().Title.text;       
        newNode.description = nodeObj.GetComponent<nodeMediaHolder>().Description.text;
        newNode.audioPath = nodeObj.GetComponent<nodeMediaHolder>().audioPath;   
        }

        newNode.indexNum = nodeObj.GetComponent<nodeMediaHolder>().NodeIndex;       

        values.Location.Equipment[0].Nodes.Add(newNode);
        JU_databaseMan.Instance.loadNodesCmd();
    }

    public void formToClassValueSync(string keyword, string value)
    {
        print("values applied " + keyword + " " + value);
        Dictionary<string, ItemClass> itemClasses = new Dictionary<string, ItemClass>();
        foreach (ItemClass item in values.Location.Equipment[0].EquipmentData)
        {
            itemClasses.Add(item.name, item);
        };
        //foreach (ItemClass item in values.Location.Equipment[0].PreviousInspection[0].InspectionData)
        //{
        //    itemClasses.Add(item.name, item);
        //};

        foreach (ItemClass item in values.Location.Equipment[0].CurrentInspection)
        {
            itemClasses.Add(item.name, item);
        };

        if (itemClasses.ContainsKey(keyword))
        {
            itemClasses[keyword].value = value;
        }else
        {
            ItemClass newItem = new ItemClass();
            newItem.value = value;
            newItem.name = keyword;
            values.Location.Equipment[0].EquipmentData.Add(newItem);
        }
            

        JU_databaseMan.Instance.loadCurrentData();
        JU_databaseMan.Instance.loadEquipmentData();
    }

}
