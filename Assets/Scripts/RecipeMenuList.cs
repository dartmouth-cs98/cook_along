using System;
using System.Collections;
using System.Collections.Generic;
using MagicLeapTools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class RecipeMenuList : MonoBehaviour
{
    
    private int _activeIndex;
    private List<int> intList;
    private List<Recipe> _recipes;
    private List<GameObject> _cards;
    public ControlInput controlInput;
    private ScrollRect _scrollRect;
    public static Recipe SelectedRecipe;
    private Color _highlightedColor;
    private Color _baseColor;

    [SerializeField]
    private GameObject cardTemplate;

    private void Awake()
    {
        controlInput.OnTriggerPressEnded.AddListener(HandleTrigger);
        controlInput.OnSwipe.AddListener(HandleSwipe);
        controlInput.OnHomeButtonTap.AddListener(HandleHomeTap); 
        _activeIndex = 0;
        _cards = new List<GameObject>();
        _scrollRect = GetComponent<ScrollRect>();
        _highlightedColor = new Color(0.937f, 0.741f, 0.42f);
        _baseColor = new Color(1f, 1f, 1f);
    }

    void HandleHomeTap()
    {
        Loader.Load	(Loader.Scene.WelcomeScreen);
    }

    void HandleTrigger()
    {
        SelectedRecipe = _recipes[_activeIndex];
        Loader.Load	(Loader.Scene.RecipeInformation);
    }

    void HandleSwipe(MLInputControllerTouchpadGestureDirection direction)
    {
        if (direction == MLInputControllerTouchpadGestureDirection.Up && _activeIndex > 0)
        {
            _scrollRect.verticalNormalizedPosition += 0.5f;
            UpdateActiveRecipe(direction);
        }

        if (direction == MLInputControllerTouchpadGestureDirection.Down && _activeIndex < _recipes.Count - 1)
        {
            _scrollRect.verticalNormalizedPosition -= 0.5f;
            UpdateActiveRecipe(direction);
        }
    }
    
    void UpdateActiveRecipe(MLInputControllerTouchpadGestureDirection direction)
    {
        GameObject previousCard = _cards[_activeIndex];
        
        if (direction == MLInputControllerTouchpadGestureDirection.Up)
        {
            _activeIndex -= 1;
        }

        if (direction == MLInputControllerTouchpadGestureDirection.Down)
        {
            _activeIndex += 1;
        }
        
        GameObject nextCard = _cards[_activeIndex];
        previousCard.GetComponent<Image>().color = _baseColor;
        nextCard.GetComponent	<Image>().color = _highlightedColor;
    }  

    void Start()
    {
        StartCoroutine(GetRecipes(PopulateRecipes));
    }

    void PopulateRecipes(List<Recipe> recipeList)
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            Recipe recipe = recipeList[i];
            GameObject card = Instantiate(cardTemplate) as GameObject;
            card.SetActive(true);
            card.GetComponent<RecipeMenuCard>().SetInfo(recipe);
            Image cardImage = card.GetComponent<Image>();
            cardImage.color = i == 0 ? _highlightedColor : _baseColor;
            card.transform.SetParent(cardTemplate.transform.parent, false);
            _cards.Add(card);
        }
        
        GetComponent<Image>().enabled = false;
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
                _recipes = recipeWrapper.recipes;
                onSuccess(recipeWrapper.recipes);
            }
        }
    }

    private class RecipeArrayWrapper
    {
        public List<Recipe> recipes;
    }
}