// Code inpisred by: https://www.studytonight.com/game-development-in-2D/update-ui-element-in-realtime-unity
// Magic Leap tutorial from https://creator.magicleap.com/learn/guides/gestures-in-unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.XR.MagicLeap;
using System.IO;


public class DynamicText : MonoBehaviour
{
	private Text thisText;
	private int step_number=1;
    private MLHandKeyPose[] gestures;   // Holds the different gestures we will look for
    private AssetBundle myLoadedAssetBundle;
    // string path;
    // string jsonString;
    Recipe myRecipe = new Recipe();
    int steps = 3;

    // RecipeStep currentStep = new RecipeStep();
    // myRecipe.steps.add(currentStep)
    // [0].instruction ="at step 1";
    // myRecipe.steps[1].instruction ="at step 2";
    // myRecipe.steps[2].instruction ="at step 3";
    

    // Start is called before the first frame update
    void Start()
    {
        for(int i =1;i<=steps;i++){
        RecipeStep currentStep = new RecipeStep();
        currentStep.instruction= "at step" +i;
        myRecipe.steps.Add(currentStep);

        }

        UnityEngine.Debug.Log(myRecipe);
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
        thisText.text = myRecipe.steps[step_number-1].instruction;
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

}


public class Recipe
{
    public long id;
    public string name;
    public string description;
    public int time;
    public int serving_size;
    public int calories;
    public List<RecipeIngredient> ingredients;
    public List<RecipeStep> steps;
    public List<string> tools;
}

public class RecipeIngredient {
    public long id;
    public string name;
    public string amount;
}

public class RecipeStep {
    public long id;
    public string instruction;
    public string videoUrl;
}


