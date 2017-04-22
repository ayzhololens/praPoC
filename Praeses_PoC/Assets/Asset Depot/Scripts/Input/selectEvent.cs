using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class selectEvent : MonoBehaviour,  IInputClickHandler, IFocusable 
{
    public UnityEvent Event;


    bool canClick;
    bool focused;
    public bool gazeExit;
    void Start()
    {

        canClick = true;
    }

    public void OnSelect()
    {

        if (this.enabled == false) return;
        if (Event != null)
        {

            Event.Invoke();

            Debug.Log("select");
            audioManager.Instance.src.Play();
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