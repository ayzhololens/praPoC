using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

public class scrollOverrider : Singleton<scrollOverrider> {

    public bool isScrolling;
    public List<GameObject> otherScrollers;

    Ray ray;
    RaycastHit hit;

    private void Update()
    {
        ray = ActorSingleton.Actor.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Button"){}
            else
            {
                wheelChecking();
            }
        }
        else
        {
            wheelChecking();
        }

        
    }

    void turnOff()
    {
        isScrolling = false;
    }

    void turnOn()
    {
        CancelInvoke();
        isScrolling = true;
        Invoke("turnOff", .7f);
    }

    void wheelChecking()
    {
        if (Input.GetAxis("Mouse ScrollWheel")*100 > 0)
        {
            turnOn();
        }

        if (isScrolling)
        {
            foreach (GameObject scroller in otherScrollers)
            {
                scroller.GetComponent<ScrollRect>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject scroller in otherScrollers)
            {
                scroller.GetComponent<ScrollRect>().enabled = true;
            }
        }
    }
}
