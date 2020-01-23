using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class StepCanvas : MonoBehaviour
{
	public List<string> URLs = new List<string>();
    public GameObject canvas;
    public int yCoord;
    public int xCoord;
    // Start is called before the first frame update
    //Timer work below under contruction
    public float timeLeft; //Seconds Overall
    public Text countdown; //UI Text Object
    public int hours;
    public int minutes;
    public int seconds;
    public String niceTime;
 
    void Start()
    {
        yCoord=-20;
        xCoord= 210;
        int height=50;
        int width=50;
        URLs.Add("https://food.fnr.sndimg.com/content/dam/images/food/fullset/2012/2/24/0/ZB0202H_classic-american-grilled-cheese_s4x3.jpg.rend.hgtvcom.616.462.suffix/1371603614279.jpeg");
        URLs.Add("https://i0.wp.com/cdn-prod.medicalnewstoday.com/content/images/articles/299/299147/cheese-varieties.jpg?w=1155&h=1537");

        foreach (string currentURL in URLs)
        {

            canvas = GameObject.Find("Canvas");
            GameObject NewObj = new GameObject(); //Create the GameObject
            RawImage NewImage = NewObj.AddComponent<RawImage>(); //Add the Image Component script
            NewImage.transform.SetParent(canvas.transform, false);
            NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(xCoord, yCoord, 0);
            NewObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            yCoord = yCoord - 50;
            NewObj.SetActive(true); //Activate the GameObject
            StartCoroutine(GetTexture(currentURL, NewObj));
        }

        countdown = GameObject.Find("Timer").GetComponent<Text>();
        timeLeft = 120;

    }
    
    void Update()
    {
        Debug.Log("In Update");
        timeLeft = timeLeft - Time.deltaTime;
        Debug.Log(timeLeft);
    
        //yield return ("it works"); new WaitForSeconds(1.0f);
        hours = Mathf.FloorToInt(timeLeft / 3600F);
        minutes = Mathf.FloorToInt((timeLeft - (hours*3600)) / 60F);
        seconds = Mathf.FloorToInt(timeLeft - (hours * 3600) - (minutes * 60));
        niceTime = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        countdown.text = ("" + niceTime); //Showing the Score on the Canvas
        
    }

    IEnumerator GetTexture(string thisURL, GameObject currrentImage) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(thisURL);
    	yield return www.SendWebRequest();

        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            currrentImage.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(www);
        }

    }


}
