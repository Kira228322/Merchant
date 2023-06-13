using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcTrader : Npc
{
    [Serializable]
    public class TraderGood
    {
        public Item Good;
        public int MaxCount;
        public int CurrentCount;
        public int CurrentPrice;

        public bool CurrentCountIsLessMaxCount()
        {
            return CurrentCount < MaxCount;
        }
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
        public int DefaultCountToBuy; // √лавное, чтобы set этого значени€ выполн€лс€ 1 раз в самом начале игры

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
    public List<TraderGood> AdditiveGoods => _npcTraderData.AdditiveGoods; // TODO ≈ще не знаю как это работает,
                                                                           // но если вдруг осталось 0 предметов, то удал€етс€ ли 
                                                                           // из листа элемент? надо провер€ть.
    public List<BuyCoefficient> BuyCoefficients => _npcTraderData.BuyCoefficients; //“аких BuyCoefficients будет столько, сколько всего есть Item.ItemType (см.ниже)
    public void OpenTradeWindow()
    {
        TradeManager.Instance.OpenTradeWindow(this);
    }
    
    public void RestockCoefficients()
    {
        foreach (var buyCoefficient in BuyCoefficients)
        {
            buyCoefficient.CountToBuy += buyCoefficient.DefaultCountToBuy / 3 + Player.Instance.Statistics.TotalDiplomacy + 1;
            if (buyCoefficient.CountToBuy > buyCoefficient.DefaultCountToBuy)
                buyCoefficient.CountToBuy = buyCoefficient.DefaultCountToBuy;
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