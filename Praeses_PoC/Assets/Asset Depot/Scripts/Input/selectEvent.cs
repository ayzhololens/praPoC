using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;

public class selectEvent : MonoBehaviour,  IInputClickHandler, IFocusable 
{
    public UnityEvent Event;


    bool focused;
    public bool gazeExit;
    void Start()
    {

    }

    public void OnSelect()
    {

        if (this.enabled == false) return;
        if (Event != null)
        {

            Event.Invoke();

            //Debug.Log("select");
            //audioManager.Instance.src.Play();
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

            OnSelect();
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

}