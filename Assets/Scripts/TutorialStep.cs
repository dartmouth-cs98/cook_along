using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            "To move through the instructions in this section, pull the trigger. To go back, click the home button",
            "Step List: \n You can jump to any step in the recipe at any time. Swipe left on the touchpad to activate step selection.",
            "Step List: \n Once activated, scroll up and down on the touchpad to choose which step to go to. Pull trigger to go to that step.",
            "Step List: \n You can exit step selection at any time by swiping right on the touchpad.",
            "Step List: \n If you make either the thumbs-up or L gesture, step selection will automatically exit.",
            "Timers: \n Your currently active timers are located at the top of the screen.",
            "Timers: \n By default, your most recently created timer is selected.",
            "Timers: \n To change that, swipe up to activate timer selection. Swipe down to exit timer selection.",
            "Timers: \n Swipe right and left to choose the timer. Pull trigger to select.",
            "Timers: \n Remember, use the open hand to start/stop the selected timer and the pinch to reset timer."
        };
    }
    
    void HandleHomeTap()
    {
        if (_textIndex == 0)
        {
            SceneManager.LoadScene(Loader.Scene.TutorialGestures.ToString());
            RepositionVars.TutorialIndex = 1;
        }
        else
        {
            PreviousInstruction();
        }

    }

    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
				RepositionVars.WelcomeScreenIndex = 3;
    }

    void HandleTrigger()
    {
        if (_textIndex >= _instructions.Count - 1)
        {
            SceneManager.LoadScene(Loader.Scene.TutorialSuccess.ToString());
            RepositionVars.TutorialIndex = 0;
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
