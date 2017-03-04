﻿using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;


namespace HoloToolkit.Unity.InputModule
{
    public class gazeEnterEvent : MonoBehaviour, IFocusable
    {
        public UnityEvent Event;

        void Start()
        {

            // dummy Start function so we can use this.enabled
        }

        void GazeEnter()
        {
            if (this.enabled == false) return;
            if (Event != null)
            {
                Event.Invoke();
            }
        }


        public void OnFocusEnter()
        {
            if (GazeManager.Instance.HitObject == this.gameObject)
            {
                GazeEnter();
            }

        }

        public void OnFocusExit()
        {

        }



    }
}