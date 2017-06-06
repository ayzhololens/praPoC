#if WINDOWS_UWP
using Newtonsoft.Json;
using System.Collections;
#endif

//using Newtonsoft.Json;
//using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;
using HoloToolkit.Unity;

public class databaseMan : Singleton<databaseMan>
{
    public string saveDir { get; set; }
    public string definitionsDir { get; set; }
    public string valuesDir { get; set; }
    public string defJsonText { get; set; }
    public string valJsonText { get; set; }

    //this defines whether we are using the pop-up model or not. This is defined through the boilerTransform.json
    //boilerTransform.json is written by the boilerTransformRecorder.cs script on the SpatialMapping object when tapping finalize during te admin phase
    public float popUp { get; set; }

    [Tooltip("This decides whether to load from a historic Json or the Json that was saved from the recent session")]
    public bool offsite;

    public MainForm definitions;
    public ValuesClass values;

    //unity does not display this on the inspector, but this is used to pair keywords with form prefabs
    public Dictionary<string, GameObject> formPairs = new Dictionary<string, GameObject>();

    private void Start()
    {
        //popup defaults to 0, this will change if during the admin phase it is set to popup
        popUp = 0;

        //establish all the necessary directories
        definitionsDir = Path.Combine(Application.persistentDataPath, "JO_JJ.json");
        if (offsite && System.IO.File.Exists(Path.Combine(Application.persistentDataPath, "savedJson.json")))
        {
            valuesDir = Path.Combine(Application.persistentDataPath, "savedJson.json");
        }
        else
        {
            valuesDir = Path.Combine(Application.persistentDataPath, "JO_JJ_values.json");
        }
        saveDir = Path.Combine(Application.persistentDataPath, "savedJson.json");
    }

    //definitions of classes pertaining Json file structure
    #region
    [System.Serializable]
    public class MainForm
    {
        public EquipmentFieldsClass EquipmentFields = new EquipmentFieldsClass();
        public EquipmentFieldsClass EquipmentInspectionFields = new EquipmentFieldsClass();
        public EquipmentFieldsClass nonDisplayedFields = new EquipmentFieldsClass();
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
        public Dictionary<string, string> Options = new Dictionary<string, string>();
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
        public string category;
        public int classifications;
        public string violationDate;
        public int status;
        public string resolveDate;
        public string requirements;
        public string conditions;
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
    #endregion

    public void saveCmd()
    {
#if WINDOWS_UWP
        string json = JsonConvert.SerializeObject(values, Formatting.Indented);
        System.IO.File.WriteAllText(saveDir, json);
#endif
        //comment these out for deployment
        //string json = JsonConvert.SerializeObject(values, Formatting.Indented);
        //System.IO.File.WriteAllText(saveDir, json);

        print("jsonSaved");
    }

    public void loadDefCmd()
    {
#if WINDOWS_UWP
        defJsonText = File.ReadAllText(definitionsDir);
        definitions = JsonConvert.DeserializeObject<MainForm>(defJsonText);
#endif
        //comment these out for deployment
        //defJsonText = File.ReadAllText(definitionsDir);
        //definitions = JsonConvert.DeserializeObject<MainForm>(defJsonText);

        print("jsonDefinitionsLoaded");
        JU_databaseMan.Instance.loadDefCmd();
    }
    
    public void loadValCmd()
    {
#if WINDOWS_UWP
        Debug.Log("reading from: " + valuesDir);
        valJsonText = File.ReadAllText(valuesDir);
        values = JsonConvert.DeserializeObject<ValuesClass>(valJsonText);
#endif
        //comment these out for deployment
        //valJsonText = File.ReadAllText(valuesDir);
        //values = JsonConvert.DeserializeObject<ValuesClass>(valJsonText);

        print("jsonValuesLoaded");
        JU_databaseMan.Instance.loadValCmd();
    }

