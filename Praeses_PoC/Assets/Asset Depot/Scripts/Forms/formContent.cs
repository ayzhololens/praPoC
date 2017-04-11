﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;


namespace HoloToolkit.Unity
{
    public class formContent : MonoBehaviour {


        [Header("Location Details")]
        public Text Address;
        public Text CityState;
        public Text ZIP;
        public Text LocationName;
        public Text NBNumber;
        [Header("Object Details")]
        public Text Object;
        public Text DueDate;
        public Text ObjectLocation;
        public Text NextInspection;
        public Text JONumber;
        public Text SerialNumber;
        public Text YearBuilt;
        public Text MAWP;
        public Text Size;
        public Text Status;
        public Text CertType;
        [Header("Inspection Overview")]
        public Text Inspector;
        public Text CurDate;
        public Text StartTime;
        public Text Violations;
        public Text Annotations;
        public Text DataFields;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void loadDetails()
        {

            JU_databaseMan database = JU_databaseMan.Instance;

            //LOCATION DETAILS
            #region
            if (Address != null)
            {
                Address.text = database.definitions.LocationFields.address1;
            }
            if (NBNumber != null)
            {
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.currentData)
                {
                    if (valueItem.name == "strNBNumber")
                    {
                        NBNumber.text = valueItem.value + " ";
                    }
                }
            }
            if (CityState != null)
            {
                CityState.text = database.definitions.LocationFields.City + ", " + database.definitions.LocationFields.State;
            }
            if (ZIP != null)
            {
                ZIP.text = database.definitions.LocationFields.Zip;

            }
            if (LocationName != null)
            {
                LocationName.text = database.definitions.LocationFields.LocationName;

            }
            #endregion

            //INSPECTION OVERVIEW
            #region 
            if (Inspector != null)
            {
                Inspector.text = metaManager.Instance.user;
            }
            if (CurDate != null)
            {
                System.DateTime dateOffset = System.DateTime.Now;
                CurDate.text = dateOffset.ToString("MM/dd/yyyy");
            }
            if (StartTime != null)
            {
                System.DateTime dateOffset = System.DateTime.Now;
                StartTime.text = dateOffset.ToString("hh:mm");
            }
            if (CertType != null)
            {

                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.historicData)
                {
                    if (valueItem.name == "intActivityTypeID")
                    {
                        foreach (JU_databaseMan.fieldItem fieldItem in database.definitions.InspectionFields.fields)
                        {
                            if (fieldItem.Name == "intActivityTypeID")
                            {
                                CertType.text = fieldItem.Options[valueItem.value];
                            }
                        }
                    }
                }
                
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.currentData)
                {
                    if (valueItem.name == "dtCertExpire")
                    {
                        CertType.text = CertType.text + " - Exp: " + valueItem.value;
                    }
                }

            }

            #endregion

            //OBJECT DETAILS
            #region
            if (DueDate != null)
            {
                System.DateTime dateOffset = System.DateTime.Now.AddDays(180);

                DueDate.text = dateOffset.ToString("MM/dd/yyyy");
            }

            if (MAWP != null)
            {
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.currentData)
                {
                    if (valueItem.name == "intStampedMAWP")
                    {
                        MAWP.text = valueItem.value;
                    }
                }
            }

            if (SerialNumber != null)
            {
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.currentData)
                {
                    if (valueItem.name == "strOtherNumber")
                    {
                        SerialNumber.text = valueItem.value;
                    }
                }
            }
            if (JONumber != null)
            {
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.currentData)
                {
                    if (valueItem.name == "strJurisNumber")
                    {
                        JONumber.text = valueItem.value;
                    }
                }
            }

            if (Size != null)
            {
                foreach (JU_databaseMan.valueItem valueItem in JU_databaseMan.Instance.values.currentData)
                {
                    if (valueItem.name == "fltSV1SizeIn")
                    {
                        Size.text = valueItem.value;
                    }
                }
            }
            #endregion


        }
    }
}