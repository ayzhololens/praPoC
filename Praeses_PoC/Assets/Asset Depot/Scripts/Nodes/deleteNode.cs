using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using HoloToolkit.Unity;

public class deleteNode : MonoBehaviour {
    GameObject node;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NodeDelete()
    {
        node = this.gameObject;
        if (node.GetComponent<nodeController>() != null)
        {
            if (!node.GetComponent<nodeController>().fromJSON)
            {
                if (node.GetComponent<nodeMediaHolder>().fieldNode)
                {
                    foreach (GameObject comment in node.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().activeComments)
                    {
                        if (comment.GetComponent<commentContents>().filepath != null)
                        {
                            string path = Path.Combine(Application.persistentDataPath, comment.GetComponent<commentContents>().filepath);
                            if (comment.GetComponent<commentContents>().isVideo && File.Exists(path))
                            {
                                File.Delete(path);
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

                            string path = Path.Combine(Application.persistentDataPath, comment.GetComponent<commentContents>().filepath);
                            if (comment.GetComponent<commentContents>().isVideo && File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            else if (comment.GetComponent<commentContents>().isPhoto && File.Exists(comment.GetComponent<commentContents>().filepath))
                            {
                                File.Delete(comment.GetComponent<commentContents>().filepath);
                            }
                        }
                    }

                    DestroyImmediate(node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview);
                    DestroyImmediate(node.GetComponent<nodeController>().linkedField);

                }

                if (node.GetComponent<nodeMediaHolder>().activeFilepath.Length > 1)
                {

                    if (File.Exists(node.GetComponent<nodeMediaHolder>().activeFilepath))
                    {
                        File.Delete(Path.Combine(Application.persistentDataPath, node.GetComponent<nodeMediaHolder>().activeFilepath));

                    }
                }
                mediaManager.Instance.activeNodes.Remove(node);
                DestroyImmediate(node.GetComponent<nodeController>().miniNode);
                DestroyImmediate(node);

            }
        }
        else
        {
            node = node.GetComponent<violationController>().linkedNode;
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

                DestroyImmediate(node.GetComponent<nodeController>().linkedField.GetComponent<violationController>().linkedPreview);
                DestroyImmediate(node.GetComponent<nodeController>().linkedField);

            }
            mediaManager.Instance.activeNodes.Remove(node);
            DestroyImmediate(node.GetComponent<nodeController>().miniNode);
            DestroyImmediate(node);
        }
    }
}
