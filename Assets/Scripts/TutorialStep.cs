using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{
    
    public ControlInput controlInput;
    public Text instructionText;
    private int _textIndex;
    private List<string> _instructions;
    
    
    private void Awake()
    {
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        controlInput.OnDoubleTap.AddListener(HandleDoubleTap);
        controlInput.OnTriggerPressEnded.AddListener(HandleTrigger);
        _textIndex = 0;
        _instructions = new List<string>()
        {
            "Swipe left to access list of steps. Scroll up and down to choose a step. Pull trigger to go to that step.",
            "Swipe up to access the list of timers. Scroll left and right to switch between timers. Use the open hand to start/top and the pinch to reset."
        };
    }
    
    void HandleHomeTap()
    {
        if (_textIndex == 0)
        {
            Loader.Load(Loader.Scene.TutorialGestures); 
        }
        else
        {
            PreviousInstruction();
        }

    }

    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
    }

    void HandleTrigger()
    {
        if (_textIndex >= _instructions.Count - 1)
        {
            Loader.Load(Loader.Scene.TutorialSuccess);
        }
        else
        {
            NextInstruction();
        }
    }

    void NextInstruction()
    {
        _textIndex += 1;
        instructionText.text = _instructions[_textIndex];
    }

    void PreviousInstruction()
    {
        _textIndex -= 1;
        instructionText.text = _instructions[_textIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
