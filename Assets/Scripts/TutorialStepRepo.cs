using System;
using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
/*
 * WelcomeScreenRepo
 * TutorialLandingRepo
 * TutorialGesturesRepo
 * RecipeChooserRepo
 * RecipeInformationRepo
 * StepDisplay Repo
 *
 */
public class TutorialStepRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	private void Awake()
	{
		switch (RepositionVars.TutorialIndex)
		{
			case 0:
				WorldCanvas.transform.position = RepositionVars.TutorialGestures_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialGestures_rotation;
				break;
			
			case 1:
				WorldCanvas.transform.position = RepositionVars.TutorialSuccess_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialSuccess_rotation;
				break;
		}
		RepositionVars.TutorialStep_position = WorldCanvas.transform.position;
		RepositionVars.TutorialStep_rotation = WorldCanvas.transform.rotation;
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * RepositionVars._distance;
			RepositionVars.TutorialStep_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.TutorialStep_rotation = WorldCanvas.transform.rotation;
		}
	}
}
