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
using System.Text;


public class StepCanvas : MonoBehaviour
{
    private GameObject canvas;

    //Setting up Variables used for video
	 private int yCoord;
    private int xCoord;
    private int height=75;
    private int width=75;
    private int VidHeight =250;
    private int VidWidth = 450;
    private VideoPlayer videoPlayer;
    private List<List<string>> URLsList;
    private List<string> URLs; 

    //setting up variables used for steps
    private Text thisText;
    private Text ingred;
    private int step_number=0;
    private string videoURL; 
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
    
    //variables for video 
    private bool firstUpdate= true; 
    private int previousURL =0 ; 
    private bool firstvideo =true; 
    private GameObject NewObj;
    private bool previousVideo = false;
 
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        yCoord=-20;
        xCoord= 210;        
        URLsList= RecipeInformation.ingredientURLlistoflist;
        URLs=URLsList[0];
        videoURL= RecipeInformation.RecipeVar.steps[step_number].videoUrl;

        MLHands.Start();
        gestures = new MLHandKeyPose[7];
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Thumb;
        gestures[2] = MLHandKeyPose.L;
		gestures[3] = MLHandKeyPose.OpenHand;
		gestures[4] = MLHandKeyPose.Pinch;
		gestures[5] = MLHandKeyPose.Finger;
     gestures[6]= MLHandKeyPose.Fist;
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);

        thisText = GameObject.Find("Recipe step").GetComponent<Text>();
        countdown = GameObject.Find("Timer").GetComponent<Text>();
        ges_instructions = GameObject.Find("Gesture instruction").GetComponent<Text>();
        ingred= GameObject.Find("Ingredients").GetComponent<Text>();
        timeLeft = (float)(-1);
    }
    
    
    //********** Update ********** 
    void Update()
    {
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
            showStart = 7;
            Hold(1);
        }

        if(timeLeft > 1)
        {
            if (timer_running)
            {
                timeLeft = timeLeft - Time.deltaTime;
            }
        
        hours = Mathf.FloorToInt(timeLeft / 3600F);
        minutes = Mathf.FloorToInt((timeLeft - (hours * 3600)) / 60F);
        seconds = Mathf.FloorToInt(timeLeft - (hours * 3600) - (minutes * 60));
        niceTime = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        
        if (!timer_running)
             {
                 countdown.text = ("Open hand to start/stop timer: " + niceTime);
             }
         else
        {countdown.text = ("" + niceTime); //Showing the Score on the Canvas
        }
        }
     
      //********* Work on Gesture Instructions **********
      if(visible){
          
         ges_instructions.GetComponent<RectTransform>().sizeDelta=new Vector2(300,300);
         ges_instructions.text = "Thumbs Up: go to next step" + 
                                      Environment.NewLine +
                                      "L Gesture: go back a step" +
                                      Environment.NewLine +
                                      "Ok Gesture: go back to recipe chooser" +
                                      Environment.NewLine +
                                      "High Five: Start/Stop Timer" +
                                      Environment.NewLine +
                                      "Pinch: Reset Timer" +
                                      Environment.NewLine +
                                      "Closed Fist: Start/Stop Video";
                                      
          showStart = showStart - Time.deltaTime; 
                                  
          if(showStart < 0){
             visible = false;
             ges_instructions.GetComponent<RectTransform>().sizeDelta=new Vector2(300,30);
          }
          
       }else{
        ges_instructions.text = "Point up to see list of actions";
       }
      

     
     //********** Work on Recipe Step Change ********** 
   if(GetOkay() && RecipeInformation.RecipeVar != null && step_number < (RecipeInformation.RecipeVar.steps.Count - 1)) {

           step_number += 1;

           called = false;
           visible = false;
           Hold(1);

           List<RawImage> SceneObject = new List<RawImage>();
           foreach (RawImage go in Resources.FindObjectsOfTypeAll(typeof(RawImage)) as RawImage[]){
                RawImage image = go as RawImage; 
                Destroy(image);
                yCoord=-20;
           }
           ingred.text = "" ;
           firstUpdate = true;
           firstvideo=true;
           if (previousVideo){
            Destroy(NewObj);
            previousVideo=false;
           }
           
      } else if (GetDone()) {
           SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
           Loader.Load(Loader.Scene.RecipeChooser);
      } else if (GetGesture(MLHands.Left, MLHandKeyPose.L) || GetGesture(MLHands.Right, MLHandKeyPose.L)) {
            step_number -= 1;
            called = false;
            visible = false;
                     
            foreach (RawImage go in Resources.FindObjectsOfTypeAll(typeof(RawImage)) as RawImage[]){
                RawImage image = go as RawImage; 
                Destroy(image);
                yCoord=-20;
           }
           ingred.text = "" ;
            Hold(1);    
           firstUpdate = true;
           firstvideo=true;
            if (previousVideo){
            Destroy(NewObj);
            previousVideo=false;
           }
      }
  
       

      //********** Work on Populating Recipe ********** 
      if (RecipeInformation.RecipeVar == null)
      {
          thisText.text = "No recipe downloaded at the moment";
      }
      else
      {     
           if (!called)
           {
               timeLeft = (float)RecipeInformation.RecipeVar.steps[step_number].time;
               called = true;
               stepTime = timeLeft;    
               countdown.text = ("");
           }
       
           thisText.text = RecipeInformation.RecipeVar.steps[step_number].instruction;
           videoURL= RecipeInformation.RecipeVar.steps[step_number].videoUrl;
           URLs=URLsList[step_number]; 
           
      }

     //********** Work on Video **********  
      string s1=""; 
      string s2=null;
      if (videoURL !=s1 & videoURL!=s2) {
        previousVideo=true;
        
        if (firstvideo){
          NewObj = new GameObject(); //Create the GameObject
          RawImage Screen = NewObj.AddComponent<RawImage>(); //Add the Image Component script
          Screen.transform.SetParent(canvas.transform,false);
          NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
          NewObj.GetComponent<RectTransform>().sizeDelta=new Vector2(100,100);
          NewObj.SetActive(true); //Activate the GameObject
          Application.runInBackground=true;
          videoPlayer=gameObject.AddComponent<VideoPlayer>();
          videoPlayer.source=VideoSource.Url;
          videoPlayer.url = videoURL;
          StartCoroutine(PlayVideo(Screen));
          firstvideo=false; 
        }

        if (GetGesture(MLHands.Left,MLHandKeyPose.Fist) || GetGesture(MLHands.Right,MLHandKeyPose.Fist)){
        NewObj.GetComponent<RectTransform>().sizeDelta=new Vector2(VidWidth,VidHeight);

        if (videoPlayer.isPlaying){
            videoPlayer.Pause();

        }
        else if (videoPlayer.isPaused){
          videoPlayer.Play();
        }
        Hold(1); 

      }


      }

         foreach (string currentURL in URLs)
         {
              if (firstUpdate){
              string currentURLcorrected;
              currentURLcorrected = currentURL;
              int length = currentURLcorrected.Length;
              if (length >0){
                ingred.text ="Ingredients:";

              	int start = 0;
              	int end = length; 
              	if(currentURLcorrected[0]== ','){
              		start =2;
              	}
              	if(currentURLcorrected[length-1]!='g'){
              		end = length-1;   
              	}
                end = end - start;
              	currentURLcorrected = currentURLcorrected.Substring(start, end);


              GameObject NewURLObj = new GameObject(); //Create the GameObject
              RawImage NewImage = NewURLObj.AddComponent<RawImage>(); //Add the Image Component script
              NewImage.transform.SetParent(canvas.transform, false);
              NewURLObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(xCoord, yCoord, 0);
              NewURLObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
              yCoord = yCoord - 75;
              NewURLObj.SetActive(true); //Activate the GameObject
              StartCoroutine(GetTexture(currentURLcorrected, NewURLObj));
              previousURL = previousURL+1 ;
            }
            }

         }
         firstUpdate=false;  
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
        Hold(1);
        videoPlayer.Pause();

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
