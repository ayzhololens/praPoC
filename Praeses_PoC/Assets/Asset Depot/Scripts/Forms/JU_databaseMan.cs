using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

using UnityEngine.UI;
using HoloToolkit.Unity;

public class JU_databaseMan : Singleton<JU_databaseMan>
{
    public MainForm definitions;
    public ValuesClass values;
    public NodesManager nodesManager;
    public ViolationsManager violationsManager;

    public Dictionary<string, GameObject> formPairs = new Dictionary<string, GameObject>();

    //class definitions
    #region
    [System.Serializable]
    public class MainForm
    {
        public LocationsClass LocationFields = new LocationsClass();
        public EquipmentInspectionFields EquipmentData = new EquipmentInspectionFields();
        public EquipmentInspectionFields InspectionFields = new EquipmentInspectionFields();
        public EquipmentInspectionFields ExtraFields = new EquipmentInspectionFields();
        public EquipmentInspectionFields nonDisplayedFields = new EquipmentInspectionFields();
    }

    [System.Serializable]
    public class EquipmentInspectionFields
    {
        public List<fieldItem> fields = new List<fieldItem>();
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
    public class LocationsClass
    {
        public string address1;
        public string address2;
        public string City;
        public string County;
        public string Country;
        public int LocationID;
        public string LocationName;
        public string State;
        public string Zip;
    }

    [System.Serializable]
    public class NodesManager
    {
        public List<nodeItem> nodes = new List<nodeItem>();
    }

    [System.Serializable]
    public class nodeItem
    {
        public List<float> transform = new List<float>();
        public string title;
        public string user;
        public string date;
        public string description;
        public List<comment> comments = new List<comment>();
        public List<media> photos = new List<media>();
        public List<media> videos = new List<media>();
        public int indexNum;
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
        public string path;
        public string user;
        public string date;
    }

    [System.Serializable]
    public class tempComment
    {
        //0 = simple, 1 =photo, 2 = video
        public string content;
        public string path;
        public string user;
        public string date;
        public int type;
    }

    [System.Serializable]
    public class ValuesClass
    {
        public List<valueItem> equipmentData = new List<valueItem>();
        public List<valueItem> historicData = new List<valueItem>();
        public List<valueItem> currentData = new List<valueItem>();
        public List<valueItem> extraData = new List<valueItem>();
    }

    [System.Serializable]
    public class valueItem
    {
        public string name;
        public string value;
        public int nodeIndex;
    }

    [System.Serializable]
    public class compareItem
    {
        public string name;
        public string displayName;
        public string value;
        public string oldValue;
        public int nodeIndex;
    }

    [System.Serializable]
    public class ViolationsManager
    {
        public List<ViolationsItem> violations = new List<ViolationsItem>();
    }

    [System.Serializable]
    public class ViolationsItem
    {
        public int category;
        public int subCategory;
        public int specific;
        public int severity;
        public string violationDate;
        public string resolveDate;
        public int status;
        public string conditions;
        public string requirements;
        public int nodeIndex;
    }
    #endregion

    ///<summary>
    ///reads definitions from databaseMan
    ///</summary>
    public void loadDefCmd()
    {
        readEquipmentFields();
        readInspectionFields();
        readNonDisplayedFields();
    }

    ///<summary>
    ///apply values from databaseMan
    ///</summary>
    public void loadValCmd()
    {
        readLocationFields();
        loadEquipmentData();
        loadHistoric();
        loadCurrentData();
        loadNodesCmd();
        loadViolationsCmd();
    }

    ///<summary>
    ///load the value contents of a node from databaseMan into this class
    ///</summary>
    public void loadNodesCmd()
    {
        nodesManager.nodes.Clear();
        //creating a new temp list so we can reorder by date later
        List<nodeItem> tempNodeList = new List<nodeItem>();

        databaseMan.ObjectsClass objectItem = databaseMan.Instance.values.Location.Equipment[0];

        //iterate through nodes in the databaseMan 
        foreach (databaseMan.NodeClass node in objectItem.Nodes)
        {
            nodeItem newNodeItem = new nodeItem();

            newNodeItem.transform = node.transform;

            newNodeItem.title = node.title;
            newNodeItem.user = node.user;
            newNodeItem.date = node.date;
            newNodeItem.description = node.description;
            //iterate through the comments within the node(for simple text type)
            foreach (databaseMan.comment commentItem in node.comments)
            {
                comment newComment = new comment();
                newComment.content = commentItem.content;
                newComment.user = commentItem.user;
                newComment.date = commentItem.date;
                newNodeItem.comments.Add(newComment);
            }
            //iterate through the medias within the node(for photo and video)
            foreach (databaseMan.media mediaItem in node.medias)
            {
                media newMedia = new media();
                newMedia.path = mediaItem.path;
                newMedia.user = mediaItem.user;
                newMedia.date = mediaItem.date;

                //for type photo
                if (mediaItem.type == 2)
                {
                    newNodeItem.photos.Add(newMedia);
                }
                //for type video
                else if (mediaItem.type == 3)
                {
                    newNodeItem.videos.Add(newMedia);
                }

            }
            newNodeItem.indexNum = node.indexNum;
            newNodeItem.type = node.type;

            tempNodeList.Add(newNodeItem);
        }

        //reorder by date before adding back into the list
        foreach (nodeItem nodeItem in tempNodeList.OrderBy(si => si.date).ToList().Reverse<nodeItem>())
        {
            nodesManager.nodes.Add(nodeItem);
        }

    }

