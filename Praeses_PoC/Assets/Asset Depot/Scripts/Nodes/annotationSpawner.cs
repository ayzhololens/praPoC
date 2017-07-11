using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{

    public class annotationSpawner : Singleton<annotationSpawner> {
        
        public GameObject spawnedAnnotation;
        public bool tapToPlaceInProgress;
        public GameObject photoNode;
        public GameObject videoNode;
        public GameObject simpleNode;
        public GameObject fieldNode;
        public GameObject violationNode;
        public GameObject Minimap;
        public Vector3 anchDist;
        public Transform SpatialMapping;
        public GameObject miniPhotoNode;
        public GameObject miniVideoNode;
        public GameObject miniSimpleNode;
        public GameObject miniFieldNode;
        public GameObject miniViolationNode;
        float scaleOffest;
        int finishCounter;
        GameObject miniAnnotation;

        bool isVideoNode;
        bool isPhotoNode;
        bool isSimpleNode;
        bool isFieldNode;
        bool isViolationNode;

        // Use this for initialization
        void Start()
        {

            scaleOffest = SpatialMapping.GetComponent<minimapSpawn>().scaleOffset;

        }

        // Update is called once per frame
        void Update()
        {

            if (tapToPlaceInProgress)
            {
                startPlacingAnnotNode();

            }

        }

        public void startPlacingAnnotNode()
        {
            Vector3 pos = GazeManager.Instance.HitPosition;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            spawnedAnnotation.transform.position = pos;
            spawnedAnnotation.transform.rotation = rot;

            finishCounter += 1;
            if (sourceManager.Instance.sourcePressed && finishCounter >= 40)
            {
                finishPlacingAnnotNode();
                finishCounter = 0;
            }




        }

        public void finishPlacingAnnotNode()
        {
            tapToPlaceInProgress = false;
            //GetComponent<annotationManager>().tapToPlaceIndicator.SetActive(false);
            //GetComponent<annotationManager>().tapToPlaceAnnotNode = false;
            spawnedAnnotation.GetComponent<BoxCollider>().enabled = true;
            spawnedAnnotation.GetComponent<selectEvent>().enabled = false;

            annotationManager.Instance.currentAnnotation = spawnedAnnotation;
            //spawnedAnnotation.GetComponent<openAnnotationNode>().openContent();

            //spawn miniNode
            GameObject boiler = GameObject.Find("boiler(Clone)");
            Vector3 boilerPos = boiler.transform.position;
            

            Minimap = SpatialMapping.GetComponent<miniMapToggle>().MiniMapHolder;
            GameObject rotatorGroup = Minimap.transform.parent.gameObject;
            rotatorGroup.transform.localScale = Vector3.one * 1 / scaleOffest;
            rotatorGroup.transform.position = boilerPos;
            anchDist = (boiler.transform.position - spawnedAnnotation.transform.position);

            if (isPhotoNode)
            {
                miniAnnotation = Instantiate(miniPhotoNode, spawnedAnnotation.transform.position, spawnedAnnotation.transform.rotation) as GameObject;
            }
            if (isVideoNode)
            {
                miniAnnotation = Instantiate(miniVideoNode, spawnedAnnotation.transform.position, spawnedAnnotation.transform.rotation) as GameObject;
            }
            if (isSimpleNode)
            {
                miniAnnotation = Instantiate(miniSimpleNode, spawnedAnnotation.transform.position, spawnedAnnotation.transform.rotation) as GameObject;
            }
            if (isFieldNode)
            {
                miniAnnotation = Instantiate(miniFieldNode, spawnedAnnotation.transform.position, spawnedAnnotation.transform.rotation) as GameObject;

            }
            if (isViolationNode)
            {
                miniAnnotation = Instantiate(miniViolationNode, spawnedAnnotation.transform.position, spawnedAnnotation.transform.rotation) as GameObject;

            }
            GetComponent<annotationManager>().activeAnnotations.Add((GameObject)miniAnnotation);
            miniAnnotation.transform.SetParent(Minimap.transform);
            rotatorGroup.transform.localPosition = Vector3.zero;
            rotatorGroup.transform.localScale = Vector3.one;
            miniAnnotation.SetActive(SpatialMapping.GetComponent<miniMapToggle>().active);
            if (!isFieldNode && !isViolationNode)
            {

                spawnedAnnotation.GetComponent<openAnnotationNode>().parentNode = spawnedAnnotation;
                spawnedAnnotation.GetComponent<openAnnotationNode>().miniNode = miniAnnotation;
                miniAnnotation.GetComponent<openAnnotationNode>().parentNode = spawnedAnnotation;
                miniAnnotation.GetComponent<openAnnotationNode>().miniNode = miniAnnotation;
            }
            if (isVideoNode)
            {
                GetComponent<annotationManager>().enableVideoRecording();
                Debug.Log("spawned video node, trying to start");
            }
            if (isSimpleNode)
            {
                GetComponent<annotationManager>().activateMedia();
            }

            if (isPhotoNode)
            {
                GetComponent<annotationManager>().enablePhotoCapture();
            }
            if (isFieldNode)
            {
                spawnedAnnotation.GetComponent<formNodeController>().linkedField = annotationManager.Instance.activeField;
                annotationManager.Instance.activeField.GetComponent<formFieldController>().linkedNode = spawnedAnnotation;
                annotationManager.Instance.activeField.GetComponent<formFieldController>().enableAttachmentCapture();
                //annotationManager.Instance.stateIndicator.SetActive(false);
            }
            if (isViolationNode)
            {
                //spawnedAnnotation.GetComponent<violationNodeController>().spawnViolation();
                annotationManager.Instance.activateMedia();
                ////annotationManager.Instance.stateIndicator.SetActive(false);
                //miniAnnotation.GetComponent<violationNodeController>().parentNode = spawnedAnnotation;
                //spawnedAnnotation.GetComponent<violationNodeController>().miniNode = miniAnnotation;
            }

            if (!violationNode)
            {

                databaseMan.Instance.addAnnotation(spawnedAnnotation);
            }
            isPhotoNode = false;
            isSimpleNode = false;
            isVideoNode = false;
            isFieldNode = false;
            isViolationNode = false;
        }

        public void spawnPhotoAnnotation()
        {
            isPhotoNode = true;
            Vector3 pos = GazeManager.Instance.HitPosition;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            spawnedAnnotation = Instantiate(photoNode, pos, rot) as GameObject;
            GetComponent<annotationManager>().activeAnnotations.Add((GameObject)spawnedAnnotation);
            spawnedAnnotation.GetComponent<BoxCollider>().enabled = false;
            spawnedAnnotation.GetComponent<openAnnotationNode>().closeContent();
            spawnedAnnotation.transform.SetParent(transform);

            //GetComponent<annotationManager>().stateIndicator.SetActive(true);
            //GetComponent<annotationManager>().stateIndicator.GetComponent<TextMesh>().text = "Place node for annotation";
            tapToPlaceInProgress = true;



        }

        public void spawnVideoAnnotation()
        {
            isVideoNode = true;
            Vector3 pos = GazeManager.Instance.HitPosition;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            spawnedAnnotation = Instantiate(videoNode, pos, rot) as GameObject;
            GetComponent<annotationManager>().activeAnnotations.Add((GameObject)spawnedAnnotation);
            spawnedAnnotation.GetComponent<BoxCollider>().enabled = false;
            spawnedAnnotation.GetComponent<openAnnotationNode>().closeContent();
            spawnedAnnotation.transform.SetParent(transform);
            tapToPlaceInProgress = true;

            //GetComponent<annotationManager>().stateIndicator.SetActive(true);
            //GetComponent<annotationManager>().stateIndicator.GetComponent<TextMesh>().text = "Place node for annotation";
        }


        public void spawnSimpleAnnotation()
        {
            isSimpleNode = true;
            Vector3 pos = GazeManager.Instance.HitPosition;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            spawnedAnnotation = Instantiate(simpleNode, pos, rot) as GameObject;
            GetComponent<annotationManager>().activeAnnotations.Add((GameObject)spawnedAnnotation);
            spawnedAnnotation.GetComponent<BoxCollider>().enabled = false;
            spawnedAnnotation.GetComponent<openAnnotationNode>().closeContent();
            spawnedAnnotation.transform.SetParent(transform);

            //GetComponent<annotationManager>().stateIndicator.SetActive(true);
            //GetComponent<annotationManager>().stateIndicator.GetComponent<TextMesh>().text = "Place node for annotation";

            tapToPlaceInProgress = true;
        }

        public void spawnFieldAnnotation()
        {
            isFieldNode = true;
            Vector3 pos = GazeManager.Instance.HitPosition;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            spawnedAnnotation = Instantiate(fieldNode, pos, rot) as GameObject;
            GetComponent<annotationManager>().activeAnnotations.Add((GameObject)spawnedAnnotation);
            spawnedAnnotation.GetComponent<BoxCollider>().enabled = false;
            spawnedAnnotation.transform.SetParent(transform);
            //GetComponent<annotationManager>().stateIndicator.SetActive(true);
            //GetComponent<annotationManager>().stateIndicator.GetComponent<TextMesh>().text = "Place node for attachment";
            tapToPlaceInProgress = true;
        }


        public void spawnViolationNode()
        {
            isViolationNode = true;
            Vector3 pos = GazeManager.Instance.HitPosition;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            spawnedAnnotation = Instantiate(violationNode, pos, rot) as GameObject;
            GetComponent<annotationManager>().activeAnnotations.Add((GameObject)spawnedAnnotation);
            spawnedAnnotation.GetComponent<BoxCollider>().enabled = false;
            spawnedAnnotation.transform.SetParent(transform);

            //GetComponent<annotationManager>().stateIndicator.SetActive(true);
            //GetComponent<annotationManager>().stateIndicator.GetComponent<TextMesh>().text = "Place node for violation";

            tapToPlaceInProgress = true;
        }


        public void spawnMiniAnnotation()
        {
            //miniAnnot.transform.SetParent(Holder);
            //anchDist = (Holder.position - miniAnnot.transform.position);

            //Minimap = GameObject.Find("MiniMapHolder");
            //GameObject miniAnnotation = Instantiate(miniAnnotationNode, Minimap.transform.position, Quaternion.identity) as GameObject;
            //miniAnnotation.transform.position = (miniAnnotation.transform.position - (anchDist * .09f));
            //miniAnnotation.transform.SetParent(Minimap.transform);
        }
    }
}