    //this function is run when a new annotation(simple, photo, video), field node, or violation node is created
    //it will add and feed the necessary information back into the class so we can save it back out as a json file
    public void addAnnotation(GameObject nodeObj)
    {
        //create a new node class feeding transform information from the 3D game object
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

        //no matter what type, get the meta info
        newNode.user = nodeObj.GetComponent<nodeMediaHolder>().User;
        newNode.date = nodeObj.GetComponent<nodeMediaHolder>().Date;

        //for the types video or photo
        if (newNode.type == 4 || newNode.type == 1)
        {
            //iterate through its list of comments
            foreach (GameObject comment in nodeObj.GetComponent<nodeMediaHolder>().activeComments)
            {
                comment newComment = new comment();
                newComment.content = comment.GetComponent<commentContents>().commentMain.text;
                newComment.user = comment.GetComponent<commentContents>().user;
                newComment.date = comment.GetComponent<commentContents>().Date;
                newNode.comments.Add(newComment);
            }
            media newMedia = new media();
            //set the differentiation between photo or video
            if (newNode.type == 1)
            {
                newMedia.type = 2;
            }
            else if (newNode.type == 4)
            {
                newMedia.type = 3;
            }

            //add metas
            newMedia.path = nodeObj.GetComponent<nodeMediaHolder>().fileName;
            newMedia.user = nodeObj.GetComponent<nodeMediaHolder>().User;
            newMedia.date = nodeObj.GetComponent<nodeMediaHolder>().Date;
            newNode.medias.Add(newMedia);
        }
        //beyond this it should be simple type field or violation
        else
        {
            foreach (GameObject comment in nodeObj.GetComponent<nodeMediaHolder>().activeComments)
            {
                //simple comment is acceptable in a simple node as well as field or violation nodes
                if (comment.GetComponent<commentContents>().isSimple)
                {
                    comment newComment = new comment();
                    newComment.content = comment.GetComponent<commentContents>().commentMain.text;
                    newComment.user = comment.GetComponent<commentContents>().user;
                    newComment.date = comment.GetComponent<commentContents>().Date;
                    newNode.comments.Add(newComment);
                }
                //these media items below refer to photo comment or video comment on a field or violation node
                else
                {
                    media newMedia = new media();
                    if (comment.GetComponent<commentContents>().isPhoto)
                    {
                        newMedia.type = 2;
                        newMedia.path = comment.GetComponent<commentContents>().fileName;
                    }
                    else
                    {
                        newMedia.type = 3;
                        newMedia.path = comment.GetComponent<commentContents>().filepath;
                    }
                    newMedia.user = comment.GetComponent<commentContents>().user;
                    newMedia.date = comment.GetComponent<commentContents>().Date;
                    newNode.medias.Add(newMedia);
                }

            }
        }

        //simple text specific operation
        if (newNode.type == 0)
        {
            newNode.title = "Simple Text";
            newNode.description = nodeObj.GetComponent<nodeMediaHolder>().Description.text;
            newNode.audioPath = nodeObj.GetComponent<nodeMediaHolder>().audioPath;
        }
        //field node specific operation
        else if (newNode.type == 2)
        {
            print("ahh");
            newNode.title = nodeObj.GetComponent<nodeController>().linkedField.GetComponent<formFieldController>().DisplayName.text;
            newNode.description = nodeObj.GetComponent<nodeController>().linkedField.GetComponent<formFieldController>().Value.text;
            newNode.audioPath = "";
        }
        //violations node specific operation
        else if (newNode.type == 3)
        {
        }
        else
        {
            newNode.title = nodeObj.GetComponent<nodeMediaHolder>().Title.text;
            newNode.description = nodeObj.GetComponent<nodeMediaHolder>().Description.text;
            newNode.audioPath = nodeObj.GetComponent<nodeMediaHolder>().audioPath;
        }

        newNode.indexNum = nodeObj.GetComponent<nodeMediaHolder>().NodeIndex;

        //after configuring the contents of the node, we add it to the nodes list in the database
        values.Location.Equipment[0].Nodes.Add(newNode);
        //update this under the JU_database for easy retrieval
        JU_databaseMan.Instance.loadNodesCmd();
    }

    //removing a node from a class, they are put into a temp list, delete the node, clearing the original nodes list
    //then re-adding them back onto the original list without the deleted node
    public void removeNode(GameObject nodeObj)
    {
        List<NodeClass> tempNodeList = new List<NodeClass>();

        foreach (NodeClass node in values.Location.Equipment[0].Nodes)
        {
            if (nodeObj.GetComponent<nodeMediaHolder>().NodeIndex != node.indexNum)
            {
                tempNodeList.Add(node);
            }
        }

        values.Location.Equipment[0].Nodes.Clear();

        foreach (NodeClass node in tempNodeList)
        {
            values.Location.Equipment[0].Nodes.Add(node);
        }

        JU_databaseMan.Instance.loadNodesCmd();
    }

    //using the keyword value, iterate through existing items in the dictionary, when a match is found, change its value
    //this is called when the done button is pressed on the keyboard
    public void formToClassValueSync(string keyword, string value)
    {
        print("values applied " + keyword + " " + value);
        Dictionary<string, ItemClass> itemClasses = new Dictionary<string, ItemClass>();
        foreach (ItemClass item in values.Location.Equipment[0].EquipmentData)
        {
            itemClasses.Add(item.name, item);
        };

        foreach (ItemClass item in values.Location.Equipment[0].CurrentInspection)
        {
            itemClasses.Add(item.name, item);
        };

        if (itemClasses.ContainsKey(keyword))
        {
            itemClasses[keyword].value = value;
        }
        else
        {
            ItemClass newItem = new ItemClass();
            newItem.value = value;
            newItem.name = keyword;
            values.Location.Equipment[0].CurrentInspection.Add(newItem);
        }


        JU_databaseMan.Instance.loadCurrentData();
        JU_databaseMan.Instance.loadEquipmentData();
    }

