using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// pulling from https://www.youtube.com/watch?v=3I5d2rUJ0pE
public static class Loader
{

    public enum Scene
    {
        welcome_screen, RecipeChooser, RecipeInformation
    }
    
    
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
