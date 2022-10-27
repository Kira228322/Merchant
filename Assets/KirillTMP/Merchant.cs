using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Merchant : MonoBehaviour
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




    [CustomEditor(typeof(Merchant))]
    public class MerchantEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
            Merchant merchant = (Merchant)target;
            
            
            
            
            while (merchant.NewPrice.Count < merchant.Goods.Count)
            {
                merchant.NewPrice.Add(0);
                merchant.CountOfGood.Add(0);
            }
            while (merchant.NewPrice.Count > merchant.Goods.Count)
            {
                merchant.NewPrice.RemoveAt(merchant.NewPrice.Count-1);
                merchant.CountOfGood.RemoveAt(merchant.CountOfGood.Count-1);
            }
            
            for (int i = 0; i < merchant.Goods.Count; i++)
            {
                if (merchant.Goods[i] == null)
                    continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(-18, true);
                EditorGUILayout.LabelField(merchant.Goods[i].Name, GUILayout.MaxWidth(90));
                EditorGUILayout.Space(1, true);
                EditorGUILayout.LabelField(merchant.Goods[i].Price + "-AvgPrice", GUILayout.MaxWidth(75));
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Price", GUILayout.MaxWidth(31));
                merchant.NewPrice[i] = EditorGUILayout.IntField(merchant.NewPrice[i]);
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Count", GUILayout.MaxWidth(36.5f));
                merchant.CountOfGood[i] = EditorGUILayout.IntField(merchant.CountOfGood[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
