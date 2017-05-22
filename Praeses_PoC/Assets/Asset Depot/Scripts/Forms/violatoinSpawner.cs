using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class violatoinSpawner :  Singleton<violatoinSpawner>{

    public violationController activeViolationController;
    public GameObject violationPrefab;
    public GameObject preexistingViolationPrefab;
    public GameObject violationCategoryPrefab;
    public GameObject violationSubCategoryPrefab;
    public GameObject violationFieldPrefab;
    public GameObject violationPreview;
    public GameObject violationPreviewField;
    public GameObject successContentHolder;
    public int rowLengthBox;
    public float hOffsetBox;
    public float vOffsetBox;
    public float vOffsetField;
    public List<string> VioCat;
    public List<string> VioSubCat;
    public List<string> Vios;
    violationsLib vioLib;

    // Use this for initialization
    void Start () {
        vioLib = violationsLib.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnViolation(GameObject vioNode)
    {
        //spawn violation
        GameObject spawnedViolation = Instantiate(violationPrefab, transform.position, Quaternion.identity);
        
        //link violation and node
        violationController curVio = spawnedViolation.GetComponent<violationController>();
        vioNode.GetComponent<nodeController>().linkedField = spawnedViolation;
        vioNode.GetComponent<nodeController>().contentHolder = curVio.contentHolder;
        curVio.linkedNode = vioNode;

        //store violation during categorization
        activeViolationController = curVio;
        activeViolationController.goToTab(0);
        populateCategories();
    }

    public void spawnViolationFromJSON(GameObject vioNode)
    {
        //spawn violation
        GameObject spawnedViolation = Instantiate(preexistingViolationPrefab, transform.position, Quaternion.identity);

        //link violation and node
        violationController curVio = spawnedViolation.GetComponent<violationController>();
        curVio.fromJson = true;
        vioNode.GetComponent<nodeController>().linkedField = spawnedViolation;
        vioNode.GetComponent<nodeController>().contentHolder = curVio.contentHolder;
        curVio.linkedNode = vioNode;

        //store violation during categorization
        activeViolationController = curVio;

        populateCategoriesFromJSON();
    }



    public void populateCategories()
    {
        activeViolationController.goToTab(0);


        Vector3 startPos = activeViolationController.boxStartPos.localPosition;
        float vOff = 0;
        float hCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);
        int rLength = rowLengthBox;

        for (int i = 0; i < VioCat.Count; i++)
        {
            
            if (i == rLength)
            {
                vOff = vOff + vOffsetBox;
                hCount = 0;
                rLength += rowLengthBox;
            }


            GameObject spawnedViolation = Instantiate(violationCategoryPrefab, transform.position, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[0].transform);
            spawnedViolation.transform.localScale = violationCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;

            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = i;
            spawnedViolation.GetComponent<violationComponent>().displayText.text =
                i + ") " +
                VioCat[i];
            spawnedViolation.GetComponent<violationComponent>().value = VioCat[i];
            hCount += 1;

        }



    }

    void populateCategoriesFromJSON()
    {
        Vector3 startPos = activeViolationController.boxStartPos.localPosition;
        float vOff = 0;
        float hCount = 0;
        int i = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);
        int rLength = rowLengthBox;
        foreach (int cat in vioLib.violationsCategory.Keys)
        {

            if (i == rLength)
            {
                vOff = vOff + vOffsetBox;
                hCount = 0;
                rLength += rowLengthBox;
            }


            GameObject spawnedViolation = Instantiate(violationCategoryPrefab, transform.position, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[0].transform);
            spawnedViolation.transform.localScale = violationCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;

            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = cat;

            spawnedViolation.GetComponent<violationComponent>().displayText.text =
                cat + ") " +
                vioLib.violationsCategory[cat];

            spawnedViolation.GetComponent<violationComponent>().value = 
                vioLib.violationsCategory[cat];
            i += 1;
            hCount += 1;

        }

        //set the value
        if (activeViolationController.violationData.Count == 0)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[0]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].category);
        }
        populateSubCategoriesFromJSON();


    }

    public void populateSubCategories(int violationIndex)
    {
        
        Vector3 startPos = activeViolationController.boxStartPos.localPosition;
        float vOff = 0;
        float hCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);
        int rLength = rowLengthBox;

        for (int i = 0; i < VioSubCat.Count; i++)
        {
            if (i == rLength)
            {

                vOff = vOff + vOffsetBox;
                hCount = 0;
                rLength += rowLengthBox;
            }

            GameObject spawnedViolation = Instantiate(violationSubCategoryPrefab, spawnPos, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[1].transform);
            spawnedViolation.transform.localScale = violationSubCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = i;

            spawnedViolation.GetComponent<violationComponent>().displayText.text = 
                activeViolationController.violationIndices[0] + "." +
                i + " " +
                VioSubCat[i];
            spawnedViolation.GetComponent<violationComponent>().value =  VioSubCat[i];

            hCount += 1;

        }

    }

    void populateSubCategoriesFromJSON()
    {

        Vector3 startPos = activeViolationController.boxStartPos.localPosition;
        float vOff = 0;
        float hCount = 0;
        int i = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);
        int rLength = rowLengthBox;

        foreach (int cat in vioLib.violationsSubCategory4.Keys)
        {
            if (i == rLength)
            {

                vOff = vOff + vOffsetBox;
                hCount = 0;
                rLength += rowLengthBox;
            }

            GameObject spawnedViolation = Instantiate(violationSubCategoryPrefab, spawnPos, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[1].transform);
            spawnedViolation.transform.localScale = violationSubCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = cat;
            spawnedViolation.GetComponent<violationComponent>().displayText.text =
            
                activeViolationController.violationIndices[0] + "." +
            cat + " " +
            vioLib.violationsSubCategory4[cat];

            spawnedViolation.GetComponent<violationComponent>().value = vioLib.violationsSubCategory4[cat];
            hCount += 1;
            i += 1;

        }
        //set the value
        if (activeViolationController.violationData.Count == 1)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[1]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].subCategory);
        }
        populateViolationsFromJSON();
    }

    public void populateViolations(int violationIndex)
    {
        Vector3 startPos = activeViolationController.fieldStartPos.localPosition;
        float vCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);

        for (int i = 0; i<Vios.Count; i++)
        {

            GameObject spawnedViolation = Instantiate(violationFieldPrefab, transform.position, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[2].transform);
            spawnedViolation.transform.localScale = violationFieldPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x, startPos.y - vOffsetField * vCount, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.fieldStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = i;


            string violatioName = Vios[i];

            spawnedViolation.GetComponent<violationComponent>().value = violatioName;
            spawnedViolation.GetComponent<violationComponent>().displayText.text =
            activeViolationController.violationIndices[0] + "." +
            activeViolationController.violationIndices[1] + "." +
             i + " " + violatioName;
            vCount += 1;
        }

    }

    void populateViolationsFromJSON()
    {
        Vector3 startPos = activeViolationController.fieldStartPos.localPosition;
        float vCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);

        foreach (int cat in vioLib.violationsSpecific41.Keys)
        {

            GameObject spawnedViolation = Instantiate(violationFieldPrefab, transform.position, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[2].transform);
            spawnedViolation.transform.localScale = violationFieldPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x, startPos.y - vOffsetField * vCount, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.fieldStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = cat;


            spawnedViolation.GetComponent<violationComponent>().displayText.text =
            activeViolationController.violationIndices[0] + "." +
            activeViolationController.violationIndices[1] + "." +
             cat + " " + JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[2];

            //string violationName = vioLib.violationsSpecific41[cat];
            //string violationName = "NB# " +
            //    +activeViolationController.violationIndices[0] + "."
            //    + activeViolationController.violationIndices[1] + "."
            //    + vCount + " | "
            //    + activeViolationController.violationData[0] + " -"
            //    + activeViolationController.violationData[1] + " ";

            spawnedViolation.GetComponent<violationComponent>().value = JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[2];
            vCount += 1;
        }

        if (activeViolationController.violationData.Count == 2)
        {
            //activeViolationController.violationData.Add("NB# " +
            //    +activeViolationController.violationIndices[0] + "."
            //    + activeViolationController.violationIndices[1] + "."
            //    + JU_databaseMan.Instance.violationsManager.violations[0].specific + " | "
            //    + activeViolationController.violationData[0] + " -"
            //    + activeViolationController.violationData[1] + " ");

            activeViolationController.violationData.Add(JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[2]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].specific);
        }
        populateSeverityFromJSON();

    }

    void populateSeverityFromJSON()
    {
        if (activeViolationController.violationData.Count == 3)
        {
            activeViolationController.violationData.Add(vioLib.violationsSeverity[JU_databaseMan.Instance.violationsManager.violations[0].severity]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].severity);
        }
        populateDueDateFromJSON();
    }

    void populateDueDateFromJSON()
    {
        if (activeViolationController.violationData.Count == 4)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.violationsManager.violations[0].resolveDate);
            activeViolationController.violationIndices.Add(0);
        }


        populateConditionsFromJSON();
    }

    void populateConditionsFromJSON()
    {
        if (activeViolationController.violationData.Count == 5)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.violationsManager.violations[0].conditions);
            activeViolationController.violationIndices.Add(0);
        }


        populateRequirementsFromJSON();

    }

    void populateRequirementsFromJSON()
    {
        if (activeViolationController.violationData.Count == 6)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.violationsManager.violations[0].requirements);
            activeViolationController.violationIndices.Add(0);
        }


        activeViolationController.vioReview.loadReview();
        activeViolationController.vioReview.submitReview(true);
        activeViolationController.goToTab(8);
        //activeViolationController.vioReview.submitReview(true);
        //populatePreviewField();
    }
}
