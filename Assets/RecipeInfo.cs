using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class RecipeInfo : MonoBehaviour
{
    
    #region Private Variables
    private ControllerConnectionHandler _controllerConnectionHandler;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
        Debug.Log("Below is static variable passed through: ");
        Debug.Log(RecipeLoader.Recipe1Active);

        MLInput.OnControllerButtonUp += HandleOnButtonUp;
        MLInput.OnControllerButtonDown += HandleOnButtonDown;
        
        // used for reference: https://github.com/larkintuckerllc/magic-leap-patterns/blob/master/Assets/MagicLeap/Examples/Scripts/MeshingExample.cs
        MLInput.OnControllerTouchpadGestureStart += OnTouchpadGestureStart;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= HandleOnButtonDown;
        MLInput.OnControllerButtonUp -= HandleOnButtonUp;
        MLInput.OnControllerTouchpadGestureStart -= OnTouchpadGestureStart;
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
            button == MLInputControllerButton.HomeTap)
        {
            Debug.Log("Button up");
            SceneManager.LoadScene("Recipe Chooser");
        }
    }
    
    private void OnTouchpadGestureStart(byte controller_id, MLInputControllerTouchpadGesture gesture)
    {
        if (gesture.Type == MLInputControllerTouchpadGestureType.Swipe
            && gesture.Direction == MLInputControllerTouchpadGestureDirection.Up)
        {
            
        }
        
        if (gesture.Type == MLInputControllerTouchpadGestureType.Swipe
            && gesture.Direction == MLInputControllerTouchpadGestureDirection.Down)
        {
            
        }
    }
}
