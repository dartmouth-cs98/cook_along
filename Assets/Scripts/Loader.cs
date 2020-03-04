using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// pulling from https://www.youtube.com/watch?v=3I5d2rUJ0pE
public static class Loader
{

    public enum Scene
    {
        WelcomeScreen, RecipeChooser, RecipeInformation, Loading, StepDisplay, TutorialLanding, TutorialGestures, TutorialStep, TutorialSuccess, RecipeMenu,
    }


    private static Action onLoaderCallback;
    
    public static void Load(Scene scene)
    {
        // set the loadercallback to the target scene
        onLoaderCallback = () => { SceneManager.LoadScene(scene.ToString()); };
        
        // load the loading scene
        SceneManager.LoadScene(Scene.Loading.ToString());

    }

    public static void LoaderCallback()
    {
        // triggered after the first update to let the sreen refresh
        // execute the load the target scene
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
