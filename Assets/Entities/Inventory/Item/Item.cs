using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newItem", menuName = "Status or Item/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public enum ItemType
    {Food, Drink ,Metal, Gem, MeleeWeapon, RangeWeapon, MagicThing} // Список нужно будет дополнять 

    public ItemType TypeOfItem;
    
    [TextArea(2,5)]public string Description;
    public Sprite Icon;
    public int Price;
    [Range(0,100)]public int Fragility; // Величина, показывающая насколько хрупкий предмет.Чем больше - тем более хрупок. max = 100
        //  Влият на вероятность разбиться во время перевозки. Например ваза будет иметь 50, а у яблок = 7.
        // Так же за то, разобьется предмет или нет отвечает "качество" дороги - ее параметр.

    public float Weight;
    public int MaxItemsInAStack;

    public int CellSizeWidth;
    public int CellSizeHeight;

    [HideInInspector] public bool IsPerishable;
    [HideInInspector] public bool IsEdible;
    [HideInInspector] public float DaysToHalfSpoil;
    [HideInInspector] public float DaysToSpoil;
    [HideInInspector] public int FoodValue; // питательность
}