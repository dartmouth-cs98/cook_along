using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Networking;


public class Timer : MonoBehaviour
{
    public int timeLeft = 60; //Seconds Overall
    public Text countdown; //UI Text Object
    public GameObject canvas;
    public string timeUrl;
    
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        GameObject NewTime = new GameObject(); //Create the GameObject
        Text thisTime = NewTime.AddComponent<Text>(); //Add the Image Component script
        NewTime.GetComponent<RectTransform>().anchoredPosition = new Vector3(250,200,0);
        NewTime.SetActive(true); //Activate the GameObject
        StartCoroutine(LoseTime(timeUrl, NewTime));
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

    IEnumerator LoseTime(string timeURL, GameObject currrentTime)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(timeUrl);
        yield return www.SendWebRequest();

        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
           // currrentTime.GetComponent<Text> = DownloadHandlerTexture.GetContent(www);
        }
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
    
    
        
    
}