    ///<summary>
    ///load the violations from databaseMan into this class
    ///</summary>
    public void loadViolationsCmd()
    {
        violationsManager.violations.Clear();
        foreach (databaseMan.ViolationsClass violation in databaseMan.Instance.values.Location.Equipment[0].Violations)
        {
            violationsManager.violations.Add(violationParser(violation));
        }
    }

    ///<summary>
    ///load the locations data from databaseMan into this class
    ///</summary>
    void readLocationFields()
    {
        definitions.LocationFields.address1 = databaseMan.Instance.values.Location.Address.address1;
        definitions.LocationFields.address2 = databaseMan.Instance.values.Location.Address.address2;
        definitions.LocationFields.City = databaseMan.Instance.values.Location.Address.City;
        definitions.LocationFields.County = databaseMan.Instance.values.Location.Address.County;
        definitions.LocationFields.Country = databaseMan.Instance.values.Location.Address.Country;
        definitions.LocationFields.LocationID = databaseMan.Instance.values.Location.LocationID;
        definitions.LocationFields.LocationName = databaseMan.Instance.values.Location.LocationName;
        definitions.LocationFields.State = databaseMan.Instance.values.Location.Address.State;
        definitions.LocationFields.Zip = databaseMan.Instance.values.Location.Address.Zip;
    }

    ///<summary>
    ///load the equipment field definitions from databaseMan into this class
    ///</summary>
    void readEquipmentFields()
    {
        foreach (databaseMan.fieldItem fieldItem in databaseMan.Instance.definitions.EquipmentFields.threeNine)
        {
            fieldItem newFieldItem = new fieldItem();
            newFieldItem.DisplayName = fieldItem.DisplayName;
            newFieldItem.FieldType = fieldItem.FieldType;
            newFieldItem.Name = fieldItem.Name;
            newFieldItem.Required = fieldItem.Required;
            newFieldItem.Options = fieldItem.Options;
            definitions.EquipmentData.fields.Add(newFieldItem);
        }
    }

    ///<summary>
    ///load the inspection field definitions from databaseMan into this class
    ///</summary>
    void readInspectionFields()
    {
        foreach (databaseMan.fieldItem fieldItem in databaseMan.Instance.definitions.EquipmentInspectionFields.threeNine)
        {
            fieldItem newFieldItem = new fieldItem();
            newFieldItem.DisplayName = fieldItem.DisplayName;
            newFieldItem.FieldType = fieldItem.FieldType;
            newFieldItem.Name = fieldItem.Name;
            newFieldItem.Required = fieldItem.Required;
            newFieldItem.Options = fieldItem.Options;
            if (fieldItem.Name == "blnIssueCertOK")
            {
                definitions.ExtraFields.fields.Add(newFieldItem);
            }
            else
            {
                definitions.InspectionFields.fields.Add(newFieldItem);
            }
        }
    }

    ///<summary>
    ///load the non displayable field definitions from databaseMan into this class
    ///</summary>
    void readNonDisplayedFields()
    {
        foreach (databaseMan.fieldItem fieldItem in databaseMan.Instance.definitions.nonDisplayedFields.threeNine)
        {
            fieldItem newFieldItem = new fieldItem();
            newFieldItem.DisplayName = fieldItem.DisplayName;
            newFieldItem.FieldType = fieldItem.FieldType;
            newFieldItem.Name = fieldItem.Name;
            newFieldItem.Required = fieldItem.Required;
            newFieldItem.Options = fieldItem.Options;

            definitions.nonDisplayedFields.fields.Add(newFieldItem);

        }
    }

