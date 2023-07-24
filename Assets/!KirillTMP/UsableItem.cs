using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newItem", menuName = "Item/UsableItem")]
public class UsableItem : Item
{
    public enum UsableType {Edible, Potion, Bottle, Teleport, Recipe, Note}
    
    [Space]
    [Header("Special settings")]
    public UsableType UsableItemType;
    
    [HideInInspector] public int UsableValue;
    [HideInInspector] public Status Effect;
    [HideInInspector] public List<CraftingRecipe> Recipes;
    [HideInInspector] public string NoteText;
}
