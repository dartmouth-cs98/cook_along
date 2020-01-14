using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Timer1 : MonoBehaviour
{
    public int timeLeft; //Seconds Overall
    public Text countdown; //UI Text Object

    void Start()
    {
        timeLeft = 200;
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }

    void Update()
    {
        int hours = Mathf.FloorToInt(timeLeft / 3600F);
        int minutes = Mathf.FloorToInt((timeLeft - (hours*3600)) / 60F);
        int seconds = Mathf.FloorToInt(timeLeft - (hours * 3600) - (minutes * 60));
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        countdown.text = ("" + niceTime); //Showing the Score on the Canvas
    }

    public IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}