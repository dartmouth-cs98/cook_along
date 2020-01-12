using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StepCanvas : MonoBehaviour
{
	public List<string> URLs = new List<string>();
    public GameObject canvas;
    public int yCoord;
    public int xCoord;
    // Start is called before the first frame update
    void Start()
    {
        yCoord=-20;
        xCoord= 210;
        int height=50;
        int width=50;
        URLs.Add("https://food.fnr.sndimg.com/content/dam/images/food/fullset/2012/2/24/0/ZB0202H_classic-american-grilled-cheese_s4x3.jpg.rend.hgtvcom.616.462.suffix/1371603614279.jpeg");
        URLs.Add("https://i0.wp.com/cdn-prod.medicalnewstoday.com/content/images/articles/299/299147/cheese-varieties.jpg?w=1155&h=1537");
        
        foreach (string currentURL in URLs)
        {
            canvas = GameObject.Find("Canvas");
            GameObject NewObj = new GameObject(); //Create the GameObject
            RawImage NewImage = NewObj.AddComponent<RawImage>(); //Add the Image Component script
            NewImage.transform.SetParent(canvas.transform,false);
            NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(xCoord,yCoord,0);
            NewObj.GetComponent<RectTransform>().sizeDelta=new Vector2(width,height);
            yCoord=yCoord-50;
            NewObj.SetActive(true); //Activate the GameObject
            StartCoroutine(GetTexture(currentURL, NewObj));
        }
    }

    IEnumerator GetTexture(string thisURL, GameObject currrentImage) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(thisURL);
    	yield return www.SendWebRequest();

        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            currrentImage.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(www);
        }
    }


    // Update is called once per frame
    void Update()
    {
     
    }
}
