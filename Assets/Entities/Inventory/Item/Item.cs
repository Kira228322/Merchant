using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public string Name;

    public enum ItemType
    {
        RichClothes, WarmClothes, LightClothes, CeramicProduct, EverydayItem, CostumeJewelry, SouthPlant, NorthPlant, 
        Cactus, MagicThing, Chemicals, Cosmetics, SouthFood, NorthFood, Food, Tea, Spices, EastSpices, MagicMaterial,
        Fish
    }
    
    public ItemType TypeOfItem;
    
    
    
    [TextArea(2,5)]public string Description;
    [HideInInspector]public Sprite Icon;
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