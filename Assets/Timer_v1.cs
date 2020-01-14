using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Networking;

public class Timer_v1: MonoBehaviour
{
    
    public int timeLeft = 60; //Seconds Overall
    public Text countdown; //UI Text Object
    public GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        StartCoroutine(LoseTime());
        Time.timeScale = 1; //Just making sure incrementation is standard to 1s
    }

    void Update()
    {
        int hours = Mathf.FloorToInt(timeLeft / 3600F);
        int minutes = Mathf.FloorToInt((timeLeft - (hours*3600)) / 60F);
        int seconds = Mathf.FloorToInt(timeLeft - (hours * 3600) - (minutes * 60));
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        countdown.text = ("" + niceTime); //Showing the Score on the Canvas
    }
    

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}
