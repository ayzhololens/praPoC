using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class selectEvent : MonoBehaviour,  IInputClickHandler, IFocusable 
{
    public UnityEvent Event;


    bool canClick;
    bool focused;

    [Header("Gaze")]
    [Tooltip("Check to trigger the gazeLeave Event when selected")]
    public bool gazeExit;

    [Header("Audio")]
    [Tooltip("SFX to play from audioManager. 10=null, 0=select, 1=success, 2=verify, 4=close")]
    public int soundIndex;

    [Header("Click Components")]
    [Tooltip("Add a click movement on select")]
    public bool clickMotion;
    [Tooltip("Override how fast the click motion will be.  0=default value")]
    public float moveSpeed;
    [Tooltip("Override how far the click motion travel.  0=default value")]
    public float moveDist;


    //add click motion
    void Start()
    {

        canClick = true;
        if (clickMotion)
        {
            gameObject.AddComponent<clickMotion>();
            if (moveSpeed == 0)
            {
                moveSpeed = .002f;
                moveDist = .01f;
            }

            GetComponent<clickMotion>().moveSpeed = moveSpeed;
            GetComponent<clickMotion>().moveDist = moveDist;


        }
    }

    public void OnSelect()
    {

        //trigger the attached clickMotion
        if (clickMotion)
        {
            GetComponent<clickMotion>().click();
        }
        //execute attached functions
        else
        {
            if (this.enabled == false) return;
            if (Event != null)
            {

                Event.Invoke();

                //trigger audioManager
                audioManager.Instance.setAndPlayAudio(soundIndex);
            }

            //trigger gazeLeave
            if (GetComponent<gazeLeaveEvent>() != null && gazeExit)
            {
                GetComponent<gazeLeaveEvent>().OnFocusExit();
            }
        }


    }

    //Triggered at the end of a clickMotion.  Essentially OnSelect()
    public void finishClick()
    {
        if (this.enabled == false) return;
        if (Event != null)
        {

            audioManager.Instance.setAndPlayAudio(soundIndex);

            Event.Invoke();

        }

        if (GetComponent<gazeLeaveEvent>() != null && gazeExit)
        {
            GetComponent<gazeLeaveEvent>().OnFocusExit();
        }
    }

    //If focused and the HL detects an airtap, call OnSelect
    //Short cooldown afterwards
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (GazeManager.Instance.HitObject == this.gameObject)
        {
            if (canClick)
            {
                OnSelect();
                canClick = false;
                Invoke("ClickReseter", .2f);
            }
        }
        
    }

    public void OnFocusEnter()
    {
        focused = true;
    }

    public void OnFocusExit()
    {
        focused = false;
    }

    void ClickReseter()
    {
        canClick = true;
    }

}