
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine;

using HoloLensXboxController;

public class xboxControllerInput : MonoBehaviour {

    [Header("Sensitivity Settings")]
    public float moveSensitivity;
    public float rotateSensitivity;
    public float ratchetSensitivity;


    [Header(" ")]
    [Tooltip("Admin Menu Content Holder")]
    public GameObject adminMenuContent;
    private ControllerInput controllerInput;

    [Tooltip("Disable if not using a controller to avoid null reference error")]
    public bool useController;
    
    void Start ()
    {
        if (useController)
        {
            //set your controller input
            controllerInput = new ControllerInput(0, 0.19f);
        }

    }
	
	// Update is called once per frame
	void Update () {

        //only check for controller input if the admin menu is open
        if (useController && adminMenuContent.activeSelf)
        {
            controllerInput.Update();

            //rotation
            if (controllerInput.GetAxisRightTrigger() != 0)
            {
                transform.Rotate(Vector3.up, (100 * rotateSensitivity) * Time.deltaTime);
            }

            if (controllerInput.GetAxisLeftTrigger() != 0)
            {

                transform.Rotate(Vector3.down, (100 * rotateSensitivity) * Time.deltaTime);
            }


            //position
            if (controllerInput.GetAxisLeftThumbstickY() > 0)
            {
                transform.position = transform.position + (transform.forward * (moveSensitivity * controllerInput.GetAxisLeftThumbstickY()));
            }
            if (controllerInput.GetAxisLeftThumbstickY() < 0)
            {
                transform.position = transform.position + (transform.forward * (moveSensitivity * controllerInput.GetAxisLeftThumbstickY()));
            }
            if (controllerInput.GetAxisLeftThumbstickX() > 0)
            {
                transform.position = transform.position + (transform.right * (moveSensitivity * controllerInput.GetAxisLeftThumbstickX()));
            }
            if (controllerInput.GetAxisLeftThumbstickX() < 0)
            {
                transform.position = transform.position + (transform.right * (moveSensitivity * controllerInput.GetAxisLeftThumbstickX()));
            }


            //position vertical
            if (controllerInput.GetButtonDown(ControllerButton.LeftThumbstick))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + ratchetSensitivity, transform.position.z);
            }


            if (controllerInput.GetButtonDown(ControllerButton.RightThumbstick))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - ratchetSensitivity, transform.position.z);
            }



            //admin menu control
            if (controllerInput.GetButtonDown(ControllerButton.A))
            {
                toggleMenu();

            }
        }


    }

    public void toggleMenu()
    {
        adminMenuContent.SetActive(!adminMenuContent.activeSelf);
    }




}
