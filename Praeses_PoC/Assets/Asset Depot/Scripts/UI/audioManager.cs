using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{


    public class audioManager : Singleton<audioManager>
    {

        public AudioClip highlightSound;
        public AudioClip selectSound;
        public AudioClip successSound;
        public AudioClip verifyContinueSound;
        public AudioClip openSound;
        public AudioClip closeSound;
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

                src.Play();

            }

        }
    }
}