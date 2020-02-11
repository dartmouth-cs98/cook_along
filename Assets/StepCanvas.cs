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
    private GameObject canvas;

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
    private bool videoInstruction = false;
    private VideoSource videoSource;
    public Button b;
    public RawImage mesh;

    //setting up variables used for steps
    private Text thisText;
    private int step_number = 0;
    private string videoURL; 
    //private List<String> URLs;
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    int numsteps;


    //setting up variables used for timer
    private float timeLeft; //Seconds Overall
    private float stepTime; //Seconds to hold per step
    private Text countdown; //UI Text Object
    private bool called = true; //used to make sure time is called only once per new step
    private bool timer_running = false; //used to toggle start and stop for timer
    // variables used for styling the display of time into hh:mm:ss
    private int hours;
    private int minutes;
    private int seconds;
    private String niceTime; 
    
    //variables for ingredient instuctions
    private Text ges_instructions;
    private bool visible = false;
    private float showStart;
 
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        //UnityEngine.Debug.Log("Started");
        yCoord=-20;
        xCoord= 210;
        //UnityEngine.Debug.Log("Before URL List");
        //URLsList= RecipeInfo.ingredientURLlistoflist;
        //URLs=URLsList[0];
        
        
        
        // URLs.Add("https://food.fnr.sndimg.com/content/dam/images/food/fullset/2012/2/24/0/ZB0202H_classic-american-grilled-cheese_s4x3.jpg.rend.hgtvcom.616.462.suffix/1371603614279.jpeg");
        // URLs.Add("https://i0.wp.com/cdn-prod.medicalnewstoday.com/content/images/articles/299/299147/cheese-varieties.jpg?w=1155&h=1537");
        //UnityEngine.Debug.Log("Before Video URL");
        videoURL= RecipeInfo.RecipeVar.steps[step_number].videoUrl;

        //UnityEngine.Debug.Log("Before ML Hands");
        MLHands.Start();
        gestures = new MLHandKeyPose[6];
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Thumb;
        gestures[2] = MLHandKeyPose.L;
		gestures[3] = MLHandKeyPose.OpenHand;
		gestures[4] = MLHandKeyPose.Pinch;
		gestures[5] = MLHandKeyPose.Finger;
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);

        //UnityEngine.Debug.Log("Before set thisText and countdown");
        thisText = GameObject.Find("Recipe step").GetComponent<Text>();
        //UnityEngine.Debug.Log(thisText);
        countdown = GameObject.Find("Timer").GetComponent<Text>();
        ges_instructions = GameObject.Find("Gesture instruction").GetComponent<Text>();
       
        //UnityEngine.Debug.Log(countdown);
        timeLeft = (float)(-1);
        
    }
    
    
    //********** Update ********** 
    void Update()
    {
         //UnityEngine.Debug.Log("In Update");
         

         //********* Work on Timer **********
        if(Time_Switch()){
        	if(timer_running){
        		timer_running = false;
        	}
        	else{
        		timer_running = true;
        	}
        	Hold(1);
        }

        if(Time_Reset()){
        	timer_running = false;
        	countdown.text = ("");
        	timeLeft = stepTime;
        	Hold(1);
        }
        
        if (Instruction()){
            visible = true;
            showStart = 10;
        }

        if(timeLeft > 1 && timer_running)
        {
        //UnityEngine.Debug.Log("In Update");
        timeLeft = timeLeft - Time.deltaTime;
        //Debug.Log(timeLeft);
        
        hours = Mathf.FloorToInt(timeLeft / 3600F);
        minutes = Mathf.FloorToInt((timeLeft - (hours * 3600)) / 60F);
        seconds = Mathf.FloorToInt(timeLeft - (hours * 3600) - (minutes * 60));
        niceTime = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        
        countdown.text = ("" + niceTime); //Showing the Score on the Canvas
        }
        
        
        
        
    UnityEngine.Debug.Log("Before Visible");
      //********* Work on Gesture Instructions **********
      if(visible){
          
          UnityEngine.Debug.Log("IN GESTURE");
     
          ges_instructions.GetComponent<RectTransform>().sizeDelta=new Vector2(300,300);
          UnityEngine.Debug.Log("Size Changed Changed");
          ges_instructions.text = "Thumbs up to go to next step" + 
                                      Environment.NewLine +
                                      "L hand gesture to go back step" +
                                      Environment.NewLine +
                                      "Ok gesture to go back to recipe chooser" +
                                      Environment.NewLine +
                                      "Open Hand to Start/Stop Timer" +
                                      Environment.NewLine +
                                      "Pinch to Reset Timer";
                                      
          showStart = showStart - Time.deltaTime; 
                                  
          if(showStart < 0){
             visible = false;
             ges_instructions.GetComponent<RectTransform>().sizeDelta=new Vector2(160,30);
             UnityEngine.Debug.Log("Time Reached");
          }
          
       }else{
        UnityEngine.Debug.Log("NO GESTURE");
        ges_instructions.text = "Point up to see list of actions";
        //UnityEngine.Debug.Log("AFTER GESTURE");
       }
      

     
     //********** Work on Recipe Step Change ********** 
    //UnityEngine.Debug.Log("Before First If Statement");
    if(GetOkay() && RecipeInfo.RecipeVar != null && step_number < (RecipeInfo.RecipeVar.steps.Count - 1)) {
           step_number += 1;
           called = false;
           Hold(1);
           //UnityEngine.Debug.Log("Inside first if statement");
      } else if (GetDone()) {
           SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
           SceneManager.LoadSceneAsync("Recipe Chooser");
           //UnityEngine.Debug.Log("Inside 1st else if of first if statement");
      } else if (GetGesture(MLHands.Left, MLHandKeyPose.L) || GetGesture(MLHands.Right, MLHandKeyPose.L)) {
            step_number -= 1;
            Hold(1);
           //UnityEngine.Debug.Log("Inside 2nd else if of first if statement");
      }
       

      //********** Work on Populating Recipe ********** 
      if (RecipeInfo.RecipeVar == null)
      {
         //UnityEngine.Debug.Log("recipe is null");
          thisText.text = "No recipe downloaded at the moment";
      }
      else
      {
           //UnityEngine.Debug.Log("trying to call and set variables");
            
           if (!called)
           {
               timeLeft = (float)RecipeInfo.RecipeVar.steps[step_number].time;
               called = true;
               stepTime = timeLeft;
           }
           //UnityEngine.Debug.Log("timeLeft is:" +timeLeft);
           
           //UnityEngine.Debug.Log(step_number + "");
           //UnityEngine.Debug.Log(RecipeInfo.RecipeVar.steps[step_number].instruction);
           
           //breaking after here
           thisText.text = RecipeInfo.RecipeVar.steps[step_number].instruction;
           //UnityEngine.Debug.Log("trying to print text");
           //UnityEngine.Debug.Log(thisText.text);
          
           videoURL= RecipeInfo.RecipeVar.steps[step_number].videoUrl;
           URLs=URLsList[step_number]; 
           UnityEngine.Debug.Log(URLs);
           
      }

       
      //********** Work on Video ********** 
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


       //********** Work on Ingrdient Images ********** 
      
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
       
    }
    


    //********** Helper Functions ********** 
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
    
    
    //********** Gesture Recognition Boolean Functions ********** 
    bool GetOkay()
    {
        if (GetGesture(MLHands.Left, MLHandKeyPose.Thumb) || GetGesture(MLHands.Right, MLHandKeyPose.Thumb))
        {
            return true;
        }

        return false;
    }

    bool Time_Switch()
    {
        if (GetGesture(MLHands.Left, MLHandKeyPose.OpenHand) || GetGesture(MLHands.Right, MLHandKeyPose.OpenHand))
        {
            return true;
        }

        return false;
    }

    bool Time_Reset()
    {
        if (GetGesture(MLHands.Left, MLHandKeyPose.Pinch) || GetGesture(MLHands.Right, MLHandKeyPose.Pinch))
        {
            return true;
        }

        return false;
    }
    
    bool Instruction()
        {
            if (GetGesture(MLHands.Left, MLHandKeyPose.Finger) || GetGesture(MLHands.Right, MLHandKeyPose.Finger))
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
