using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggers : MonoBehaviour {

    public GameObject maleAvatar;
    bool smile;
    public float time;

    private void Start()
    {

    }

    private void Update()
    {
        maleAvatar.GetComponent<LoomDeformerLoomie_male>().targetPosition = Camera.main.transform.position;
    }

    private void OnMouseDown()
    {
        makeSmileToggle();
    }

    public void makeSmileToggle()
    {
        if (!smile)
        {
            StartCoroutine(smoothSmile(Vector3.zero, Vector3.one, time));
            smile = true;
        }else
        {
            StartCoroutine(smoothSmile(Vector3.one, Vector3.zero, time));
            smile = false;
        }
    }

    public IEnumerator smoothSmile(Vector3 start, Vector3 end, float seconds)
    {
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            Vector3 tempV = Vector3.Lerp(start, end, Mathf.SmoothStep(0.0f, 1, t));
            maleAvatar.GetComponent<LoomDeformerLoomie_male>().Smile = tempV.x;

            yield return true;
        }
    }
}
