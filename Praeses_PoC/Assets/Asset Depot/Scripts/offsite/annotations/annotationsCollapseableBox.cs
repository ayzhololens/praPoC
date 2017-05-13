using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class annotationsCollapseableBox : MonoBehaviour {

 //   public GameObject onIcon;
 //   public GameObject offIcon;
 //   public GameObject scrollBox;
 //   public bool boxState;
 //   public float expandSize;
 //   public GameObject nextLiner;
 //   float initNextLinerY;
 //   public List<GameObject> collapsableChild;
 //   public collapsableManager bigBox;
         
	//// Use this for initialization
	//void Start () {
 //       if (nextLiner != null)
 //       {
 //           initNextLinerY = -45;
 //       }
 //       openCollapseable(0);
 //   }
	
	//// Update is called once per frame
	//void Update () {
		
	//}

 //   void toggleBox()
 //   {
 //       if (boxState)
 //       {
 //           closeCollapseable();
 //       }
 //       else
 //       {
 //           openCollapseable(0);
 //       }
 //   }

 //   public void closeCollapseable()
 //   {
 //       onIcon.SetActive(false);
 //       offIcon.SetActive(true);
 //       scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
 //                                                                       0);
 //       if (nextLiner != null)
 //       {
 //           nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
 //                                                   initNextLinerY, 0);
 //       }
 //       if (collapsableChild.Count > 0)
 //       {
 //           foreach (GameObject child in collapsableChild)
 //           {
 //               child.SetActive(false);
 //           }
 //       }
 //       boxState = false;
 //       bigBox.startCollapse -= expandSize;
 //       bigBox.readjustBox();
 //   }

 //   public void openCollapseable(float expandOffset)
 //   {
 //       onIcon.SetActive(true);
 //       offIcon.SetActive(false);
 //       scrollBox.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollBox.GetComponent<RectTransform>().rect.width,
 //           expandSize);
 //       boxState = true;
 //       if (nextLiner != null)
 //       {
 //           nextLiner.GetComponent<RectTransform>().localPosition = new Vector3(nextLiner.GetComponent<RectTransform>().localPosition.x,
 //                                                                   initNextLinerY - (expandSize/ 2.246608f), 
 //                                                                   0);
 //       }
 //       if (collapsableChild.Count > 0)
 //       {
 //           foreach (GameObject child in collapsableChild)
 //           {
 //               child.SetActive(true);
 //           }
 //       }

 //       bigBox.startCollapse += (expandSize + expandOffset);
 //       bigBox.readjustBox();
 //   }

 //   private void OnMouseDown()
 //   {
 //       toggleBox();
 //   }
}
