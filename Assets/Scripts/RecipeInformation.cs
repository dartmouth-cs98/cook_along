using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Text;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.XR.MagicLeap;

public class RecipeInformation : MonoBehaviour
{
    
    public ControlInput controlInput;
    
    #region Private Variables
    public static List<List<string>> ingredientURLlistoflist;
    private Text _title;
    private Text _ingredients;
    private Text _tools;
    private Text _other_info;
    int number_to_show = 6;
    #endregion
    
    private void Awake()
    {
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetRecipeInfo(RecipeMenuList.SelectedRecipe);
        StartCoroutine(getURLs());
    }

    IEnumerator getURLs()
    {
        Debug.Log("Getting URLs");
        String url = "https://cookalong-api.herokuapp.com/recipes/allingredients/" + RecipeMenuList.SelectedRecipe.id;

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
                //check this json handling is correct
                string result = req.downloadHandler.text;
                int length = result.Length;
                result = result.Substring(1,length -2 );
                result = result.Replace("[","");
                string[] splitResult = result.Split(char.Parse("]"));
                ingredientURLlistoflist = new List<List<string>>();

                foreach (string current in splitResult){
                    if (String.IsNullOrEmpty(current) || current.Equals(",")){
                        List<string> empty = new List<string>();
                        ingredientURLlistoflist.Add(empty);
                    }
                    else{
                        string[] delimiter = new string[] {"\",\""};
                        string[] splitCurrent = current.Split(delimiter, StringSplitOptions.None);
                        List<string> add = new List<string>(splitCurrent);
                        ingredientURLlistoflist.Add(add);

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    String GetIngredientString(List<RecipeIngredient> ingredients)
    {
        
        int i = 0;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Ingredients:");
        foreach (RecipeIngredient ingredient in ingredients)
        {
            if (i < number_to_show){
                sb.AppendLine(" - " + ingredient.amount + " " + ingredient.name);
                i++;
            }
            
        }
        return sb.ToString();
    }

    String GetToolsString(List<string> tools)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Tools:");
        int i = 0; 
        foreach (string tool in tools)
        {
            if (i<number_to_show){
                sb.AppendLine(" - " + tool);
                i++;
            }

        }
        return sb.ToString();
    }

    String GetOtherInfoString(int calories, int time, int serving_size)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Time: " + time + " minutes");
        sb.AppendLine(serving_size + " servings");
        sb.AppendLine(calories + " calories");
        return sb.ToString();
    }

    void SetRecipeInfo(Recipe recipeObj)
    {
        _title = GameObject.Find("Header").GetComponent<Text>();
        _title.text = recipeObj.name;
        _ingredients = GameObject.Find("Ingredients").GetComponent<Text>();
        _ingredients.text = GetIngredientString(recipeObj.ingredients);
        _tools = GameObject.Find("Tools").GetComponent<Text>();
        _tools.text = GetToolsString(recipeObj.tools);
        _other_info = GameObject.Find("Other information").GetComponent<Text>();
        _other_info.text = GetOtherInfoString(recipeObj.calories, recipeObj.time, recipeObj.serving_size);
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(RecipeMenuList.SelectedRecipe.imgUrl);

        yield return www.SendWebRequest();

        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            GameObject.Find("Recipe Photo").GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(www);
        }

    }

    private void HandleHomeTap()
    {
        Loader.Load(Loader.Scene.RecipeMenu);
    }
}
