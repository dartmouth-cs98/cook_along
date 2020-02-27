using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RecipeMenuList : MonoBehaviour
{ 
    [SerializeField] 
    private GameObject cardTemplate;

    private List<int> intList;

    void Start()
    {
        StartCoroutine(GetRecipes(PopulateRecipes));
    }

    void PopulateRecipes(List<Recipe> recipeList)
    {
        foreach(Recipe recipe in recipeList)
        {
            GameObject card = Instantiate(cardTemplate) as GameObject;
            card.SetActive(true);
            card.GetComponent<RecipeMenuCard>().SetInfo(recipe);
            card.transform.SetParent(cardTemplate.transform.parent, false);
        }
    }

    IEnumerator GetRecipes(Action<List<Recipe>> onSuccess)
    {
        String url = "https://cookalong-api.herokuapp.com/recipes";
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();
            
            if (req.isHttpError || req.isNetworkError)
            {
                Debug.Log("HIT ERROR");
                Debug.Log(req.error);
            }
            else
            {
                string result = req.downloadHandler.text;
                result = "{\"recipes\":" + result + "}";
                RecipeArrayWrapper recipeWrapper = JsonUtility.FromJson<RecipeArrayWrapper>(result);
                onSuccess(recipeWrapper.recipes);
            }
        }
    }

    private class RecipeArrayWrapper
    {
        public List<Recipe> recipes;
    }
}
