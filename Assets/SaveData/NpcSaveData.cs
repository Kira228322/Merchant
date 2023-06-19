using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class NpcSaveData
{
    public int ID;
    public int Money;

    public NpcSaveData(int id, int money)
    {
        ID = id;
        Money = money;
    }
}

[Serializable]
public class NpcTraderSaveData: NpcSaveData
{

    [Serializable]
    public class SavedTraderGood
    {
        public string nameOfGood;
        public int maxCount;
        public int currentCount;
        public int currentPrice;

        public SavedTraderGood(NpcTrader.TraderGood traderGood)
        {
            nameOfGood = traderGood.Good.Name;
            maxCount = traderGood.MaxCount;
            currentCount = traderGood.CurrentCount;
            currentPrice = traderGood.CurrentPrice;
        }
    }

    public int LastRestock;
    public List<SavedTraderGood> Goods = new();
    public List<SavedTraderGood> AdditiveGoods = new();
    public List<NpcTrader.BuyCoefficient> BuyCoefficients;
    public NpcTraderSaveData(int id, int money, int lastRestock, List<NpcTrader.TraderGood> goods, List<NpcTrader.TraderGood> additiveGoods, List<NpcTrader.BuyCoefficient> buyCoefficients) : base(id, money)
    {
        LastRestock = lastRestock;
        foreach (NpcTrader.TraderGood traderGood in goods)
        {
            Goods.Add(new(traderGood));
        }
        foreach (NpcTrader.TraderGood traderGood in additiveGoods)
        {
            AdditiveGoods.Add(new(traderGood));
        }
        BuyCoefficients = buyCoefficients;
    }
}

