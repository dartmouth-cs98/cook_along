using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class WelcomeStart : MonoBehaviour
{
    
    private MLInputController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }
    
    void OnDestroy () {
        //Stop receiving input by the Control
        MLInput.Stop();
    }
    
    void UpdateButtonInfo()
    {
        if (_controller.TriggerValue > 0.8f)
        {
            Loader.Load(Loader.Scene.RecipeChooser);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonInfo();
    }
}
