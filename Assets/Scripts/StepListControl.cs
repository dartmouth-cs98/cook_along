using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepListControl : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;


    void Start()
    {
        List<RecipeStep> steps = RecipeMenuList.SelectedRecipe.steps;
        for (int i = 0; i < steps.Count; i++)
        {
            
        }
    }
    
    
    void GenButtons()
    {
        
    }
}
