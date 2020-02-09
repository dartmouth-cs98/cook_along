using System;
using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class WelcomeStart : MonoBehaviour
{
    
    public ControlInput controlInput;

    // Start is called before the first frame update
    private void Awake()
    {
        controlInput.OnTriggerDown.AddListener(HandleTrigger);
    }

    public void HandleTrigger()
    {
        Loader.Load(Loader.Scene.RecipeChooser);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
