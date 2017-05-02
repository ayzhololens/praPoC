using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewViolationController : MonoBehaviour {
    public GameObject contentHolder;
    public List<GameObject> vioFields;
    public List<GameObject> vioResolvedFields;
    public Transform fieldParent;
    public Transform activePos;
    public Transform resolvedPos;
    public float offsetDist;
    public GameObject resolved;
    public Transform frontHolder;

	// Use this for initialization
	void Start () {
        frontHolder = ActorSingleton.Actor.transform.GetChild(0);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void repositionFields()
    {
        for(int i=0; i<vioFields.Count; i++)
        {
            Vector3 vioPos = vioFields[i].transform.localPosition;
            vioFields[i].transform.localPosition = new Vector3(vioPos.x, vioPos.y - offsetDist, vioPos.z);
        }
        resolvedPos.localPosition = new Vector3(resolvedPos.localPosition.x, resolvedPos.localPosition.y - offsetDist, resolvedPos.localPosition.z);
        resolved.transform.localPosition = new Vector3(resolved.transform.localPosition.x, resolved.transform.localPosition.y - offsetDist, resolved.transform.localPosition.z);
        repositionResolvedField();
    }

    public void repositionResolvedField()
    {
        for (int i = 0; i < vioResolvedFields.Count; i++)
        {
            Vector3 vioPos = vioResolvedFields[i].transform.localPosition;
            vioResolvedFields[i].transform.localPosition = new Vector3(vioPos.x, vioPos.y - offsetDist, vioPos.z);
        }
    }

    public void reorderFields(GameObject movedField)
    {
        int index = 0;
        for (int i = 0; i <vioFields.Count; i++)
        {
            if(vioFields[i] = movedField)
            {
                //vioFields.RemoveAt(i);
                index = i;
            }
        }

        Debug.Log(index);
        for (int i = (index-1); i>-1; i--)
        {
            Debug.Log("hey " + i);
            Vector3 vioPos = vioFields[i].transform.localPosition;
            Debug.Log(vioPos);
            vioFields[i].transform.localPosition = new Vector3(vioPos.x, vioPos.y + offsetDist, vioPos.z);
            Debug.Log(vioFields[i].transform.localPosition);


            //if (vioFields[i] = movedField)
            //{
            //    vioFields.RemoveAt(i);
            //}
        }
        resolvedPos.localPosition = new Vector3(resolvedPos.localPosition.x, resolvedPos.localPosition.y + offsetDist, resolvedPos.localPosition.z);
        resolved.transform.localPosition = new Vector3(resolved.transform.localPosition.x, resolved.transform.localPosition.y + offsetDist, resolved.transform.localPosition.z);
        vioFields.RemoveAt(index);
        //repositionResolvedField();
    }

    public void closeViewer()
    {
        contentHolder.SetActive(false);
    }

    public void openViewer()
    {
        contentHolder.transform.position = frontHolder.position;
        contentHolder.SetActive(true);
    }
}
