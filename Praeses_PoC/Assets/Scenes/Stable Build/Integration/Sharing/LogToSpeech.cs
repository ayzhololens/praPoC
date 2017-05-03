using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogToSpeech : MonoBehaviour
{
#if !UNITY_EDITOR
    private TextToSpeechManager textToSpeech;

    private Queue<string> logs = new Queue<string>();

    void Start()
    {
        textToSpeech = GetComponent<TextToSpeechManager>();
    }

    void Update()
    {
        StartCoroutine(ReadLogs());
    }


    IEnumerator ReadLogs()
    {
        if (logs.Count > 0 && textToSpeech.IsSpeaking() == false)
        {
            string text = logs.Dequeue();
            textToSpeech.SpeakText(text);
        }

        yield return null;
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            logs.Enqueue(logString);
        }
    }
#endif
}