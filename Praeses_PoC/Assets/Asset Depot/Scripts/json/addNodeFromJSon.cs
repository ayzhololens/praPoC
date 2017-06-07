using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using System;
using System.Linq;

public class addNodeFromJSon : Singleton<addNodeFromJSon> {

    public GameObject[] nodeprefabs;

    [Header("For Offsite Use")]
    public Text changedText;
    public GameObject offSiteHolder;

    ///<summary>
    ///spawn a single node onsite by feeding the argument nodeClass 
    ///</summary>
    public void spawnNode(JU_databaseMan.nodeItem nodeClass)
    {
        Vector3 pos = new Vector3(nodeClass.transform[0], nodeClass.transform[1], nodeClass.transform[2]);
        Quaternion rot = new Quaternion(nodeClass.transform[3], nodeClass.transform[4], nodeClass.transform[5], nodeClass.transform[6]);
        Vector3 sca = new Vector3(nodeClass.transform[7], nodeClass.transform[8], nodeClass.transform[9]);

        GameObject spawnedNode = Instantiate(nodeprefabs[nodeClass.type], pos, rot);
        spawnedNode.transform.SetParent(mediaManager.Instance.gameObject.transform);
        spawnedNode.transform.localPosition = pos;
        spawnedNode.transform.localRotation = rot;
        spawnedNode.transform.localScale = sca;
        spawnedNode.GetComponent<nodeMediaHolder>().type = nodeClass.type;
        mediaManager.Instance.activeNodes.Add(spawnedNode);
        spawnedNode.GetComponent<nodeMediaHolder>().User = nodeClass.user;
        spawnedNode.GetComponent<nodeMediaHolder>().Date = nodeClass.date;
        spawnedNode.GetComponent<nodeMediaHolder>().NodeIndex = nodeClass.indexNum;

        spawnedNode.GetComponent<nodeController>().fromJSON = true;

        //Apply this to node type field 
        #region

        if (nodeClass.type == 2)//type = 2 field
        {
            List<JU_databaseMan.valueItem> tempvalues = new List<JU_databaseMan.valueItem>();
            foreach (JU_databaseMan.valueItem valueJU in JU_databaseMan.Instance.values.equipmentData)
            {
                tempvalues.Add(valueJU);
            }
            foreach (JU_databaseMan.valueItem valueJU in JU_databaseMan.Instance.values.historicData)
            {
                tempvalues.Add(valueJU);
            }

            foreach (JU_databaseMan.valueItem valueJU in tempvalues)
            {
                if (valueJU.nodeIndex != 0)
                {
                    //print(valueJU.name + "has node!");
                    //spawnedNode.GetComponent<formNodeController>().linkedField = fieldSpawner.Instance.ActiveFields[valueJU.name];
                    //spawnedNode.GetComponent<formNodeController>().linkedField.GetComponent<formFieldController>().linkedNode = spawnedNode;

                    //get content holder of masterform
                    spawnedNode.GetComponent<nodeController>().contentHolder = fieldSpawner.Instance.MasterForm.GetComponent<formController>().contentHolder;
                    fieldSpawner.Instance.MasterForm.GetComponent<formController>().fieldNodes.Add(spawnedNode);

                    if(spawnedNode.GetComponent<nodeController>().miniNode == null)
                    {
                        nodeSpawner.Instance.spawnMiniNode(spawnedNode, 4);

                    }

                    //link field 
                    if (valueJU.nodeIndex == nodeClass.indexNum)
                    {
                        //print(nodeClass.title + " node index: " + nodeClass.indexNum + " is trying to link to " + valueJU.name + " node index " + valueJU.nodeIndex);
                        spawnedNode.GetComponent<nodeController>().linkedField = fieldSpawner.Instance.ActiveFields[valueJU.name];
                        spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<formFieldController>().linkedNode = spawnedNode;

                    }

                    //spawnedNode.GetComponent<formFieldController>().linkedNode = spawnedNode;


                    List<databaseMan.tempComment> tempList = new List<databaseMan.tempComment>();
                    List<databaseMan.tempComment> spawnList = new List<databaseMan.tempComment>();
                    foreach (JU_databaseMan.comment commentJU in nodeClass.comments)
                    {
                        databaseMan.tempComment newItem = new databaseMan.tempComment();
                        newItem.date = commentJU.date;
                        newItem.type = 1;
                        newItem.content = commentJU.content;
                        tempList.Add(newItem);
                    }
                    foreach (JU_databaseMan.media photoJU in nodeClass.photos)
                    {
                        databaseMan.tempComment newItem = new databaseMan.tempComment();
                        newItem.date = photoJU.date;
                        newItem.type = 2;
                        newItem.path = photoJU.path;
                        tempList.Add(newItem);
                    }
                    foreach (JU_databaseMan.media videoJU in nodeClass.videos)
                    {
                        databaseMan.tempComment newItem = new databaseMan.tempComment();
                        newItem.date = videoJU.date;
                        newItem.type = 3;
                        newItem.path = videoJU.path;
                        tempList.Add(newItem);
                    }

                    spawnList = tempList.OrderBy(si => si.date).ToList();

                    foreach (databaseMan.tempComment comment in spawnList.Reverse<databaseMan.tempComment>())
                    {
                        if (comment.type == 1 && spawnedNode.GetComponent<nodeController>().linkedField != null)
                        {
                            GameObject comment3D = spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().spawnSimpleCommentFromJSON();

                            comment3D.GetComponent<commentContents>().Date = comment.date;
                            comment3D.GetComponent<commentContents>().user = comment.user;
                            comment3D.GetComponent<commentContents>().commentMetaUser.text = comment.user;
                            comment3D.GetComponent<commentContents>().commentMetaDate.text = comment.date;
                            comment3D.GetComponent<commentContents>().commentMain.text = comment.content;
                        }
                        else if (comment.type == 2 && spawnedNode.GetComponent<nodeController>().linkedField != null)
                        {
                            GameObject comment3D = spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().spawnPhotoCommentFromJSON();


                            comment3D.GetComponent<commentContents>().Date = comment.date;
                            comment3D.GetComponent<commentContents>().user = comment.user;
                            comment3D.GetComponent<commentContents>().commentMetaUser.text = comment.user;
                            comment3D.GetComponent<commentContents>().commentMetaDate.text = comment.date;
                            comment3D.GetComponent<commentContents>().filepath = System.IO.Path.Combine(Application.persistentDataPath, comment.path);
                            comment3D.GetComponent<commentContents>().loadPhoto();
                        }
                        else if (comment.type == 3 && spawnedNode.GetComponent<nodeController>().linkedField != null)
                        {
                            GameObject comment3D = spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().spawnVideoCommentFromJSON(comment.path);

                            comment3D.GetComponent<commentContents>().Date = comment.date;
                            comment3D.GetComponent<commentContents>().user = comment.user;
                            comment3D.GetComponent<commentContents>().commentMetaUser.text = comment.user;
                            comment3D.GetComponent<commentContents>().commentMetaDate.text = comment.date;
                            comment3D.GetComponent<commentContents>().LoadVideo();
                        }
                    }

                }

            }

        }
        #endregion
        //apply this to node type violation
        #region
        else if (nodeClass.type == 3)// type = 3 violation
        {

            violatoinSpawner.Instance.spawnViolationFromJSON(spawnedNode);
            nodeSpawner.Instance.spawnMiniNode(spawnedNode, 3);

            List<databaseMan.tempComment> tempList = new List<databaseMan.tempComment>();
            List<databaseMan.tempComment> spawnList = new List<databaseMan.tempComment>();
            foreach (JU_databaseMan.comment commentJU in nodeClass.comments)
            {
                databaseMan.tempComment newItem = new databaseMan.tempComment();
                newItem.date = commentJU.date;
                newItem.type = 1;
                newItem.user = commentJU.user;
                newItem.content = commentJU.content;
                tempList.Add(newItem);

            }
            foreach (JU_databaseMan.media photoJU in nodeClass.photos)
            {
                databaseMan.tempComment newItem = new databaseMan.tempComment();
                newItem.date = photoJU.date;
                newItem.type = 2;
                newItem.path = photoJU.path;
                tempList.Add(newItem);

            }
            foreach (JU_databaseMan.media videoJU in nodeClass.videos)
            {
                databaseMan.tempComment newItem = new databaseMan.tempComment();
                newItem.date = videoJU.date;
                newItem.type = 3;
                newItem.path = videoJU.path;
                tempList.Add(newItem);

            }

            spawnList = tempList.OrderBy(si => si.date).ToList();

            foreach (databaseMan.tempComment comment in spawnList.Reverse<databaseMan.tempComment>())
            {

                if (comment.type == 1 && spawnedNode.GetComponent<nodeController>().linkedField != null)
                {
                    GameObject comment3D = spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().spawnSimpleCommentFromJSON();

                    comment3D.GetComponent<commentContents>().Date = comment.date;
                    comment3D.GetComponent<commentContents>().user = comment.user;
                    comment3D.GetComponent<commentContents>().commentMetaUser.text = comment.user;
                    comment3D.GetComponent<commentContents>().commentMetaDate.text = comment.date;
                    comment3D.GetComponent<commentContents>().commentMain.text = comment.content;
                }
                else if (comment.type == 2 && spawnedNode.GetComponent<nodeController>().linkedField != null)
                {
                    GameObject comment3D = spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().spawnPhotoCommentFromJSON();


                    comment3D.GetComponent<commentContents>().Date = comment.date;
                    comment3D.GetComponent<commentContents>().user = comment.user;
                    comment3D.GetComponent<commentContents>().commentMetaUser.text = comment.user;
                    comment3D.GetComponent<commentContents>().commentMetaDate.text = comment.date;
                    comment3D.GetComponent<commentContents>().filepath = System.IO.Path.Combine(Application.persistentDataPath, comment.path);
                    comment3D.GetComponent<commentContents>().fileName = comment.path;
                    comment3D.GetComponent<commentContents>().loadPhoto();
                }
                else if (comment.type == 3 && spawnedNode.GetComponent<nodeController>().linkedField != null)
                {
                    GameObject comment3D = spawnedNode.GetComponent<nodeController>().linkedField.GetComponent<commentManager>().spawnVideoCommentFromJSON(comment.path);

                    comment3D.GetComponent<commentContents>().Date = comment.date;
                    comment3D.GetComponent<commentContents>().user = comment.user;
                    comment3D.GetComponent<commentContents>().commentMetaUser.text = comment.user;
                    comment3D.GetComponent<commentContents>().commentMetaDate.text = comment.date;
                    comment3D.GetComponent<commentContents>().LoadVideo();
                }
            }

        }
        #endregion
        //apply this for annotation types
        else
        {

            spawnedNode.GetComponent<nodeMediaHolder>().Title.text = nodeClass.title;
            spawnedNode.GetComponent<nodeMediaHolder>().Description.text = nodeClass.description;
            foreach (JU_databaseMan.comment commentJU in nodeClass.comments)
            {
                GameObject comment3D = spawnedNode.GetComponent<commentManager>().spawnSimpleCommentFromJSON();

                comment3D.GetComponent<commentContents>().Date = commentJU.date;
                comment3D.GetComponent<commentContents>().user = commentJU.user;
                comment3D.GetComponent<commentContents>().commentMetaUser.text = commentJU.user;
                comment3D.GetComponent<commentContents>().commentMetaDate.text = commentJU.date;
                comment3D.GetComponent<commentContents>().commentMain.text = commentJU.content;
            }

            if (nodeClass.type == 0)//simple type=0
            {
                nodeSpawner.Instance.spawnMiniNode(spawnedNode, 0);
            }
            else if (nodeClass.type == 1)//photo type=1
            {

                nodeSpawner.Instance.spawnMiniNode(spawnedNode, 1);
                spawnedNode.GetComponent<nodeMediaHolder>().activeFilepath = nodeClass.photos[0].path;
                spawnedNode.GetComponent<nodeMediaHolder>().loadPhoto(System.IO.Path.Combine(Application.persistentDataPath, nodeClass.photos[0].path));
            }
            else if (nodeClass.type == 4)//video type=4
            {

                nodeSpawner.Instance.spawnMiniNode(spawnedNode, 2);
                spawnedNode.GetComponent<nodeMediaHolder>().activeFilepath = nodeClass.videos[0].path;
                spawnedNode.GetComponent<nodeMediaHolder>().LoadVideo();
            }
        }

        //this applies to all 
        spawnedNode.GetComponent<nodeController>().setUpNode();
        spawnedNode.GetComponent<nodeController>().closeNode();
        spawnedNode.GetComponent<BoxCollider>().enabled = true;

    }

