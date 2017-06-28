using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHightlight : MonoBehaviour {

    [Tooltip("Starting or default material")]
    public Material mainMat;

    //Mat that objects will be set to
    Material curMat;

    [Tooltip("Highlight Material")]
    public Material highlightMat;
    [Tooltip("Alternate default material")]
    public Material altMat;

    [Tooltip("If you want to highlight a UnityUi 'panel' element")]
    public bool isPanel;
    Color panelMain;
    public Color panelHighlight;

    [Tooltip("Highlight something other than this object")]
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
	

    //Highlight Object
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


    //Unhighlight the object
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

    //Switches the default material back to the original
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


    //Changes default material back to the original
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
}
