using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class UserSelection : MonoBehaviour
{
    private MLInputController _controller;

    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    void UpdateButtonInfo()
    {
        RaycastHit hit;
        if (Physics.Raycast(_controller.Position, transform.forward, out hit))
        {
            if (hit.transform.gameObject.tag == "Button")
            {
                if (_controller.TriggerValue > 0.8f)
                {
                    Loader.Load(Loader.Scene.RecipeChooser);
                }

                hit.transform.gameObject.GetComponent<Image>().color = Color.green;
            }
        }
    }

    void OnDestroy () {
        //Stop receiving input by the Control
        MLInput.Stop();
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateButtonInfo();
    }
}
