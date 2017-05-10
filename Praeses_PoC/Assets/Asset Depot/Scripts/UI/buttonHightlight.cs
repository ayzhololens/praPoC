using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHightlight : MonoBehaviour {

    public Material mainMat;
    Material curMat;
    public Material highlightMat;
    public Material altMat;
    public bool isPanel;
    Color panelMain;
    public Color panelHighlight;
    public GameObject objectOverride;

    // Use this for initialization
    void Start () {

        curMat = mainMat;

        if (!isPanel)
        {
            if(objectOverride != null)
            {
                objectOverride.GetComponent<Renderer>().material = curMat;

                
            }
            else
            {
                GetComponent<Renderer>().material = curMat;
                

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
        curMat = altMat;
        if (objectOverride != null)
        {
            objectOverride.GetComponent<Renderer>().material = curMat;
        }
        else
        {
            GetComponent<Renderer>().material = curMat;

        }
    }

    public void revertMat()
    {

        curMat = mainMat;
        if (objectOverride != null)
        {
            objectOverride.GetComponent<Renderer>().material = curMat;
        }
        else
        {
            GetComponent<Renderer>().material = curMat;

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
            objectOverride.GetComponent<Renderer>().material = curMat;
        }
        else if(!isPanel)
        {
            GetComponent<Renderer>().material = curMat;
        }
        if (isPanel)
        {
            GetComponent<Image>().color = panelMain;
        }

    }
}
