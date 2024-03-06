using System;
using System.Collections.Generic;

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
