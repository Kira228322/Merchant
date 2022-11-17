using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Trader : MonoBehaviour
{
    // Хоть у каждого Item'a уже указана цена, за которую его можно купить у торговца
    // каждый торговец будет иметь свою цену на этот товар. ИБО В ЭТОМ И СМЫСЛ ИГРЫ епт
    [SerializeField] private string _name;
    public string Name => _name;
    public List<Item> Goods;
    [HideInInspector] public List<Item> MerchantGoods;

    [HideInInspector] public List<int> NewPrice;
    [HideInInspector] public List<int> CountOfGood;
    // private int[] _newPrice;
    // private int[] _countOfGood;

    private void Start()
    {
        MerchantGoods = new List<Item>();
        for (int i = 0; i < Goods.Count; i++)
        {
            MerchantGoods.Add(Instantiate(Goods[i])); // Важно продублировать объект типа ScriptableObject, тк иначе
            MerchantGoods[i].Price = NewPrice[i]; // он будет ссылаться на оригинал и изменять его понял. нужен дубликат
        }
    }

    public void SellItem(Item item)
    {
        for (int i = 0; i < Goods.Count; i++)
        {
            if (item == Goods[i])
            {
                CountOfGood[i]--;
                break;
            }
        }
    }



    [CustomEditor(typeof(Trader))]
    public class MerchantEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
            Trader trader = (Trader)target;
            
            
            
            
            while (trader.NewPrice.Count < trader.Goods.Count)
            {
                trader.NewPrice.Add(0);
                trader.CountOfGood.Add(0);
            }
            while (trader.NewPrice.Count > trader.Goods.Count)
            {
                trader.NewPrice.RemoveAt(trader.NewPrice.Count-1);
                trader.CountOfGood.RemoveAt(trader.CountOfGood.Count-1);
            }
            
            for (int i = 0; i < trader.Goods.Count; i++)
            {
                if (trader.Goods[i] == null)
                    continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(-18, true);
                EditorGUILayout.LabelField(trader.Goods[i].Name, GUILayout.MaxWidth(90));
                EditorGUILayout.Space(1, true);
                EditorGUILayout.LabelField(trader.Goods[i].Price + "-AvgPrice", GUILayout.MaxWidth(75));
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Price", GUILayout.MaxWidth(31));
                trader.NewPrice[i] = EditorGUILayout.IntField(trader.NewPrice[i]);
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Count", GUILayout.MaxWidth(36.5f));
                trader.CountOfGood[i] = EditorGUILayout.IntField(trader.CountOfGood[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
