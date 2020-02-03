using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;
using System.IO;


public class StepCanvas : MonoBehaviour
{

    //Setting up Variables used for video
	public int yCoord;
    public int xCoord;
    private int height=50;
    private int width=50;
    private int VidHeight =250;
    private int VidWidth = 450;
    public VideoPlayer videoPlayer;
    private List<List<string>> URLsList;
    private List<string> URLs;
    private GameObject canvas;  
    private bool videoInstruction = false;
    private VideoSource videoSource;
    public Button b;
    public RawImage mesh;

    //setting up variables used for steps
    private Text thisText;
    private int step_number=0;
    private string videoURL; 
    //private List<String> URLs;
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    int numsteps;


    //setting up variables used for timer
    private float timeLeft; //Seconds Overall
    private Text countdown; //UI Text Object
    private int hours;
    private int minutes;
    private int seconds;
    private String niceTime; 
    private bool called = true;
 
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("Started");
        yCoord=-20;
        xCoord= 210;
        UnityEngine.Debug.Log("Before URL List");
        //URLsList= RecipeInfo.ingredientURLlistoflist;
        //URLs=URLsList[0];
        // URLs.Add("https://food.fnr.sndimg.com/content/dam/images/food/fullset/2012/2/24/0/ZB0202H_classic-american-grilled-cheese_s4x3.jpg.rend.hgtvcom.616.462.suffix/1371603614279.jpeg");
        // URLs.Add("https://i0.wp.com/cdn-prod.medicalnewstoday.com/content/images/articles/299/299147/cheese-varieties.jpg?w=1155&h=1537");
        UnityEngine.Debug.Log("Before Video URL");
        videoURL= RecipeInfo.RecipeVar.steps[step_number].videoUrl;

