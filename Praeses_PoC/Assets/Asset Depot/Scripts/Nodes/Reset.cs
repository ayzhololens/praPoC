using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                newObj.transform.position = frontHolderInstance.Instance.setFrontHolder(2.0f).transform.position;
                clearedObjs.Add(newObj);
            }

            fieldSpawner.Instance.reloadForm();
        }

        

        // Update is called once per frame
        void Update()
        {

        }

        public void reloadLvl()
        {
            SceneManager.LoadScene(0);
        }

       
        public void wipeObjects()
        {

            for (int i = 0; i < clearedObjs.Count; i++)
            {
                //clearedObjs.Add(reloadedObjects[i]);
                DestroyImmediate(clearedObjs[i]);
                
            }
            clearedObjs.Clear();
            spawnObjs();
        }


        void spawnObjs()
        {
            for (int i = 0; i < reloadedObjects.Length; i++)
            {
                GameObject newObj = Instantiate(reloadedObjects[i], transform.position, transform.rotation);
                newObj.transform.position = Camera.main.transform.forward;
                newObj.name = newObj.name + " respawned";
                clearedObjs.Add(newObj);
            }

            ReloadIt();
        }


        void ReloadIt()
        {
            fieldSpawner.Instance.reloadForm();
            fitBoxControl.Instance.toggleFitbox(false);

        }

        public void clearNodes()
        {
            foreach(GameObject node in mediaManager.Instance.activeNodes)
            {
                nodeMediaHolder nodeMedia = node.GetComponent<nodeMediaHolder>();
                    if (nodeMedia.fieldNode)
                    {
                            foreach (GameObject comment in node.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().activeComments)
                            {
                                commentContents com = comment.GetComponent<commentContents>();
                                if (com.filepath != null && !node.GetComponent<nodeController>().fromJSON)
                                {
                                    if (com.isVideo && File.Exists(Path.Combine(Application.persistentDataPath, com.filepath)))
                                    {
                                        File.Delete(Path.Combine(Application.persistentDataPath, com.filepath));
                                    }
                                    else if (com.isPhoto && File.Exists(com.filepath))
                                    {
                                        File.Delete(com.filepath);
                                    }
                                }
                            }
                    }
                    if (nodeMedia.violationNode)
                    {
                        foreach (GameObject comment in node.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().activeComments)
                    {
                        commentContents com = comment.GetComponent<commentContents>();
                        if (com.filepath != null && !node.GetComponent<nodeController>().fromJSON)
                            {
                               
                                if (com.isVideo && File.Exists(Path.Combine(Application.persistentDataPath, com.filepath)))
                                {
                                    File.Delete(Path.Combine(Application.persistentDataPath, com.filepath));
                                }
                                else if (com.isPhoto && File.Exists(com.filepath))
                                {
                                    File.Delete(com.filepath);
                                }
                            }
                        }
                        //node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview.GetComponent<viewViolationContent>().viewViolationHolder.GetComponent<viewViolationController>().vioFields.Remove(node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview);
                        //DestroyImmediate(node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview);
                        DestroyImmediate(node.GetComponent<nodeController>().linkedField);

                    }

                    if (nodeMedia.activeFilepath.Length>1)
                    {


                        if (File.Exists(Path.Combine(Application.persistentDataPath, nodeMedia.activeFilepath)) && !node.GetComponent<nodeController>().fromJSON)
                        {
                        File.Delete(Path.Combine(Application.persistentDataPath, nodeMedia.activeFilepath));

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
