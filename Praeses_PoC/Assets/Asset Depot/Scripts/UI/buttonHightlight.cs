using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHightlight : MonoBehaviour {

    public Material mainMat;
    Material mainMatTemp;
    public Material highlightMat;
    public Material altMat;
    public bool isPanel;
    Color panelMain;
    public Color panelHighlight;
    public GameObject objectOverride;

    // Use this for initialization
    void Start () {
        if (!isPanel)
        {
            if(objectOverride != null)
            {
                objectOverride.GetComponent<Renderer>().material = mainMat;
            }
            else
            {
                GetComponent<Renderer>().material = mainMat;

            }
        }
        if (isPanel)
        {
            panelMain = GetComponent<Image>().color;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateMat()
    {
        mainMatTemp = mainMat;
        mainMat = altMat;
        //altMat = mainMatTemp;
        if (objectOverride != null)
        {
            objectOverride.GetComponent<Renderer>().material = mainMat;
        }
        else
        {
            GetComponent<Renderer>().material = mainMat;

        }
    }

    public void highlight()
    {
        if (!isPanel)
        {
            if (objectOverride != null)
            {
                objectOverride.GetComponent<Renderer>().material = highlightMat;
            }
            else
            {
                GetComponent<Renderer>().material = highlightMat;

            }
        }
        if (isPanel)
        {
            GetComponent<Image>().color = panelHighlight;
        }
        
    }

    public void unHighlight()
    {
        if (objectOverride != null)
        {
            objectOverride.GetComponent<Renderer>().material = mainMat;
        }
        else if(!isPanel)
        {
            GetComponent<Renderer>().material = mainMat;
        }
        if (isPanel)
        {
            GetComponent<Image>().color = panelMain;
        }

    }
}
