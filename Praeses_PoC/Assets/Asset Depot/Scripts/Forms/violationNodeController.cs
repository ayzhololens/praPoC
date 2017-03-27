﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HoloToolkit.Unity
{

    public class violationNodeController : MonoBehaviour
    {

        public GameObject violationPrefab;
        public GameObject linkedViolation;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void spawnViolation()
        {

            linkedViolation = Instantiate(violationPrefab, transform.position, Quaternion.identity);
            linkedViolation.GetComponent<violationController>().linkedNode = this.gameObject;
            violatoinSpawner.Instance.activeViolationController = linkedViolation.GetComponent<violationController>();
            violatoinSpawner.Instance.populateCategories();
        }



        public void openViolation()
        {
            linkedViolation.GetComponent<violationController>().openViolation();

        }


    }
}