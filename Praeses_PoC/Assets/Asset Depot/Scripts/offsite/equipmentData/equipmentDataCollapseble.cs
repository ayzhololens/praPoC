using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equipmentDataCollapseble : MonoBehaviour {

    public GameObject onIcon;
    public GameObject offIcon;
    public bool boxState;
    public float expandSize;
    public GameObject nextLiner;
    float initNextLinerY;
    public GameObject listGrp;
         
	// Use this for initialization
	void Start () {
        if (nextLiner != null)
        {
            initNextLinerY = -136;
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void toggleBox()
    {
        if (boxState)
        {
            closeCollapseable();
        }
        else
        {
            openCollapseable(0);
        }
    }

    public void closeCollapseable()
    {
        onIcon.SetActive(false);
        offIcon.SetActive(true);
        if (nextLiner != null)
        {
            nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
                                                    initNextLinerY, 0);
        }
        boxState = false;
        listGrp.SetActive(false);
    }

    public void openCollapseable(float expandOffset)
    {
        onIcon.SetActive(true);
        offIcon.SetActive(false);
        boxState = true;
        if (nextLiner != null)
        {
            nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
                                                                    initNextLinerY - (expandSize), 
                                                                    0);
        }
        listGrp.SetActive(true);
    }

    private void OnMouseDown()
    {
        toggleBox();
    }
}
