using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class vioPreviewComponent : MonoBehaviour {

    public Color[] resolutionColors;
    public GameObject resolutionBox;
    public string[] resolutionOptions;
    public Text resolutionText;
    public Text user;
    public Text date;
    public GameObject[] commentText;
    public Transform commentPos;
    public float commentOffset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setResolution(int index)
    {
        resolutionBox.GetComponent<Renderer>().material.color = resolutionColors[index];
        resolutionText.text = resolutionOptions[index];
    }
}