    ///<summary>
    ///spawn nodes over loop
    ///</summary>
    public void spawnNodeList()
    {
        foreach (JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
        { 
        spawnNode(node);
        }
    }

    ///<summary>
    ///string split for short names of file directories
    ///</summary>
    public virtual string shortNameProcessor(string inString)
        {
        string[] result;
        result = inString.Split('/');
        return result[result.Length-1];
        }

    ///<summary>
    ///for offsite experience to show host information
    ///</summary>
    public void changeTextTest()
    {
        changedText.text = ("Host : " + JU_databaseMan.Instance.nodesManager.nodes[0].user);
    }

    ///<summary>
    ///spawn a single node offsite by feeding the argument nodeClass, this is a more simplified version
    ///because on the offsite version the nodes are not clickable to open
    ///</summary>
    public void spawnNodeOffsite(JU_databaseMan.nodeItem nodeClass)
    {
        Vector3 pos = new Vector3(nodeClass.transform[0], nodeClass.transform[1], nodeClass.transform[2]);
        Quaternion rot = new Quaternion(nodeClass.transform[3], nodeClass.transform[4], nodeClass.transform[5], nodeClass.transform[6]);
        Vector3 sca = new Vector3(nodeClass.transform[7], nodeClass.transform[8], nodeClass.transform[9]);

        GameObject spawnedNode = Instantiate(nodeprefabs[nodeClass.type], Vector3.zero, new Quaternion(0,0,0,0));
        spawnedNode.transform.SetParent(offSiteHolder.transform);
        spawnedNode.transform.localPosition = pos;
        spawnedNode.transform.localRotation = rot;
        spawnedNode.transform.localScale = sca*4;
        spawnedNode.name = nodeClass.title;
        offsiteJSonLoader.Instance.nodes3DList.Add(nodeClass.indexNum, spawnedNode);

        if (nodeClass.indexNum == 4)
        {
            GameObject connectObj = GameObject.Find("guidedTargetDemo");
            connectObj.GetComponent<cameraZoomOverTime>().targetObject = spawnedNode;
        }
    }

    ///<summary>
    ///spawn nodes over loop offsite
    ///</summary>
    public void spawnNodeOffsiteList()
    {
        foreach (JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
        {
            spawnNodeOffsite(node);
        }
    }
}
