// inspired by https://www.mirimad.com/unity-play-video-on-canvas/
// also:https://github.com/luzan/UnityVideoPlayer/blob/master/Unity%20VideoPlayer%20-%20Part%201/Assets/StreamVideo.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour {
     public RawImage rawImage;
     public VideoPlayer videoPlayer;
     private VideoSource videoSource;

  // Use this for initialization
  void Start () {
          Application.runInBackground=true;
          videoPlayer.source=VideoSource.Url;
          videoPlayer.url = "https:/dl.dropbox.com/s/f5suv9je1vya4pd/3%20Ways%20To%20Chop%20Onions%20Like%20A%20Pro.mp4?dl=1";
          StartCoroutine(PlayVideo());
  }
  IEnumerator PlayVideo()
     {
          videoPlayer.playOnAwake=false;
          videoPlayer.Prepare();
          
          while (!videoPlayer.isPrepared)
          {
            yield return null;
          }
          rawImage.texture = videoPlayer.texture;
          videoPlayer.Play();

     }
}