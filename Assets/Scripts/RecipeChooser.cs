using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class RecipeChooser : MonoBehaviour
{
    
    public ControlInput controlInput;
    
    #region Private Variables
    private GameObject _recipe1;
    private Image _recipe1Image;
    public static bool Recipe1Active;
    private GameObject _recipe2;
    private Image _recipe2Image;
    #endregion
    
    private void Awake()
    {
        controlInput.OnTriggerDown.AddListener(HandleTrigger);
        controlInput.OnSwipe.AddListener(HandleSwipe);
    }


    void Start()
    {
        _recipe1 = GameObject.Find("Viewport/Recipe 1");
        _recipe1Image = _recipe1.GetComponent<Image>();
        _recipe2 = GameObject.Find("Viewport/Recipe 2");
        _recipe2Image = _recipe2.GetComponent<Image>();
        Recipe1Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        updateActiveRecipe();
    }

    void HandleTrigger()
    {
        Loader.Load(Loader.Scene.RecipeInformation);
    }
    
    void HandleSwipe(MLInputControllerTouchpadGestureDirection direction)
    {
        Recipe1Active = !Recipe1Active;
    }

    void updateActiveRecipe()
    {
        if (Recipe1Active)
        {
            _recipe1Image.color = new Color(0.937f, 0.741f, 0.42f);
            _recipe2Image.color = new Color(1f, 1f, 1f);
        }
        else
        {
            _recipe2Image.color = new Color(0.937f, 0.741f, 0.42f);
            _recipe1Image.color = new Color(1f, 1f, 1f);
        }
    }
}

