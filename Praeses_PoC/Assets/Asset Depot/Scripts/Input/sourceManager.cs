using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;


public class sourceManager : Singleton<sourceManager>, ISourceStateHandler, IInputHandler
{
    [Tooltip("True is the finger is in downward airtap pinch")]
    public bool sourcePressed;
    [Tooltip("True is the finger is in view frustum and ready position")]
    public bool sourceDetected;


    // Use this for initialization
    void Start()
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //finger ready and in view frustum
    public void OnSourceDetected(SourceStateEventData eventData)
    {

        if (!sourceDetected)
        {
            sourceDetected = true;
        }

    }

    //Finger removed from view frustum or not ready
    public void OnSourceLost(SourceStateEventData eventData)
    {
        if (sourceDetected)
        {
            sourceDetected = false;
            sourcePressed = false;
        }
    }

    //finger released after an airtap
    public void OnInputUp(InputEventData eventData)
    {

        if (sourcePressed)
        {
            sourcePressed = false;
        }

    }

    //finger held in downward position of an airtap
    public void OnInputDown(InputEventData eventData)
    {
        if (!sourcePressed)
        {
            sourcePressed = true;
        }
    }


}