    //this is called when a node is edited through a keyboard
    public void nodeToClassValueSync(int nodeIndex, string value, int valueType)
    {
        print("values applied to node index: " + nodeIndex.ToString() + " " + value);
        Dictionary<int, NodeClass> nodeClasses = new Dictionary<int, NodeClass>();

        foreach (NodeClass node in values.Location.Equipment[0].Nodes)
        {
            nodeClasses.Add(node.indexNum, node);
        };

        if (nodeClasses.ContainsKey(nodeIndex))
        {
            if (valueType == 0)
            {
                nodeClasses[nodeIndex].title = value;
            }
            else
            {
                nodeClasses[nodeIndex].description = value;
            }

        }

        JU_databaseMan.Instance.loadCurrentData();
        JU_databaseMan.Instance.loadEquipmentData();
    }

    //this is called when a new comment is created on a node
    public void commentToClassValueSync(int nodeIndex, tempComment comment)
    {
        Dictionary<int, NodeClass> nodeClasses = new Dictionary<int, NodeClass>();

        foreach (NodeClass node in values.Location.Equipment[0].Nodes)
        {
            nodeClasses.Add(node.indexNum, node);
        };

        if (nodeClasses.ContainsKey(nodeIndex))
        {
            //differentiate between simple text comment
            if (comment.type == 0)
            {
                comment newComment = new comment();
                newComment.content = comment.content;
                newComment.user = comment.user;
                newComment.date = comment.date;
                nodeClasses[nodeIndex].comments.Add(newComment);
            }
            //or media comments(photo or video) defined by the type
            else
            {
                media newMedia = new media();
                newMedia.path = comment.path;
                newMedia.user = comment.user;
                newMedia.date = comment.date;
                newMedia.type = comment.type;
                nodeClasses[nodeIndex].medias.Add(newMedia);
            }

        }

        JU_databaseMan.Instance.loadNodesCmd();

    }

    //this is called when a  new violation is created or when a violation is read from json
    public void syncViolation(violationController violation)
    {
        ViolationsClass vioClass = new ViolationsClass();
        vioClass.category = (violation.violationIndices[0].ToString() + "." + violation.violationIndices[1].ToString() + "." + violation.violationIndices[2].ToString());
        vioClass.classifications = violation.violationIndices[3];
        vioClass.violationDate = metaManager.Instance.dateShort();
        vioClass.status = violation.violationIndices[7];
        vioClass.resolveDate = violation.violationData[4];
        vioClass.conditions = violation.violationData[5];
        vioClass.requirements = violation.violationData[6];
        vioClass.nodeIndex = violation.linkedNode.GetComponent<nodeMediaHolder>().NodeIndex;

        values.Location.Equipment[0].Violations.Add(vioClass);

        JU_databaseMan.Instance.loadViolationsCmd();
    }

    //this is called when a violation is being edited, as well as if new comments are added to it
    public void updateVio(violationController violation)
    {
        foreach(ViolationsClass vioClass in values.Location.Equipment[0].Violations)
        {
            if (violation.linkedNode.GetComponent<nodeMediaHolder>().NodeIndex == vioClass.nodeIndex)
            {
                vioClass.status = violation.violationIndices[7];
            }
        }


        foreach (NodeClass node in values.Location.Equipment[0].Nodes)
        {
            if (node.indexNum == violation.linkedNode.GetComponent<nodeMediaHolder>().NodeIndex)
            {
                node.comments.Clear();
                node.medias.Clear();

                commentManager comManag = violation.GetComponent<commentManager>();


                foreach (GameObject comment in comManag.activeComments)
                {
                    if (comment.GetComponent<commentContents>().isSimple)
                    {
                        comment newComment = new comment();
                        newComment.content = comment.GetComponent<commentContents>().commentMain.text;
                        newComment.user = comment.GetComponent<commentContents>().user;
                        newComment.date = comment.GetComponent<commentContents>().Date;
                        node.comments.Add(newComment);
                    }
                    else
                    {
                        media newMedia = new media();
                        
                        if (comment.GetComponent<commentContents>().isPhoto)
                        {
                            newMedia.type = 2;
                            newMedia.path = comment.GetComponent<commentContents>().fileName;

                        }
                        else
                        {
                            newMedia.type = 3;
                            newMedia.path = comment.GetComponent<commentContents>().filepath;
                        }
                        newMedia.user = comment.GetComponent<commentContents>().user;
                        newMedia.date = comment.GetComponent<commentContents>().Date;
                        node.medias.Add(newMedia);
                    }

                }

            }
        }
        

 

        JU_databaseMan.Instance.loadViolationsCmd();
    }
}
