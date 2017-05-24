using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class fieldCollapseableBox : Singleton<fieldCollapseableBox> {

    public GameObject fieldDeltaPrefab;
    public GameObject changedFieldParent;
    Dictionary<string, GameObject> deltaCollection = new Dictionary<string, GameObject>();
    public List<JU_databaseMan.compareItem> compareItemList;

    public GameObject scrollBox;
    public bool boxState;
    public float expandSize;
    public GameObject nextLiner;
    float initNextLinerY;
    public collapsableManager bigBox;

    private void Start()
    {
        expandSize =0;
    }

    public void populateFieldDeltas()
    {
        deltaCollection.Clear();
        float yOffset = -100;

        List<JU_databaseMan.valueItem> tempItemValuesList = new List<JU_databaseMan.valueItem>();
        List<JU_databaseMan.fieldItem> tempItemDefinitionsList = new List<JU_databaseMan.fieldItem>();

        foreach (JU_databaseMan.valueItem hist in JU_databaseMan.Instance.values.historicData)
        {
            tempItemValuesList.Add(hist);
        }

        foreach (JU_databaseMan.fieldItem extra in JU_databaseMan.Instance.definitions.ExtraFields.fields)
        {
            tempItemDefinitionsList.Add(extra);
        }

        foreach (JU_databaseMan.fieldItem hist in JU_databaseMan.Instance.definitions.InspectionFields.fields)
        {
            tempItemDefinitionsList.Add(hist);
        }

        //compare deltas
        foreach (JU_databaseMan.valueItem historyItem in tempItemValuesList)
        {
            foreach (JU_databaseMan.valueItem currentItem in JU_databaseMan.Instance.values.currentData)
            {
                if (historyItem.name == currentItem.name)
                {
                    if (historyItem.value != currentItem.value)
                    {
                        //print(currentItem.name + " deltas: " + historyItem.value + " and " + currentItem.value);
                        //definitions
                        JU_databaseMan.compareItem newFieldItem = new JU_databaseMan.compareItem();
                        newFieldItem.name = currentItem.name;
                        foreach (JU_databaseMan.fieldItem item in tempItemDefinitionsList)
                        {
                            if (item.Name == currentItem.name)
                            {
                                newFieldItem.displayName = item.DisplayName;
                            }
                        }
                        newFieldItem.value = currentItem.value;
                        newFieldItem.oldValue = historyItem.value;
                        addOneFieldDelta(changedFieldParent, yOffset, newFieldItem);
                        compareItemList.Add(newFieldItem);
                        yOffset += -60;
                    }
                }
            }
        }

        scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
                                            scrollBox.GetComponent<RectTransform>().rect.height +90);

        bigBox.startCollapse += 25;
        bigBox.readjustBox();
        //values
        if (compareItemList.Count > 0)
        {
            foreach (JU_databaseMan.compareItem compareItem in compareItemList)
            {
                insertComparativeValues(compareItem);
            }
        }

    }

    void addOneFieldDelta(GameObject parentObj, float yOffset, JU_databaseMan.compareItem compareItem)
    {
        GameObject newItem;
        float initExpandSize = expandSize;
        newItem = Instantiate(fieldDeltaPrefab);
        newItem.transform.SetParent(parentObj.transform);
        newItem.GetComponent<RectTransform>().localPosition = new Vector3(866.5f, yOffset, 0);
        newItem.GetComponent<RectTransform>().localScale = Vector3.one;
        newItem.GetComponent<offsiteFieldItemValueHolder>().displayName.text = compareItem.displayName;
        newItem.GetComponent<offsiteFieldItemValueHolder>().value.text = compareItem.value;
        newItem.GetComponent<offsiteFieldItemValueHolder>().oldValue.text = compareItem.oldValue;
        deltaCollection.Add(compareItem.name, newItem);
        expandSize = 60;
        scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
                                                    scrollBox.GetComponent<RectTransform>().rect.height + expandSize);
        bigBox.startCollapse += (expandSize);
        bigBox.readjustBox();
        nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
                                                                    nextLiner.GetComponent<RectTransform>().localPosition.y - (expandSize*.688f),
                                                                    0);
    }

    void insertComparativeValues(JU_databaseMan.compareItem compareItem)
    {

        if (deltaCollection.ContainsKey(compareItem.name))
        {
            deltaCollection[compareItem.name].GetComponent<offsiteFieldItemValueHolder>().value.text = compareItem.value;
            deltaCollection[compareItem.name].GetComponent<offsiteFieldItemValueHolder>().displayName.text = compareItem.displayName;
            deltaCollection[compareItem.name].GetComponent<offsiteFieldItemValueHolder>().oldValue.text = compareItem.oldValue;
        }
        else
        {
            //print(valueItem.name + " does not exist");
        }
    }

}
