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
        RecipeVar = getRecipe();
        _title = GameObject.Find("Header").GetComponent<Text>();
        _title.text = RecipeVar.name;
        _ingredients = GameObject.Find("Ingredients").GetComponent<Text>();
        _ingredients.text = GetIngredientString(RecipeVar.ingredients);
        _tools = GameObject.Find("Tools").GetComponent<Text>();
        _tools.text = GetToolsString(RecipeVar.tools);
        _other_info = GameObject.Find("Other information").GetComponent<Text>();
        _other_info.text = GetOtherInfoString(RecipeVar.calories, RecipeVar.time, RecipeVar.serving_size);
        
        MLInput.OnControllerButtonUp += HandleOnButtonUp;
        MLInput.OnControllerButtonDown += HandleOnButtonDown;
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


    Recipe getRecipe()
    {
        String url = "http://localhost:8080/grilledcheese";
        if (!RecipeLoader.Recipe1Active)
        {
            url = "http://localhost:8080/pho";
        }
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Recipe info = JsonUtility.FromJson<Recipe>(jsonResponse);
        return info;
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
