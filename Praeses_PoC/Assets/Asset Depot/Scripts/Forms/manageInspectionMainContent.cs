using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manageInspectionMainContent : MonoBehaviour {
    
    public GameObject collapseIcon;
    public float offsetDist;
    public int direction;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void repositionFields()
    {

        direction = direction * -1;
        collapseIcon.transform.localScale = new Vector3(collapseIcon.transform.localScale.x, collapseIcon.transform.localScale.y * -1, collapseIcon.transform.localScale.z);

  
        for (int i=1; i<transform.parent.childCount; i++)
        {

            Vector3 newPos = new Vector3(transform.parent.GetChild(i).localPosition.x, transform.parent.GetChild(i).localPosition.y + (offsetDist*direction), transform.parent.GetChild(i).localPosition.z);
            transform.parent.GetChild(i).localPosition = newPos;
        }
        for(int i =1; i<transform.childCount; i++)
        {

            transform.GetChild(i).gameObject.SetActive(!transform.GetChild(i).gameObject.activeSelf);
        }
        
    }
}
