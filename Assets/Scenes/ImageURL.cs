// Code inpired from: https://docs.unity3d.com/Manual/UnityWebRequest-CreatingUnityWebRequests.html
// also inspired from: https://stackoverflow.com/questions/55260531/get-image-from-a-server-in-unity-2018
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ImageURL : MonoBehaviour
{
    void Start() {
        StartCoroutine(GetTexture());
    }
 
    IEnumerator GetTexture() {

         UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://food.fnr.sndimg.com/content/dam/images/food/fullset/2012/2/24/0/ZB0202H_classic-american-grilled-cheese_s4x3.jpg.rend.hgtvcom.616.462.suffix/1371603614279.jpeg");

    	yield return www.SendWebRequest();

        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            this.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(www);
        }
    }

  
}
