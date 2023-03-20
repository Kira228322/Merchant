using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class NpcSaveData
{
    public int ID;
    public int Affinity;

    public NpcSaveData(int affinity, int id)
    {
        Affinity = affinity;
        ID = id;
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

        public SavedTraderGood(NPCTrader.TraderGood traderGood)
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
    public List<NPCTrader.BuyCoefficient> BuyCoefficients;
    public NpcTraderSaveData(int affinity, int id, int lastRestock, List<NPCTrader.TraderGood> goods, List<NPCTrader.TraderGood> additiveGoods, List<NPCTrader.BuyCoefficient> buyCoefficients) : base(affinity, id)
    {
        LastRestock = lastRestock;
        foreach (NPCTrader.TraderGood traderGood in goods)
        {
            Goods.Add(new(traderGood));
        }
        foreach (NPCTrader.TraderGood traderGood in additiveGoods)
        {
            AdditiveGoods.Add(new(traderGood));
        }
        BuyCoefficients = buyCoefficients;
    }
}

