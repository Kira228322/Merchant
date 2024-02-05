using System;
using System.Collections.Generic;
using System.Linq;
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
        public Item.ItemType ItemType;
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
            ItemType = original.ItemType;
            Coefficient = original.Coefficient;
            CountToBuy = original.CountToBuy;
            DefaultCountToBuy = original.DefaultCountToBuy;
        }
        public BuyCoefficient(TraderType.TraderGoodType traderGoodType)
        {
            ItemType = traderGoodType.ItemType;
            Coefficient = traderGoodType.Coefficient;
            CountToBuy = traderGoodType.CountToBuy;
            DefaultCountToBuy = traderGoodType.CountToBuy;
        }
        public BuyCoefficient()
        {

        }
    }
    
    private NpcTraderData _npcTraderData;

    private void Awake()
    {
        _npcTraderData = (NpcTraderData)NpcData;
    }

    public List<TraderGood> Goods => _npcTraderData.Goods;
    public List<TraderGood> AdditiveGoods => _npcTraderData.AdditiveGoods; 
    public List<BuyCoefficient> BuyCoefficients => _npcTraderData.BuyCoefficients; //Таких BuyCoefficients будет столько, сколько всего есть Item.ItemType (см.ниже)
    public void OpenTradeWindow()
    {
        TradeManager.Instance.OpenTradeWindow(this);
    }

    public void SellItem(Item item)
    {
        TraderGood traderGood = Goods.FirstOrDefault(good => good.Good.Name == item.Name);
        if (traderGood != null)
        {
            traderGood.CurrentCount--;
            return;
        }
        traderGood = AdditiveGoods.FirstOrDefault(good => good.Good.Name == item.Name);
        if (traderGood != null)
        {
            traderGood.CurrentCount--;
            if (traderGood.CurrentCount <= 0)
            {
                AdditiveGoods.Remove(traderGood);
            }
        }
        else
        {
            Debug.LogError("Такого предмета не было в Goods или AdditiveGoods");
        }
    }
}