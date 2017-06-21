using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHightlight : MonoBehaviour {

    [Tooltip("Starting or Default material")]
    public Material mainMat;

    //Mat that objects will be set to
    Material curMat;


    [Tooltip("Highlight Material")]
    public Material highlightMat;
    [Tooltip("Alternate default material")]
    public Material altMat;

    //for when you want to highlight a UI panel thats driven by color rather than materials

    [Tooltip("for when you want to highlight a UI panel thats driven by color rather than materials")]
    public bool isPanel;
    Color panelMain;
    public Color panelHighlight;

    [Tooltip("Highlight something other than this gameobject")]
    public GameObject objectOverride;
    

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


    //set the object mat to highlight
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

    //unhighlight the object
    public void unHighlight()
    {
        if (objectOverride != null)
        {
            objectOverride.GetComponent<Renderer>().material = curMat;
        }
        else if (!isPanel)
        {
            GetComponent<Renderer>().material = curMat;
        }
        if (isPanel)
        {
            GetComponent<Image>().color = panelMain;
        }

    }

    //change the default material to an alternate on. Useful for giving color feedback when something has been selected
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

    //change the default material back to original
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
