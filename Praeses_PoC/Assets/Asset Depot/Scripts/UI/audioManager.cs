using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{


    public class audioManager : Singleton<audioManager>
    {


        [Tooltip("Index: 0")]
        public AudioClip selectSound;
        [Tooltip("Index: 1")]
        public AudioClip successSound;
        [Tooltip("Index: 2")]
        public AudioClip verifyContinueSound;
        [Tooltip("Index: 3")]
        public AudioClip openSound;
        [Tooltip("Index: 4")]
        public AudioClip closeSound;
        [Tooltip("Index: 5")]
        public AudioClip highlightSound;
        public AudioSource src;


        public void setAndPlayAudio(int index)
        {
            //set index to 10 to not play any audio on select
            if(index != 10)
            {
                if (index == 0)
                {
                    src.clip = selectSound;
                }
                if (index == 1)
                {
                    src.clip = successSound;
                }
                if (index == 2)
                {
                    src.clip = verifyContinueSound;
                }
                if (index == 3)
                {
                    src.clip = openSound;
                }
                if (index == 4)
                {
                    src.clip = closeSound;
                }
                if (index == 5)
                {
                    src.clip = highlightSound;
                }

                src.Play();

            }

        }
    }
}