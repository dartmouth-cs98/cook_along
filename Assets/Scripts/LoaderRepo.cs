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
public class LoaderRepo : MonoBehaviour
{
	public Vector3 _position;
	public Quaternion _rotation;

  	public GameObject WorldCanvas;
  	public GameObject _camera;
	
	private void Awake()
	{
		switch (RepositionVars.LoadIndex)
		{
			// WelcomeScreen
			case 0:
				_position = RepositionVars.WelcomeScreen_position;
				_rotation = RepositionVars.WelcomeScreen_rotation; 
				break;

			// TutorialLanding
			case 1:
				_position = RepositionVars.TutorialLanding_position;
				_rotation = RepositionVars.TutorialLanding_rotation; 
				break;
			
			// TutorialGestures
			case 2:
				_position = RepositionVars.TutorialGestures_position;
				_rotation = RepositionVars.TutorialGestures_rotation; 
				break;

			// TutorialStep
			case 3:
				_position = RepositionVars.TutorialStep_position;
				_rotation = RepositionVars.TutorialStep_rotation; 
				break;

			// TutorialSuccess
			case 4:
				_position = RepositionVars.TutorialSuccess_position;
				_rotation = RepositionVars.TutorialSuccess_rotation; 
				break;

			// RecipeMenu
			case 5:
				_position = RepositionVars.RecipeMenu_position;
				_rotation = RepositionVars.RecipeMenu_rotation; 
				break;

			// RecipeInformation
			case 6:
				_position = RepositionVars.RecipeInformation_position;
				_rotation = RepositionVars.RecipeInformation_rotation; 
				break;

			// StepDisplay
			case 7:
				_position = RepositionVars.StepDisplay_position;
				_rotation = RepositionVars.StepDisplay_rotation; 
				break;
		}

		WorldCanvas.transform.position = _position;
		WorldCanvas.transform.rotation = _rotation;
	}
}
