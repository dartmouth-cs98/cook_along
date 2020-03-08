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
public class TutorialSuccessRepo : MonoBehaviour
{
	public ControlInput controlInput;
  	public GameObject WorldCanvas;
  	public GameObject _camera;

	private void Awake()
	{
		WorldCanvas.transform.position = RepositionVars.TutorialStep_position;
		WorldCanvas.transform.rotation = RepositionVars.TutorialStep_rotation;

		RepositionVars.TutorialSuccess_position = WorldCanvas.transform.position;
		RepositionVars.TutorialSuccess_rotation = WorldCanvas.transform.rotation;

		RepositionVars.LoadIndex = 4;
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * RepositionVars._distance;
			RepositionVars.TutorialSuccess_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.TutorialSuccess_rotation = WorldCanvas.transform.rotation;
		}
	}
}
