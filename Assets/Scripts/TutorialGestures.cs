using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGestures : MonoBehaviour
{
    
    public ControlInput controlInput;
    public Text instructionText;
    // private int _textIndex;
    // private List<string> _instructions;
    
    private void Awake()
    {
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
        controlInput.OnDoubleTap.AddListener(HandleDoubleTap);
        // _textIndex = 0;
        // _instructions = new List<string>()
        // {
        //     "Under development",
        // };
    }

    void HandleHomeTap()
    {
        Loader.Load(Loader.Scene.TutorialLanding);
    }
    
    void HandleDoubleTap(Vector4 param)
    {
        Loader.Load(Loader.Scene.WelcomeScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
