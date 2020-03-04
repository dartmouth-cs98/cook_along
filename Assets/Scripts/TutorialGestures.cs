using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class TutorialGestures : MonoBehaviour
{
    
    public ControlInput controlInput;
    public Text instruction;
    public Image gestureImage;
    public Sprite openHand;
    public Sprite fist;
    public Sprite okay;
    public Sprite thumb;
    public Sprite pinch;
    public Sprite l;
    public Sprite finger;
    public Sprite check;
    private int _textIndex;
    private List<string> _instructions;
    private List<Sprite> _images;
    private bool _checkForGesture;
    
    private MLHandKeyPose[] _gestures;   // Holds the different gestures we will look for
    

    private void Awake()
    {
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        controlInput.OnDoubleTap.AddListener(HandleDoubleTap);
        _textIndex = 0;
        _instructions = new List<string>()
        {
            "The thumbs up sign takes you to the next step. Make a thumbs up.",
            "The relaxed point (L sign) takes you to the previous step. Make a relaxed point.",
            "The open hand starts or stops a timer if that step requires it. Make an open hand.",
            "The closed pinch resets a timer if one has been started. Make a closed pinch.",
            "The closed fist starts or stops the video if that step contains one. Make a closed fist.",
            "The okay sign exits back to the recipe menu. Make an okay sign.",
            "The closed point (point upwards) shows you a help menu for these gestures. Make a closed point.",
            "Good job! Now make a thumbs up to move to the next phase of the tutorial"
        };
        _images = new List<Sprite>()
        {
            thumb, l, openHand, pinch, fist, okay, finger, check
        };
        _checkForGesture = true;
        StartMlHands();
    }
    
    void OnDestroy () {
        MLHands.Stop();
    }

    void StartMlHands()
    {
        MLHands.Start();
        _gestures = new MLHandKeyPose[7];
        _gestures[0] = MLHandKeyPose.Thumb;
        _gestures[1] = MLHandKeyPose.L;
        _gestures[2] = MLHandKeyPose.OpenHand;
        _gestures[3] = MLHandKeyPose.Pinch;
        _gestures[4] = MLHandKeyPose.Fist;
        _gestures[5] = MLHandKeyPose.Ok;
        _gestures[6]= MLHandKeyPose.Finger;
        MLHands.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
    }


    bool CheckForGesture(MLHandKeyPose type)
    {
        return (DoesGestureMatch(MLHands.Left, type) || DoesGestureMatch(MLHands.Right, type));
    }
    
    bool DoesGestureMatch(MLHand hand, MLHandKeyPose type)
    {
        return (hand != null && hand.KeyPose == type && hand.KeyPoseConfidence > 0.9f);
    }

    void HandleHomeTap()
    {
        if (_textIndex == 0)
        {
            Loader.Load(Loader.Scene.TutorialLanding);
        }
        else
        {
            PreviousGesture();
        }
        
    }

    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
    }
    
    IEnumerator NextGesture()
    {
        yield return new WaitForSeconds(2);
        _textIndex += 1;
        instruction.text = _instructions[_textIndex];
        gestureImage.sprite = _images[_textIndex];
        _checkForGesture = true;
    }

    void PreviousGesture()
    {
        if (_textIndex != 0)
        {
            _textIndex -= 1;
            instruction.text = _instructions[_textIndex];
            gestureImage.sprite = _images[_textIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_checkForGesture && _textIndex < _gestures.Length && CheckForGesture(_gestures[_textIndex]))
        {
            instruction.text = "That's perfect!";
            _checkForGesture = false;
            StartCoroutine(NextGesture());
        }

        if (_checkForGesture && _textIndex == _gestures.Length && CheckForGesture(MLHandKeyPose.Thumb))
        {
          Loader.Load(Loader.Scene.TutorialStep);  
        }
    }
}
