﻿/**
 * Magic Leap tutorial from
 * https://creator.magicleap.com/learn/guides/gestures-in-unity
 */
/**
 * Plan for toggling through steps:
 * Consolidate steps in a single folder
 * Load steps from folder
 * Use gestures to move through steps (scenes)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class GestureScript : MonoBehaviour {
	
	private bool OKHandPose = false;
	private MLHandKeyPose[] gestures;	// Holds the different gestures we will look for
	private AssetBundle myLoadedAssetBundle;
  private string[] stepPaths;
	private int stepIndex; // marks the recipe step

	void Start () {
		MLHands.Start();

		gestures = new MLHandKeyPose[2];
		
		gestures[0] = MLHandKeyPose.Ok;
		gestures[1] = MLHandKeyPose.Thumb;
		
		MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);

		// myLoadedAssetBundle = AssetBundle.LoadFromFile("steps");
		// stepPaths = myLoadedAssetBundle.GetAllScenePaths();
	}

	void onDestroy () {
		MLHands.Stop();
	}

	void Update () {
		// TODO: 
		// test this out on MagicLeap.
		// Extend this to toggling through steps
		if (GetOkay()) {	
			SceneManager.LoadScene("Recipe Information");
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
		|| GetGesture(MLHands.Left, MLHandKeyPose.Ok)
		|| (GetGesture(MLHands.Right, MLHandKeyPose.Thumb) 
		|| GetGesture(MLHands.Right, MLHandKeyPose.Ok) {
			return true;
		}

		else {
			return false;
		}
	}

}