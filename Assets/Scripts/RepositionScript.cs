using System;
using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class RepositionScript : MonoBehaviour
{

	public ControlInput controlInput;
  public GameObject WorldCanvas;
  public MLPersistentBehavior persistentBehavior;
  public GameObject Camera;

	private const float _distance = 2.0f;

	private void Awake()
	{
		controlInput.OnBumperDown.AddListener(HandleBumperDown);
	}
	public void HandleBumperDown()
  {
		WorldCanvas.transform.position = Camera.transform.position + Camera.transform.forward * _distance;
		WorldCanvas.transform.rotation = Camera.transform.rotation;
		persistentBehavior.UpdateBinding();
	}

	// Update is called once per frame
	void Update()
	{
	}
}
