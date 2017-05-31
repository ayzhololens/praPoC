using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCursorScript : MonoBehaviour {

    //icons will snap to hand manipulated cursor as they enter the appropriate collider boxes

    [Header("Icons List")]
    public GameObject rIcon;
    Vector3 rIconOriPos;
    public GameObject lIcon;
    Vector3 lIconOriPos;
    public GameObject uIcon;
    Vector3 uIconOriPos;
    public GameObject dIcon;
    Vector3 dIconOriPos;

    [Header("Parent Object")]
    public Transform oriParent;

    [Header("Current Icon Parameters")]
    public int iconIndex;
    public Color myColor;
    public Color slo;
    public Color fas;

	// Use this for initialization
	void Start () {
        //record the starting points of where the icons go when set to neutral
        rIconOriPos = rIcon.transform.localPosition;
        lIconOriPos = lIcon.transform.localPosition;
        uIconOriPos = uIcon.transform.localPosition;
        dIconOriPos = dIcon.transform.localPosition;
    }

	void Update () {
        parentToCursor();
    }

    void resetPos()
    {
        rIcon.transform.SetParent(oriParent);
        rIcon.transform.localPosition = rIconOriPos;
        lIcon.transform.SetParent(oriParent);
        lIcon.transform.localPosition = lIconOriPos;
        uIcon.transform.SetParent(oriParent);
        uIcon.transform.localPosition = uIconOriPos;
        dIcon.transform.SetParent(oriParent);
        dIcon.transform.localPosition = dIconOriPos;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        rIcon.GetComponent<MeshRenderer>().material.color = Color.white;
        dIcon.GetComponent<MeshRenderer>().material.color = Color.white;
        lIcon.GetComponent<MeshRenderer>().material.color = Color.white;
        uIcon.GetComponent<MeshRenderer>().material.color = Color.white;

    }

    void parentToCursor()
    {
        if (iconIndex == 1) {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            uIcon.transform.SetParent(transform);
        uIcon.transform.localPosition = new Vector3(0, 0, 0);
            uIcon.GetComponent<MeshRenderer>().material.color = slo;
            rIcon.GetComponent<MeshRenderer>().material.color = myColor;
            dIcon.GetComponent<MeshRenderer>().material.color = myColor;
            lIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 2)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            rIcon.transform.SetParent(transform);
            rIcon.transform.localPosition = new Vector3(0, 0, 0);
            rIcon.GetComponent<MeshRenderer>().material.color = slo;
            uIcon.GetComponent<MeshRenderer>().material.color = myColor;
            dIcon.GetComponent<MeshRenderer>().material.color = myColor;
            lIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 3)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            dIcon.transform.SetParent(transform);
            dIcon.transform.localPosition = new Vector3(0, 0, 0);
            dIcon.GetComponent<MeshRenderer>().material.color = slo;
            rIcon.GetComponent<MeshRenderer>().material.color = myColor;
            uIcon.GetComponent<MeshRenderer>().material.color = myColor;
            lIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 4)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            lIcon.transform.SetParent(transform);
            lIcon.transform.localPosition = new Vector3(0, 0, 0);
            lIcon.GetComponent<MeshRenderer>().material.color = slo;
            rIcon.GetComponent<MeshRenderer>().material.color = myColor;
            dIcon.GetComponent<MeshRenderer>().material.color = myColor;
            uIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 5)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            uIcon.transform.SetParent(transform);
            uIcon.transform.localPosition = new Vector3(0, 0, 0);
            uIcon.GetComponent<MeshRenderer>().material.color = fas;
            rIcon.GetComponent<MeshRenderer>().material.color = myColor;
            dIcon.GetComponent<MeshRenderer>().material.color = myColor;
            lIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 6)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            rIcon.transform.SetParent(transform);
            rIcon.transform.localPosition = new Vector3(0, 0, 0);
            rIcon.GetComponent<MeshRenderer>().material.color = fas;
            uIcon.GetComponent<MeshRenderer>().material.color = myColor;
            dIcon.GetComponent<MeshRenderer>().material.color = myColor;
            lIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 7)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            dIcon.transform.SetParent(transform);
            dIcon.transform.localPosition = new Vector3(0, 0, 0);
            dIcon.GetComponent<MeshRenderer>().material.color = fas;
            rIcon.GetComponent<MeshRenderer>().material.color = myColor;
            uIcon.GetComponent<MeshRenderer>().material.color = myColor;
            lIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else if (iconIndex == 8)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            lIcon.transform.SetParent(transform);
            lIcon.transform.localPosition = new Vector3(0, 0, 0);
            lIcon.GetComponent<MeshRenderer>().material.color = fas;
            rIcon.GetComponent<MeshRenderer>().material.color = myColor;
            dIcon.GetComponent<MeshRenderer>().material.color = myColor;
            uIcon.GetComponent<MeshRenderer>().material.color = myColor;
        }
        else
        {
            resetPos();
        }
    }
}
