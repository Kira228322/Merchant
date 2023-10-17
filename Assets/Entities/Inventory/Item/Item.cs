using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public string Name;

    public enum ItemType
    {
        Null = 1337, 
        //1337 чтобы добавить Null, но не засрать уже существующие предметы
        //(Ну обращение ведь по номеру этого Enum. Если бы я сделал ему номер 0,
        //то все существующие предметы сдвинулись бы на одну категорию назад)
        RichClothes = 0, WarmClothes, LightClothes, CeramicProduct, EverydayItem, CostumeJewelry, SouthPlant, NorthPlant, 
        Cactus, MagicThing, Chemicals, Cosmetics, SouthFood, NorthFood, Food, Tea, Spices, EastSpices, MagicMaterial,
        Fish, Seeds, MagicSeeds, SouthFruitAndBerry, NorthFruitAndBerry, Sushi, AlcoholDrink, Drink, Potion, Equipment, Armor, RangeWeapon, 
        Materials, PreciousMetalsGems, MeleeWeapon
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

    public string TranslatedItemType => translatedItemTypes[TypeOfItem];

    public static string TranslateItemType(ItemType type)
    {
        return translatedItemTypes[type];
    }

    private static readonly Dictionary<ItemType, string> translatedItemTypes = new()
    {
        {ItemType.Null, ""},
        {ItemType.RichClothes, "Богатая одежда" },
        {ItemType.WarmClothes, "Тёплая одежда" },
        {ItemType.LightClothes, "Лёгкая одежда"},
        {ItemType.CeramicProduct, "Керамика"},
        {ItemType.EverydayItem, "Повседневная вещь"},
        {ItemType.CostumeJewelry, "Драгоценности"},
        {ItemType.SouthPlant, "Южное растение"},
        {ItemType.NorthPlant, "Северное растение"},
        {ItemType.Cactus, "Кактус"},
        {ItemType.MagicThing, "Магический предмет"},
        {ItemType.Chemicals, "Химикаты"},
        {ItemType.Cosmetics, "Косметика"},
        {ItemType.SouthFood, "Южная еда"},
        {ItemType.NorthFood, "Северная еда"},
        {ItemType.Food, "Еда"},
        {ItemType.Tea, "Чай"},
        {ItemType.Spices, "Специи"},
        {ItemType.EastSpices, "Восточные специи"},
        {ItemType.MagicMaterial, "Магические материалы"},
        {ItemType.Fish, "Рыба"},
        {ItemType.Seeds, "Семена"},
        {ItemType.MagicSeeds, "Магические семена"},
        {ItemType.SouthFruitAndBerry, "Южный фрукт/ягода"},
        {ItemType.NorthFruitAndBerry, "Северный фрукт/ягода"},
        {ItemType.Sushi, "Суши"},
        {ItemType.AlcoholDrink, "Спиртной напиток"},
        {ItemType.Drink, "Напиток"},
        {ItemType.Potion, "Эликсир"},
        {ItemType.Equipment, "Экипировка"},
        {ItemType.Armor, "Броня"},
        {ItemType.RangeWeapon, "Дальнобойное оружие"},
        {ItemType.Materials, "Материалы"},
        {ItemType.PreciousMetalsGems, "Драгоценные камни/металлы"},
        {ItemType.MeleeWeapon, "Оружие ближнего боя"}
    };

    
}