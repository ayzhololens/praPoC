using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


namespace HoloToolkit.Unity
{

    public class metaManager : Singleton<metaManager>
    {

        public string user;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public string date()
        {
            string tempdate = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            return tempdate;
        }

        public string dateShort()
        {
            string tempdate = System.DateTime.Now.ToString("MM/dd/yyyy");
            return tempdate;
        }
    }
}
