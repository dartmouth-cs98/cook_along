using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE TO DANIELLE: make sure to turn play into true whenever the sound hasn't played for the timer chosen 

public class noiseNotification : MonoBehaviour
{
	public AudioClip notification;
	private AudioSource audio;
	private bool play;
	private bool stops; 
	private float audioLength;

    // Start is called before the first frame update
    void Start()
    {
    	audio= gameObject.AddComponent<AudioSource>();
    	audio.clip = notification;
    	play = true; 
        
    }

    // Update is called once per frame
    void Update()
    {
    	if (play){
    		audio.PlayOneShot(notification);
    		play = false; 
    	}
    }

}
