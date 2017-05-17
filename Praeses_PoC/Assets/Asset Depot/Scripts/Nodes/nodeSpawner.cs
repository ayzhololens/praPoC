using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Examples.GazeRuler;

namespace HoloToolkit.Unity
{

    public class nodeSpawner : Singleton<nodeSpawner>
    {

        public GameObject[] nodePrefab;
        public GameObject[] miniNodePrefab;

        GameObject spawnedNode;
        int spawnedIndex;
        bool placingInProgress;
        bool reposInProgress;
        int offsetCounter;

        //gaze position and rotation
        Vector3 lookPos;
        Quaternion lookRot;

        public formFieldController linkedField;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (placingInProgress)
            {
                nodePlacement();
            }
            if (reposInProgress)
            {
                repositionNode(spawnedNode);
            }

        }

        public void spawnNode(int nodeIndex)
        {
            //set status indicator
            mediaManager.Instance.setStatusIndicator("Tap to place node");

            //spawn the right node
            spawnedIndex = nodeIndex;
            spawnedNode = Instantiate(nodePrefab[spawnedIndex], getNodeLoc(lookPos), getNodeRot(lookRot));
            
            //add node to active node list and set the parent
            mediaManager.Instance.activeNodes.Add(spawnedNode);
            spawnedNode.transform.SetParent(transform);

            //turn off the collider so we dont activate it when placing or capturing
            spawnedNode.GetComponent<BoxCollider>().enabled = false;
            placingInProgress = true;
        }

        Vector3 getNodeLoc(Vector3 camPos)
        {
            camPos = GazeManager.Instance.HitPosition;
            return camPos;
        }
        Quaternion getNodeRot(Quaternion camRot)
        {
            camRot = Quaternion.FromToRotation(Vector3.up, GazeManager.Instance.HitInfo.normal);
            return camRot;
        }

        void nodePlacement()
        {
            //update node placement
            spawnedNode.transform.position = getNodeLoc(lookPos);
            spawnedNode.transform.rotation = getNodeRot(lookRot);

            //wait a short while before they can lock placement so it doesnt autolock
            offsetCounter += 1;
            if(sourceManager.Instance.sourcePressed && offsetCounter >= 40)
            {
                lockNodePlacement();
                offsetCounter = 0;
            }
        }

        public void spawnMiniNode(GameObject parentNode, int spawnIndex)
        {
            //get minimap components, scale and offset it to real space
            minimapSpawn miniMapComponent = minimapSpawn.Instance;
            Vector3 boilerPos = miniMapComponent.boilerPivot;
            Transform miniMap = miniMapComponent.miniMapHolder.transform;
            Transform rotatorGroup = miniMap.parent;
            Transform scalerGroup = rotatorGroup.parent.parent;
            Quaternion initRot = rotatorGroup.rotation;

            rotatorGroup.localScale = Vector3.one * (1 / miniMapComponent.scaleOffset) * (1/scalerGroup.localScale.x);
            rotatorGroup.position = boilerPos;
            rotatorGroup.rotation = new Quaternion(0,0,0,0);

            //spawn miniNode and parent it correctly
            GameObject miniNode = Instantiate(miniNodePrefab[spawnIndex], parentNode.transform.position, parentNode.transform.rotation);
            miniNode.GetComponent<nodeController>().parentNode = parentNode;


            //reset rotator group to position miniNode
            miniNode.transform.SetParent(miniMap);
            rotatorGroup.localPosition = Vector3.zero;
            rotatorGroup.localScale = Vector3.one;
            rotatorGroup.rotation = initRot;
            miniNode.SetActive(miniMapToggle.Instance.active);

            parentNode.GetComponent<nodeController>().miniNode = miniNode;
        }

