using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting recipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe: ScriptableObject
{
    [Serializable]
    public class CraftingItem
    {
        public Item item;
        public int amount;
    }

    public int RequiredCraftingLevel;
    public List<CraftingItem> RequiredItems = new();
    public CraftingStationType RequiredCraftingStation;
    
    public Item ResultingItem;
    public int ResultAmount;
    public AudioClip _SoundOfCrafting;
}
