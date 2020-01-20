using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class UserSelection : MonoBehaviour
{
    private MLInputController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void OnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (button == MLInputControllerButton.Bumper)
        {
            Loader.Load(Loader.Scene.RecipeChooser);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
