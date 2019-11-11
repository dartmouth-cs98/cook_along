using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class Timer : MonoBehaviour
{

    public int Time; //Seconds Overall
    public Text Timer_disp; //UI Text Object

    // A constructor that takes parameter to set the properties
    public Timer(int given_time)
    {
        Time = given_time;
    }

    void Start()
    {
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }
    void Update()
    {
        Timer_disp.text = ("" + Time); //Showing the Score on the Canvas
    }
    //Simple Coroutine
    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Time--;
        }
    }

}


