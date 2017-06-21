using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity.InputModule;


namespace HoloToolkit.Unity.InputModule
{
    public class gazeEnterEvent : MonoBehaviour, IFocusable
    {
        public UnityEvent Event;
        AudioSource aud;



        void Start()
        {
            if (Event != null)
            {
                if (GetComponent<AudioSource>() == null)
                {
                    gameObject.AddComponent<AudioSource>();
                }
                if (aud == null)
                {
                    aud = GetComponent<AudioSource>();
                    aud.playOnAwake = false;
                    aud.spatialBlend = 1;
                    aud.minDistance = 2;
                    aud.maxDistance = 5;
                    aud.clip = audioManager.Instance.highlightSound;

                }
            }
        }

        void GazeEnter()
        {
            if (this.enabled == false) return;
            if (Event != null)
            {
                aud.Play();
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