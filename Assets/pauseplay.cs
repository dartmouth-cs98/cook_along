using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseplay : MonoBehaviour
{
	public StepCanvas step;
	public Button b;
    // Start is called before the first frame update
    void Start()
    {
    	Button btn = b.GetComponent<Button>();
    	btn.onClick.AddListener(step.TaskOnClick);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
