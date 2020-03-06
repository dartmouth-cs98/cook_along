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
public class WelcomeScreenRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	void Awake()
	{

		switch (RepositionVars.WelcomeScreenIndex)
		{
			// Start
			case 0:
				WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * RepositionVars._distance;		
				WorldCanvas.transform.rotation = _camera.transform.rotation;
				break;
			
			// TutorialLanding
			case 1:
				WorldCanvas.transform.position = RepositionVars.TutorialLanding_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialLanding_rotation;
				break;
			
			// TutorialGestures
			case 2:
				WorldCanvas.transform.position = RepositionVars.TutorialGestures_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialGestures_rotation;
				break;

			// TutorialStep
			case 3:
				WorldCanvas.transform.position = RepositionVars.TutorialStep_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialStep_rotation;
				break;
		
			// TutorialSuccess
			case 4:
				WorldCanvas.transform.position = RepositionVars.TutorialSuccess_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialSuccess_rotation;
				break;

			// RecipeMenu
			case 5:
				WorldCanvas.transform.position = RepositionVars.RecipeMenu_position;
				WorldCanvas.transform.rotation = RepositionVars.RecipeMenu_rotation;
				break;
		}
		
		RepositionVars.WelcomeScreen_position = WorldCanvas.transform.position;
		RepositionVars.WelcomeScreen_rotation = WorldCanvas.transform.rotation;

		RepositionVars.LoadIndex = 0;
	}


	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * RepositionVars._distance;
			RepositionVars.WelcomeScreen_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.WelcomeScreen_rotation = WorldCanvas.transform.rotation;
		}
	}
}
