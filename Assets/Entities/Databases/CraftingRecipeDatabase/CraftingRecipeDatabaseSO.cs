using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe Database", menuName = "Databases/Crafting Recipe Database")]
public class CraftingRecipeDatabaseSO : ScriptableObject
{
    public List<CraftingRecipe> RecipeList;
}
