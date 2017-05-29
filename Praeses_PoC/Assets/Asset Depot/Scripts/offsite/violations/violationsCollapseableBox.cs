using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class violationsCollapseableBox : MonoBehaviour {

    public int vioInt;
    public Text dateAddedValue;
    public Text dueValue;
    public Text title;
    public Text severity;
    public Image severityBox;
    public Text inspector;
    public Text category;
    public Text subcategory;
    public Text conditions;
    public Text requirements;

    public GameObject collapseIcon;
    public GameObject expandIcon;
    public GameObject collapseContent;
    public GameObject lineItem;
    public collapsableManager bigBox;
    public GameObject vioBox;
    public bool isOpen;
    public List<GameObject> childItems = new List<GameObject>();
    public int expandSize;

    public GameObject addObject;

    public GameObject fixd;
    public GameObject nu;
    public GameObject haz;
    public GameObject sta;

    public void updateTitleContents()
    {
        dateAddedValue.text = JU_databaseMan.Instance.violationsManager.violations[vioInt].violationDate;
        dueValue.text = JU_databaseMan.Instance.violationsManager.violations[vioInt].resolveDate;
        string code = JU_databaseMan.Instance.violationsManager.violations[vioInt].category.ToString() + "."
                    + JU_databaseMan.Instance.violationsManager.violations[vioInt].subCategory.ToString() + "."
                    + JU_databaseMan.Instance.violationsManager.violations[vioInt].specific.ToString();
        string strTitle = JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[vioInt])[2];
        title.text = code + " - " + strTitle;
        fixd.SetActive(false);
        nu.SetActive(false);
        haz.SetActive(false);
        if (JU_databaseMan.Instance.violationsManager.violations[vioInt].status == 1)//fixed
        {
            //then color is green
            fixd.SetActive(true);
        }
        else if (JU_databaseMan.Instance.violationsManager.violations[vioInt].status == 0)//new
        {
            //then color is grey
            nu.SetActive(true);
        }
        else
        {
            if(JU_databaseMan.Instance.violationsManager.violations[vioInt].severity == 1)
            {
                //then color is orange
                haz.SetActive(true);
            }
            else
            {
                sta.SetActive(true);
            }

        }
    }

    public void updateCollapseableContents()
    {
        JU_databaseMan.ViolationsItem vio = JU_databaseMan.Instance.violationsManager.violations[vioInt];
        if (vio.nodeIndex != 0)
        {
            foreach(JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
            {
                if (node.indexNum == vio.nodeIndex)
                {
                    inspector.text = node.user;
                }
            }
        }
        else
        {
            inspector.text = metaManager.Instance.user;
        }
        category.text = vio.category.ToString() + "." + JU_databaseMan.Instance.categoryStringer(vio)[0];
        subcategory.text = vio.subCategory.ToString() + "." + JU_databaseMan.Instance.categoryStringer(vio)[1];
        conditions.text = vio.conditions;
        requirements.text = vio.requirements;
    }

    private void Start()
    {
        bigBox = collapsableManager.Instance;
        childItems.Add(lineItem);
        addObject.GetComponent<Collider>().enabled = false;
        addObject.GetComponent<Renderer>().enabled = false;
    }

    public void toggleVioBox()
    {
        if (isOpen)
        {
            closeBox();
            isOpen = false;
        }
        else
        {
            openBox();
            isOpen = true;
        }
    }

    private void openBox()
    {
        collapseIcon.SetActive(true);
        expandIcon.SetActive(false);
        collapseContent.SetActive(true);
        vioBox.GetComponent<RectTransform>().sizeDelta = new Vector2(vioBox.GetComponent<RectTransform>().rect.width,
                                vioBox.GetComponent<RectTransform>().rect.height + expandSize);
        bigBox.startCollapse += expandSize;
        bigBox.readjustBox();
        foreach(GameObject childItem in childItems)
        {
            childItem.GetComponent<RectTransform>().localPosition = new Vector3(childItem.GetComponent<RectTransform>().localPosition.x,
                                                        childItem.GetComponent<RectTransform>().localPosition.y - (expandSize),
                                                        childItem.GetComponent<RectTransform>().localPosition.z);
        };

        addObject.GetComponent<Collider>().enabled = true;
        addObject.GetComponent<Renderer>().enabled = true;
    }

    private void closeBox()
    {
        collapseIcon.SetActive(false);
        expandIcon.SetActive(true);
        collapseContent.SetActive(false);
        vioBox.GetComponent<RectTransform>().sizeDelta = new Vector2(vioBox.GetComponent<RectTransform>().rect.width,
                        vioBox.GetComponent<RectTransform>().rect.height - expandSize);
        //bigBox.GetComponent<RectTransform>().sizeDelta = new Vector2(vioBox.GetComponent<RectTransform>().rect.width,
        //                vioBox.GetComponent<RectTransform>().rect.height - expandSize);
        foreach (GameObject childItem in childItems)
        {
            childItem.GetComponent<RectTransform>().localPosition = new Vector3(childItem.GetComponent<RectTransform>().localPosition.x,
                                                                childItem.GetComponent<RectTransform>().localPosition.y + (expandSize),
                                                                childItem.GetComponent<RectTransform>().localPosition.z);
        };
        addObject.GetComponent<Collider>().enabled = false;
        addObject.GetComponent<Renderer>().enabled = false;
        bigBox.startCollapse -= expandSize;
        bigBox.readjustBox();
    }

    private void OnMouseDown()
    {
        toggleVioBox();
    }
}
