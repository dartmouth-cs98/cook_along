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
public class StepDisplayRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	private const float _distance = 2.0f;

	private void Awake()
	{
		WorldCanvas.transform.position = RepositionVars.RecipeInformation_position;
		WorldCanvas.transform.rotation = RepositionVars.RecipeInformation_rotation;

		RepositionVars.StepDisplay_position = WorldCanvas.transform.position;
		RepositionVars.StepDisplay_rotation = WorldCanvas.transform.rotation;

		RepositionVars.RecipeChooserIndex = 1;
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * _distance;
			RepositionVars.StepDisplay_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.StepDisplay_rotation = WorldCanvas.transform.rotation;
		}
	}
}
