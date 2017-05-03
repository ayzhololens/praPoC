using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class formNodeController : MonoBehaviour {
    public GameObject masterForm;
    public GameObject linkedField;
    public Transform contentHolder;
    public Transform contentLoc;
    float camDistance;
    float contentDistance;
    public float distanceThreshold;
    public float speed;
    bool contentOpen;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        //camDistance = Vector3.Distance(transform.position, ActorSingleton.Actor.transform.position);
        //if (contentOpen)
        //{
        //    if (camDistance > distanceThreshold)
        //    {
        //        contentDistance = Vector3.Distance(masterForm.GetComponent<formController>().contentHolder.transform.position, ActorSingleton.Actor.transform.position);
        //        if (contentDistance > 2 && masterForm.GetComponent<formController>().contentHolder.GetComponent<SimpleTagalong>().enabled != true)
        //        {
        //            masterForm.GetComponent<formController>().contentHolder.transform.position = Vector3.MoveTowards(masterForm.GetComponent<formController>().contentHolder.transform.position, ActorSingleton.Actor.transform.position, speed / 1.5f);
        //        }

        //        if (contentDistance < 2 && masterForm.GetComponent<formController>().contentHolder.GetComponent<SimpleTagalong>().enabled != true)
        //        {
        //            masterForm.GetComponent<formController>().contentHolder.GetComponent<SimpleTagalong>().enabled = true;
        //        }



        //    }

        //    if (camDistance < distanceThreshold)
        //    {
        //        if (masterForm.GetComponent<formController>().contentHolder.GetComponent<SimpleTagalong>().enabled == true)
        //        {
        //            masterForm.GetComponent<formController>().contentHolder.GetComponent<SimpleTagalong>().enabled = false;
        //            masterForm.GetComponent<formController>().contentHolder.GetComponent<Interpolator>().enabled = false;
        //        }

        //        masterForm.GetComponent<formController>().contentHolder.transform.position = Vector3.MoveTowards(contentHolder.transform.position, contentLoc.position, speed);


        //    }

        //}






    }



    public void openForm()
    {
        if (masterForm == null)
        {
            masterForm = fieldSpawner.Instance.MasterForm;
        }
        if (linkedField != null)
        {
            linkedField.GetComponent<formFieldController>().linkedNode = this.gameObject;
        }
        //masterForm.SetActive(true);
        contentOpen = true;
        masterForm.GetComponent<formController>().openForm();
        //masterForm.GetComponent<formController>().contentHolder.transform.position = contentLoc.position;
        //masterForm.transform.position = contentLoc.position;
        //for (int i = 0; i < linkedField.transform.parent.childCount; i++)
        //{
        //    if (linkedField.transform.parent.GetChild(i).gameObject != linkedField && linkedField.transform.parent.GetChild(i).gameObject.GetComponent<subMenu>()!=null)
        //    {

        //        linkedField.transform.parent.GetChild(i).gameObject.GetComponent<subMenu>().turnOffCounter();
        //        linkedField.transform.parent.GetChild(i).gameObject.GetComponent<formFieldController>().attachmentParent.gameObject.SetActive(false);
        //    }
        //}
        //linkedField.GetComponent<formFieldController>().attachmentParent.gameObject.SetActive(true);
    }




}
