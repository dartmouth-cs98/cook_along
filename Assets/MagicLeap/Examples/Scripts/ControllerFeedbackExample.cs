// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2019 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Collections;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// This class provides examples of how you can use haptics and LEDs
    /// on the Control.
    /// </summary>
    [RequireComponent(typeof(ControllerConnectionHandler))]
    public class ControllerFeedbackExample : MonoBehaviour
    {
        #region Private Variables
        private ControllerConnectionHandler _controllerConnectionHandler;
        private Canvas _canvas;
        private Button _button;
        
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initialize variables, callbacks and check null references.
        /// </summary>
        void Start()
        {
            _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
            _canvas = GetComponent<Canvas>();
            GameObject buttonObject = GameObject.Find("Button");
            _button = buttonObject.GetComponent<Button>();

            MLInput.OnControllerButtonUp += HandleOnButtonUp;
            MLInput.OnControllerButtonDown += HandleOnButtonDown;
            MLInput.OnTriggerDown += HandleOnTriggerDown;

        }

        /// <summary>
        /// Update controller input based feedback.
        /// </summary>
        void Update()
        {
        }

        /// <summary>
        /// Stop input api and unregister callbacks.
        /// </summary>
        void OnDestroy()
        {
            MLInput.OnTriggerDown -= HandleOnTriggerDown;
            MLInput.OnControllerButtonDown -= HandleOnButtonDown;
            MLInput.OnControllerButtonUp -= HandleOnButtonUp;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the event for button down.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="button">The button that is being pressed.</param>
        private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
        {
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if (controller != null && controller.Id == controllerId &&
                button == MLInputControllerButton.Bumper)
            {
                Debug.Log("Clicked bumper down from feedback script");
                _button.onClick.Invoke();
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
                Debug.Log("Bumper released from feedback script");
            }
        }

        /// <summary>
        /// Handles the event for trigger down.
        /// </summary>
        /// <param name="controller_id">The id of the controller.</param>
        /// <param name="value">The value of the trigger button.</param>
        private void HandleOnTriggerDown(byte controllerId, float value)
        {
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if (controller != null && controller.Id == controllerId)
            {
                Debug.Log("Trigger Down");
            }
        }
        #endregion
    }
}
