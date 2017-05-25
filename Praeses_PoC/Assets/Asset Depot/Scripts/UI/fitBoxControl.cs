using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class fitBoxControl : Singleton<fitBoxControl> {
    public GameObject fitBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toggleFitbox()
    {
        if (fitBox.activeSelf)
        {
            mainMenuController.Instance.openMainMenu();
            mainMenuController.Instance.contentHolder.transform.position = frontHolderInstance.Instance.setFrontHolder(1.5f).transform.position; 
        }
        fitBox.SetActive(!fitBox.activeSelf);
    }
}
