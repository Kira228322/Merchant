using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public string Name;

    public enum ItemType
    {
        Null = 1337, 
        //1337 ����� �������� Null, �� �� ������� ��� ������������ ��������
        //(�� ��������� ���� �� ������ ����� Enum. ���� �� � ������ ��� ����� 0,
        //�� ��� ������������ �������� ���������� �� �� ���� ��������� �����)
        RichClothes = 0, WarmClothes, LightClothes, CeramicProduct, EverydayItem, CostumeJewelry, SouthPlant, NorthPlant, 
        Cactus, MagicThing, Chemicals, Cosmetics, SouthFood, NorthFood, Food, Tea, Spices, EastSpices, MagicMaterial,
        Fish, Seeds, MagicSeeds, SouthFruit, NorthFruit, Sushi, AlcoholDrink, Drink, Potion
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
        {ItemType.RichClothes, "������� ������" },
        {ItemType.WarmClothes, "Ҹ���� ������" },
        {ItemType.LightClothes, "˸���� ������"},
        {ItemType.CeramicProduct, "��������"},
        {ItemType.EverydayItem, "������������ ����"},
        {ItemType.CostumeJewelry, "�������������"},
        {ItemType.SouthPlant, "����� ��������"},
        {ItemType.NorthPlant, "�������� ��������"},
        {ItemType.Cactus, "������"},
        {ItemType.MagicThing, "���������� �������"},
        {ItemType.Chemicals, "��������"},
        {ItemType.Cosmetics, "���������"},
        {ItemType.SouthFood, "����� ���"},
        {ItemType.NorthFood, "�������� ���"},
        {ItemType.Food, "���"},
        {ItemType.Tea, "���"},
        {ItemType.Spices, "������"},
        {ItemType.EastSpices, "��������� ������"},
        {ItemType.MagicMaterial, "���������� ���������"},
        {ItemType.Fish, "����"},
        {ItemType.Seeds, "������"},
        {ItemType.MagicSeeds, "���������� ������"},
        {ItemType.SouthFruit, "����� �����"},
        {ItemType.NorthFruit, "�������� �����"},
        {ItemType.Sushi, "����"},
        {ItemType.AlcoholDrink, "�������� �������"},
        {ItemType.Drink, "�������"},
    };

    
}