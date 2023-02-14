using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCTrader : NPC
{
    [Header("Trader settings")]
    [SerializeField] private List<TraderType> _traderTypes;
    [SerializeField] private int _restockCycle;
    [Space] 
    [SerializeField] private List<Item> _goods;
    public List<TraderGood> TraderGoods = new();
    
    [HideInInspector] public List<float> Coefficient = new ();
    [HideInInspector] public List<int> CountToBuy = new ();
    [HideInInspector] public List<int> CurrentCountToBuy = new ();

    [HideInInspector] public List<int> NewPrice = new ();
    [HideInInspector] public List<int> CountOfGood = new ();

    private int _lastRestock;
    public class TraderGood
    {
        public Item Good;
        internal int _maxCount;
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
            TraderGoods[i].Good.Price = NewPrice[i];
            TraderGoods[i].Count = CountOfGood[i];
            TraderGoods[i]._maxCount = CountOfGood[i];
        }
    }

    public void Restock()
    {
        // _lastRestock = GameTime.Day; TODO
        
        foreach (var good in TraderGoods)
        {
            if (Random.Range(0,11) == 0)
                continue;
            
            switch (good._maxCount)
            {
                case int n when n <= 4: // Редкие предметы, которых у торговца мало, они могут не всегда прибавиться за ресток
                    if (Random.Range(0, 2) == 0) // 50% шанс, что будет ресток этой шмотки
                    {
                        if (good._maxCount == 1)
                            good.Count++;
                        else
                            good.Count += Random.Range(1, good._maxCount);
                    }
                    break;
                
                case int n when n >= 5 && n <= 19: // предметы средней средности
                    good.Count += Random.Range(good._maxCount / 3, good._maxCount/2 + 1);
                    break;
                
                case int n when n >= 20: // товары, которые торговец поставляет массово
                    good.Count += Random.Range(good._maxCount * 3 / 5, good._maxCount * 3 / 4 + 2);
                    break;
            }

            if (good.Count > good._maxCount)
                good.Count = good._maxCount;
            
            // TODO
            // еще хочу сделать так, чтобы с небольшим шансом торговец начал торговать каким-то новым предметом
            // Который подходит его специализации. Но для этого надо с базой данных работать
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

            while (trader.NewPrice.Count < trader._goods.Count)
            {
                trader.NewPrice.Add(0);
                trader.CountOfGood.Add(0);
            }
            while (trader.NewPrice.Count > trader._goods.Count)
            {
                trader.NewPrice.RemoveAt(trader.NewPrice.Count-1);
                trader.CountOfGood.RemoveAt(trader.CountOfGood.Count-1);
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
                trader.NewPrice[i] = EditorGUILayout.IntField(trader.NewPrice[i]);
                
                EditorGUILayout.Space(2, true);
                EditorGUILayout.LabelField("Count", GUILayout.MaxWidth(36.5f));
                trader.CountOfGood[i] = EditorGUILayout.IntField(trader.CountOfGood[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
