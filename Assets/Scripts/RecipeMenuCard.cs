using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RecipeMenuCard : MonoBehaviour
{

    [SerializeField] private Text titleText;
    [SerializeField] private Text caloriesTimeText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private RawImage recipeImage;

    public void SetInfo(Recipe recipe)
    {
        titleText.text = recipe.name;
        int hours = recipe.time / 60;
        int minutes = recipe.time % 60;
        caloriesTimeText.text = recipe.calories + "cal | " + hours + "hr" + minutes + "min";
        descriptionText.text = recipe.description;
        StartCoroutine(GetRecipeImageTexture(recipe.imgUrl, recipeImage));
    }

    IEnumerator GetRecipeImageTexture(string imgUrl, RawImage img)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(imgUrl))
        {
            yield return req.SendWebRequest();
            
            if (req.isHttpError || req.isNetworkError)
            {
                Debug.Log("HIT ERROR");
                Debug.Log(req.error);
            }
            else
            {
                img.texture = DownloadHandlerTexture.GetContent(req);
            }
        }
    }

}
