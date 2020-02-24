using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class TutorialGestures : MonoBehaviour
{
    
    public ControlInput controlInput;
    public Text instructionText;
    public Image gestureImage;
    private int _textIndex;
    private List<string> _instructions;
    private MLHandKeyPose[] _gestures;   // Holds the different gestures we will look for
    

    private void Awake()
    {
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        controlInput.OnDoubleTap.AddListener(HandleDoubleTap);
        _textIndex = 0;
        _instructions = new List<string>()
        {
            "The open hand starts or stops a timer if that step requires it. Make an open hand.",
            "The closed fist starts or stops the video if that step contains one. Make a closed fist.",
            "The okay sign takes you back to the recipe chooser. Make an okay sign.",
            "The thumbs up sign takes you to the next step. Make a thumbs up.",
            "The closed pinch resets a timer if one exits/has been started. Make a closed pinch.",
            "The relaxed point (L sign) takes you to the previous step. Make a relaxed point.",
            "The closed point (point upwards) shows you a help menu for these gestures. Make a closed point."
        };
        StartMlHands();
    }
    
    void onDestroy () {
        MLHands.Stop();
    }

    void StartMlHands()
    {
        MLHands.Start();
        _gestures = new MLHandKeyPose[7];
        _gestures[0] = MLHandKeyPose.Ok;
        _gestures[1] = MLHandKeyPose.Thumb;
        _gestures[2] = MLHandKeyPose.L;
        _gestures[3] = MLHandKeyPose.OpenHand;
        _gestures[4] = MLHandKeyPose.Pinch;
        _gestures[5] = MLHandKeyPose.Finger;
        _gestures[6]= MLHandKeyPose.Fist;
        MLHands.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
    }


    bool CheckGesture(MLHandKeyPose type)
    {
        return (DoesGestureMatch(MLHands.Left, type) || DoesGestureMatch(MLHands.Right, type));
    }
    
    bool DoesGestureMatch(MLHand hand, MLHandKeyPose type)
    {
        return (hand != null && hand.KeyPose == type && hand.KeyPoseConfidence > 0.9f);
    }

    void HandleHomeTap()
    {
        PreviousGesture();
    }

    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
    }
    
    void NextGesture()
    {
        _textIndex += 1;
        instructionText.text = _instructions[_textIndex];
    }

    void PreviousGesture()
    {
        _textIndex -= 1;
        instructionText.text = _instructions[_textIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
