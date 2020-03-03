using System;
using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class WelcomeStart : MonoBehaviour
{
    
    public ControlInput controlInput;
    
    #region Private Variables
    private GameObject _startButton;
    private Image _startButtonImage;
    private GameObject _tutorialButton;
    private Image _tutorialButtonImage;
    private bool _startActivated;
    #endregion
    
    private void Awake()
    {
        controlInput.OnTriggerPressEnded.AddListener(HandleTrigger);
        controlInput.OnSwipe.AddListener(HandleSwipe);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _startButton = GameObject.Find("StartButton");
        _startButtonImage = _startButton.GetComponent<Image>();
        _tutorialButton = GameObject.Find("TutorialButton");
        _tutorialButtonImage = _tutorialButton.GetComponent<Image>();
        _startActivated = true;
    }

    void HandleTrigger()
    {
        if (_startActivated)
        {
            Loader.Load(Loader.Scene.RecipeMenu);
        }
        else
        {
            Loader.Load(Loader.Scene.TutorialLanding);
        }
        
    }
    
    void HandleSwipe(MLInputControllerTouchpadGestureDirection direction)
    {
        _startActivated = !_startActivated;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActiveButton();
    }
    
    void UpdateActiveButton()
    {
        if (_startActivated)
        {
            _startButtonImage.color = new Color(0.937f, 0.741f, 0.42f);
            _tutorialButtonImage.color = new Color(1f, 1f, 1f);
        }
        else
        {
            _tutorialButtonImage.color = new Color(0.937f, 0.741f, 0.42f);
            _startButtonImage.color = new Color(1f, 1f, 1f);
        }
    }
}
