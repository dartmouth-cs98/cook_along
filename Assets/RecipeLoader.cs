using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class RecipeLoader : MonoBehaviour
{
    
    #region Private Variables
    MLInputController _controller;
    private GameObject _recipe1;
    private Image _recipe1Image;
    public static bool Recipe1Active;
    private GameObject _recipe2;
    private Image _recipe2Image;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);

        _recipe1 = GameObject.Find("Viewport/Recipe 1");
        _recipe1Image = _recipe1.GetComponent<Image>();
        _recipe2 = GameObject.Find("Viewport/Recipe 2");
        _recipe2Image = _recipe2.GetComponent<Image>();
        Recipe1Active = true;

        // used for reference: https://github.com/larkintuckerllc/magic-leap-patterns/blob/master/Assets/MagicLeap/Examples/Scripts/MeshingExample.cs
        MLInput.OnControllerTouchpadGestureStart += OnTouchpadGestureStart;
    }

    // Update is called once per frame
    void Update()
    {
        updateActiveRecipe();
        if (_controller	 != null && _controller.TriggerValue > 0.2f) {
            Debug.Log	("Should hypothetically go to recipe info now");
            SceneManager.LoadScene("Recipe Information");
        }
        
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
    
    void OnDestroy()
    {
        MLInput.OnControllerTouchpadGestureStart -= OnTouchpadGestureStart;
        MLInput.Stop();
    }
    
    
    private void OnTouchpadGestureStart(byte controller_id, MLInputControllerTouchpadGesture gesture)
    {
        if (gesture.Type == MLInputControllerTouchpadGestureType.Swipe
            && gesture.Direction == MLInputControllerTouchpadGestureDirection.Up)
        {
            Recipe1Active = !Recipe1Active;
        }
        
        if (gesture.Type == MLInputControllerTouchpadGestureType.Swipe
            && gesture.Direction == MLInputControllerTouchpadGestureDirection.Down)
        {
            Recipe1Active = !Recipe1Active;
        }
    }
}

