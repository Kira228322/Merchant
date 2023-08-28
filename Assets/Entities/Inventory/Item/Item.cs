using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public string Name;

    public enum ItemType
    {
        RichClothes, WarmClothes, LightClothes, CeramicProduct, EverydayItem, CostumeJewelry, SouthPlant, NorthPlant, 
        Cactus, MagicThing, Chemicals, Cosmetics, SouthFood, NorthFood, Food, Tea, Spices, EastSpices, MagicMaterial
    }
    
    public ItemType TypeOfItem;
    
    
    
    [TextArea(2,5)]public string Description;
    public Sprite Icon;
    public int Price;
    [Range(0,50)]public int Fragility; 

    public float Weight;
    public int MaxItemsInAStack;

    public int CellSizeWidth;
    public int CellSizeHeight;

    [HideInInspector] public bool IsPerishable;
    [HideInInspector] public bool IsQuestItem;
    
    [HideInInspector] public float DaysToHalfSpoil;
    [HideInInspector] public float DaysToSpoil; 
}