using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLanding : MonoBehaviour
{
    
    public ControlInput controlInput;

    private void Awake()
    {
        controlInput.OnTriggerPressEnded.AddListener(HandleTrigger);
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
    }

    void HandleTrigger()
    {
        Loader.Load(Loader.Scene.RecipeChooser);
    }

    void HandleHomeTap()
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
