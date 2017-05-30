using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class selectEvent : MonoBehaviour,  IInputClickHandler, IFocusable 
{
    public UnityEvent Event;


    bool canClick;
    bool focused;
    public bool gazeExit;
    public bool clickMotion;
    public int soundIndex;
    public float moveSpeed;
    public float moveDist;


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


        if (clickMotion)
        {
            GetComponent<clickMotion>().click();
        }
        else
        {
            if (this.enabled == false) return;
            if (Event != null)
            {

                Event.Invoke();

                //Debug.Log("select");
                audioManager.Instance.setAndPlayAudio(soundIndex);
            }

            if (GetComponent<gazeLeaveEvent>() != null && gazeExit)
            {
                GetComponent<gazeLeaveEvent>().OnFocusExit();
            }
        }


    }


    public void finishClick()
    {
        if (this.enabled == false) return;
        if (Event != null)
        {
            Debug.Log("select");
            audioManager.Instance.setAndPlayAudio(soundIndex);

            Event.Invoke();

        }

        if (GetComponent<gazeLeaveEvent>() != null && gazeExit)
        {
            GetComponent<gazeLeaveEvent>().OnFocusExit();
        }
    }


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