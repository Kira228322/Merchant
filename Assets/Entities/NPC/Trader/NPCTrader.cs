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
        [HideInInspector] public int Count;
        public int CurrentPrice;
    }
    [Serializable]
    public class TraderBuyCoefficient
    {
        public Item.ItemType itemType;
        public float Coefficient;
        public int CountToBuy;
        public int DefaultCountToBuy; // �������, ����� set ����� �������� ���������� 1 ��� � ����� ������ ����
    }
    
    private List<TraderType> _traderTypes = new();

    [HideInInspector] public List<TraderGood> Goods = new();
    [HideInInspector] public List<TraderGood> AdditiveGoods = new();
    [HideInInspector] public List<TraderBuyCoefficient> BuyCoefficients = new(); //����� BuyCoefficients ����� �������, ������� ����� ���� Item.ItemType (��.����)

    protected void Start()
    {
        SetNPCFromData(NpcData);
        SetTraderStats();
    }
    public void SetNPCFromData(NPCData npcData)
    {
        TraderData traderData = npcData as TraderData;
        _traderTypes = traderData.TraderTypes;
        Goods = traderData.Goods;
    }
    private void SetTraderStats()
    {
        foreach (Item.ItemType itemType in Enum.GetValues(typeof(Item.ItemType)))
        //Enum.GetValues(typeof(Item.ItemType)) ���������� ��� ��������� �������� Item.ItemType
        //(���� ����� ������� ����� ItemType, ���� �� �������� ������������)
        {
            TraderBuyCoefficient traderBuyCoefficient = new();
            traderBuyCoefficient.itemType = itemType;
            traderBuyCoefficient.Coefficient = 0;
            traderBuyCoefficient.CountToBuy = 0;
            //���� �� �� ������������ (�.� �� ������ ����� �� ����� ������� ����� ����������� ����� ���������, �� ��� ��������� 0, ��� � ����.)
            BuyCoefficients.Add(traderBuyCoefficient);
        }
        //����� ������� �� ������� ������� BuyCoefficient, ������� � ��� ����� ���������� Item.ItemType.
        //������ ����� ������ ������� �������� ���� �������� �������������
        foreach (TraderBuyCoefficient traderBuyCoefficient in BuyCoefficients)
        {

            //����� ��� traderGoodType, � ������� ������������ ����� �� itemType:
            List<TraderType.TraderGoodType> acceptableTraderGoodTypes =
            _traderTypes.SelectMany(traderType => traderType.TraderGoodTypes.Where
            (traderGoodType => traderGoodType.ItemType == traderBuyCoefficient.itemType)).ToList();
            //���� Linq ���������� ��� TraderGoodType, �������
            //����� ����� �� ItemType ��� � ������ ���������������� traderBuyCoefficient

            foreach (TraderType.TraderGoodType traderGoodType in acceptableTraderGoodTypes)
            {
                traderBuyCoefficient.CountToBuy += traderGoodType.CountToBuy;
            }
            foreach (TraderType.TraderGoodType traderGoodType in acceptableTraderGoodTypes)
            {
                if (traderGoodType.Coefficient == 1)
                {
                    traderBuyCoefficient.Coefficient = 1;
                    break;
                }
                traderBuyCoefficient.Coefficient += traderGoodType.Coefficient;
            }
            if (acceptableTraderGoodTypes.Count != 0) //�������� ������� �� ����, ����� ���� ����� ����� Coefficient & CountToBuy == 0
            {
                traderBuyCoefficient.CountToBuy = (int)Math.Ceiling((float)traderBuyCoefficient.CountToBuy / acceptableTraderGoodTypes.Count);
                traderBuyCoefficient.DefaultCountToBuy = traderBuyCoefficient.CountToBuy;
                if (traderBuyCoefficient.Coefficient != 1)
                    traderBuyCoefficient.Coefficient = (float)Math.Round(traderBuyCoefficient.Coefficient / acceptableTraderGoodTypes.Count, 2);
            }
        }
    }

    public void Restock()
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

        int count = Random.Range(0, Player.Instance.Statistics.TotalDiplomacy/3 + 2); // �� ������ 3 ���������� ���� �� +1
            // �������������� ������ � ��������
        for (int i = 0; i < count; i++)
        {
            TraderBuyCoefficient traderBuyCoefficient;
            bool isMainGood;
            Item newItem;
            bool reallyNew = true;

            if (Random.Range(0, 6) == 0)
                isMainGood = false; // �� ���� ��� ������ �������� 
            else
                isMainGood = true; // ���� ��� ������ ��������

            while (true)
            {
                traderBuyCoefficient = BuyCoefficients[Random.Range(0, BuyCoefficients.Count)];
                // � ���� ������ ���� 1
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
                            Count = Random.Range(1, 3)
                        };

                        // ����� ������� ����� ����������� ���� ����� �������, ���� ����� ������ ������� ����
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
                case int n when n <= 4: // ������ ��������, ������� � �������� ����, ��� ����� �� ������ ����������� �� ������
                    if (Random.Range(0, 101) <= 50 + Player.Instance.Statistics.TotalDiplomacy)//50% ����, ��� ����� ������ ���� ������
                    {
                        if (traderGood.MaxCount == 1)
                            traderGood.Count++;
                        else
                            traderGood.Count += Random.Range(1, traderGood.MaxCount);
                    }
                    break;

                case int n when n >= 5 && n <= 19: // �������� ������� ���������
                    traderGood.Count += Random.Range(traderGood.MaxCount / 3, traderGood.MaxCount / 2 + 1);
                    break;

                case int n when n >= 20: // ������, ������� �������� ���������� �������
                    traderGood.Count += Random.Range(traderGood.MaxCount * 3 / 5, traderGood.MaxCount * 3 / 4 + 2);
                    break;
            }

            if (traderGood.Count > traderGood.MaxCount)
                traderGood.Count = traderGood.MaxCount;
        }
    }
    public void SellItem(Item item)
    {
        foreach (TraderGood traderGood in Goods)
        {
            if (item == traderGood.Good)
            {
                traderGood.Count--;
                break;
            }
        }
    }
}
