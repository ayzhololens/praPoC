using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine.UI;

public class populateOffsite : MonoBehaviour {

    public List<GameObject> cameras;
    public GameObject spatialMap;

    [Header("Overview")]
    public Text vioNum;
    public Text anoNum;
    public Text fieNum;
    //this differentates offsite versus indeviceoffsie
    public bool inDevice;

    // Use this for initialization
    void Start () {
        //we are putting delays because it can't load at the same time it's trying to unpack the JSON files
        Invoke("loadCommand", .2f);
        Invoke("populateForm", .5f);
        Invoke("populateVio", .5f);
        boilerType();

        if (inDevice) { }
        else
        {
            loadSpatialMesh();
        }

        //this is if we want to expand further on individual camera view at runtime focusing on specific objects
        //foreach (GameObject cam in cameras)
        //{
        //    cam.GetComponent<CameraControlOffsite>().focus(0);
        //}
    }

    void loadCommand()
    {
        databaseMan.Instance.loadDefCmd();
        databaseMan.Instance.loadValCmd();
    }

    public void populateForm()
    {
        offsiteJSonLoader.Instance.populateAddress();
        offsiteJSonLoader.Instance.populateEquipment();
        offsiteJSonLoader.Instance.equipmentCollapse.toggleBox();
        addNodeFromJSon.Instance.spawnNodeOffsiteList();
    }

    public void populateVio()
    {
        violationsParentSpawner.Instance.populateViolations();
        fieldCollapseableBox.Instance.populateFieldDeltas();
        annotationsCollapseableBox.Instance.populateNodes();

        populateSummary();
    }

    public void loadSpatialMesh()
    {
        spatialMap.GetComponent<boilerTransformRecorder>().importData();
        spatialMap.GetComponent<SpatialMappingObserver>().LoadSpatialMeshes("JU_spatialMesh");
    }

    public void boilerType()
    {
        spatialMap.GetComponent<boilerTransformRecorder>().boilerType();
    }

    void populateSummary()
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
