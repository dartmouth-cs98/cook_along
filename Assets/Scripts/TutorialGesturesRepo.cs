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
public class TutorialGesturesRepo : MonoBehaviour
{
	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public GameObject _camera;

	private void Awake()
	{
		switch (RepositionVars.TutorialIndex)
		{
			case 0:
				WorldCanvas.transform.position = RepositionVars.TutorialLanding_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialLanding_rotation;
				break;
			
			case 1:
				WorldCanvas.transform.position = RepositionVars.TutorialStep_position;
				WorldCanvas.transform.rotation = RepositionVars.TutorialStep_rotation;
				break;
		}

		RepositionVars.TutorialGestures_position = WorldCanvas.transform.position;
		RepositionVars.TutorialGestures_rotation = WorldCanvas.transform.rotation;

		RepositionVars.LoadIndex = 2;
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * RepositionVars._distance;
			RepositionVars.TutorialGestures_position = WorldCanvas.transform.position;

			WorldCanvas.transform.rotation = _camera.transform.rotation;
			RepositionVars.TutorialGestures_rotation = WorldCanvas.transform.rotation;
		}
	}
}
