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
        }
        fitBox.SetActive(!fitBox.activeSelf);
    }
}
