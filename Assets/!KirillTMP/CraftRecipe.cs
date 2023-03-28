using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRecipe", menuName = "Item/CraftRecipe")]
public class CraftRecipe : ScriptableObject 
{
    [SerializeField] private Item _targetItem;
    [SerializeField] private List<Item> _requireItems;
    [SerializeField] private int _requireCraftLvl;
}
