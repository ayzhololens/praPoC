﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offsiteFieldItemValueHolder : MonoBehaviour {
    public Text name;
    public InputField value;
    public InputField oldValue;
    public InputField displayName;
    public Text meta;
    public Text content;
    public string user;
    public string date;
    public string path;
    public int nodeIndex;
    public GameObject thumbnail;

    public void onEditChangeUpdateJSon()
    {
        databaseMan.Instance.formToClassValueSync(gameObject.name, value.text);
    }

}
