using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Recipe
{
    public long id;
    public string name;
    public string description;
    public string imgUrl;
    public int time;
    public int serving_size;
    public int calories;
    public List<RecipeIngredient> ingredients;
    public List<RecipeStep> steps;
    public List<string> tools;
}

[Serializable]
public class RecipeIngredient {
    public long id;
    public string name;
    public string amount;
}

[Serializable]
public class RecipeStep {
    public long id;
    public string instruction;
    public string videoUrl;
}
