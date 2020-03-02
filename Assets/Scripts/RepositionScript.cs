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
  public GameObject _camera;

	private const float _distance = 2.0f;

	private void Awake()
	{
		controlInput.OnBumperDown.AddListener(HandleBumperDown);
	}
/*	void start()
	{
		WorldCanvas = GameObject.Find("Canvas");
		_camera = Camera.main.gameObject;
	}*/
	public void HandleBumperDown()
  {
	}

	// Update is called once per frame
	void Update()
	{
		if (controlInput.Bumper) {
			WorldCanvas.transform.position = _camera.transform.position + _camera.transform.forward * _distance;
			WorldCanvas.transform.rotation = _camera.transform.rotation;
			persistentBehavior.UpdateBinding();
		}
	}
}
