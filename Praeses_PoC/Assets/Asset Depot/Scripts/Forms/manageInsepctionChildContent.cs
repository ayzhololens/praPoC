using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manageInsepctionChildContent : MonoBehaviour {

    public GameObject downLoadIcon;
    public GameObject completeIcon;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void enableDownloadTimer()
    {
        for (int i = 1; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<manageInsepctionChildContent>().CompleteTimer();
        }
    }

    public void CompleteTimer()
    {
        completeIcon.SetActive(true);
        downLoadIcon.SetActive(false);
    }


}
