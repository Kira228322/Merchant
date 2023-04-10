using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerRecipesSaveData
{
    public List<string> recipeNames = new();
    public PlayerRecipesSaveData(List<CraftingRecipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            recipeNames.Add(recipe.ResultingItem.Name);
        }
    }
}
