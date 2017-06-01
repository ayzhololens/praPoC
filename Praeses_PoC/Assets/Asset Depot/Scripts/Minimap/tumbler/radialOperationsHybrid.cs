using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class radialOperationsHybrid : MonoBehaviour
    {
        public float rotationFactor { get; set; }
        [Header("Manipulation Parameter")]
        public float rotationMultiplier;
        public GameObject tumbledObject;
        [Tooltip("Typing whether or not this is rotation(1) or scale(2)")]
        public int typing;

        [Header("Hand Manipulation Hooks")]
        [Tooltip("Object for world orient local space to camera")]
        public followCursorScript followCur;
        [Tooltip("Index to mark which icons attach themselves to cursor when triggered")]
        public int cursorIndex;

        //1 = rotation
        //2 = scaler

        // Update is fixed for smoother manipulation
        void FixedUpdate()
        {
            if (sourceManager.Instance.sourcePressed)
            {
                if (typing == 1)
                {
                    tumbledObject.transform.Rotate(new Vector3(0, -1 * rotationFactor* rotationMultiplier, 0));
                }
                else if (typing == 2)
                {
                    float scaleFactor1 = 1 + rotationFactor;
                    float scaleMin = .5f;
                    float scaleMax = 2;
                    tumbledObject.transform.localScale = new Vector3(Mathf.Clamp(tumbledObject.transform.localScale.x * scaleFactor1, scaleMin, scaleMax),
                                                            Mathf.Clamp(tumbledObject.transform.localScale.y * scaleFactor1, scaleMin, scaleMax),
                                                            Mathf.Clamp(tumbledObject.transform.localScale.z * scaleFactor1, scaleMin, scaleMax));
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag != "handCursorCollide"){ return; };
                followCur.iconIndex = cursorIndex;
                if (typing == 1)
                {
                    rotationFactor = 2;
                }
                else if (typing == 2)
                {
                    rotationFactor = .01f * rotationMultiplier;
                }
      
        }

        private void OnTriggerExit(Collider other)
        {
            
            followCur.iconIndex = 0;
            rotationFactor = 0;

        }


    }
}
