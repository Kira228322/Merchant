using System;
using System.Collections.Generic;
using System.Linq;

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
public class NpcTraderSaveData : NpcSaveData
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

    public List<SavedTraderGood> Goods = new();
    public List<SavedTraderGood> AdditiveGoods = new();
    public List<SavedTraderGood> Recipes = new();
    public List<NpcTrader.BuyCoefficient> BuyCoefficients;
    public NpcTraderSaveData(int id, int money, List<NpcTrader.TraderGood> goods, List<NpcTrader.TraderGood> additiveGoods, List<NpcTrader.BuyCoefficient> buyCoefficients, List<NpcTrader.TraderGood> recipes) : base(id, money)
    {
        foreach (NpcTrader.TraderGood traderGood in goods)
        {
            Goods.Add(new(traderGood));
        }
        foreach (NpcTrader.TraderGood traderGood in additiveGoods)
        {
            AdditiveGoods.Add(new(traderGood));
        }
        foreach (NpcTrader.TraderGood recipe in recipes)
        {
            Recipes.Add(new(recipe));
        }
        BuyCoefficients = buyCoefficients;
    }
}
[Serializable]
public class NpcQuestGiverSaveData : NpcSaveData
{

    public int LastGiveQuestDay;
    public NpcQuestGiverSaveData(int id, int money, int day) : base(id, money)
    {
        LastGiveQuestDay = day;
    }
}

[Serializable]
public class NpcWagonUpgraderSaveData : NpcSaveData
{
    public List<string> CurrentUpgrades;
    public NpcWagonUpgraderSaveData(int id, int money, List<WagonPart> currentUpgrades) : base(id, money)
    {
        CurrentUpgrades = currentUpgrades.Select(upgrade => upgrade.Name).ToList();
    }
}

