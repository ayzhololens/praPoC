using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class slideOut : MonoBehaviour {

    public float slideSpeed;
    public float slideDist;
    bool isMovingF;
    bool isMovingB;
    Vector3 startPos;
    public bool isOut;


    public GameObject[] subButtons;
    public GameObject hideButton;

    public int timeOut;
    int initTimeOut;

	// Use this for initialization
	void Start () {
        initTimeOut = timeOut;
        timeOut = 0;
        startPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void FixedUpdate () {


        if (isMovingF)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + slideSpeed, startPos.y, startPos.z);
            if (transform.localPosition.x <= startPos.x + slideDist)
            {
                transform.localPosition = new Vector3((startPos.x + slideDist), startPos.y, startPos.z);
                isMovingF = false;
                isOut = true;
                timeOut = initTimeOut;

            }
        }


        if (isMovingB)
        {

                transform.localPosition = new Vector3(transform.localPosition.x - slideSpeed, startPos.y, startPos.z);
                if (transform.localPosition.x >= startPos.x)
                {
                    transform.localPosition = startPos;
                    isMovingF = false;
                    isOut = false;

            }
            


        }

        if(isOut && !subButtons[0].activeSelf)
        {
            foreach(GameObject button in subButtons)
            {
                button.SetActive(true);
            }
            hideButton.SetActive(false);
        }

        if (!isOut && subButtons[0].activeSelf)
        {
            foreach (GameObject button in subButtons)
            {
                button.SetActive(false);
            }
            hideButton.SetActive(true);
        }

        if (isOut)
        {
            foreach (GameObject button in subButtons)
            {
                if (GazeManager.Instance.HitObject == button)
                {
                    timeOut = initTimeOut;
                }
            }


        }

        if (timeOut > 0)
        {
            timeOut -= 1;
        }
        else
        {
            if (isOut)
            {
                slideBackward();
            }
        }


    }

    public void slideForward()
    {
        isMovingF = true;
        isMovingB = false;
        

    }

    public void slideBackward()
    {

        isMovingB = true;
        isMovingF = false;

    }


}
