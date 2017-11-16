using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoloToolkit.Unity
{

    public class Reset : MonoBehaviour
    {


        [Tooltip("Prefabs that should be instantiated on start and when the scene is reloaded")]
        public GameObject[] reloadedObjects;
        public List<GameObject> clearedNodes;
        public List<GameObject> clearedObjs;

        // Use this for initialization
        void Start()
        {
            //instantiate the prefabs then add them to a list that will be cleared in between sessions
            for (int i = 0; i < reloadedObjects.Length; i++)
            {
                GameObject newObj = Instantiate(reloadedObjects[i], transform.position, transform.rotation);
                newObj.transform.position = frontHolderInstance.Instance.setFrontHolder(2.0f).transform.position;
                clearedObjs.Add(newObj);
            }

            //reload the master form
            fieldSpawner.Instance.reloadForm();
        }

        



        //deletes node comments and stores the nodes to be deleted
        public void clearNodes()
        {
            foreach(GameObject node in mediaManager.Instance.activeNodes)
            {
                nodeMediaHolder nodeMedia = node.GetComponent<nodeMediaHolder>();

                //grab all the comments associated with the field and violation nodes then delete them 
                if (nodeMedia.fieldNode || nodeMedia.violationNode)
                {
                    foreach (GameObject comment in node.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().activeComments)
                    {
                        if(comment!=null)
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
                }

                //delete the associated violation form
                if (nodeMedia.violationNode)
                {
                        DestroyImmediate(node.GetComponent<nodeController>().linkedField);
                }

                //delete the media on an annotation node
                if (nodeMedia.activeFilepath!=null)
                {
                    if (File.Exists(Path.Combine(Application.persistentDataPath, nodeMedia.activeFilepath)) && !node.GetComponent<nodeController>().fromJSON)
                    {
                        File.Delete(Path.Combine(Application.persistentDataPath, nodeMedia.activeFilepath));
                    }
                }
                
                //add each node to a list to get deleted
                clearedNodes.Add(node);
                
            }
            if (clearedNodes.Count > 0)
            {
                wipeNodes();

            }



        }

        void wipeNodes()
        {
            //delete each node and clear the node list
            foreach(GameObject node in clearedNodes)
            {
                mediaManager.Instance.activeNodes.Remove(node);
                Destroy(node.GetComponent<nodeController>().miniNode);
                Destroy(node);
            }

            clearedNodes.Clear();
            wipeObjects();

            //reload database values to clear any user changes
            databaseMan.Instance.loadValCmd();
        }
        

        public void wipeObjects()
        {
            //destroy all the reloadable objects
            for (int i = 0; i < clearedObjs.Count; i++)
            {
                DestroyImmediate(clearedObjs[i]);

            }
            clearedObjs.Clear();
            respawnObjs();
        }


        void respawnObjs()
        {
            //reinstantiate them
            for (int i = 0; i < reloadedObjects.Length; i++)
            {
                GameObject newObj = Instantiate(reloadedObjects[i], transform.position, transform.rotation);
                newObj.transform.position = Camera.main.transform.forward;
                newObj.name = newObj.name + " respawned";
                clearedObjs.Add(newObj);
            }

            ReloadObjs();
        }


        void ReloadObjs()
        {
            //reload and reset the fitbox and the app is good to go :)
            fieldSpawner.Instance.reloadForm();
            fitBoxControl.Instance.toggleFitbox(false);

        }


        //reload the entire unity scene
        public void reloadLvl()
        {
            SceneManager.LoadScene(0);
        }
    }
}
