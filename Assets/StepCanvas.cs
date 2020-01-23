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

	public int yCoord;
    public int xCoord;
    public VideoPlayer videoPlayer;
    private List<string> URLs = new List<string>();
    private GameObject canvas;  
    private bool videoInstruction = true;
    private VideoSource videoSource;
    public Button b;
    public RawImage mesh;

    private Text thisText;
    private int step_number=0;
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    int numsteps;


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
        int VidHeight =250;
        int VidWidth =450;
        URLs.Add("https://food.fnr.sndimg.com/content/dam/images/food/fullset/2012/2/24/0/ZB0202H_classic-american-grilled-cheese_s4x3.jpg.rend.hgtvcom.616.462.suffix/1371603614279.jpeg");
        URLs.Add("https://i0.wp.com/cdn-prod.medicalnewstoday.com/content/images/articles/299/299147/cheese-varieties.jpg?w=1155&h=1537");
       

        thisText = GameObject.Find("Recipe step").GetComponent<Text>();
        MLHands.Start();
        gestures = new MLHandKeyPose[3];
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Thumb;
        gestures[2] = MLHandKeyPose.L;
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);



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

        countdown = GameObject.Find("Timer").GetComponent<Text>();
        timeLeft = 120;

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

          videoPlayer.url = "https:/dl.dropbox.com/s/f5suv9je1vya4pd/3%20Ways%20To%20Chop%20Onions%20Like%20A%20Pro.mp4?dl=1";
          StartCoroutine(PlayVideo(Screen));

        }

    }
    
    void Update()
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




    if(GetOkay() && RecipeInfo.RecipeVar != null && step_number < (RecipeInfo.RecipeVar.steps.Count - 1)) {
           step_number += 1;
           Hold(1);
      } else if (GetDone()) {
           SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
           SceneManager.LoadSceneAsync("Recipe Chooser");
      } else if (GetGesture(MLHands.Left, MLHandKeyPose.L) || GetGesture(MLHands.Right, MLHandKeyPose.L)) {
            step_number -= 1;
            Hold(1);
      }

      if (RecipeInfo.RecipeVar == null)
      {
          thisText.text = "No recipe downloaded at the moment";
      }
      else
      {
          thisText.text = RecipeInfo.RecipeVar.steps[step_number].instruction;
      }

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
