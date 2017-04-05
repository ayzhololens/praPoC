﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class fieldSpawner : Singleton<fieldSpawner>
    {
        public GameObject MasterForm;
        public Transform FieldInspectionParent;
        public Transform EquipmentDataParent;
        public Transform LocationDataParent;
        public GameObject stringFieldPrefab;
        public GameObject buttonFieldPrefab;
        public Transform fieldStartPos;
        Vector3 fieldInitPos;
        public float offsetDist;
        public Dictionary<string, GameObject> ActiveFields = new Dictionary<string, GameObject>();
        public List<GameObject> IFCollection;
        public List<GameObject> EDCollection;
        public List<GameObject> LDCollection;

        // Use this for initialization
        void Start()
        {
            fieldInitPos = fieldStartPos.localPosition;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void populateFields()
        {
            populateIF();
            populateED();
            populateLD();

            //distribute location data
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
            foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.presentData)
            {
                if (ActiveFields.ContainsKey(valueItem.name))
                {
                    //print(valueItem.value+" applied to "+ ActiveFields[valueItem.name].GetComponent<formFieldController>().DisplayName.text);
                    ActiveFields[valueItem.name].GetComponent<formFieldController>().Value.text = valueItem.value;
                }
                else
                {
                    //print(valueItem.name + " is not available");
                }

            }

            //distribute historic values in field parentheses
            foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.historicData)
            {
                if (ActiveFields.ContainsKey(valueItem.name))
                {
                    ActiveFields[valueItem.name].GetComponent<formFieldController>().previousValue.text = ("(" + valueItem.value + ")");
                }
            }
        }

        void populateIF()
        {
            fieldStartPos.localPosition = fieldInitPos;
            int fieldCount = JU_databaseMan.Instance.definitions.InspectionFields.fields.Count;
            for (int i = 0; i < fieldCount; i++)
            {
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
                    spawnedField.GetComponent<formFieldController>().curButtons[1].GetComponent<formButtonController>().buttonText.text = "no";
                }
                else
                {
                    spawnedField = Instantiate(stringFieldPrefab, transform.position, Quaternion.identity);
                }
                spawnedField.transform.SetParent(FieldInspectionParent);
                spawnedField.transform.localPosition = fieldStartPos.localPosition;
                spawnedField.transform.localScale = stringFieldPrefab.transform.localScale;
                spawnedField.transform.localRotation = stringFieldPrefab.transform.localRotation;
                fieldStartPos.position = new Vector3(fieldStartPos.position.x, fieldStartPos.position.y - offsetDist, fieldStartPos.position.z);
                spawnedField.GetComponent<formFieldController>().DisplayName.text = JU_databaseMan.Instance.definitions.InspectionFields.fields[i].DisplayName;
                spawnedField.GetComponent<formFieldController>().trueName = JU_databaseMan.Instance.definitions.InspectionFields.fields[i].Name;
                ActiveFields.Add(spawnedField.GetComponent<formFieldController>().trueName, spawnedField);
                IFCollection.Add(spawnedField);
            }
            //FieldInspectionParent.gameObject.SetActive(false);
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
            //FieldInspectionParent.gameObject.SetActive(false);
        }
    }
}