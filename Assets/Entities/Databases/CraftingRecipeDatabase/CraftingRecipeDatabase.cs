using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingRecipeDatabase : MonoBehaviour
{
    public CraftingRecipeDatabaseSO Recipes;
    private static CraftingRecipeDatabase Instance; //�������� ���������, ������ ��� ��������� � ����� ������ ����� ������ GetItem (�����������, ��� �����),
                                           //������ ����� �� ����� ������ ������ � ���������

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
        //��������� Linq, ����������� foreach (var item in itemlist) { if (item.Name == name) return item; else return null; } 
        if (result != null)
        {
            return result;
        }
        
        Debug.LogWarning("������ ������ �� ����������!");
        return null;
    }
}
