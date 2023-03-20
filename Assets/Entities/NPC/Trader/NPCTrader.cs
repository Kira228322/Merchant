using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCTrader : NPC
{
    [Serializable]
    public class TraderGood
    {
        public Item Good;
        public int MaxCount;
        public int CurrentCount;
        public int CurrentPrice;
        public TraderGood(string itemName, int maxCount, int count, int currentPrice)
        {
            Good = ItemDatabase.GetItem(itemName);
            MaxCount = maxCount;
            CurrentCount = count;
            CurrentPrice = currentPrice;
        }
        public TraderGood(NpcTraderSaveData.SavedTraderGood savedTraderGood)
        {
            Good = ItemDatabase.GetItem(savedTraderGood.nameOfGood);
            CurrentCount = savedTraderGood.currentCount;
            CurrentPrice = savedTraderGood.currentPrice;
        }
        public TraderGood(TraderGood original)
        {
            Good = original.Good;
            MaxCount = original.MaxCount;
            CurrentCount = original.CurrentCount;
            CurrentPrice = original.CurrentPrice;
        }
        public TraderGood()
        {

        }
    }
    [Serializable]
    public class BuyCoefficient
    {
        public Item.ItemType itemType;
        public float Coefficient;
        private int _countToBuy;
        public int CountToBuy
        {
            get => _countToBuy;
            set
            {
                _countToBuy = value;
                
            }
        }
        public int DefaultCountToBuy; // Главное, чтобы set этого значения выполнялся 1 раз в самом начале игры

        public BuyCoefficient(BuyCoefficient original)
        {
            itemType = original.itemType;
            Coefficient = original.Coefficient;
            CountToBuy = original.CountToBuy;
            DefaultCountToBuy = original.DefaultCountToBuy;
        }
        public BuyCoefficient(TraderType.TraderGoodType traderGoodType)
        {
            itemType = traderGoodType.ItemType;
            Coefficient = traderGoodType.Coefficient;
            CountToBuy = traderGoodType.CountToBuy;
            DefaultCountToBuy = traderGoodType.CountToBuy;
        }
        public BuyCoefficient()
        {

        }
    }
    
    [SerializeField] private NpcTraderData _npcTraderData;
    
    public List<TraderGood> Goods => _npcTraderData.Goods;
    public List<TraderGood> AdditiveGoods => _npcTraderData.AdditiveGoods;
    public List<BuyCoefficient> BuyCoefficients => _npcTraderData.BuyCoefficients; //Таких BuyCoefficients будет столько, сколько всего есть Item.ItemType (см.ниже)
    public void OpenTradeWindow()
    {
        if (_npcTraderData.LastRestock + _npcTraderData.RestockCycle >= GameTime.CurrentDay)
        {
            int count = (GameTime.CurrentDay - _npcTraderData.LastRestock) / _npcTraderData.RestockCycle;
            
            if (count > 3)
                count = 3;
            
            for (int i = 0; i < count; i++)
                Restock();
            
            _npcTraderData.LastRestock = GameTime.CurrentDay; 
        }
        TradeManager.Instance.OpenTradeWindow(this);
    }
    
    private void Restock()
    {
        RestockMainGoods();        
        RestockNewItems();
        RestockCoefficients();
    }

    private void RestockCoefficients()
    {
        foreach (var buyCoefficient in BuyCoefficients)
        {
            buyCoefficient.CountToBuy += buyCoefficient.DefaultCountToBuy / 3 + Player.Instance.Statistics.TotalDiplomacy + 1;
            if (buyCoefficient.CountToBuy > buyCoefficient.DefaultCountToBuy)
                buyCoefficient.CountToBuy = buyCoefficient.DefaultCountToBuy;
        }
    }

    private void RestockNewItems()
    {
        if (AdditiveGoods.Count > 5)
            AdditiveGoods.Clear();

        int count = Random.Range(0, Player.Instance.Statistics.TotalDiplomacy/3 + 2); // за каждые 3 дипломатии шанс на +1
            // дополнительную шмотку у торговца
        for (int i = 0; i < count; i++)
        {
            BuyCoefficient traderBuyCoefficient;
            bool isMainGood;
            Item newItem;
            bool reallyNew = true;

            if (Random.Range(0, 5) == 0)
                isMainGood = false; // не мейн тип шмотки торговца 
            else
                isMainGood = true; // мейн тип шмотки торговца

            while (true)
            {
                traderBuyCoefficient = BuyCoefficients[Random.Range(0, BuyCoefficients.Count)];
                // у мейн шмоток коэф 1
                if ( (traderBuyCoefficient.Coefficient == 1) == isMainGood) 
                {
                    newItem = ItemDatabase.GetRandomItemOfThisType(traderBuyCoefficient.itemType);
                    if (Goods.Any(t => newItem == Goods[i].Good))
                        reallyNew = false;

                    if (reallyNew)
                    {
                        TraderGood newGood = new()
                        {
                            Good = newItem,
                            CurrentCount = Random.Range(1, 3)
                        };

                        // новый предмет будет продаваться либо много дешевле, либо много дороже средней цены
                        if (Random.Range(0, 2) == 1) 
                            newGood.CurrentPrice = Random.Range(newItem.Price * 72 / 100, newItem.Price * 8 / 10 + 1);
                        else
                            newGood.CurrentPrice =
                                Random.Range(newItem.Price * 12 / 10, newItem.Price * 128 / 100 + 1);
                        
                        AdditiveGoods.Add(newGood);
                        break;
                    }
                }
            }
            
        }
    }
    
    private void RestockMainGoods()
    {
        foreach (TraderGood traderGood in Goods)
        {
            if (Random.Range(0, 11) == 0) continue;

            switch (traderGood.MaxCount)
            {
                case int n when n <= 4: // Редкие предметы, которых у торговца мало, они могут не всегда прибавиться за ресток
                    if (Random.Range(0, 101) <= 50 + Player.Instance.Statistics.TotalDiplomacy)//50% шанс, что будет ресток этой шмотки
                    {
                        if (traderGood.MaxCount == 1)
                            traderGood.CurrentCount++;
                        else
                            traderGood.CurrentCount += Random.Range(1, traderGood.MaxCount);
                    }
                    break;

                case int n when n >= 5 && n <= 19: // предметы средней средности
                    traderGood.CurrentCount += Random.Range(traderGood.MaxCount / 3, traderGood.MaxCount / 2 + 1);
                    break;

                case int n when n >= 20: // товары, которые торговец поставляет массово
                    traderGood.CurrentCount += Random.Range(traderGood.MaxCount * 3 / 5, traderGood.MaxCount * 3 / 4 + 2);
                    break;
            }

            if (traderGood.CurrentCount > traderGood.MaxCount)
                traderGood.CurrentCount = traderGood.MaxCount;
        }
    }
    public void SellItem(Item item)
    {
        foreach (TraderGood traderGood in Goods)
        {
            if (item == traderGood.Good)
            {
                traderGood.CurrentCount--;
                break;
            }
        }
    }
}

[CustomEditor(typeof(NPCTrader))]
public class NpcTraderDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.FindProperty("NpcData").isExpanded = false;

        DrawPropertiesExcluding(serializedObject, "NpcData");

        serializedObject.ApplyModifiedProperties();
    }
}