        public void lockNodePlacement()
        {

            print("locpos, x: " + spawnedNode.transform.localPosition.x + ", y: " + spawnedNode.transform.localPosition.y + ", z: " + spawnedNode.transform.localPosition.z);
            print("locrot, x: " + spawnedNode.transform.localRotation.x + ", y: " + spawnedNode.transform.localRotation.y + ", z: " + spawnedNode.transform.localRotation.z + ", w: " + spawnedNode.transform.localRotation.w);
            print("locscale, x: " + spawnedNode.transform.localScale.x + ", y: " + spawnedNode.transform.localScale.y + ", z: " + spawnedNode.transform.localScale.z);
            print("wpos, x: " + spawnedNode.transform.position.x + ", y: " + spawnedNode.transform.position.y + ", z: " + spawnedNode.transform.position.z);
            print("wrot, x: " + spawnedNode.transform.rotation.x + ", y: " + spawnedNode.transform.rotation.y + ", z: " + spawnedNode.transform.rotation.z + ", w: " + spawnedNode.transform.rotation.w);


            placingInProgress = false;
            mediaManager.Instance.currentNode = spawnedNode;
            mediaManager.Instance.disableStatusIndicator();



            spawnMiniNode(spawnedNode, spawnedIndex);

            spawnedNode.GetComponent<nodeMediaHolder>().NodeIndex = JU_databaseMan.Instance.nodesManager.nodes.Count+1;
            
            //spawnedNode.GetComponent<nodeMediaHolder>().NodeIndex = mediaManager.Instance.nodeIndex;
            //mediaManager.Instance.nodeIndex += 1;


            if (spawnedIndex == 0)
            {
                //simple node so activate it immediately
                mediaManager.Instance.activateMedia();
            }

            if(spawnedIndex == 1)
            {
                //photo node so enable photo capture
                mediaManager.Instance.enablePhotoCapture();
            }

            if(spawnedIndex == 2)
            {
                //video node
                mediaManager.Instance.enableVideoRecording();
            }

            if(spawnedIndex == 3)
            {
                //violation node, pass it into violation spawner
                violatoinSpawner.Instance.spawnViolation(spawnedNode);
                mediaManager.Instance.activateMedia();
            }

            if(spawnedIndex == 4)
            {
                //get content holder of masterform
                spawnedNode.GetComponent<nodeController>().contentHolder = fieldSpawner.Instance.MasterForm.GetComponent<formController>().contentHolder;
                fieldSpawner.Instance.MasterForm.GetComponent<formController>().fieldNodes.Add(spawnedNode);
                spawnedNode.GetComponent<nodeController>().linkedField = linkedField.gameObject;
                //activate media to store user and date
                mediaManager.Instance.activateMedia();

                print(linkedField);

                //link field and node then enable attachment capture

                linkedField.linkedNode = spawnedNode;
                linkedField.enableAttachmentCapture();

            }


        }

        //get linked form field
        public void getLinkedField(formFieldController curField)
        {

            linkedField = curField;

            print("1");

        }

        public void repositionNode(GameObject node)
        {
            if(spawnedNode!=node || !reposInProgress)
            {

                node.GetComponent<nodeController>().closeNode();
                node.GetComponent<BoxCollider>().enabled = false;
                spawnedNode = node;
                reposInProgress = true;

            }
            //update node placement
            spawnedNode.transform.position = getNodeLoc(lookPos);
            spawnedNode.transform.rotation = getNodeRot(lookRot);

            //wait a short while before they can lock placement so it doesnt autolock
            offsetCounter += 1;
            if (sourceManager.Instance.sourcePressed && offsetCounter >= 40)
            {
                reposInProgress = false;
                reposMiniNode(spawnedNode);
                node.GetComponent<BoxCollider>().enabled = true;
                offsetCounter = 0;
                node.GetComponent<nodeController>().openNode();
            }
        }

        void reposMiniNode(GameObject node)
        {
            //get minimap components, scale and offset it to real space
            minimapSpawn miniMapComponent = minimapSpawn.Instance;
            Vector3 boilerPos = miniMapComponent.boilerPivot;
            Transform miniMap = miniMapComponent.miniMapHolder.transform;
            Transform rotatorGroup = miniMap.parent;
            rotatorGroup.localScale = Vector3.one * (1 / miniMapComponent.scaleOffset);
            rotatorGroup.position = boilerPos;

            //spawn miniNode and parent it correctly
            GameObject miniNode = node.GetComponent<nodeController>().miniNode;
            miniNode.transform.position = node.transform.position;
            miniNode.transform.rotation = node.transform.rotation;


            //reset rotator group to position miniNode
            miniNode.transform.SetParent(miniMap);
            rotatorGroup.localPosition = Vector3.zero;
            rotatorGroup.localScale = Vector3.one;
        }


    

    }


}