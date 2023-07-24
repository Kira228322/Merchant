using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newTraderData", menuName = "NPCs/TraderData")]
public class NpcTraderData : NpcData, IResetOnExitPlaymode, ISaveable<NpcTraderSaveData>
{
    public List<TraderType> TraderTypes;
    public bool IsBlackMarket;

    [HideInInspector, SerializeField] private List<NpcTrader.TraderGood> _baseGoods = new ();
    [HideInInspector, SerializeField] private List<NpcTrader.BuyCoefficient> _baseBuyCoefficients = new ();
    //AdditiveGoods рассчитываются только в рантайме или при загрузке, с инспектором никак не связаны

    [HideInInspector] public List<NpcTrader.TraderGood> BaseGoods => _baseGoods;
    [HideInInspector] public List<NpcTrader.BuyCoefficient> BaseBuyCoefficients => _baseBuyCoefficients;

    [HideInInspector] public List<NpcTrader.TraderGood> Goods;
    [HideInInspector] public List<NpcTrader.TraderGood> AdditiveGoods;
    [HideInInspector] public List<NpcTrader.BuyCoefficient> BuyCoefficients;
    
    public void RestockCoefficients()
    {
        foreach (var buyCoefficient in BuyCoefficients)
        {
            buyCoefficient.CountToBuy += buyCoefficient.DefaultCountToBuy / 3 + Player.Instance.Statistics.TotalDiplomacy + 1;
            if (buyCoefficient.CountToBuy > buyCoefficient.DefaultCountToBuy)
                buyCoefficient.CountToBuy = buyCoefficient.DefaultCountToBuy;
        }
    }

    NpcTraderSaveData ISaveable<NpcTraderSaveData>.SaveData()
    {
        return new(ID, CurrentMoney, Goods, AdditiveGoods, BuyCoefficients);
    }

    void ISaveable<NpcTraderSaveData>.LoadData(NpcTraderSaveData data)
    {
        LoadData(data);

        Goods = data.Goods.Select(good => new NpcTrader.TraderGood(good)).ToList();

        AdditiveGoods = data.AdditiveGoods.Select(good => new NpcTrader.TraderGood(good)).ToList();

        BuyCoefficients = data.BuyCoefficients.Select(buyCoefficient => new NpcTrader.BuyCoefficient(buyCoefficient)).ToList();
    }

    private void OnEnable()
    {
        Goods = _baseGoods.Select(good => new NpcTrader.TraderGood(good)).ToList();
        AdditiveGoods.Clear(); //AdditiveGoods рассчитываются только в рантайме или при загрузке сохранения, с инспектором никак не связаны
        SetBuyCoefficients();
        BuyCoefficients = _baseBuyCoefficients.Select(buyCoefficient => new NpcTrader.BuyCoefficient(buyCoefficient)).ToList();
    }
    void IResetOnExitPlaymode.ResetOnExitPlaymode()
    {
        ResetOnExitPlaymode();
        Goods.Clear();
        AdditiveGoods.Clear();
        BuyCoefficients.Clear();
    }

    private void SetBuyCoefficients()
    {
        _baseBuyCoefficients.Clear();
        Dictionary<Item.ItemType, TraderType.TraderGoodType> mergedTraderGoods = new();

        foreach (var traderGoodType in TraderTypes.SelectMany(tt => tt.TraderGoodTypes))
        {
            // Если в словаре уже есть такой ItemType, то обновляет его значения
            // (считает средние между старым и новым или приравнивает к единице)
            // Если ещё нету, то добавляет новый TraderGoodType с этим ItemType
            mergedTraderGoods[traderGoodType.ItemType] = mergedTraderGoods.TryGetValue(traderGoodType.ItemType, out var existingTraderGoodType)
                ? new TraderType.TraderGoodType
                {
                    ItemType = traderGoodType.ItemType,
                    Coefficient = traderGoodType.Coefficient != 1 && existingTraderGoodType.Coefficient != 1
                    ? (existingTraderGoodType.Coefficient + traderGoodType.Coefficient) / 2
                    : 1,
                    CountToBuy = traderGoodType.Coefficient != 1 && existingTraderGoodType.Coefficient != 1
                    ? (existingTraderGoodType.CountToBuy + traderGoodType.CountToBuy) / 2
                    : Math.Max(existingTraderGoodType.CountToBuy, traderGoodType.CountToBuy)
                }
                : new(traderGoodType);
        }

        List<NpcTrader.BuyCoefficient> resultingBuyCoefficients = mergedTraderGoods.Values.Select
            (type => new NpcTrader.BuyCoefficient(type)).ToList();
        _baseBuyCoefficients.AddRange(resultingBuyCoefficients);

    }
    
}
