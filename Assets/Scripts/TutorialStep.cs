using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{
    
    public ControlInput controlInput;
    public Text instructionText;
    public Image screenshot;
    public Sprite buttons;
    public Sprite activateList;
    public Sprite scrollList;
    public Sprite deactivateList;
    public Sprite thumbL;
    public Sprite timers;
    public Sprite activateTimer;
    public Sprite scrollTimer;
    public Sprite openHandPinch;
    public Sprite exitTimer;
    private List<Sprite> _images;
    
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
            "Step List: \n You can use the step list to jump to any step in the recipe at any time.",
            "Step List: \n Swipe left on the touchpad to activate step selection.",
            "Step List: \n Once activated, scroll up and down on the touchpad to choose which step to go to. Pull trigger to go to that step.",
            "Step List: \n You can exit step selection at any time by swiping right on the touchpad.",
            "Step List: \n If you make either the thumbs-up or L gesture, step selection will automatically exit.",
            "Timers: \n Your currently active timers are located at the top of the screen.",
            "Timers: \n To control these timers, swipe up to activate timer selection.",
            "Timers: \n Swipe right and left to choose the timer",
            "Timers: \n Remember, use the open hand to start/stop the selected timer and the pinch to reset timer.",
            "Timers: \n Swipe down to exit timer selection."
        };

        _images = new List<Sprite>()
        {
            buttons, deactivateList, activateList, scrollList, deactivateList, thumbL, timers, activateTimer, scrollTimer,
            openHandPinch, exitTimer
        };
    }
    
    void HandleHomeTap()
    {
        if (_textIndex == 0)
        {
            Loader.Load(Loader.Scene.TutorialGestures); 
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
            Loader.Load(Loader.Scene.TutorialSuccess);
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
        screenshot.sprite = _images[_textIndex];
        SetPicSize	();
    }

    void PreviousInstruction()
    {
        _textIndex -= 1;
        instructionText.text = _instructions[_textIndex];
        screenshot.sprite = _images[_textIndex];
        SetPicSize	();
    }

    void SetPicSize()
    {
        if (SquarePic(_textIndex))
        {
            screenshot.rectTransform.sizeDelta = new Vector2	(200, 200);
        }
        else
        {
            screenshot.rectTransform.sizeDelta = new Vector2	(350, 200);
        }
    }

    bool SquarePic(int i)
    {
        return i == 5 || i == 9;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
