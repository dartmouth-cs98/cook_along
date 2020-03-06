using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSuccess : MonoBehaviour
{
    
    public ControlInput controlInput;

    private void Awake()
    {
        controlInput.OnTriggerPressEnded.AddListener(HandleTrigger);
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        controlInput.OnDoubleTap.AddListener(HandleDoubleTap);
    }

    void HandleTrigger()
    {
			Loader.Load(Loader.Scene.RecipeMenu);
			RepositionVars.RecipeMenuIndex = 2;
    }

    void HandleHomeTap()
    {
        Loader.Load(Loader.Scene.TutorialLanding);
				RepositionVars.TutorialIndex = 2;
    }

    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
				RepositionVars.WelcomeScreenIndex = 4;
    }
}
