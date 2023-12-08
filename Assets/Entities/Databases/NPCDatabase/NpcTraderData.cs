using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newTraderData", menuName = "NPCs/TraderData")]
public class NpcTraderData : NpcData, IResetOnExitPlaymode, ISaveable<NpcTraderSaveData>
{
    public List<TraderType> TraderTypes;
    [Tooltip("����� �� �������� �������� ����������� ��������")]public bool IsBlackMarket;
    [Tooltip("����� �� �������� ��������� ��, ��� ����� � ������")]public bool HaveAdditiveGoods;

    [HideInInspector, SerializeField] private List<NpcTrader.TraderGood> _baseGoods = new ();
    [HideInInspector, SerializeField] private List<NpcTrader.BuyCoefficient> _baseBuyCoefficients = new ();
    //AdditiveGoods �������������� ������ � �������� ��� ��� ��������, � ����������� ����� �� �������

    [HideInInspector] public List<NpcTrader.TraderGood> BaseGoods => _baseGoods;
    [HideInInspector] public List<NpcTrader.BuyCoefficient> BaseBuyCoefficients => _baseBuyCoefficients;

    [HideInInspector] public List<NpcTrader.TraderGood> Goods;
    [HideInInspector] public List<NpcTrader.TraderGood> AdditiveGoods;
    [HideInInspector] public List<NpcTrader.BuyCoefficient> BuyCoefficients;
    
    public void RestockCoefficients()
    {
        if (CurrentMoney > GameStartMoney)
            CurrentMoney -= (CurrentMoney - GameStartMoney + 3) / 4;
        else
            CurrentMoney += (GameStartMoney - CurrentMoney + 1) / 2;
        
        foreach (var buyCoefficient in BuyCoefficients)
        {
            buyCoefficient.CountToBuy += buyCoefficient.DefaultCountToBuy / 4 + Player.Instance.Statistics.Diplomacy.Total + 1;
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
        if (AdditiveGoods != null)
            AdditiveGoods.Clear(); //AdditiveGoods �������������� ������ � �������� ��� ��� �������� ����������, � ����������� ����� �� �������
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
        //����� ������, ��� ����������: ������� ������� ��� TraderGoodTypes �� TraderTypes.
        //��� ������� �� ������ IGrouping �� LINQ �� �������� ItemType.
        //���� ���� ������������� (�.� ����������� ����� 1 ���� �� � ������ ��������� � ������), ��
        //��� ��������� �������� ��������������� ����������� 1 � ������������ CountToBuy �� ����.
        //���� ������������� ���, �� � ������ ������ ��������� ������� ����������� � ������� CountToBuy
        //(��� ���� ��������, ��� ���� � ������ ������ ���� �������,
        //�� ��� ����������� � CountToBuy ��������� ��������)

        var allGoodTypes = TraderTypes
            .SelectMany(tt => tt.TraderGoodTypes)               
            .Select(traderGoodType => new TraderType.TraderGoodType(traderGoodType))                       
            .ToList();

        var mergedTraderGoods = allGoodTypes
            .GroupBy(item => item.ItemType)                             
            .ToDictionary(group => group.Key, group =>
            {
                float averageCoefficient = group.Average(item => item.Coefficient);
                int averageCountToBuy = (int)group.Average(item => item.CountToBuy);
                int maxCountToBuy = group.Max(item => item.CountToBuy);
                return new TraderType.TraderGoodType()
                {
                    ItemType = group.Key,
                    Coefficient = group.Max(item => item.Coefficient) == 1 ? 1 : averageCoefficient,
                    CountToBuy = group.Max(item => item.Coefficient) == 1 ? maxCountToBuy : averageCountToBuy
                };
            });
        _baseBuyCoefficients.Clear();
        _baseBuyCoefficients.AddRange(mergedTraderGoods.Values.Select(type => new NpcTrader.BuyCoefficient(type)));
    }
    
}
