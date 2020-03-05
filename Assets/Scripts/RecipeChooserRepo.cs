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
public class RecipeChooserRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	private const float _distance = 2.0f;

	private void Awake()
	{
	switch (RecipeChooserTranIndex)
	{
		case 0:
			WorldCanvas.transform.position = RepositionVars.WelcomeScreenRepo_position;
			WorldCanvas.transform.rotation = RepositionVars.WelcomeScreenRepo_rotation;
			break;

		case 1:
			WorldCanvas.transform.position = RepositionVars.StepDisplayRepo_position;
			WorldCanvas.transform.rotation = RepositionVars.StepDisplayRepo_rotation;
	}

		RepositionVars.RecipeChooser_position = WorldCanvas.transform.position;
		RepositionVars.RecipeChooser_rotation = WorldCanvas.transform.rotation;

	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * _distance;
			RepositionVars.RecipeChooser_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.RecipeChooser_rotation = WorldCanvas.transform.rotation;
		}
	}
}
