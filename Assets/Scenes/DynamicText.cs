// Code inpisred by: https://www.studytonight.com/game-development-in-2D/update-ui-element-in-realtime-unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicText : MonoBehaviour
{
	private Text thisText;
	private int step_number;
    // Start is called before the first frame update
    void Start()
    {
    	thisText = GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.P))
        {
            step_number +=1;
        }
        thisText.text = "Step " + step_number;
    }
}
