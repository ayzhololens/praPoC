using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


namespace HoloToolkit.Unity
{

    public class metaManager : Singleton<metaManager>
    {

        [Tooltip("User that will appear across the app")]
        public string user;

        //full date and time
        public string date()
        {
            string tempdate = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            return tempdate;
        }

        //date without time
        public string dateShort()
        {
            string tempdate = System.DateTime.Now.ToString("MM/dd/yyyy");
            return tempdate;
        }
    }
}
