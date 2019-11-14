// Code inpisred by: https://www.studytonight.com/game-development-in-2D/update-ui-element-in-realtime-unity
// Magic Leap tutorial from https://creator.magicleap.com/learn/guides/gestures-in-unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.XR.MagicLeap;


public class DynamicText : MonoBehaviour
{
	private Text thisText;
	private int step_number;
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    // Start is called before the first frame update
    void Start()
    {
    	thisText = GetComponent<Text>();
        MLHands.Start();

        gestures = new MLHandKeyPose[2];
        
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Thumb;
        
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(GetOkay())
        {
            step_number +=1;
            int delay = 3; 
            Stopwatch stopWatch= new Stopwatch();
            stopWatch.Start();
            float curr = stopWatch.ElapsedMilliseconds/1000;
            while (curr <delay){
                curr = stopWatch.ElapsedMilliseconds/1000;
            }
            stopWatch.Stop();
        }
        thisText.text = "Step " + step_number;
    }

    void onDestroy () {
        MLHands.Stop();
    }


    bool GetGesture(MLHand hand, MLHandKeyPose type)    {
        
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
    bool GetOkay() {
        
        if (GetGesture(MLHands.Left, MLHandKeyPose.Thumb) 
        || GetGesture(MLHands.Left, MLHandKeyPose.Ok)
        || GetGesture(MLHands.Right, MLHandKeyPose.Thumb) 
        || GetGesture(MLHands.Right, MLHandKeyPose.Ok)) {
            return true;
        }

        else {
            return false;
        }
    }

    // void Hold(int delay){
    //     Stopwatch stopWatch= new Stopwatch();
    //     stopWatch.Start();
    //     float curr = stopWatch.ElapsedMilliseconds/1000;
    //     while (curr <delay){
    //         curr = stopWatch.ElapsedMilliseconds/1000;
    //     }
    //     stopWatch.Stop();
    // }

}


