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
public class NpcSaveDataTrader: NpcSaveData
{
    public int LastRestock;
    public List<NPCTrader.TraderGood> Goods;
    public List<NPCTrader.TraderGood> AdditiveGoods;
    public List<NPCTrader.TraderBuyCoefficient> BuyCoefficients;
    public NpcSaveDataTrader(int affinity, int id, int lastRestock, List<NPCTrader.TraderGood> goods, List<NPCTrader.TraderGood> additiveGoods, List<NPCTrader.TraderBuyCoefficient> buyCoefficients) : base(affinity, id)
    {
        LastRestock = lastRestock;
        Goods = goods;
        AdditiveGoods = additiveGoods;
        BuyCoefficients = buyCoefficients;
    }
}

