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
        populateCategories();
    }

    public void spawnViolationFromJSON(GameObject vioNode)
    {
        //spawn violation
        GameObject spawnedViolation = Instantiate(preexistingViolationPrefab, transform.position, Quaternion.identity);

        //link violation and node
        violationController curVio = spawnedViolation.GetComponent<violationController>();
        vioNode.GetComponent<nodeController>().linkedField = spawnedViolation;
        vioNode.GetComponent<nodeController>().contentHolder = curVio.contentHolder;
        curVio.linkedNode = vioNode;

        //store violation during categorization
        activeViolationController = curVio;

        populateCategoriesFromJSON();
    }



    public void populateCategories()
    {

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
            spawnedViolation.GetComponent<violationComponent>().optionTitle.text =
                VioCat[i];
            hCount += 1;

        }



    }

    void populateCategoriesFromJSON()
    {
        Vector3 startPos = activeViolationController.boxStartPos.localPosition;
        float vOff = 0;
        float hCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);
        int rLength = rowLengthBox;

        foreach(int cat in vioLib.violationsCategory.Keys)
        {
            if (hCount == rLength)
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
            spawnedViolation.GetComponent<violationComponent>().optionTitle.text = 
                vioLib.violationsCategory[cat];
            hCount += 1;

        }

        //set the value
        if (activeViolationController.violationData.Count == 0)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[0]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].category);
        }
        populateSubCategoriesFromJSON();

        Text linkedText = activeViolationController.violationTabButtons[0].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        linkedText.text = JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[0];
        linkedText.color = Color.white;


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
            spawnedViolation.GetComponent<violationComponent>().optionTitle.text =  VioSubCat[i];
            hCount += 1;

        }

    }

    void populateSubCategoriesFromJSON()
    {

        Vector3 startPos = activeViolationController.boxStartPos.localPosition;
        float vOff = 0;
        float hCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);
        int rLength = rowLengthBox;

        foreach (int cat in vioLib.violationsSubCategory4.Keys)
        {
            if (hCount == rLength)
            {

                vOff = vOff + vOffsetBox;
                hCount = 0;
                rLength *= 2;
            }

            GameObject spawnedViolation = Instantiate(violationSubCategoryPrefab, spawnPos, Quaternion.identity);
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[1].transform);
            spawnedViolation.transform.localScale = violationSubCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = cat;
            spawnedViolation.GetComponent<violationComponent>().optionTitle.text = vioLib.violationsSubCategory4[cat];
            hCount += 1;

        }
        //set the value
        if (activeViolationController.violationData.Count == 1)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[1]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].subCategory);
        }
        populateViolationsFromJSON();

        Text linkedText = activeViolationController.violationTabButtons[1].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        linkedText.text = JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[1];
        linkedText.color = Color.white;

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

            spawnedViolation.GetComponent<violationComponent>().optionTitle.text = violatioName;
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

            //string violationName = vioLib.violationsSpecific41[cat];
            string violationName = "NB# " +
                +activeViolationController.violationIndices[0] + "."
                + activeViolationController.violationIndices[1] + "."
                + vCount + " | "
                + activeViolationController.violationData[0] + " -"
                + activeViolationController.violationData[1] + " ";

            spawnedViolation.GetComponent<violationComponent>().optionTitle.text = violationName;
            vCount += 1;
        }

        if (activeViolationController.violationData.Count == 2)
        {
            activeViolationController.violationData.Add("NB# " +
                +activeViolationController.violationIndices[0] + "."
                + activeViolationController.violationIndices[1] + "."
                + JU_databaseMan.Instance.violationsManager.violations[0].specific + " | "
                + activeViolationController.violationData[0] + " -"
                + activeViolationController.violationData[1] + " ");


            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].specific);
        }
        populateSeverityFromJSON();
        Text linkedText = activeViolationController.violationTabButtons[2].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        linkedText.text = "NB# " +
                +activeViolationController.violationIndices[0] + "."
                + activeViolationController.violationIndices[1] + "."
                + JU_databaseMan.Instance.violationsManager.violations[0].specific + " | "
                + activeViolationController.violationData[0] + " -"
                + activeViolationController.violationData[1] + " "; 
        linkedText.color = Color.white;
    }

    void populateSeverityFromJSON()
    {
        if (activeViolationController.violationData.Count == 3)
        {
            activeViolationController.violationData.Add(vioLib.violationsSeverity[JU_databaseMan.Instance.violationsManager.violations[0].severity]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].severity);
        }
        populateDueDateFromJSON();
        Text linkedText = activeViolationController.violationTabButtons[3].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        linkedText.text = vioLib.violationsSeverity[JU_databaseMan.Instance.violationsManager.violations[0].severity];
        linkedText.color = Color.white;
    }

    void populateDueDateFromJSON()
    {
        if (activeViolationController.violationData.Count == 4)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.violationsManager.violations[0].resolveDate);
            activeViolationController.violationIndices.Add(0);
        }

        Text linkedText = activeViolationController.violationTabButtons[4].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        linkedText.text = JU_databaseMan.Instance.violationsManager.violations[0].resolveDate;
        linkedText.color = Color.white;

        populateConditionsFromJSON();
    }

    void populateConditionsFromJSON()
    {
        if (activeViolationController.violationData.Count == 5)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.violationsManager.violations[0].conditions);
            activeViolationController.violationIndices.Add(0);
        }

        Text linkedText = activeViolationController.violationTabButtons[5].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        linkedText.text = JU_databaseMan.Instance.violationsManager.violations[0].conditions;
        linkedText.color = Color.white;

        populateRequirementsFromJSON();

    }

    void populateRequirementsFromJSON()
    {
        if (activeViolationController.violationData.Count == 6)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.violationsManager.violations[0].requirements);
            activeViolationController.violationIndices.Add(0);
        }

        Text linkedText = activeViolationController.violationTabButtons[6].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        linkedText.text = JU_databaseMan.Instance.violationsManager.violations[0].requirements;
        linkedText.color = Color.white;
        activeViolationController.vioReview.loadReview();
        activeViolationController.vioReview.submitReview(true);
        populatePreviewField();
    }


    public void populatePreviewField()
    {

        if(activeViolationController.linkedPreview == null)
        {
            violationPreview.GetComponent<viewViolationController>().repositionFields();
            GameObject spawnedPreview = Instantiate(violationPreviewField, violationPreview.transform.position, Quaternion.identity);
            spawnedPreview.transform.SetParent(violationPreview.GetComponent<viewViolationController>().fieldParent);
            spawnedPreview.transform.position = violationPreview.GetComponent<viewViolationController>().activePos.position;
            spawnedPreview.transform.localScale = violationPreviewField.transform.localScale;
            spawnedPreview.transform.localRotation = violationPreviewField.transform.localRotation;
            spawnedPreview.GetComponent<viewViolationContent>().linkedViolation = activeViolationController.gameObject;
            spawnedPreview.GetComponent<viewViolationContent>().viewViolationHolder = violationPreview;
            activeViolationController.linkedPreview = spawnedPreview;
            violationPreview.GetComponent<viewViolationController>().vioFields.Add(spawnedPreview);
        }

        viewViolationContent spawnedContent = activeViolationController.linkedPreview.GetComponent<viewViolationContent>();
        spawnedContent.ViolationName.text = activeViolationController.violationData[2];
        spawnedContent.DueDate.text = "Due: " + activeViolationController.violationData[4];
        spawnedContent.DueDate.text = spawnedContent.DueDate.text.Substring(0, spawnedContent.DueDate.text.Length-11);
        spawnedContent.Severity.text = "Severity: " + activeViolationController.violationData[3];
        spawnedContent.metaDate.text = "Added: " + activeViolationController.linkedNode.GetComponent<nodeMediaHolder>().Date;
        spawnedContent.metaDate.text = spawnedContent.metaDate.text.Substring(0, spawnedContent.metaDate.text.Length - 11);

    }
}
