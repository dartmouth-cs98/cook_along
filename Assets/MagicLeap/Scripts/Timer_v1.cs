using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerExample : MonoBehaviour
{
    string s;

    void Update()
    {
        s = System.TimeSpan.FromSeconds((int)Time.timeSinceLevelLoad).ToString();
    }

    void OnGUI()
    {
        GUILayout.Label(s);
    }
}
