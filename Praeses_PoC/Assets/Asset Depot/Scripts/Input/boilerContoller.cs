
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine;

using HoloLensXboxController;

public class boilerContoller : MonoBehaviour {


    public float moveSensitivity;
    public float rotateSensitivity;
    public float ratchetSensitivity;
    public GameObject[] popUpBoiler;
    public GameObject normalBoiler;
    public GameObject mainMenuContent;
    private ControllerInput controllerInput;

    // Use this for initialization
    void Start ()
    {
        controllerInput = new ControllerInput(0, 0.19f);

    }
	
	// Update is called once per frame
	void Update () {

        controllerInput.Update();
        if (controllerInput.GetAxisRightTrigger() != 0 )
        {
            transform.Rotate(Vector3.up, (100*rotateSensitivity) * Time.deltaTime);
        }
        
        if (controllerInput.GetAxisLeftTrigger() != 0)
        {

            transform.Rotate(Vector3.down, (100 * rotateSensitivity) * Time.deltaTime);
        }

        if (controllerInput.GetAxisLeftThumbstickY() > 0)
        {
            transform.position = transform.position + (transform.forward* (moveSensitivity * controllerInput.GetAxisLeftThumbstickY()));
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

        if (controllerInput.GetButtonDown(ControllerButton.LeftThumbstick))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + ratchetSensitivity, transform.position.z);
        }


        if (controllerInput.GetButtonDown(ControllerButton.RightThumbstick))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - ratchetSensitivity, transform.position.z);
        }

        if (controllerInput.GetButtonDown(ControllerButton.A))
        {
            toggleMenu();
                
        }

    }

    public void toggleMenu()
    {
        mainMenuContent.SetActive(!mainMenuContent.activeSelf);
    }

    public void switchBoiler()
    {
        if (popUpBoiler[0].activeSelf)
        {


            normalBoiler.SetActive(true);
            for (int i = 0; i < popUpBoiler.Length; i++)
            {

                popUpBoiler[i].SetActive(false);

                if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, "JO_JJ_values.json")))
                {
                    databaseMan.Instance.valuesDir = Path.Combine(Application.persistentDataPath, "JO_JJ_values.json");
                }


            }
        }
        else if (normalBoiler.activeSelf)
        {
            normalBoiler.SetActive(false);
            for (int i = 0; i < popUpBoiler.Length; i++)
            {

                popUpBoiler[i].SetActive(true);

                if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, "JO_JJ_valuesPopUp.json")))
                {
                   databaseMan.Instance.valuesDir = Path.Combine(Application.persistentDataPath, "JO_JJ_valuesPopUp.json");
                }
            }
        }
    }
}
