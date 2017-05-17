using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using HoloToolkit.Unity;

public class boilerTransformRecorder : MonoBehaviour {

    [Serializable]
    public class geometryInfo
    {
        public List<float> transform = new List<float>();
    }

    public geometryInfo boilerClass = new geometryInfo();
    public GameObject offsiteBoilerHolder;

    public List<GameObject> boilerMain = new List<GameObject>();
    public List<GameObject> boilerPopUp = new List<GameObject>();

    void grabBoilerTransform()
    {
        Transform boiler = boilerSpawner.Instance.boiler.transform;
        boilerClass.transform.Clear();
        boilerClass.transform.Add(boiler.localPosition.x);
        boilerClass.transform.Add(boiler.localPosition.y);
        boilerClass.transform.Add(boiler.localPosition.z);
        boilerClass.transform.Add(boiler.localRotation.x);
        boilerClass.transform.Add(boiler.localRotation.y);
        boilerClass.transform.Add(boiler.localRotation.z);
        boilerClass.transform.Add(boiler.localRotation.w);
        boilerClass.transform.Add(boiler.localScale.x);
        boilerClass.transform.Add(boiler.localScale.y);
        boilerClass.transform.Add(boiler.localScale.z);
        boilerClass.transform.Add(databaseMan.Instance.popUp);
    }

    public void exportData()
    {
        grabBoilerTransform();
        string json = JsonUtility.ToJson(boilerClass);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "boilerTransform.json"), json);
    }

    public void importData()
    {
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "boilerTransform.json"));
        boilerClass.transform.Clear();
        boilerClass = JsonUtility.FromJson<geometryInfo>(json);

        offsiteBoilerHolder.transform.localPosition = new Vector3(boilerClass.transform[0], boilerClass.transform[1], boilerClass.transform[2]);
        offsiteBoilerHolder.transform.localRotation = new Quaternion(boilerClass.transform[3], boilerClass.transform[4], boilerClass.transform[5], boilerClass.transform[6]);
        //offsiteBoilerHolder.transform.localScale = new Vector3(boilerClass.transform[7], boilerClass.transform[8], boilerClass.transform[9]);

        if (boilerClass.transform[10] == 0)
        {
            foreach(GameObject obj in boilerMain)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in boilerPopUp)
            {
                obj.SetActive(false);
            }
        }else if (boilerClass.transform[10] == 0)
        {
            foreach (GameObject obj in boilerPopUp)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in boilerMain)
            {
                obj.SetActive(false);
            }
        }
    }
}
