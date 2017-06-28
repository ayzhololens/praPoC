using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subMenu : MonoBehaviour {

    [Tooltip("SubObjects to reveal")]
    public GameObject[] subButtons;
    [Tooltip("Objects to hide when the subButtons are active")]
    public GameObject[] hide;

    //subbutton state
    public bool subButtonsOn { get; set; }

    [Tooltip("How long until the subButtons turn off")]
    public float timeOutCounter;


    //Turn on subButtons and turn of hidden objects
    public void turnOnSubButtons()
    {
        for(int i=0; i<subButtons.Length; i++)
        {
            subButtons[i].SetActive(true);
        }

        subButtonsOn = true;

        if (hide.Length > 0)
        {
            for(int i =0; i<hide.Length; i++) { hide[i].SetActive(false); }
        }
    }

    //Turn off subButtons after a set time
    public void turnOffCounter()
    {
        subButtonsOn = false;
        Invoke("turnOffSubButtons", timeOutCounter);
    }

    //Cancel that turn off
    public void keepOn()
    {
        subButtonsOn = true;
        CancelInvoke("turnOffSubButtons");
    }


    //Revert objects and turn subButtons off
    public void turnOffSubButtons()
    {
        if (!subButtonsOn)
        {
            if (gameObject.GetComponent<popForward>() != null)
            {
                gameObject.GetComponent<popForward>().moveBackward();
            }
            if (gameObject.GetComponent<buttonHightlight>() != null)
            {
                gameObject.GetComponent<buttonHightlight>().unHighlight();
            }
            for (int i = 0; i < subButtons.Length; i++)
            {
                subButtons[i].SetActive(false);
            }



            if (hide.Length > 0)
            {
                for (int i = 0; i < hide.Length; i++) { hide[i].SetActive(true); }
            }
            
        }


    }
}
