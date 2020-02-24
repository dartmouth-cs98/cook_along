using System;
using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLanding : MonoBehaviour
{
    
    public ControlInput controlInput;
    public Text instructionText;
    private int _textIndex;
    private List<string> _instructions;

    private void Awake()
    {
        controlInput.OnTriggerPressEnded.AddListener(HandleTrigger);
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        controlInput.OnDoubleTap.AddListener(HandleDoubleTap);
        controlInput.OnBumperHold.AddListener(HandleBumperHold);
        _textIndex = 0;
        _instructions = new List<string>()
        {
            "The trigger selects an item. Pull the trigger.",
            "The home button moves you back a screen. Hit the home button",
            "Holding the bumper repositions the screen. The screen will be placed on release. Hold the bumper.",
            "Great job! Pull the trigger to move to the next phase of the tutorial. Press the home button to start this section again from the beginning."
        };
    }

    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
    }

    void HandleTrigger()
    {
        if (_textIndex == 0)
        {
            UpdateInstructions();
        }

        if (_textIndex == 3)
        {
            Loader.Load(Loader.Scene.TutorialGestures);
        }
        
    }

    void HandleHomeTap()
    {
        if (_textIndex == 1)
        {
            UpdateInstructions();
        }

        if (_textIndex == 3)
        {
            _textIndex = -1;
            UpdateInstructions();
        }
    }
    
    void HandleBumperHold()
    {
        if (_textIndex == 2)
        {
            UpdateInstructions();
        }
    }
    
    void UpdateInstructions()
    {
        _textIndex += 1;
        instructionText.text = _instructions[_textIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
