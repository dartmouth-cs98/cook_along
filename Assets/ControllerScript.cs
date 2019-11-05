using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class ControllerScript : MonoBehaviour
{
    //private GameObject _cube;
    private MLInputController _controller;
    private bool _bumper = false;
    private bool _home = false;

    void Start()
    {
        // probably link canvas somehow
        //_cube = GameObject.Find("Cube");
        //_cube.SetActive(false);
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.Stop();
    }

    void Update()
    {
        if (_bumper)
        {
             
        }
    }

    void OnButtonDown(byte controller_id, MLInputControllerButton button)
    {
        if ((button == MLInputControllerButton.Bumper))
        {
            if (_bumper)
            {
                _bumper = false;
            }
            else
            {
                _bumper = true;
            }
        }
    }
}