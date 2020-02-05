// Code inpisred by: https://www.studytonight.com/game-development-in-2D/update-ui-element-in-realtime-unity
// Magic Leap tutorial from https://creator.magicleap.com/learn/guides/gestures-in-unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;
using System.IO;


public class DynamicText : MonoBehaviour
{
    private Text thisText;
    private int step_number=0;
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    int numsteps;

    // Start is called before the first frame update
    void Start() 
    {
        thisText = GetComponent<Text>();
        MLHands.Start();
        gestures = new MLHandKeyPose[3];
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Thumb;
        gestures[2] = MLHandKeyPose.L;
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);
    }

    // Update is called once per frame
    void Update()
    {
      if(GetOkay() && RecipeInfo.RecipeVar != null && step_number < (RecipeInfo.RecipeVar.steps.Count - 1)) {
           step_number += 1;
           Hold(1);
      } else if (GetDone())
      {
          Loader.Load(Loader.Scene.RecipeChooser);
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




