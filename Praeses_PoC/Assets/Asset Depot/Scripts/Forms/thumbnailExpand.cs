using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class thumbnailExpand : MonoBehaviour
    {
        public float ExpandMult;
        public float ShrinkMult;
        Vector3 startScale;
        Vector3 simpleStartScale;
        Vector3 photoStartScale;
        Vector3 videoStartScale;
        public formFieldController linkedField;
        public float breakOutScale;
        public Transform breakOutPos;
        Vector3 startPos;
        bool brokeOut;

    }
}