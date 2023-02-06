using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCTrader : NPC
{
    [SerializeField] private List<TraderType> _traderTypes;
    
    
    [SerializeField] private List<Item> _goods;
    public List<TraderGood> TraderGoods = new();
    
    [HideInInspector] public List<float> Coefficient = new ();
    [HideInInspector] public List<int> CountToBuy = new ();
    [HideInInspector] public List<int> CurrentCountToBuy = new ();

    [HideInInspector] public List<int> _newPrice = new ();
    [HideInInspector] public List<int> _countOfGood = new ();
    public class TraderGood
    {
        public Item Good;
        public int Count;
    }
    
    
    private void Start()
    {
        SetTradersStats();

        TraderGoods = new List<TraderGood>();
        for (int i = 0; i < _goods.Count; i++)
        {
            TraderGoods.Add(new TraderGood());
            TraderGoods[i].Good = Instantiate(_goods[i]);
            TraderGoods[i].Good.Price = _newPrice[i];
            TraderGoods[i].Count = _countOfGood[i];
        }
    }

    private void SetTradersStats()
    {
        if (_traderTypes.Count == 1)  // Если торговец одного типа
            for (int i = 0; i < _traderTypes[0].GoodsType.Count; i ++)
            {
                Coefficient.Add(_traderTypes[0].Coefficient[i]);
                CountToBuy.Add(_traderTypes[0].CountToBuy[i]);
            }
        else // Если торговец смешанного типа
        {
            foreach (var unused in _traderTypes[0].GoodsType)
            {
                Coefficient.Add(0);
                CountToBuy.Add(0);
            }

            for (int i = 0; i < CountToBuy.Count; i++)
            {
                for (int j = 0; j < _traderTypes.Count; j++)
                    CountToBuy[i] += _traderTypes[j].CountToBuy[i];
                
                // Среднее значение, округленное вверх 
                CountToBuy[i] = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(CountToBuy[i]) / _traderTypes.Count));
                
                for (int j = 0; j < _traderTypes.Count; j++)
                {
                    if (_traderTypes[j].Coefficient[i] == 1) // Если данный тип продукта это его специализация 
                    { // (коэф за который покупает этот товар == 1), то конечный коэф = 1
                        Coefficient[i] = 1;
                        break;
                    } // Если не спеца, то складываем все, потом делим на колличество => среднее значение
                    Coefficient[i] += _traderTypes[j].Coefficient[i];
                    
                }

                if (Coefficient[i] != 1)
                    Coefficient[i] = Convert.ToSingle(Math.Round(Coefficient[i] / _traderTypes.Count, 2));
            }
        }
        CurrentCountToBuy = new List<int>(CountToBuy);
    }

    public void SellItem(Item item)
    {
        for (int i = 0; i < TraderGoods.Count; i++)
        {
            if (item == TraderGoods[i].Good)
            {
                TraderGoods[i].Count--;
                break;
            }
        }
    }



    [CustomEditor(typeof(NPCTrader))]
    public class MerchantEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
            NPCTrader trader = (NPCTrader)target;

            while (trader._newPrice.Count < trader._goods.Count)
            {
                trader._newPrice.Add(0);
                trader._countOfGood.Add(0);
            }
            while (trader._newPrice.Count > trader._goods.Count)
            {
                trader._newPrice.RemoveAt(trader._newPrice.Count-1);
                trader._countOfGood.RemoveAt(trader._countOfGood.Count-1);
            }
            
            for (int i = 0; i < trader._goods.Count; i++)
            {
                if (trader._goods[i] == null)
                    continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(-18, true);
                EditorGUILayout.LabelField(trader._goods[i].Name, GUILayout.MaxWidth(90));
                EditorGUILayout.Space(1, true);
                EditorGUILayout.LabelField(trader._goods[i].Price + "-AvgPrice", GUILayout.MaxWidth(75));
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Price", GUILayout.MaxWidth(31));
                trader._newPrice[i] = EditorGUILayout.IntField(trader._newPrice[i]);
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Count", GUILayout.MaxWidth(36.5f));
                trader._countOfGood[i] = EditorGUILayout.IntField(trader._countOfGood[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
