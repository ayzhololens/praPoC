using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class fieldSpawner : Singleton<fieldSpawner>
    {
        public GameObject MasterForm { get; set; }
        public Transform FieldInspectionParent { get; set; }
        public Transform EquipmentDataParent { get; set; }
        public Transform LocationDataParent { get; set; }
        public Transform ExtDataParent { get; set; }
        public GameObject stringFieldPrefab;
        public GameObject buttonFieldPrefab;
        public Transform fieldStartPos { get; set; }
        Vector3 fieldInitPos;
        public float offsetDist;
        public Dictionary<string, GameObject> ActiveFields = new Dictionary<string, GameObject>();
        public List<GameObject> IFCollection;
        public List<GameObject> EDCollection;
        public List<GameObject> LDCollection;
        public List<GameObject> ExDCollection;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void reloadForm()
        {
            IFCollection.Clear();
            EDCollection.Clear();
            LDCollection.Clear();
            ExDCollection.Clear();
            ActiveFields.Clear();

            formController mForm = formController.Instance;

            MasterForm = mForm.gameObject;
            FieldInspectionParent = mForm.fieldTabs[2].transform;
            LocationDataParent = mForm.fieldTabs[0].transform;
            EquipmentDataParent = mForm.fieldTabs[1].transform;
            ExtDataParent = mForm.submitInspection.contents[2];

            fieldStartPos = mForm.fieldStartPos;
            fieldInitPos = fieldStartPos.localPosition;
            


        }

        public void populateFields(bool reload)
        {

            MasterForm.GetComponent<formController>().preloadFormData();

            populateIF();
            populateED();
            populateLD();
            populateExD();

            if (!reload)
            {
                //distribute location data)
                ActiveFields["address1"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.address1;
                ActiveFields["address2"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.address2;
                ActiveFields["City"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.City;
                ActiveFields["County"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.County;
                ActiveFields["Country"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.Country;
                ActiveFields["LocationID"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.LocationID.ToString();
                ActiveFields["LocationName"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.LocationName;
                ActiveFields["State"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.State;
                ActiveFields["Zip"].GetComponent<formFieldController>().Value.text = JU_databaseMan.Instance.definitions.LocationFields.Zip;


                //distribute present values in field
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.equipmentData)
                {
                    if (ActiveFields.ContainsKey(valueItem.name))
                    {
                        //print(valueItem.value+" applied to "+ ActiveFields[valueItem.name].GetComponent<formFieldController>().DisplayName.text);
                        ActiveFields[valueItem.name].GetComponent<formFieldController>().Value.text = valueItem.value;
                        ActiveFields[valueItem.name].GetComponent<formFieldController>().nodeIndex = valueItem.nodeIndex;
                    }
                    else
                    {
                        //print(valueItem.name + " is not available");
                    }

                }

                // distribute historic values in field parentheses
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.historicData)
                {
                    print(valueItem.name);
                    ActiveFields[valueItem.name].GetComponent<formFieldController>().nodeIndex = valueItem.nodeIndex;
                    print("arfter");
                    if (ActiveFields.ContainsKey(valueItem.name))
                    {
                        //correct naming
                        foreach (JU_databaseMan.fieldItem field in JU_databaseMan.Instance.definitions.EquipmentData.fields)
                        {
                            if (field.Options.ContainsKey(valueItem.value))
                            {
                                ActiveFields[valueItem.name].GetComponent<formFieldController>().previousValue.text = ("(" + valueItem.value + ")");
                            }
                        }

                        foreach (JU_databaseMan.fieldItem field in JU_databaseMan.Instance.definitions.InspectionFields.fields)
                        {
                            if (field.Options.ContainsKey(valueItem.value))
                            {
                                ActiveFields[valueItem.name].GetComponent<formFieldController>().previousValue.text = ("(" + field.Options[valueItem.value] + ")");
                            }
                        }

                        foreach (JU_databaseMan.fieldItem field in JU_databaseMan.Instance.definitions.ExtraFields.fields)
                        {
                            print(field.Name);

                            if (field.Options.ContainsKey(valueItem.value))
                            {
                                ActiveFields[valueItem.name].GetComponent<formFieldController>().previousValue.text = ("(" + field.Options[valueItem.value] + ")");
                            }
                        }
                    }
                }
            }

        }

        void populateExD() {

            int fieldCount = JU_databaseMan.Instance.definitions.ExtraFields.fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {

                GameObject spawnedField;
                if (JU_databaseMan.Instance.definitions.ExtraFields.fields[i].FieldType == 16)
                {



                    //spawnedField = Instantiate(buttonFieldPrefab, transform.position, Quaternion.identity);
                    //spawnedField.GetComponent<formFieldController>().populateButtons(3);
                    //spawnedField.GetComponent<formFieldController>().curButtons[0].GetComponent<formButtonController>().buttonText.text = "Yes";
                    //spawnedField.GetComponent<formFieldController>().curButtons[0].GetComponent<formButtonController>().buttonIndex = 1;
                    //spawnedField.GetComponent<formFieldController>().curButtons[1].GetComponent<formButtonController>().buttonText.text = "No";
                    //spawnedField.GetComponent<formFieldController>().curButtons[1].GetComponent<formButtonController>().buttonIndex = 0;
                    //spawnedField.GetComponent<formFieldController>().curButtons[2].GetComponent<formButtonController>().buttonText.text = "Other";
                    //spawnedField.GetComponent<formFieldController>().curButtons[2].GetComponent<formButtonController>().buttonIndex = 2;

                    

                    spawnedField = Instantiate(buttonFieldPrefab, transform.position, Quaternion.identity);

                    spawnedField.GetComponent<formFieldController>().populateButtons(JU_databaseMan.Instance.definitions.ExtraFields.fields[i].Options.Count);

                    List<string> keyCollection = new List<string>();
                    foreach (string keyIn in JU_databaseMan.Instance.definitions.ExtraFields.fields[i].Options.Keys)
                    {
                        keyCollection.Add(keyIn);

                    }
                    List<int> keyInts = new List<int>();
                    foreach (string keyStr in keyCollection)
                    {
                        int temp = int.Parse(keyStr);
                        keyInts.Add(temp);
                    }
                    for (int m = 0; m < keyCollection.Count; m++)
                    {


                        spawnedField.GetComponent<formFieldController>().curButtons[m].GetComponent<formButtonController>().buttonText.text = (JU_databaseMan.Instance.definitions.ExtraFields.fields[i].Options[keyCollection[m]]);
                        spawnedField.GetComponent<formFieldController>().curButtons[m].GetComponent<formButtonController>().buttonIndex = keyInts[m];
                    }


                    spawnedField.GetComponent<formFieldController>().showUpdate = true;
                    spawnedField.GetComponent<formFieldController>().ignoreDeltaCheck = true;
                    spawnedField.transform.SetParent(ExtDataParent);
                    submitInspection submitIns = MasterForm.GetComponent<formController>().submitInspection;
                    spawnedField.transform.localPosition = submitIns.issuePos.localPosition;
                    spawnedField.transform.localScale = submitIns.issuePos.localScale;
                    spawnedField.transform.localEulerAngles = new Vector3(0, 0, 0);




                    for (int o = 3; o < submitIns.contents.Length; o++)
                    {
                        submitIns.contents[o].position = new Vector3(submitIns.contents[o].position.x, submitIns.contents[o].position.y - offsetDist, submitIns.contents[o].position.z);
                    }

                    spawnedField.GetComponent<formFieldController>().DisplayName.text = JU_databaseMan.Instance.definitions.ExtraFields.fields[i].DisplayName;
                    spawnedField.GetComponent<formFieldController>().trueName = JU_databaseMan.Instance.definitions.ExtraFields.fields[i].Name;
                    ActiveFields.Add(spawnedField.GetComponent<formFieldController>().trueName, spawnedField);


                    ExDCollection.Add(spawnedField);

                }

            }

        }

        void populateIF()
        {
            fieldStartPos.localPosition = fieldInitPos;
            int fieldCount = JU_databaseMan.Instance.definitions.InspectionFields.fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {
                MasterForm.GetComponent<formController>().totalFields = i+1;
                MasterForm.GetComponent<formController>().updateFieldStatus(0);
                GameObject spawnedField;
                if (JU_databaseMan.Instance.definitions.InspectionFields.fields[i].FieldType == 1)
                {

                    spawnedField = Instantiate(buttonFieldPrefab, transform.position, Quaternion.identity);

                    spawnedField.GetComponent<formFieldController>().populateButtons(JU_databaseMan.Instance.definitions.InspectionFields.fields[i].Options.Count);

                    List<string> keyCollection = new List<string>();
                    foreach(string keyIn in JU_databaseMan.Instance.definitions.InspectionFields.fields[i].Options.Keys)
                    {
                        keyCollection.Add(keyIn);
                       
                    }
                    List<int> keyInts = new List<int>();
                    foreach(string keyStr in keyCollection)
                    {
                        int temp = int.Parse(keyStr);
                        keyInts.Add(temp);
                    }
                    for (int m = 0; m<keyCollection.Count; m++)
                    {
                        spawnedField.GetComponent<formFieldController>().curButtons[m].GetComponent<formButtonController>().buttonText.text = (JU_databaseMan.Instance.definitions.InspectionFields.fields[i].Options[keyCollection[m]]);
                        spawnedField.GetComponent<formFieldController>().curButtons[m].GetComponent<formButtonController>().buttonIndex = keyInts[m];
                    }
                    
                }
                else if(JU_databaseMan.Instance.definitions.InspectionFields.fields[i].FieldType == 16)
                {
                    spawnedField = Instantiate(buttonFieldPrefab, transform.position, Quaternion.identity);
                    spawnedField.GetComponent<formFieldController>().populateButtons(2);
                    spawnedField.GetComponent<formFieldController>().curButtons[0].GetComponent<formButtonController>().buttonText.text = "yes";
                    spawnedField.GetComponent<formFieldController>().curButtons[0].GetComponent<formButtonController>().buttonIndex = 1;
                    spawnedField.GetComponent<formFieldController>().curButtons[1].GetComponent<formButtonController>().buttonText.text = "no";
                    spawnedField.GetComponent<formFieldController>().curButtons[1].GetComponent<formButtonController>().buttonIndex = 0;
                }
                else if (JU_databaseMan.Instance.definitions.InspectionFields.fields[i].FieldType == 14)
                {

                    spawnedField = Instantiate(stringFieldPrefab, transform.position, Quaternion.identity);
                    spawnedField.GetComponent<formFieldController>().Value.text = System.DateTime.Now.ToString("MM/dd/yyyy");

                }
                else
                {
                    spawnedField = Instantiate(stringFieldPrefab, transform.position, Quaternion.identity);
                }
                spawnedField.GetComponent<formFieldController>().showUpdate = true;
                spawnedField.transform.SetParent(FieldInspectionParent);
                spawnedField.transform.localPosition = fieldStartPos.localPosition;
                spawnedField.transform.localScale = stringFieldPrefab.transform.localScale;
                spawnedField.transform.localRotation = stringFieldPrefab.transform.localRotation;
                fieldStartPos.position = new Vector3(fieldStartPos.position.x, fieldStartPos.position.y - offsetDist, fieldStartPos.position.z);
                
                spawnedField.GetComponent<formFieldController>().DisplayName.text = JU_databaseMan.Instance.definitions.InspectionFields.fields[i].DisplayName;
                spawnedField.GetComponent<formFieldController>().trueName = JU_databaseMan.Instance.definitions.InspectionFields.fields[i].Name;
                ActiveFields.Add(spawnedField.GetComponent<formFieldController>().trueName, spawnedField);



                //print(spawnedField.GetComponent<formFieldController>().showUpdate);
                IFCollection.Add(spawnedField);
            }

            GameObject Submit = MasterForm.GetComponent<formController>().Sumbit;
            Submit.transform.SetParent(FieldInspectionParent);
            Submit.transform.localPosition = fieldStartPos.localPosition;
            MasterForm.GetComponent<formController>().goToTab(2);
        }

        void populateED()
        {
            fieldStartPos.localPosition = fieldInitPos;
            int fieldCount = JU_databaseMan.Instance.definitions.EquipmentData.fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {

                GameObject spawnedField = Instantiate(stringFieldPrefab, transform.position, Quaternion.identity);
                spawnedField.transform.SetParent(EquipmentDataParent);
                spawnedField.transform.localPosition = fieldStartPos.localPosition;
                spawnedField.transform.localScale = stringFieldPrefab.transform.localScale;
                spawnedField.transform.localRotation = stringFieldPrefab.transform.localRotation;
                fieldStartPos.position = new Vector3(fieldStartPos.position.x, fieldStartPos.position.y - offsetDist, fieldStartPos.position.z);
                spawnedField.GetComponent<formFieldController>().DisplayName.text = JU_databaseMan.Instance.definitions.EquipmentData.fields[i].DisplayName;
                spawnedField.GetComponent<formFieldController>().trueName = JU_databaseMan.Instance.definitions.EquipmentData.fields[i].Name;
                ActiveFields.Add(spawnedField.GetComponent<formFieldController>().trueName, spawnedField);
                EDCollection.Add(spawnedField);

                spawnedField.SetActive(false);
            }
        }

        void populateLD()
        {
            fieldStartPos.localPosition = fieldInitPos;

            string[] keys = new string[] { "address1", "address2", "City", "County", "Country", "LocationID", "LocationName", "State", "Zip" };

            foreach (string key in keys)
            {
                GameObject spawnedField = Instantiate(stringFieldPrefab, transform.position, Quaternion.identity);
                spawnedField.transform.SetParent(LocationDataParent);
                spawnedField.transform.localPosition = fieldStartPos.localPosition;
                spawnedField.transform.localScale = stringFieldPrefab.transform.localScale;
                spawnedField.transform.localRotation = stringFieldPrefab.transform.localRotation;
                fieldStartPos.position = new Vector3(fieldStartPos.position.x, fieldStartPos.position.y - offsetDist, fieldStartPos.position.z);
                spawnedField.GetComponent<formFieldController>().DisplayName.text = key;
                spawnedField.GetComponent<formFieldController>().trueName = key;
                ActiveFields.Add(spawnedField.GetComponent<formFieldController>().trueName, spawnedField);
                LDCollection.Add(spawnedField);
                spawnedField.SetActive(false);
            }
        }


        public void testSpawn()
        {

            fieldStartPos.localPosition = fieldInitPos;
            for (int i = 0; i < 10; i++)
            {
                GameObject spawnedField = Instantiate(stringFieldPrefab, transform.position, Quaternion.identity);
                spawnedField.transform.SetParent(FieldInspectionParent);
                spawnedField.transform.localPosition = fieldStartPos.localPosition;
                spawnedField.transform.localScale = stringFieldPrefab.transform.localScale;
               spawnedField.transform.localRotation = stringFieldPrefab.transform.localRotation;
                fieldStartPos.position = new Vector3(fieldStartPos.position.x, fieldStartPos.position.y - offsetDist, fieldStartPos.position.z);
                IFCollection.Add(spawnedField);
            }
        }
    }
}