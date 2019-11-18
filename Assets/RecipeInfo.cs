using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.XR.MagicLeap;

public class RecipeInfo : MonoBehaviour
{
    
    #region Private Variables
    private ControllerConnectionHandler _controllerConnectionHandler;
    public static Recipe RecipeVar;
    private Text _title;
    private Text _ingredients;
    private Text _tools;
    private Text _other_info;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {

        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
        MLInput.OnControllerButtonUp += HandleOnButtonUp;
        MLInput.OnControllerButtonDown += HandleOnButtonDown;
        StartCoroutine(GetRecipe(SetRecipeInfo));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= HandleOnButtonDown;
        MLInput.OnControllerButtonUp -= HandleOnButtonUp;
    }

    String GetIngredientString(List<RecipeIngredient> ingredients)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Ingredients:");
        foreach (RecipeIngredient ingredient in ingredients)
        {
            sb.AppendLine(" - " + ingredient.amount + " " + ingredient.name);
        }
        return sb.ToString();
    }

    String GetToolsString(List<string> tools)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Tools:");
        foreach (string tool in tools)
        {
            sb.AppendLine(" - " + tool);
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
        Debug.Log("HI FROM SET RECIPE INFO");
        RecipeVar = recipeObj;
        _title = GameObject.Find("Header").GetComponent<Text>();
        _title.text = recipeObj.name;
        _ingredients = GameObject.Find("Ingredients").GetComponent<Text>();
        _ingredients.text = GetIngredientString(recipeObj.ingredients);
        _tools = GameObject.Find("Tools").GetComponent<Text>();
        _tools.text = GetToolsString(recipeObj.tools);
        _other_info = GameObject.Find("Other information").GetComponent<Text>();
        _other_info.text = GetOtherInfoString(recipeObj.calories, recipeObj.time, recipeObj.serving_size);
    }

    // https://www.red-gate.com/simple-talk/dotnet/c-programming/calling-restful-apis-unity3d/
    IEnumerator GetRecipe(Action<Recipe> onSuccess)
    {
        Debug.Log("HI FROM GETRECIPE");
        String url = "https://dynamodb.us-west-2.amazonaws.com";
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
                Debug.Log(result);
                Recipe info = new Recipe();
                List<RecipeStep> stepList = new List<RecipeStep>();
                List<RecipeIngredient> ingredientList = new List<RecipeIngredient>();
                for (int j = 1; j <= 5; j++)
                {
                    RecipeIngredient inggredient = new RecipeIngredient();
                    inggredient.name = "ingredient" + j;
                    inggredient.amount = ""+j;
                    ingredientList.Add(inggredient);
                }
                info.ingredients=ingredientList;
                info.id = 1;
                info.name = "grilled cheese sup";
                info.time = 10;
                info.serving_size = 2;
                info.calories = 150;
                List<string> myTools = new List<string>(new string[] { "knife", "skillet" });
                info.tools = myTools;
                onSuccess(info);
            }
            
        }
    }
    
    
    private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId &&
            button == MLInputControllerButton.Bumper)
        {
            Debug.Log("Button down");
        }
    }

    /// <summary>
    /// Handles the event for button up.
    /// </summary>
    /// <param name="controller_id">The id of the controller.</param>
    /// <param name="button">The button that is being released.</param>
    private void HandleOnButtonUp(byte controllerId, MLInputControllerButton button)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId &&
            button == MLInputControllerButton.HomeTap)
        {
            Debug.Log("Button up");
            SceneManager.LoadScene("Recipe Chooser");
        }
    }
}
