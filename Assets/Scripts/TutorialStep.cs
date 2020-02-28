using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{
    
    public ControlInput controlInput;
    // public Text instructionText;
    // private int _textIndex;
    // private List<string> _instructions;
    
    
    private void Awake()
    {
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        // _textIndex = 0;
        // _instructions = new List<string>()
        // {
        //     "The open hand starts or stops a timer if that step requires it. Make an open hand.",
        //     "The closed fist starts or stops the video if that step contains one. Make a closed fist.",
        //     "The okay sign takes you back to the recipe chooser. Make an okay sign.",
        //     "The thumbs up sign takes you to the next step. Make a thumbs up.",
        //     "The closed pinch resets a timer if one exits/has been started. Make a closed pinch.",
        //     "The relaxed point (L sign) takes you to the previous step. Make a relaxed point.",
        //     "The closed point (point upwards) shows you a help menu for these gestures. Make a closed point.",
        //     "Good job! Now make a thumbs up to move to the next phase of the tutorial"
        // };
    }
    
    void HandleHomeTap()
    {
        Loader.Load(Loader.Scene.TutorialGestures);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
