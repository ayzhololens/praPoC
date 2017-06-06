using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class copyFiles : MonoBehaviour {

    [Tooltip("Names of files in streaming assets to copy to device.  Include extensions (.jpg, .json, etc)")]
    public string[] copiedFiles;

    // Use this for initialization
    void Start()
    {
        copy();
    }

    
    public void copy()
    {
        for (int i =0; i<copiedFiles.Length; i++)
        {
            string tempPath = Path.Combine(Application.streamingAssetsPath, copiedFiles[i]);
            string newPath = Path.Combine(Application.persistentDataPath, copiedFiles[i]);

            if (!File.Exists(newPath))
            {
                File.Copy(tempPath, newPath);
            }
            else
            {
                Debug.Log("File Already exists:  " + copiedFiles[i]);
            }
            
        }

    }
}
