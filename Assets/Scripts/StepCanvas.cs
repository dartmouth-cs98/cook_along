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
using MagicLeapTools;
using UnityEngine.XR.MagicLeap;



public class StepCanvas : MonoBehaviour
{
    private GameObject canvas;

    //Setting up Variables used for video
	 private int yCoord;
    private int xCoord;
    private int height=75;
    private int width=75;
    private int VidHeight =200;
    private int VidWidth = 400;
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
    private List<GameObject> _timers;
    private List<List<float>> timeLeft; //Seconds Overall From Step //Going to be List of string of stepNum and TimeLeft
    private List<float> stepTime; //Seconds to hold per step
    private List<Text> countdown; //UI Text Object
    private bool called; //used to make sure time is called only once per new step
    private List<bool> timer_running; //used to toggle start and stop for timer
    // variables used for styling the display of time into hh:mm:ss
    private List<int> hours;
    private List<int> minutes;
    private List<int> seconds;
    private List<string> niceTime; 
    private int active_timer;
    
    public ControlInput controlInput;
    
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
        yCoord=90;
        xCoord= 255;        
        URLsList= RecipeInformation.ingredientURLlistoflist;
        URLs=URLsList[0];
        videoURL= RecipeMenuList.SelectedRecipe.steps[step_number].videoUrl;

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
        startPopulateTimers();
        ges_instructions = GameObject.Find("Gesture instruction").GetComponent<Text>();
        ingred= GameObject.Find("Ingredients").GetComponent<Text>();
    }
    
    
    //********** Update ********** 
    void Update()
    {
         //********* Work on Timer **********
        if(Time_Switch()){
        	if(timer_running[active_timer]){
        		timer_running[active_timer] = false;
        	}
        	else{
        		timer_running[active_timer] = true;
        	}
        	Hold(1);
        }

        if(Time_Reset()){
        	timer_running[active_timer] = false;
        	countdown[active_timer].text = ("");
        	timeLeft[active_timer][1] = stepTime[active_timer];
        	Hold(1);
        }
        
        if (Instruction()){
            visible = true;
            showStart = 7;
            Hold(1);
        }

        for (int i = 0; i < countdown.Count; i++)
            if(timeLeft[i][1] > 1)
            {
                if (timer_running[i])
                {
                    timeLeft[i][1] = timeLeft[i][1] - Time.deltaTime;
                }
            
            hours[i] = Mathf.FloorToInt(timeLeft[i][1] / 3600F);
            minutes[i] = Mathf.FloorToInt((timeLeft[i][1] - (hours[i] * 3600)) / 60F);
            seconds[i] = Mathf.FloorToInt(timeLeft[i][1] - (hours[i] * 3600) - (minutes[i] * 60));
            niceTime[i] = String.Format("{0:00}:{1:00}:{2:00}", hours[i], minutes[i], seconds[i]);
            
            if (!timer_running[i])
                     {
                        countdown[i].color = new Color(1f, 1.0f, 1.0f);
                        countdown[i].text = (niceTime+"");
                     }
             else
                    {
                        countdown[i].color = new Color(0.56f, 0.56f, 0.75f);
                        countdown[i].text = ("" + niceTime); //Showing the Score on the Canvas
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
   if(GetOkay() && RecipeMenuList.SelectedRecipe != null && step_number < (RecipeMenuList.SelectedRecipe.steps.Count - 1)) {

           step_number += 1;

           called = false;
           visible = false;
           Hold(1);

           List<RawImage> SceneObject = new List<RawImage>();
           foreach (RawImage go in Resources.FindObjectsOfTypeAll(typeof(RawImage)) as RawImage[]){
                RawImage image = go as RawImage; 
                Destroy(image);
                yCoord=90;
           }
           ingred.text = "" ;
           firstUpdate = true;
           firstvideo=true;
           if (previousVideo){
            Destroy(NewObj);
            previousVideo=false;
           }
           
      } else if (GetDone()) {
            Loader.Load(Loader.Scene.RecipeMenu);
      } else if (GetGesture(MLHands.Left, MLHandKeyPose.L) || GetGesture(MLHands.Right, MLHandKeyPose.L)) {
            step_number -= 1;
            if (step_number<0){
              step_number=0;
            }
            called = false;
            visible = false;
                     
            foreach (RawImage go in Resources.FindObjectsOfTypeAll(typeof(RawImage)) as RawImage[]){
                RawImage image = go as RawImage; 
                Destroy(image);
                yCoord=-60;
           }
           ingred.text = "" ;
            Hold(1);    
           firstUpdate = true;
           firstvideo=true;
            // if (previousVideo){

            Destroy(videoPlayer);
            Destroy(NewObj);
           //  previousVideo=false;
           // }
      }
  
       

      //********** Work on Populating Recipe ********** 
      if (RecipeMenuList.SelectedRecipe == null)
      {
          thisText.text = "No recipe downloaded at the moment";
      }
      else
      {     
           if (!called)
           {

                called = true;
                int i = 0;
                while(timeLeft[i][0] == -1.0 && i < 3.0)
                {
                    i++;
                }
               timeLeft[i][1] = (float)RecipeInformation.RecipeVar.steps[step_number].time;
               timeLeft[i][0] = (float)step_number;
               stepTime[i] = timeLeft[i][1];    
               countdown[i].text = ("");
           }
       
           thisText.text = RecipeMenuList.SelectedRecipe.steps[step_number].instruction;
           videoURL= RecipeMenuList.SelectedRecipe.steps[step_number].videoUrl;
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
          NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,-150,0);
          NewObj.GetComponent<RectTransform>().sizeDelta=new Vector2(125,100);
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
   
   void HandleSwipe(MLInputControllerTouchpadGestureDirection direction)     
   {
       if (direction == MLInputControllerTouchpadGestureDirection.Right && active_timer < 2)
       {
           UpdateActiveTimer(direction);
       }

       if (direction == MLInputControllerTouchpadGestureDirection.Left && active_timer > 0)
       {
           UpdateActiveTimer(direction);
       }
   }
       
   void UpdateActiveTimer(MLInputControllerTouchpadGestureDirection direction)
   {
       int oldIndex = (int)active_timer;
       
       if (direction == MLInputControllerTouchpadGestureDirection.Right)
       {
           active_timer += 1;
       }

       if (direction == MLInputControllerTouchpadGestureDirection.Left)
       {
           active_timer -= 1;
       }
       
       countdown[oldIndex].color = new Color(0.56f, 0.56f, 0.75f);
       countdown[active_timer].color = Color.yellow;
   }  

   void startPopulateTimers()
   {
            countdown = new List<Text>(); 
            timeLeft = new List<List<float>>(); 
            hours = new List<int>(); 
            minutes = new List<int>(); 
            seconds = new List<int>(); 
            niceTime = new List<string>(); 
            timer_running = new List<bool>();
            stepTime = new List<float>();
            
            countdown.Add(GameObject.Find("Timer_1").GetComponent<Text>());
            countdown.Add(GameObject.Find("Timer_2").GetComponent<Text>());
            countdown.Add(GameObject.Find("Timer_3").GetComponent<Text>());
            
            for (int i = 0; i < 3; i++){
                timer_running.Add(false); 
                List<float> currTimer = new List<float>(); 
                currTimer.Add((float)-1.0);
                currTimer.Add((float)-1.0);
                timeLeft.Add(currTimer);    
            }     
            active_timer = 0;   
            controlInput.OnSwipe.AddListener(HandleSwipe);         
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
