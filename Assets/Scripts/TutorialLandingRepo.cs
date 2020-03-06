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
public class TutorialLandingRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	private void Awake()
	{
		switch (RepositionVars.TutorialIndex)
		{
			case 0:
				WorldCanvas.transform.position = RepositionVars.WelcomeScreen_position;
				WorldCanvas.transform.rotation = RepositionVars.WelcomeScreen_rotation;
				break;

			case 1:
				WorldCanvas.transform.position = RepositionVars.TutorialGestures_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialGestures_rotation;
				break;

			case 2:
				WorldCanvas.transform.position = RepositionVars.TutorialSuccess_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialSuccess_rotation;
				break;
		}

		RepositionVars.TutorialLanding_position = WorldCanvas.transform.position;
		RepositionVars.TutorialLanding_rotation = WorldCanvas.transform.rotation;

		RepositionVars.LoadIndex = 1;
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * RepositionVars._distance;
			RepositionVars.TutorialLanding_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.TutorialLanding_rotation = WorldCanvas.transform.rotation;
		}
	}
}
