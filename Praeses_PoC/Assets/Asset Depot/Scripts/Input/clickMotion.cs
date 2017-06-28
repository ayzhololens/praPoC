using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickMotion : MonoBehaviour
{
    Vector3 startPos;
    bool movingF;
    bool movingB;
    public float moveSpeed;
    public float moveDist;


    // Use this for initialization
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //click moving inwards, when it hit it's distance the bool will switch off and trigger moving outwards
        if (movingF)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveSpeed);
            if(transform.localPosition.z >= startPos.z + moveDist)
            {
                movingF = false;
                movingB = true;
            }
        }

        //when it reaches the original position send a message to the onselect event
        if (movingB)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveSpeed);
            if (transform.localPosition.z <= startPos.z)
            {
                transform.localPosition = startPos;
                movingF = false;
                movingB = false;

                gameObject.SendMessage("finishClick", SendMessageOptions.DontRequireReceiver);
            }
        }

    }

    public void click()
    {
        movingF = true;
    }


}
