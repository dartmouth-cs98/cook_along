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
using Debug = UnityEngine.Debug;


public class StepCanvas : MonoBehaviour
{
    private GameObject canvas;
    public static bool fromScroller; 

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
    public static int step_number=0;
    private string videoURL; 
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    int numsteps;


    //setting up variables used for timer
    private List<List<float>> timeLeft; //Seconds Overall From Step //Going to be List of string of stepNum and TimeLeft
    private List<float> stepTime; //Seconds to hold per step
    public static List<Text> countdown; //UI Text Object
    private bool called; //used to make sure time is called only once per new step
    private List<bool> timer_running; //used to toggle start and stop for timer
    public static bool hasTime = false;
    
    // variables used for styling the display of time into hh:mm:ss
    private List<int> hours;
    private List<int> minutes;
    private List<int> seconds;
    private List<string> niceTime; 
    private List<float> timerCountdown;
    
    private int active_timer;
    public static List<int> currActive;
     
    // Audio for Timer Notifications
    private List<Text> timer_notifs;
    public AudioClip notification;
    private AudioSource audio;
    private List<bool> play;
    
    //variables for ingredient instuctions
    private Text ges_instructions;
    private bool visible = false;
    //private float showStart;
    
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
        fromScroller = false; 
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
        audio= gameObject.AddComponent<AudioSource>();
        audio.clip = notification;
        startPopulateTimers();
        ges_instructions = GameObject.Find("Gesture instruction").GetComponent<Text>();
        ingred= GameObject.Find("Ingredients").GetComponent<Text>();
    }
    
    
    //********** Update ********** 
    void Update()
    {
        URLsList= RecipeInformation.ingredientURLlistoflist;
        active_timer = StepListControl.active_timer_index;     
       
        if (active_timer >= 0)
        {
             //********* Work on Timer **********
            if(Time_Switch()){
                if(timer_running[active_timer]){
                    timer_running[active_timer] = false;
                    Hold(1);
                }
                else{
                    timer_running[active_timer] = true;
                    Hold(1);
                }
                
            }
    
            if(Time_Reset()){
                timer_running[active_timer] = false;
                countdown[active_timer].text = (""); 
                timeLeft[active_timer][1] = stepTime[active_timer];
                timerCountdown[active_timer] = 10;
                Hold(1);
            }
        
        }

        for (int i = 0; i < 3; i++)
        {
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
                
                
                
                if(i == active_timer)
                {
                    countdown[i].color = new Color(0.85f, 0.85f, 0.10f);
                    countdown[i].text = ("Step " + (int)(timeLeft[i][0]+1) + ".  " + niceTime[i]); //Showing the Score on the Canvas
                }else if (!timer_running[i])
                 {
                    countdown[i].color = new Color(1f, 1.0f, 1.0f);
                    countdown[i].text = ("Step " + (int)(timeLeft[i][0]+1) + ".  " + niceTime[i]);
                 }else{
                    countdown[i].color = new Color(0.56f, 0.56f, 0.75f);
                    countdown[i].text = ("Step " + (int)(timeLeft[i][0]+1) + ".  " + niceTime[i]); //Showing the Score on the Canvas
                 }
            }
            
            else 
            {
              
                if(timeLeft[i][1] > -15.0)
                {
                    currActive.Remove(i);
                    //timerCountdown[i] -=  Time.deltaTime; 
                    if(timerCountdown[i] > 0)
                    {
                        timerCountdown[i] -=  Time.deltaTime; 
                        if (play[i])
                        {
                            audio.PlayOneShot(notification);
                            play[i] = false; 
                        }
                         countdown[i].color = new Color(1f, 0f, 0f);
                         countdown[i].text = ("00:00:00"); //Showing the Score on the Canvas
                         timer_notifs[i].GetComponent<RectTransform>().sizeDelta=new Vector2(170,50);
                         timer_notifs[i].text = "Timer has run out for step: " + (int)(timeLeft[i][0]+1.0);        
                    }
                    else
                    {   
                        timer_running[i] = false;
                        timeLeft[i][0] = (float)(-1.0);
                        timeLeft[i][1] = (float)(-50.0);
                        timerCountdown[i] = (float)(10);
                        countdown[i].text = ("");
                        timer_notifs[i].text = "";
                        stepTime[i] = ((float)(-1.0));
                        play[i] = true;
                        hours[i] = 0;
                        hours[i] = 0;
                        hours[i] = 0;
                        niceTime[i] = "";
                        timer_notifs[i].GetComponent<RectTransform>().sizeDelta=new Vector2(170,0);
                        hasTime = false;
                        for(int d = 0; d < 3; d++){
                            if (timeLeft[d][0]!=((float)(-1.0)))
                            {
                                hasTime = true;
                            }
                        }
                    }
                }
            }
        }
        
            
    
      //********* Work on Gesture Instructions **********
      if (Instruction()){
          if(visible){
              visible = false;
          }
          else
          {
              visible = true;
          }
          Hold(1);
      }
              
      if(visible)
      {
          
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
                                      "Closed Fist: Start/Stop Video"+
                                      Environment.NewLine +
                                      "Point Up Again to Close Gesture Menu";

          
      }else
      {
          ges_instructions.GetComponent<RectTransform>().sizeDelta=new Vector2(300,30);
          ges_instructions.text = "Point up to see list of actions";
      }
                                      
   
         //********** Work on Recipe Step Change ********** 
       if((GetOkay() || fromScroller) && RecipeMenuList.SelectedRecipe != null && step_number < (RecipeMenuList.SelectedRecipe.steps.Count - 1)) {   
               
               if(!fromScroller)
               {
                    step_number += 1;
               }  
               
               called = false;
               visible = false;
               
               if (StepListControl.Selecting)
               {
                   StepListControl.Selecting = false;
                   StepListControl.ScrollRect.verticalNormalizedPosition = StepListControl.PrevNormPosition - 0.2f;
                   GameObject.Find("Viewport").GetComponent<Image>().color = Color.white;
               }
               else
               {
                   StepListControl.ScrollRect.verticalNormalizedPosition -= 0.2f;
               }
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
                   Destroy(videoPlayer);
                   Destroy(NewObj);
                   previousVideo=false;
               }
               
               fromScroller = false;
               
          }else if (GetDone()) {
                Loader.Load(Loader.Scene.RecipeMenu);
          } else if (GetL()) {  
                step_number -= 1;
                if (step_number<0){
                  step_number=0;
                }
                
                if (StepListControl.Selecting)
                {
                    StepListControl.Selecting = false;
                    StepListControl.ScrollRect.verticalNormalizedPosition = StepListControl.PrevNormPosition + 0.2f;
                    GameObject.Find("Viewport").GetComponent<Image>().color = Color.white;
                }
                else
                {
                    StepListControl.ScrollRect.verticalNormalizedPosition += 0.2f; 
                }    
                      
                called = false;
                visible = false;
<<<<<<< HEAD
                         
                foreach (RawImage go in Resources.FindObjectsOfTypeAll(typeof(RawImage)) as RawImage[]){
                    RawImage image = go as RawImage; 
                    Destroy(image);
                    yCoord=90;
               }
               ingred.text = "" ;
                Hold(1);    
               firstUpdate = true;
               firstvideo=true;
               if (previousVideo){
                  Destroy(videoPlayer);
                  Destroy(NewObj);
                  previousVideo=false;
               }
          }

        //********** Work on Populating Recipe ********** 
        if (RecipeMenuList.SelectedRecipe == null)
        {
            thisText.text = "No recipe downloaded at the moment";
        } else
        {     
           if (!called)
           {
                called = true;
                int posTime = RecipeMenuList.SelectedRecipe.steps[step_number].time;
                if (posTime > 0)
                {
                     bool alreadyin = false;
                     for(int j = 0; j < 3; j++)
                     {
                        if(timeLeft[j][0]==((float)(step_number)))
                        {
                            alreadyin = true;
                        }
                     }
                     
                     if(!alreadyin)
                     {
                        int i = 0;
                        while(((timeLeft[i][1]) != ((float)(-50.0))) && i < 3)
                        {
                            i++;
                        }
                       timeLeft[i][1] = (float)RecipeMenuList.SelectedRecipe.steps[step_number].time;
                       timeLeft[i][0] = (float)step_number;
                       stepTime[i] = timeLeft[i][1];    
                       countdown[i].text = ("");
                       if(currActive.Count != 0 && i < currActive[0])
                       {
                            currActive.Insert(0, i);
                       }
                       else
                       {
                            currActive.Add(i);
                       }
                       hasTime = true;
                     }
                 }
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
                } else if (videoPlayer.isPaused){
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
                    previousURL = previousURL+1; 
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


    void OnDestroy () {
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
   
   bool GetL()  
    {
           if (GetGesture(MLHands.Left, MLHandKeyPose.L) || GetGesture(MLHands.Right, MLHandKeyPose.L))
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
   
   void startPopulateTimers()
   {
            countdown = new List<Text>(); 
            timer_notifs = new List<Text>(); 
            timeLeft = new List<List<float>>(); 
            hours = new List<int>(); 
            minutes = new List<int>(); 
            seconds = new List<int>(); 
            niceTime = new List<string>(); 
            timer_running = new List<bool>();
            stepTime = new List<float>();
            timerCountdown = new List<float>();
            play = new List<bool>();
            currActive = new List<int>();
            
            countdown.Add(GameObject.Find("Timer_1").GetComponent<Text>());
            countdown.Add(GameObject.Find("Timer_2").GetComponent<Text>());
            countdown.Add(GameObject.Find("Timer_3").GetComponent<Text>());
            
            timer_notifs.Add(GameObject.Find("Notice1").GetComponent<Text>());
            timer_notifs.Add(GameObject.Find("Notice2").GetComponent<Text>());
            timer_notifs.Add(GameObject.Find("Notice3").GetComponent<Text>());
            
            for (int i = 0; i < 3; i++){
                timer_running.Add(false); 
                hours.Add(0);
                minutes.Add(0);
                seconds.Add(0);
                niceTime.Add("");
                List<float> currTimer = new List<float>(); 
                currTimer.Add((float)(-1.0));
                currTimer.Add((float)(-50.0));
                timeLeft.Add(currTimer); 
                timerCountdown.Add(10);
                play.Add(true);  
                stepTime.Add((float)(-1.0)); 
            }     
                      
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
