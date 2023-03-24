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
    {Food, Drink ,Metal, Gem, MeleeWeapon, RangeWeapon, MagicThing} // ������ ����� ����� ��������� 

    public ItemType TypeOfItem;
    
    [TextArea(2,5)]public string Description;
    public Sprite Icon;
    public int Price;
    [Range(0,100)]public int Fragility; // ��������, ������������ ��������� ������� �������.��� ������ - ��� ����� ������. max = 100
        //  ����� �� ����������� ��������� �� ����� ���������. �������� ���� ����� ����� 50, � � ����� = 7.
        // ��� �� �� ��, ���������� ������� ��� ��� �������� "��������" ������ - �� ��������.

    public float Weight;
    public int MaxItemsInAStack;

    public int CellSizeWidth;
    public int CellSizeHeight;

    [HideInInspector] public bool IsPerishable;
    [HideInInspector] public bool IsEdible;
    [HideInInspector] public float DaysToHalfSpoil;
    [HideInInspector] public float DaysToSpoil;
    [HideInInspector] public int FoodValue; // �������������
}