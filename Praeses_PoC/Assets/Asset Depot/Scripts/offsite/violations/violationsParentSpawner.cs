using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

public class violationsParentSpawner : Singleton<violationsParentSpawner> {

    public int expandSize;
    public RectTransform VioBox;
    public GameObject violationPrefab;
    public GameObject parentObj;
    //will need this
    public collapsableManager bigBox { get; set; }
    public GameObject childItem;

    List<GameObject> spawnedVioPrefabs = new List<GameObject>();

    //for adding comments
    public GameObject addNewCommentBox;
    public InputField field;
    public Button addNoteDone;

    // Use this for initialization
    void Start () {
        bigBox = collapsableManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void expandBox(int totalLines)
    {
        VioBox.sizeDelta = new Vector2(VioBox.rect.width, expandSize * totalLines);
        childItem.GetComponent<RectTransform>().localPosition = new Vector3(childItem.GetComponent<RectTransform>().localPosition.x,
                                                                        childItem.GetComponent<RectTransform>().localPosition.y - (expandSize),
                                                                        childItem.GetComponent<RectTransform>().localPosition.z);
        bigBox.startCollapse += expandSize;
        bigBox.readjustBox();
    }

    public void populateViolations()
    {
        int totalVio = 0;

        foreach(JU_databaseMan.ViolationsItem vio in JU_databaseMan.Instance.violationsManager.violations)
        {
            totalVio++;
            GameObject newItem;
            newItem = Instantiate(violationPrefab);
            newItem.transform.SetParent(parentObj.transform);
            newItem.GetComponent<RectTransform>().localPosition = new Vector3(0, (totalVio-1) * -175, 0);
            newItem.GetComponent<RectTransform>().localScale = Vector3.one;
            newItem.GetComponent<violationsCollapseableBox>().vioBox = VioBox.gameObject;

            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addNewCommentWindow = addNewCommentBox;
            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().field = field;
            newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addNoteDone = addNoteDone;

            foreach(JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
            {
                if (node.indexNum == vio.nodeIndex)
                {
                    foreach (JU_databaseMan.media media in node.videos)
                    {
                        newItem.GetComponent<violationsCollapseableBox>().addObject.GetComponent<addCommentButton>().addOneVideo();
                    }
                }
            }    

            newItem.GetComponent<violationsCollapseableBox>().childItems.Add(childItem);
            foreach(GameObject vioPre in spawnedVioPrefabs)
            {
                vioPre.GetComponent<violationsCollapseableBox>().childItems.Add(newItem);
            }
            expandBox(totalVio);

            newItem.GetComponent<violationsCollapseableBox>().vioInt = totalVio - 1;
            newItem.GetComponent<violationsCollapseableBox>().updateTitleContents();
            newItem.GetComponent<violationsCollapseableBox>().updateCollapseableContents();
            spawnedVioPrefabs.Add(newItem);
        }
        bigBox.startCollapse += 25;
        bigBox.readjustBox();
    }
}
