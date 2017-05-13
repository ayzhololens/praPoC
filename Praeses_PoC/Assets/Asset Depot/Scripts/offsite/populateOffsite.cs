using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine.UI;

public class populateOffsite : MonoBehaviour {

    public List<GameObject> cameras;
    public GameObject spatialMap;

    //overview
    public Text vioNum;
    public Text anoNum;
    public Text fieNum;

    // Use this for initialization
    void Start () {
        Invoke("loadCommand", .2f);
        Invoke("populateForm", .5f);
        loadSpatialMesh();

        //foreach (GameObject cam in cameras)
        //{
        //    cam.GetComponent<CameraControlOffsite>().focus(0);
        //}
    }

    void loadCommand()
    {
        databaseMan.Instance.loadDefCmd();
    }

    void populateForm()
    {
        offsiteJSonLoader.Instance.populateEquipment();
        offsiteJSonLoader.Instance.equipmentCollapse.toggleBox();
        violationsParentSpawner.Instance.populateViolations();
        fieldCollapseableBox.Instance.populateFieldDeltas();
        //offsiteJSonLoader.Instance.populateNodes();
        addNodeFromJSon.Instance.spawnNodeOffsiteList();
        populateSummary();
    }

    void loadSpatialMesh()
    {
        spatialMap.GetComponent<boilerTransformRecorder>().importData();
        spatialMap.GetComponent<SpatialMappingObserver>().LoadSpatialMeshes("JU_spatialMesh");
    }

    public void populateSummary()
    {
        int violationsNum = 0;
        int annotationsNum = 0;
        int fieldsNum = 0;
        foreach (JU_databaseMan.nodeItem node in JU_databaseMan.Instance.nodesManager.nodes)
        {
            if(node.type == 3)
            {
                violationsNum++;
            }else if (node.type == 2)
            {
                fieldsNum++;
            }
            else
            {
                annotationsNum++;
            }
        }
        vioNum.text = violationsNum.ToString();
        anoNum.text = annotationsNum.ToString();
        fieNum.text = fieldsNum.ToString();
    }

}
