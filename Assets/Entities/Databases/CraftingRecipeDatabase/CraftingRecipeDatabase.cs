using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingRecipeDatabase : MonoBehaviour
{
    public CraftingRecipeDatabaseSO Recipes;
    private static CraftingRecipeDatabase Instance; //—инглтон приватный, потому что обращение с базой только через методы GetItem (статические, это важно),
                                           //другим челам не нужен доступ именно к синглтону

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static CraftingRecipe GetRecipe(string name)
    {
        CraftingRecipe result = Instance.Recipes.RecipeList.FirstOrDefault(recipe => recipe.ResultingItem.Name.ToLower() == name.ToLower());
        //¬ыражение Linq, аналогичное foreach (var item in itemlist) { if (item.Name == name) return item; else return null; } 
        if (result != null)
        {
            return result;
        }
        
        Debug.LogWarning("“акого ресипе не существует!");
        return null;
    }
}
