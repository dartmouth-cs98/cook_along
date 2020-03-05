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
public class RecipeInformationRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	private const float _distance = 2.0f;

	private void Awake()
	{
		WorldCanvas.transform.position = RepositionVars.RecipeMenu_position;
		WorldCanvas.transform.rotation = RepositionVars.RecipeMenu_rotation;

		RepositionVars.RecipeInformation_position = WorldCanvas.transform.position;
		RepositionVars.RecipeInformation_rotation = WorldCanvas.transform.rotation;
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * _distance;
			RepositionVars.RecipeInformation_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.RecipeInformation_rotation = WorldCanvas.transform.rotation;
		}
	}
}
