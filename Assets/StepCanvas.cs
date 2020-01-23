using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;


public class StepCanvas : MonoBehaviour
{

	public int yCoord;
    public int xCoord;
    public VideoPlayer videoPlayer;
    private List<string> URLs = new List<string>();
    private GameObject canvas;  
    private bool videoInstruction = true;
    private VideoSource videoSource;
    public Button b;
    public RawImage mesh;


    // Start is called before the first frame update
    void Start()
    {
        yCoord=-20;
        xCoord= 210;
        int height=50;
        int width=50;
        int VidHeight =250;
        int VidWidth =450;
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
        if (videoInstruction){
            GameObject NewObj = new GameObject(); //Create the GameObject
            RawImage Screen = NewObj.AddComponent<RawImage>(); //Add the Image Component script
            Screen.transform.SetParent(canvas.transform,false);
            NewObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);
            NewObj.GetComponent<RectTransform>().sizeDelta=new Vector2(VidWidth,VidHeight);
            NewObj.SetActive(true); //Activate the GameObject
        Application.runInBackground=true;
          videoPlayer.source=VideoSource.Url;
          b.image.rectTransform.sizeDelta= new Vector2(30,30);
          mesh.GetComponent<RectTransform>().sizeDelta= new Vector2(1000,1000);

          videoPlayer.url = "https:/dl.dropbox.com/s/f5suv9je1vya4pd/3%20Ways%20To%20Chop%20Onions%20Like%20A%20Pro.mp4?dl=1";
          StartCoroutine(PlayVideo(Screen));

        }
    }


      IEnumerator PlayVideo(RawImage rawImage)
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

    public void TaskOnClick(){

        if (videoPlayer.isPlaying){
            videoPlayer.Pause();


        }
        else{
          videoPlayer.Play();

        }
    }

    // Update is called once per frame
    void Update()
    {


     
    }
}