        UnityEngine.Debug.Log("Before ML Hands");
        MLHands.Start();
        gestures = new MLHandKeyPose[3];
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Thumb;
        gestures[2] = MLHandKeyPose.L;
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);

        UnityEngine.Debug.Log("Before set thisText and countdown");
        thisText = GameObject.Find("Recipe step").GetComponent<Text>();
        UnityEngine.Debug.Log(thisText);
        countdown = GameObject.Find("Timer").GetComponent<Text>();
        UnityEngine.Debug.Log(countdown);
        timeLeft = (float)(-1);
    }
    
    void Update()
    {
         UnityEngine.Debug.Log("In Update");
         
        if(timeLeft > 0)
        {
        // Debug.Log("In Update");
        timeLeft = timeLeft - Time.deltaTime;
        //Debug.Log(timeLeft);
        
        //yield return ("it works"); new WaitForSeconds(1.0f);
        hours = Mathf.FloorToInt(timeLeft / 3600F);
        minutes = Mathf.FloorToInt((timeLeft - (hours*3600)) / 60F);
        seconds = Mathf.FloorToInt(timeLeft - (hours * 3600) - (minutes * 60));
        niceTime = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        
        countdown.text = ("" + niceTime); //Showing the Score on the Canvas

        }
        
        
    //UnityEngine.Debug.Log("Before First If Statement");
    if(GetOkay() && RecipeInfo.RecipeVar != null && step_number < (RecipeInfo.RecipeVar.steps.Count - 1)) {
           step_number += 1;
           called = false;
           Hold(1);
           UnityEngine.Debug.Log("Inside first if statement");
      } else if (GetDone()) {
           SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
           SceneManager.LoadSceneAsync("Recipe Chooser");
           //UnityEngine.Debug.Log("Inside 1st else if of first if statement");
      } else if (GetGesture(MLHands.Left, MLHandKeyPose.L) || GetGesture(MLHands.Right, MLHandKeyPose.L)) {
            step_number -= 1;
            Hold(1);
           //UnityEngine.Debug.Log("Inside 2nd else if of first if statement");
      }

      if (RecipeInfo.RecipeVar == null)
      {
         //UnityEngine.Debug.Log("recipe is null");
          thisText.text = "No recipe downloaded at the moment";
      }
      else
      {
           UnityEngine.Debug.Log("trying to call and set variables");
            
           if (!called)
           {
               timeLeft = (float)RecipeInfo.RecipeVar.steps[step_number].time;
               called = true;
           }
           UnityEngine.Debug.Log("timeLeft is:" +timeLeft);
           
           UnityEngine.Debug.Log(step_number + "");
           UnityEngine.Debug.Log(RecipeInfo.RecipeVar.steps[step_number].instruction);
           
           //breaking after here
           thisText.text = RecipeInfo.RecipeVar.steps[step_number].instruction;
           UnityEngine.Debug.Log("trying to print text");
           UnityEngine.Debug.Log(thisText.text);
          
           videoURL= RecipeInfo.RecipeVar.steps[step_number].videoUrl;
           URLs=URLsList[step_number]; 
           UnityEngine.Debug.Log(URLs);
           
      }

      if (videoInstruction){
        GameObject NewObj = new GameObject(); //Create the GameObject
        RawImage Screen = NewObj.AddComponent<RawImage>(); //Add the Image Component script
        Screen.transform.SetParent(canvas.transform,false);
        NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
        NewObj.GetComponent<RectTransform>().sizeDelta=new Vector2(VidWidth,VidHeight);
        NewObj.SetActive(true); //Activate the GameObject
        Application.runInBackground=true;
        videoPlayer.source=VideoSource.Url;
        b.image.rectTransform.sizeDelta= new Vector2(30,30);
        mesh.GetComponent<RectTransform>().sizeDelta= new Vector2(1000,1000);
        videoPlayer.url = videoURL;
        StartCoroutine(PlayVideo(Screen));

      }


       canvas = GameObject.Find("Canvas");
       foreach (string currentURL in URLs)
       {
            GameObject NewObj = new GameObject(); //Create the GameObject
            RawImage NewImage = NewObj.AddComponent<RawImage>(); //Add the Image Component script
            NewImage.transform.SetParent(canvas.transform, false);
            NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(xCoord, yCoord, 0);
            NewObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            yCoord = yCoord - 50;
            NewObj.SetActive(true); //Activate the GameObject
            StartCoroutine(GetTexture(currentURL, NewObj));
       }
       
       
       //time each step here
    }
    

    IEnumerator PlayVideo(RawImage rawImage)
    {
        videoPlayer.playOnAwake=false;
        videoPlayer.Prepare();
        
        while (!videoPlayer.isPrepared)
        {
        yield return null;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();

    }

    IEnumerator GetTexture(string thisURL, GameObject currrentImage) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(thisURL);
    	yield return www.SendWebRequest();

        if(www.isNetworkError) {
           UnityEngine.Debug.Log(www.error);
        }
        else {
            currrentImage.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(www);
        }
        
    }

    public void TaskOnClick(){

        if (videoPlayer.isPlaying){
            videoPlayer.Pause();

        }
        else{
          videoPlayer.Play();
        }
    }

    void onDestroy () {
        MLHands.Stop();
    }
    
    bool GetGesture(MLHand hand, MLHandKeyPose type) {
        if (hand != null) {
            if (hand.KeyPose == type) {
                if (hand.KeyPoseConfidence > 0.9f) {
                    return true;
                }
            }
        }
        return false;
    }
    
    // Cleans up logic for reading the 'All Good' gesture
    bool GetOkay()
    {
        if (GetGesture(MLHands.Left, MLHandKeyPose.Thumb) || GetGesture(MLHands.Right, MLHandKeyPose.Thumb))
        {
            return true;
        }

        return false;
    }

    bool GetDone()
   {
       if (GetGesture(MLHands.Left, MLHandKeyPose.Ok) || GetGesture(MLHands.Right, MLHandKeyPose.Ok)) 
       {
           return true;
       }

       return false;
   }

   void Hold(int delay){
       Stopwatch stopWatch = new Stopwatch();
       stopWatch.Start();
       float curr = stopWatch.ElapsedMilliseconds / 1000;
       while (curr < delay)
       {
           curr = stopWatch.ElapsedMilliseconds / 1000;
       }
       stopWatch.Stop();
   }

}
