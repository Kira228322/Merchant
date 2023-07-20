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
        RichClothes, WarmClothes, LightClothers, CeramicProduct, EverydayItem, CostumeJewelry, SouthPlant, NorthPlant, 
        Cactus
    }
    
    public ItemType TypeOfItem;
    
    
    
    [TextArea(2,5)]public string Description;
    public Sprite Icon;
    public int Price;
    [Range(0,100)]public int Fragility; 

    public float Weight;
    public int MaxItemsInAStack;

    public int CellSizeWidth;
    public int CellSizeHeight;

    [HideInInspector] public bool IsPerishable;
    [HideInInspector] public bool IsQuestItem;
    
    [HideInInspector] public float DaysToHalfSpoil;
    [HideInInspector] public float DaysToSpoil; 
}