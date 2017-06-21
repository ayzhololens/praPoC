using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickMotion : MonoBehaviour
{
    Vector3 startPos;
    bool movingF;
    bool movingB;

    [Tooltip("Speed")]
    public float moveSpeed;
    [Tooltip("How far it moves before moving back")]
    public float moveDist;


    // Use this for initialization
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //click moving inwards, when it hit it's distance, the bool will switch off and trigger the moving outwards motion
        if (movingF)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveSpeed);
            if(transform.localPosition.z >= startPos.z + moveDist)
            {
                movingF = false;
                movingB = true;
            }
        }


        //click moving outwards, when it hits the initial position, the bool will switch off and send a message to trigger the 
        //finishClick() function on the selectEvent
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

    //start moving in Update
    public void click()
    {
        movingF = true;
    }


}
