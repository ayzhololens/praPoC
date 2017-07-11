using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class vioControl :  Singleton<vioControl>{

    public violationController activeViolationController { get; set; }
    [Tooltip("Prefab for a new violatoin")]
    public GameObject violationPrefab;
    [Tooltip("Prefab for a violatoin from JSON")]
    public GameObject preexistingViolationPrefab;
    [Tooltip("Content Holder for the violation sucess pane")]
    public GameObject successContentHolder;

    [Header ("Violation Component Prefabs")]
    [Tooltip("Prefab for Categories section")]
    public GameObject violationCategoryPrefab;
    [Tooltip("Prefab for SubCategories section")]
    public GameObject violationSubCategoryPrefab;
    [Tooltip("Prefab for Specific Violations section")]
    public GameObject violationFieldPrefab;

    
    [Header ("Layout")]
    [Tooltip("Length or box rows")]
    public int rowLengthBox;
    [Tooltip("Horizontal offset of boxes")]
    public float hOffsetBox;
    [Tooltip("Vertical offset of boxes")]
    public float vOffsetBox;
    [Tooltip("Vertical offset of fields")]
    public float vOffsetField;

    [Header ("Violation Values")]
    [Tooltip ("List available categories here")]
    public List<string> VioCat;
    [Tooltip("List available subcategories here")]
    public List<string> VioSubCat;
    [Tooltip("List available specific violations here")]
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

            //set transform values
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[0].transform);
            spawnedViolation.transform.localScale = violationCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;


            //populate values from VioCat List
            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = i+1;
            spawnedViolation.GetComponent<violationComponent>().displayText.text =
                (i + 1) + ") " +
                VioCat[i];
            spawnedViolation.GetComponent<violationComponent>().value = VioCat[i];
            hCount += 1;

        }



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

            //transform values
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[1].transform);
            spawnedViolation.transform.localScale = violationSubCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;


            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = i+1;

            //set values
            spawnedViolation.GetComponent<violationComponent>().displayText.text = 
                activeViolationController.violationIndices[0] + "." +
                 (i + 1) + " " +
                VioSubCat[i];
            spawnedViolation.GetComponent<violationComponent>().value =  VioSubCat[i];

            hCount += 1;

        }

    }

    public void populateViolations(int violationIndex)
    {
        Vector3 startPos = activeViolationController.fieldStartPos.localPosition;
        float vCount = 0;
        Vector3 spawnPos = new Vector3(startPos.x, startPos.y, startPos.z);

        for (int i = 0; i<Vios.Count; i++)
        {

            GameObject spawnedViolation = Instantiate(violationFieldPrefab, transform.position, Quaternion.identity);

            //transform values
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[2].transform);
            spawnedViolation.transform.localScale = violationFieldPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x, startPos.y - vOffsetField * vCount, startPos.z);
            spawnedViolation.transform.localRotation = activeViolationController.fieldStartPos.localRotation;
            spawnedViolation.transform.localPosition = spawnPos;


            spawnedViolation.GetComponent<violationComponent>().linkedViolation = activeViolationController;
            spawnedViolation.GetComponent<violationComponent>().Index = i;


            string violatioName = Vios[i];

            //set values
            spawnedViolation.GetComponent<violationComponent>().value = violatioName;
            spawnedViolation.GetComponent<violationComponent>().displayText.text =
            activeViolationController.violationIndices[0] + "." +
            activeViolationController.violationIndices[1] + "." +
              (i + 1) + " " + violatioName;
            vCount += 1;
        }

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

    /// <summary>
    /// Similar to the nonJSON functions.  This one will populate from VioLib and automatically set the index and value on the violationController
    /// </summary>
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

            //set transform values
            spawnedViolation.transform.SetParent(activeViolationController.violationTabs[0].transform);
            spawnedViolation.transform.localScale = violationCategoryPrefab.transform.localScale;
            spawnPos = new Vector3(startPos.x + hOffsetBox * hCount, startPos.y - vOff, startPos.z);
            spawnedViolation.transform.localPosition = spawnPos;
            spawnedViolation.transform.localRotation = activeViolationController.boxStartPos.localRotation;


            //populate values from Vio Lib
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

        //set the value of the correct category on the violation contoller
        if (activeViolationController.violationData.Count == 0)
        {
            activeViolationController.violationData.Add(JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[0]);
            activeViolationController.violationIndices.Add(JU_databaseMan.Instance.violationsManager.violations[0].category);
        }

        populateSubCategoriesFromJSON();


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
            

            spawnedViolation.GetComponent<violationComponent>().value = JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[0])[2];
            vCount += 1;
        }

        if (activeViolationController.violationData.Count == 2)
        {
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
    }
}
