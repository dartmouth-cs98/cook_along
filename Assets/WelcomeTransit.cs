using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class WelcomeTransit : MonoBehaviour
{
    
    #region Private Variables
    MLInputController _controller;
    #endregion
    
    
    // Start is called before the first frame update
    void Start()
    {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller	 != null && _controller.TriggerValue > 0.2f)
        {
            SceneManager.LoadSceneAsync("Recipe Chooser");
            Hold(1);
        }
    }
    
    void OnDestroy()
    {
        MLInput.Stop();
    }
    
    void Hold(int delay)
    {
        Stopwatch stopWatch = new Stopwatch();

        stopWatch.Start();
        float curr = stopWatch.ElapsedMilliseconds / 1000;

        while ( curr < delay) {
            curr = stopWatch.ElapsedMilliseconds / 1000;
        }
        stopWatch.Stop();
    }
}
