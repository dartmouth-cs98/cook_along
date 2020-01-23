using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR.WSA;
public class Control : MonoBehaviour {

  #region Public Variables
  public enum Mode {LOOSE, SOFT, HARD};
  public Mode WorldMode;
 // public Material FrameMat;
  public GameObject WorldCanvas;
  public GameObject Text;
  public GameObject Light;
  public GameObject Camera;
  #endregion

  #region Private Variables
  private HeadLockScript _headlock;
  private const float _triggerThreshold = 0.2f;
  private const float _rotspeed = 10.0f;
  private bool _triggerPressed = false;
  private MLInputController _control;
  private MLHandKeyPose[] gestures;	// Holds the different gestures we will look for
  private WorldAnchor anchor;
  #endregion

  #region Unity Methods
  private void Start() {
    // Get the HeadLockScript script
    _headlock = GetComponentInChildren<HeadLockScript>();
    // Setup and Start Magic Leap input and add a button event (will be used for the HomeTap)
    MLInput.Start();
    MLInput.OnControllerButtonUp += OnButtonUp;
    _control = MLInput.GetController(MLInput.Hand.Left);
	
	gestures = new MLHandKeyPose[1];
	gestures[0] = MLHandKeyPose.Ok;
	MLHands.Start();
	MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);
    // Reset the scene
    Reset();
  }
  private void Update() {
    // Check the inputs and update the scene
    CheckControl();

    // Update the head lock state
    CheckStates();

	if(GetOkay()) {
		anchor = WorldCanvas.gameObject.AddComponent<WorldAnchor>();
	}
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

   /// Reset
   /// Resets the scene back to the starting Instruction screen
   ///
   private void Reset() {
     WorldMode = Mode.LOOSE;
   }

   /// CheckControl
   /// Monitor the trigger input to "increment" the  world mode
   ///
   private void CheckControl() {
     if (_control.TriggerValue > _triggerThreshold) {
       _triggerPressed = true;
     }
     else if (_control.TriggerValue == 0.0f && _triggerPressed) {
       _triggerPressed = false;
       if (WorldMode == Mode.LOOSE) {
         WorldMode = Mode.SOFT;
       }
       else if (WorldMode == Mode.SOFT) {
         WorldMode = Mode.HARD;
       }
       else {
         WorldMode = Mode.LOOSE;
       }
     }
   }

	bool GetGesture(MLHand hand, MLHandKeyPose type)	{
		
		if (hand != null) {
			if (hand.KeyPose == type) {
				if (hand.KeyPoseConfidence > 0.9f) {
					return true;
				}
			}
		}

		return false;
	}

	// Cleans up logic for reading the 'All Good' gesture
	bool GetOkay() {
		
		if (GetGesture(MLHands.Left, MLHandKeyPose.Thumb)
			|| GetGesture(MLHands.Right, MLHandKeyPose.Thumb)) {
			return true;
		}

		else {
			return false;
		}
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
