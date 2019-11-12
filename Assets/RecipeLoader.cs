using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class RecipeLoader : MonoBehaviour
{
    
    #region Private Variables
    private ControllerConnectionHandler _controllerConnectionHandler;
    private GameObject _recipe1;
    private bool _recipe1Active;
    private GameObject _recipe2;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();

        _recipe1 = GameObject.Find("Viewport/Recipe 1");
        _recipe2 = GameObject.Find("Viewport/Recipe 2");
        _recipe1Active = true;

        MLInput.OnControllerButtonUp += HandleOnButtonUp;
        MLInput.OnControllerButtonDown += HandleOnButtonDown;
        
        // used for reference: https://github.com/larkintuckerllc/magic-leap-patterns/blob/master/Assets/MagicLeap/Examples/Scripts/MeshingExample.cs
        MLInput.OnControllerTouchpadGestureStart += OnTouchpadGestureStart;
    }

    // Update is called once per frame
    void Update()
    {
        updateActiveRecipe();
    }

    void updateActiveRecipe()
    {
        if (_recipe1Active)
        {
            _recipe1.GetComponent<Image>().color = new Color(0, 213, 0);
            _recipe2.GetComponent<Image>().color = new Color(245, 223, 186);
        }
        else
        {
            _recipe2.GetComponent<Image>().color = new Color(0, 213, 0);
            _recipe1.GetComponent<Image>().color = new Color(245, 223, 186);
        }
    }
    
    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= HandleOnButtonDown;
        MLInput.OnControllerButtonUp -= HandleOnButtonUp;
    }
    
    private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId &&
            button == MLInputControllerButton.Bumper)
        {
            Debug.Log("Button down");
        }
    }

    /// <summary>
    /// Handles the event for button up.
    /// </summary>
    /// <param name="controller_id">The id of the controller.</param>
    /// <param name="button">The button that is being released.</param>
    private void HandleOnButtonUp(byte controllerId, MLInputControllerButton button)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId &&
            button == MLInputControllerButton.Bumper)
        {
            Debug.Log("Button up");
            SceneManager.LoadScene("Recipe Information");
        }
    }
    
    private void OnTouchpadGestureStart(byte controller_id, MLInputControllerTouchpadGesture gesture)
    {
        if (gesture.Type == MLInputControllerTouchpadGestureType.Swipe
            && gesture.Direction == MLInputControllerTouchpadGestureDirection.Up)
        {
            _recipe1Active = !_recipe1Active;
        }
        
        if (gesture.Type == MLInputControllerTouchpadGestureType.Swipe
            && gesture.Direction == MLInputControllerTouchpadGestureDirection.Down)
        {
            _recipe1Active = !_recipe1Active;
        }
        
        
    }
}

