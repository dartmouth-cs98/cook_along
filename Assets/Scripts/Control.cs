using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
public class Control : MonoBehaviour {

  #region Public Variables  
  public enum Mode {LOOSE, SOFT, HARD};
  public Mode WorldMode;
  public GameObject WorldCanvas;
  public MLPersistentBehavior persistentBehavior;
  public GameObject Light;
  public GameObject Camera;
  #endregion
  
  #region Private Variables
  private HeadLockScript _headlock;
  private const float _triggerThreshold = 0.2f;
  private const float _rotspeed = 10.0f;
  private const float _distance = 2.0f;
  private bool _triggerPressed = false;
  private MLInputController _control;
  #endregion
  
  #region Unity Methods
  private void Start() {
    // Get the HeadLockScript script
    // Setup and Start Magic Leap input and add a button event (will be used for the HomeTap)
    MLInput.Start();
    MLInput.OnControllerButtonUp += OnButtonUp;
    _control = MLInput.GetController(MLInput.Hand.Left);
	WorldMode = Mode.SOFT;
    // Reset the scene
    Reset();
  }
  private void Update() {
	
	CheckControl();

    // Update the head lock state
    CheckStates();
    
  }
  private void OnDestroy () {
    MLInput.OnControllerButtonUp -= OnButtonUp;
    MLInput.Stop();
  }
  #endregion
  
  #region Private Methods
  /// CheckStates
  /// Switch headlock mode depending on the world mode
  ///
  private void CheckStates() {
    if (WorldMode == Mode.LOOSE) {
      _headlock.HeadLock(WorldCanvas, 1.75f);
    }
    else if (WorldMode == Mode.SOFT) {
      _headlock.HeadLock(WorldCanvas, 5.0f);
     }
     else {
       _headlock.HardHeadLock(WorldCanvas);
     }
   }
   /// CheckControl
   /// Monitor the trigger input to "increment" the  world mode
   ///
   private void CheckControl() {
     	if (_control.TriggerValue > _triggerThreshold) {
       		_triggerPressed = true;
			WorldCanvas.transform.position = Camera.transform.position + Camera.transform.forward * _distance;
			WorldCanvas.transform.rotation = Camera.transform.rotation;
			persistentBehavior.UpdateBinding();
     	}
     	//else if (_control.TriggerValue == 0.0f && _triggerPressed) {
		//	_triggerPressed = false;
		//}   
	}
    
   /// Reset
   /// Resets the scene back to the starting Instruction screen
   ///    
   private void Reset() {
     WorldMode = Mode.LOOSE;
   }
   

   /// OnButtonUp
   /// Button event - reset scene when home button is tapped
   ///
   private void OnButtonUp(byte controller_id, MLInputControllerButton button) {
     if (button == MLInputControllerButton.HomeTap) {
       Reset();
     }
   }
   #endregion
}