    ///<summary>
    ///load the equipment field values from databaseMan into this class
    ///</summary>
    public void loadEquipmentData()
    {
        values.equipmentData.Clear();
        foreach (databaseMan.ItemClass item in databaseMan.Instance.values.Location.Equipment[0].EquipmentData)
        {
            if(item.name == "intActivityTypeID" || item.name == "dtActivityDate")
            {
                valueItem newValueItem = new valueItem();
                newValueItem.name = item.name;
                newValueItem.value = item.value;
                newValueItem.nodeIndex = item.nodeIndex;

                values.equipmentData.Add(newValueItem);
            }
            else
            {
                valueItem newValueItem = new valueItem();
                newValueItem.name = item.name;
                newValueItem.value = item.value;
                newValueItem.nodeIndex = item.nodeIndex;

                values.equipmentData.Add(newValueItem);
            }

        }
    }

    ///<summary>
    ///load the historic values from databaseMan into this class, for later display in parentheses
    ///</summary>
    void loadHistoric()
    {
        foreach (databaseMan.ItemClass item in databaseMan.Instance.values.Location.Equipment[0].PreviousInspection[0].InspectionData)
        {
            valueItem newValueItem = new valueItem();
            newValueItem.name = item.name;
            newValueItem.value = item.value;
            newValueItem.nodeIndex = item.nodeIndex;
            if (item.name == "blnIssueCertOK")
            {
                values.extraData.Add(newValueItem);
            }
            values.historicData.Add(newValueItem);

        }
    }

    ///<summary>
    ///load the current values from databaseMan into this class, for delta comparisons
    ///</summary>
    public void loadCurrentData()
    {
        values.currentData.Clear();
        foreach (databaseMan.ItemClass item in databaseMan.Instance.values.Location.Equipment[0].CurrentInspection)
        {
            valueItem newValueItem = new valueItem();
            newValueItem.name = item.name;
            newValueItem.value = item.value;
            newValueItem.nodeIndex = item.nodeIndex;

            values.currentData.Add(newValueItem);
        }
    }

    ///<summary>
    ///string splitting violation categories
    ///</summary>
    public virtual string categoryParser(string numbers, int category)
    {
        string[] result = numbers.Split('.');
        return result[category];
    }

    ///<summary>
    ///converts violation item from databaseMan to JU_databaseMan
    ///</summary>
    public virtual ViolationsItem violationParser(databaseMan.ViolationsClass incomingItem)
    {
        ViolationsItem newViolation = new ViolationsItem();

        newViolation.category = int.Parse(categoryParser(incomingItem.category, 0));
        newViolation.subCategory = int.Parse(categoryParser(incomingItem.category, 1));
        newViolation.specific = int.Parse(categoryParser(incomingItem.category, 2));
        newViolation.severity = incomingItem.classifications;
        newViolation.violationDate = incomingItem.violationDate;
        newViolation.resolveDate = incomingItem.resolveDate;
        newViolation.status = incomingItem.status;
        newViolation.conditions = incomingItem.conditions;
        newViolation.requirements = incomingItem.requirements;
        newViolation.nodeIndex = incomingItem.nodeIndex;
        return newViolation;
    }

    ///<summary>
    ///converts violation category int code into its string dictionary pairs in violationsLib.cs, with exceptions for incomplete library
    ///</summary>
    public virtual List<string> categoryStringer(ViolationsItem violation)
    {

        List<string> categoryString = new List<string>();

        if (violationsLib.Instance.categoryLib.categoryList.ContainsKey(violation.category))
        {
            string tempString = violationsLib.Instance.categoryLib.categoryList[violation.category].name;
            categoryString.Add(tempString);
            if (violationsLib.Instance.categoryLib.categoryList[violation.category].subCategoryList.ContainsKey(violation.subCategory))
            {
                string tempString1 = violationsLib.Instance.categoryLib.categoryList[violation.category].subCategoryList[violation.subCategory].name;
                categoryString.Add(tempString1);
                if (violationsLib.Instance.categoryLib.categoryList[violation.category].subCategoryList[violation.subCategory].specificList.ContainsKey(violation.specific))
                {
                    string tempString2 = violationsLib.Instance.categoryLib.categoryList[violation.category].subCategoryList[violation.subCategory].specificList[violation.specific].name;
                    categoryString.Add(tempString2);
                }
                else
                {
                    print("key : " + violation.specific + " in specific doesn't exist");
                    categoryString.Add("Handhole or gasket or gasket seat installation is not satisfactory");
                }
            }
            else
            {
                print("key : " + violation.subCategory + " in subcategory doesn't exist");
                categoryString.Add("Water Leaks");
                categoryString.Add("Handhole or gasket or gasket seat installation is not satisfactory");
            }
        }
        else
        {
            print("key : " + violation.category + " in category doesn't exist");
            categoryString.Add("Boiler Components");
            categoryString.Add("Water Leaks");
            categoryString.Add("Handhole or gasket or gasket seat installation is not satisfactory");
        }

        return (categoryString);
    }
}
