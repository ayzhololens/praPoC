using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class nodeHighlight : MonoBehaviour {
        
        Material mat;
        public GameObject[] NodeMesh;
        public Material nodeMat;
        public Material nodeHighlightMat;

        // Use this for initialization
        void Start() {



        }

        // Update is called once per frame
        void Update() {

        }

        public void highlightNode()
        {

            for (int i = 0; i < NodeMesh.Length; i++)
            {

                    NodeMesh[i].GetComponent<Renderer>().material = nodeHighlightMat;

            }
            


    }

        public void unhighlightNode()
        {
            for (int i = 0; i < NodeMesh.Length; i++)
            {

                NodeMesh[i].GetComponent<Renderer>().material = nodeMat;

            }
            
        }
    }
}
