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

	private const float _distance = 2.0f;

	void Awake()
	{

		switch (RecipeVars.WelcomeScreenIndex)
		{
			case 0:
				WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * _distance;		
				WorldCanvas.transform.rotation = _camera.transform.rotation;
				break;
			
			case 1:
				WorldCanvas.transform.position = RecipeVars.TutorialLanding_position;
				WorldCanvas.transform.rotation = RecipeVars.TutorialLanding_rotation;
				break;

			case 2:
				WorldCanvas.transform.position = RecipeVars.TutorialGestures_position;
				WorldCanvas.transform.rotation = RecipeVars.TutorialGestures_rotation;
				break;

			case 3:
				WorldCanvas.transform.position = RecipeVars.TutorialStep_position;
				WorldCanvas.transform.rotation = RecipeVars.TutorialStep_rotation;
				break;

			case 4:
				WorldCanvas.transform.position = RecipeVars.TutorialSuccess_position;
				WorldCanvas.transform.rotation = RecipeVars.TutorialSuccess_rotation;
				break;
		}
		
		RecipeVars.WelcomeScreen_position = WorldCanvas.transform.position;
		RecipeVars.WelcomeScreen_rotation = WorldCanvas.transform.rotation;
	}


	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * _distance;
			RecipeVars.WelcomeScreen_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RecipeVars.WelcomeScreen_rotation = WorldCanvas.transform.rotation;
		}
	}
}
