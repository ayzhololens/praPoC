using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class timerManager : Singleton<timerManager> {

    [Tooltip("Circular loader located under the cursor hierarchy")]
    public GameObject cursorTimer;
    public bool isCounting { get; set; }
    [Tooltip("Countdown in seconds, recommended 1.3seconds")]
    public float counter;
    float startCounter;
    public bool menuOpen { get; set; }

    // Use this for initialization
    void Start () {
        startCounter = counter;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    public void radialCountDown()
    {
        if (!isCounting)
        {
            startCounter = counter;
            cursorTimer.SetActive(true);
            cursorTimer.GetComponent<tumblerRadialCounter>().radialCounterOn();
            isCounting = true;
        }

        if (isCounting)
        {
            counter -= Time.deltaTime;

            if (counter < 0)
            {
                radialManagement.Instance.SendMessage("turnOnRadialMenu", SendMessageOptions.DontRequireReceiver);
                menuOpen = true;
                counter = startCounter;
            }
        }
        


    }

    public void CountInterrupt()
    {

        cursorTimer.GetComponent<tumblerRadialCounter>().radialCounterInterrupt();
        counter = startCounter;
        isCounting = false;

    }

    public void tumbleCountDown()
    {
        if (!isCounting)
        {
            startCounter = counter;
            cursorTimer.SetActive(true);
            cursorTimer.GetComponent<tumblerRadialCounter>().radialCounterOn();
            isCounting = true;
        }

        if (isCounting)
        {
            counter -= Time.deltaTime;

            if (counter < 0)
            {
                onModelDragHybrid.Instance.colliderOn();
            }
        }
    }
}
