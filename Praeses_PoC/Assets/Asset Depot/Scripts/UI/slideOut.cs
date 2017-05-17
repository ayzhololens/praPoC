using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class slideOut : MonoBehaviour {

    public float slideSpeed;
    public float slideDist;
    bool isMovingF;
    bool isMovingB;
    Vector3 startPos;
    public float timer;
    bool keptOut;

    public subMenu subMenu;

	// Use this for initialization
	void Start () {
        startPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (isMovingF)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + slideSpeed, startPos.y, startPos.z);
            if (transform.localPosition.x <= startPos.x + slideDist)
            {
                transform.localPosition = new Vector3((startPos.x + slideDist), startPos.y, startPos.z);
                isMovingF = false;
            }
        }


        if (isMovingB)
        {

                transform.localPosition = new Vector3(transform.localPosition.x - slideSpeed, startPos.y, startPos.z);
                if (transform.localPosition.x >= startPos.x)
                {
                    transform.localPosition = startPos;
                    isMovingF = false;


                if (subMenu != null)
                {
                    subMenu.turnOffSubButtons();
                }

            }
            


        }

    }

    public void slideForward()
    {
        formController.Instance.gameObject.BroadcastMessage("slideBackward", SendMessageOptions.DontRequireReceiver);

        isMovingF = true;
        isMovingB = false;

        if (subMenu != null)
        {
            subMenu.turnOnSubButtons();
        }

    }

    public void slideBackward()
    {
    if (!keptOut)
    {
        isMovingB = true;
        isMovingF = false;
        
    }

    }

    public void slideBackTimer()
    {
        Invoke("slideBackward", timer);
        keptOut = false;
    }

    public void keepOut()
    {
        keptOut = true;
    }
}
