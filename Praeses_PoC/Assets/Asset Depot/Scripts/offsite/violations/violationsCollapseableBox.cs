using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class violationsCollapseableBox : MonoBehaviour {

    public GameObject onIcon;
    public GameObject offIcon;
    public int vioInt;
    public Text dateAddedValue;
    public Text dueValue;
    public Text title;
    public Text severity;
    public Image severityBox;

    public void updateTitleContents()
    {
        dateAddedValue.text = JU_databaseMan.Instance.violationsManager.violations[vioInt].violationDate;
        dueValue.text = JU_databaseMan.Instance.violationsManager.violations[vioInt].resolveDate;
        string code = JU_databaseMan.Instance.violationsManager.violations[vioInt].category.ToString() + "." 
                    + JU_databaseMan.Instance.violationsManager.violations[vioInt].subCategory.ToString() + "."
                    + JU_databaseMan.Instance.violationsManager.violations[vioInt].specific.ToString();
        string strTitle = JU_databaseMan.Instance.categoryStringer(JU_databaseMan.Instance.violationsManager.violations[vioInt])[2];
        title.text = code + " - " + strTitle;
        severity.text = violationsLib.Instance.violationsSeverity[JU_databaseMan.Instance.violationsManager.violations[vioInt].severity];
        if(JU_databaseMan.Instance.violationsManager.violations[vioInt].severity == 3)//fixed
        {
            //then color is green
            severityBox.color = new Color(.059f, .545f, .122f);
        }
        else
        {
            //then color is orange
            severityBox.color = new Color(.917f, .443f, .122f);
        }
    }

    //public GameObject scrollBox;
    //public bool boxState;
    //public float expandSize;
    //public GameObject nextLiner;
    //float initNextLinerY;
    //public List<GameObject> collapsableChild;

    //void toggleBox()
    //{
    //    if (boxState)
    //    {
    //        closeCollapseable();
    //    }
    //    else
    //    {
    //        openCollapseable(0);
    //    }
    //}

    //public void closeCollapseable()
    //{
    //    onIcon.SetActive(false);
    //    offIcon.SetActive(true);
    //    scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
    //                                                                    0);
    //    if (nextLiner != null)
    //    {
    //        nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
    //                                                initNextLinerY, 0);
    //    }
    //    if (collapsableChild.Count > 0)
    //    {
    //        foreach (GameObject child in collapsableChild)
    //        {
    //            child.SetActive(false);
    //        }
    //    }
    //    boxState = false;
    //}

    //public void openCollapseable(float expandOffset)
    //{
    //    onIcon.SetActive(true);
    //    offIcon.SetActive(false);
    //    scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
    //        expandSize);
    //    boxState = true;
    //    if (nextLiner != null)
    //    {
    //        nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
    //                                                                initNextLinerY - (expandSize/ 2.246608f), 
    //                                                                0);
    //    }
    //    if (collapsableChild.Count > 0)
    //    {
    //        foreach (GameObject child in collapsableChild)
    //        {
    //            child.SetActive(true);
    //        }
    //    }
    //}
}
