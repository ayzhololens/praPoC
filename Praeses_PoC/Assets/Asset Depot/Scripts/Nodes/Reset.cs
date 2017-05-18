using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HoloToolkit.Unity
{

    public class Reset : MonoBehaviour
    {


        public GameObject[] reloadedObjects;
        public List<GameObject> clearedNodes;
        public List<GameObject> clearedObjs;

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < reloadedObjects.Length; i++)
            {
                GameObject newObj = Instantiate(reloadedObjects[i], transform.position, transform.rotation);

                clearedObjs.Add(newObj);
            }

            fieldSpawner.Instance.reloadForm();
        }

        // Update is called once per frame
        void Update()
        {

        }

       
        public void wipeObjects()
        {

            for (int i = 0; i < reloadedObjects.Length; i++)
            {
                //clearedObjs.Add(reloadedObjects[i]);
                DestroyImmediate(clearedObjs[i]);
                Instantiate(reloadedObjects[i], transform.position, transform.rotation);
            }
            fieldSpawner.Instance.reloadForm();
        }

        public void clearNodes()
        {
            foreach(GameObject node in mediaManager.Instance.activeNodes)
            {
                    if (node.GetComponent<nodeMediaHolder>().fieldNode)
                    {
                            foreach (GameObject comment in node.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().activeComments)
                            {
                                if (comment.GetComponent<commentContents>().filepath != null)
                                {
                                    if (comment.GetComponent<commentContents>().isVideo && File.Exists(Path.Combine(Application.persistentDataPath, comment.GetComponent<commentContents>().filepath)))
                                    {
                                        File.Delete(Path.Combine(Application.persistentDataPath, comment.GetComponent<commentContents>().filepath));
                                    }
                                    else if (comment.GetComponent<commentContents>().isPhoto && File.Exists(comment.GetComponent<commentContents>().filepath))
                                    {
                                        File.Delete(comment.GetComponent<commentContents>().filepath);
                                    }
                                }
                            }
                    }
                    if (node.GetComponent<nodeMediaHolder>().violationNode)
                    {
                        foreach (GameObject comment in node.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().activeComments)
                        {
                            if (comment.GetComponent<commentContents>().filepath != null)
                            {
                               
                                if (comment.GetComponent<commentContents>().isVideo && File.Exists(Path.Combine(Application.persistentDataPath, comment.GetComponent<commentContents>().filepath)))
                                {
                                    File.Delete(Path.Combine(Application.persistentDataPath, comment.GetComponent<commentContents>().filepath));
                                }
                                else if (comment.GetComponent<commentContents>().isPhoto && File.Exists(comment.GetComponent<commentContents>().filepath))
                                {
                                    File.Delete(comment.GetComponent<commentContents>().filepath);
                                }
                            }
                        }
                        //node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview.GetComponent<viewViolationContent>().viewViolationHolder.GetComponent<viewViolationController>().vioFields.Remove(node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview);
                        //DestroyImmediate(node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview);
                        DestroyImmediate(node.GetComponent<nodeController>().linkedField);

                    }

                    if (node.GetComponent<nodeMediaHolder>().activeFilepath.Length>1)
                    {
                        Debug.Log(Path.Combine(Application.persistentDataPath, node.GetComponent<nodeMediaHolder>().activeFilepath));

                        if (File.Exists(node.GetComponent<nodeMediaHolder>().activeFilepath))
                        {
                            File.Delete(Path.Combine(Application.persistentDataPath, node.GetComponent<nodeMediaHolder>().activeFilepath));

                        }
                    }



                    clearedNodes.Add(node);
                
            }
            if (clearedNodes.Count > 0)
            {
                wipeList();

            }



        }

        void wipeList()
        {
            foreach(GameObject node in clearedNodes)
            {
                mediaManager.Instance.activeNodes.Remove(node);
                Destroy(node.GetComponent<nodeController>().miniNode);
                //databaseMan.Instance.removeNode(node);
                Destroy(node);
            }

            clearedNodes.Clear();
            wipeObjects();
            databaseMan.Instance.loadValCmd();
        }
    }
}
