using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    //this script checks for miniMapMesh tag and hides and reveal objects accordingly
    public class boundingHide : MonoBehaviour
    {

        int startingLayer;
        public radialOperationsHybrid[] rotators;

        private void OnTriggerStay(Collider other)
        {
            for (int i = 0; i < rotators.Length; i++)
            {
                if (rotators[i].rotationFactor != 0)
                {
                    return;
                }


            }
            if (other.gameObject.GetComponent<MeshRenderer>() && other.gameObject.tag == "miniMapMesh")
            {
                if (other.gameObject.GetComponent<MeshRenderer>().enabled == true) { return; };
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                other.gameObject.layer = 0;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            for (int i = 0; i < rotators.Length; i++)
            {
                if (rotators[i].rotationFactor != 0)
                {
                    return;
                }
                
                
            }
            if (other.gameObject.GetComponent<MeshRenderer>() && other.gameObject.tag == "miniMapMesh")
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.layer = 2;

            }
        }
    }
}