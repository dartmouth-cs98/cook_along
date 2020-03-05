using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionVars : MonoBehaviour
{
	public static Vector3 WelcomeScreen_position;
	public static Quaternion WelcomeScreen_rotation;

	public static Vector3 TutorialLanding_position;
	public static Quaternion TutorialLanding_rotation;

	public static Vector3 TutorialGestures_position;
	public static Quaternion TutorialGestures_rotation;

	public static Vector3 TutorialStep_position;
	public static Quaternion TutorialStep_rotation;
	
	public static Vector3	TutorialSuccess_position;
	public static Quaternion TutorialSuccess_rotation;
	
	public static Vector3 RecipeChooser_position;
	public static Quaternion RecipeChooser_rotation;

	public static Vector3 RecipeMenu_position;
	public static Quaternion RecipeMenu_rotation;
	
	public static Vector3 RecipeInformation_position;
	public static Quaternion RecipeInformation_rotation;
	
	public static Vector3 StepDisplay_position;
	public static Quaternion StepDisplay_rotation;

	// indices - keep track of which coordinates to update

	// 0 - start
	// 1 - TutorialLanding
	// 2 - TutorialGestures
	// 3 - TutorialStep
	// 4 - TutorialSuccess
	public static int WelcomeScreenIndex;
	
	// 0 - WelcomeScreen
	// 1 - StepDisplay
	// 2 - TutorialSuccess
	public static int RecipeChooserIndex;
	public static int RecipeMenuIndex;
	
	// 0 - prev
	// 1 - next
	public static int TutorialIndex;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);	
	}
}
