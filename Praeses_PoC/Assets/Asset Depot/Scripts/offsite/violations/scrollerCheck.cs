using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollerCheck : MonoBehaviour {

    public bool isScrolling;
    public bool isMouseDown;
    public bool clickBlocker;
    Scrollbar bar;

    public void scroll()
    {
        isScrolling = true;
    }

    private void Start()
    {
        bar = gameObject.GetComponent<ScrollRect>().horizontalScrollbar;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            isMouseDown = true;
        }

        if (isScrolling && isMouseDown)
        {
            clickBlocker = true;
        }else
        {
            clickBlocker = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            isScrolling = false;
        }
    }

}
