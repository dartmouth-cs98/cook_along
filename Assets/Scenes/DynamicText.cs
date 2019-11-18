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
    private string [] steps = { "1. Preheat skillet over medium heat", "2. Generously butter one side of a slice of bread", "3. Place bread butter-side-down onto skillet bottom and add 1 slice of cheese", "4. Butter a second slice of bread on one side and place butter-side-up on top of sandwich", "5. Grill until lightly browned and flip over; continue grilling until cheese is melted", "6. Repeat with remaining 2 slices of bread, butter and slice of cheese" };
    private string[] ing = { "bread", "butter", "cheddar cheese"};
    private string[] amt = { "4 slices", "3 tablespoons", "2 slices" };

    // RecipeStep currentStep = new RecipeStep();
    // myRecipe.steps.add(currentStep)
    // [0].instruction ="at step 1";
    // myRecipe.steps[1].instruction ="at step 2";
    // myRecipe.steps[2].instruction ="at step 3";


    // Start is called before the first frame update
    void Start()
    {
      

    List<RecipeStep> stepList = new List<RecipeStep>();
    for (int i = 1; i <= numsteps; i++)
       {
          
           RecipeStep currentStep = new RecipeStep();
    
           currentStep.instruction = steps[i-1];
          
           currentStep.videoUrl = "www.google.com";
          
           stepList.Add(currentStep);
    
       }
       myRecipes.steps=stepList;

       List<RecipeIngredient> ingredientList = new List<RecipeIngredient>();
       for (int j = 0; j < 3; j++)
       {
           RecipeIngredient inggredient = new RecipeIngredient();
           inggredient.name = ing[j];
           inggredient.amount = amt[j];
           ingredientList.Add(inggredient);
       }
       myRecipes.ingredients=ingredientList;
       myRecipes.id = 1;
       myRecipes.name = "Grilled Cheese";
       myRecipes.time = 10;
       myRecipes.serving_size = 2;
       myRecipes.imgUrl = "https://assets.bonappetit.com/photos/57acf62a53e63daf11a4dbee/16:9/w_2560,c_limit/best-ever-grilled-cheese.jpg";
       myRecipes.calories = 300;
       List<string> myTools = new List<string>(new string[] { "knife", "skillet", "stove" });
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




