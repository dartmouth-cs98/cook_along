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
    // string path;
    // string jsonString;
    Recipe myRecipes = new Recipe();
     int numsteps = 3;
    private string [] stepList = new string[] {"step 1: efjer","step 2: egweg", "step 3: ajfjfa"};

    // RecipeStep currentStep = new RecipeStep();
    // myRecipe.steps.add(currentStep)
    // [0].instruction ="at step 1";
    // myRecipe.steps[1].instruction ="at step 2";
    // myRecipe.steps[2].instruction ="at step 3";
    

    // Start is called before the first frame update
    void Start()
    {
      

    List<RecipeSteps> stepList = new List<RecipeSteps>();
    for (int i = 1; i <= numsteps; i++)
       {
          
           RecipeSteps currentStep = new RecipeSteps();
    
           currentStep.instruction = "at step" + i;
          
           currentStep.videoUrl = "www.google.com";
          
           stepList.Add(currentStep);
    
       }
       myRecipes.steps=stepList;

       List<RecipeIngredients> ingredientList = new List<RecipeIngredients>();
       for (int j = 1; j <= 5; j++)
       {
           RecipeIngredients inggredient = new RecipeIngredients();
           inggredient.name = "ingredient" + j;
           inggredient.amount = ""+j;
           ingredientList.Add(inggredient);
       }
       myRecipes.ingredients=ingredientList;
       myRecipes.id = 1;
       myRecipes.name = "grilled cheese";
       myRecipes.time = 10;
       myRecipes.serving_size = 2;
       myRecipes.calories = 150;
       List<string> myTools = new List<string>(new string[] { "knife", "skillet" });
       myRecipes.tools = myTools;




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
      if(GetOkay())
    {
           step_number += 1;
           Hold(3);


       } // && step_number == myRecipes.steps.Count
       else if (GetDone())
       {
           SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
           SceneManager.LoadSceneAsync("welcome_screen");
       }
       else if (GetGesture(MLHands.Left, MLHandKeyPose.L)
                || GetGesture(MLHands.Right, MLHandKeyPose.L))
       {
            step_number -= 1;
            Hold(3);
            
       }
        thisText.text = myRecipes.steps[step_number].instruction;

       
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
        || GetGesture(MLHands.Right, MLHandKeyPose.Thumb))  {
            return true;
        }

        else {
            return false;
        }
    }


    bool GetDone()
   {
       if (GetGesture(MLHands.Left, MLHandKeyPose.Ok)
           || GetGesture(MLHands.Right, MLHandKeyPose.Ok))
       {
           return true;
       }
       else
       {
           return false;
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


public class Recipe
{
    public long id;
    public string name;
    public string description;
    public int time;
    public int serving_size;
    public int calories;
    public List<RecipeIngredients> ingredients;
    public List<RecipeSteps> steps;
    public List<string> tools;
}

public class RecipeIngredients {
    public long id;
    public string name;
    public string amount;
}

public class RecipeSteps {
    public long id;
    public string instruction;
    public string videoUrl;
}




