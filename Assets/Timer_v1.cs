using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_v1: MonoBehaviour
{
    public static float timer = 120f; //2 minutes
    //public static bool timeStarted = false;

    void Update()
    {
        //timer is the 
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            TimerOver();
        }
        
    }

    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), niceTime);
    }

    void TimerOver()
    {
        //filler to do something when the timer is over
        return;
    }

}
