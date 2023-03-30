using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newItem", menuName = "Item/UsableItem")]
public class UsableItem : Item
{
    public enum UsableType {Edible, Potion, Bottle, Teleport, Recipe}
    
    [Space]
    [Header("Special settings")]
    public UsableType UsableItemType;
    
    [HideInInspector]public int UsableValue;
    [HideInInspector]public Status Effect;
    [HideInInspector]public CraftingRecipe Recipe;
}
