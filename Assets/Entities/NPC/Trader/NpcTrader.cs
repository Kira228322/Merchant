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
        public int DefaultCountToBuy; // �������, ����� set ����� �������� ���������� 1 ��� � ����� ������ ����

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
    public List<BuyCoefficient> BuyCoefficients => _npcTraderData.BuyCoefficients; //����� BuyCoefficients ����� �������, ������� ����� ���� Item.ItemType (��.����)
    public void OpenTradeWindow()
    {
        // TODO
        // ������ ������ ������������, ����� ����������� ���� � ���. ��� �������, ���� ����� � ����������� �������� �����������
        // ���� ����������� ������ � ������������ ������ � ������. �������� ����� ����� �� �������. ������� ����� ���������� ��� ���� ��������� �� �������
        // � ������ � ����. �������� � 00:00 �� 01:00, ���� ���������� ������ � 00:30 �� ������ ���� ��� ������. � ����� ����������� ������, ����� 
        // ���������� ������ �� �����
        
        if (_npcTraderData.LastRestock + _npcTraderData.RestockCycle <= GameTime.CurrentDay)
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
        // �������� ����� ����������� � ������ ��������� ����� �������, �������� �� ������� ���.
        // ��� ������������� ���� �����
    }
    
    private void RestockMainGoods()
    {
        //TODO
        // ����������� �� ������ � ����������� ������� ����� ����������� �������� � ������ �������. ���������� �� ���� ��������� �������
        // � ������� ������ ����������� � ������� 5 �����. 3 �������� ������� ������. 2 �������� ������� �� 2 �����, 1 �������- ����.
        
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