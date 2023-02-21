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
    }
    
    private List<TraderType> _traderTypes = new();
    private int _restockCycle;
    private int _lastRestock;
    public NPCTraderData NPCData; //TODO: npcData лучше забирать из базы данных, чем по ручной ссылке.
    [HideInInspector] public List<TraderGood> Goods = new();
    [HideInInspector] public List<TraderBuyCoefficient> BuyCoefficients = new(); //Таких BuyCoefficients будет столько, сколько всего есть Item.ItemType (см.ниже)

    protected override void Start()
    {
        SetNPCFromData(NPCData);
        SetTraderStats();

    }
    public override void SetNPCFromData(NPCData npcData)
    {
        base.SetNPCFromData(npcData);
        NPCTraderData npcTraderData = npcData as NPCTraderData;
        _traderTypes = npcTraderData.TraderTypes;
        Goods = npcTraderData.Goods;
        _restockCycle = npcTraderData.RestockCycle;
        _lastRestock = npcTraderData.LastRestock;
    }
    private void SetTraderStats()
    {
        foreach (Item.ItemType itemType in Enum.GetValues(typeof(Item.ItemType)))
        //Enum.GetValues(typeof(Item.ItemType)) возвращает все возможные значения Item.ItemType
        //(если потом добавим новые ItemType, сюда не придется возвращаться)
        {
            TraderBuyCoefficient traderBuyCoefficient = new();
            traderBuyCoefficient.itemType = itemType;
            traderBuyCoefficient.Coefficient = 0;
            traderBuyCoefficient.CountToBuy = 0;
            //Если мы не переприсвоим (т.е не найдем таких же типов айтемов среди назначенных типов торговцев, то они останутся 0, как и надо.)
            BuyCoefficients.Add(traderBuyCoefficient);
        }
        //Таким образом мы создали столько BuyCoefficient, сколько у нас всего существует Item.ItemType.
        //Теперь будем искать средние значения либо значения специализации
        foreach (TraderBuyCoefficient traderBuyCoefficient in BuyCoefficients)
        {

            //найти все traderGoodType, у которых наличествует такой же itemType:
            List<TraderType.TraderGoodType> acceptableTraderGoodTypes =
            _traderTypes.SelectMany(traderType => traderType.TraderGoodTypes.Where
            (traderGoodType => traderGoodType.ItemType == traderBuyCoefficient.itemType)).ToList();
            //Этот Linq возвращает все TraderGoodType, которые
            //имеют такой же ItemType как у нашего рассматриваемого traderBuyCoefficient
            //https://i.redd.it/dfa2qicl7rc91.png

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
            if (acceptableTraderGoodTypes.Count != 0) //избежать деления на ноль, такие типы будут иметь Coefficient & CountToBuy == 0
            {
                traderBuyCoefficient.CountToBuy = (int)Math.Ceiling((float)traderBuyCoefficient.CountToBuy / acceptableTraderGoodTypes.Count);
                if (traderBuyCoefficient.Coefficient != 1)
                    traderBuyCoefficient.Coefficient = (float)Math.Round(traderBuyCoefficient.Coefficient / acceptableTraderGoodTypes.Count, 2);
            }
        }
    }

    public void Restock()
    {
        foreach (TraderGood traderGood in Goods)
        {
            if (UnityEngine.Random.Range(0, 11) == 0) continue;

            switch (traderGood.MaxCount)
            {
                case int n when n <= 4: // Редкие предметы, которых у торговца мало, они могут не всегда прибавиться за ресток
                    if (Random.Range(0, 2) == 0) // 50% шанс, что будет ресток этой шмотки
                    {
                        if (traderGood.MaxCount == 1)
                            traderGood.Count++;
                        else
                            traderGood.Count += Random.Range(1, traderGood.MaxCount);
                    }
                    break;

                case int n when n >= 5 && n <= 19: // предметы средней средности
                    traderGood.Count += Random.Range(traderGood.MaxCount / 3, traderGood.MaxCount / 2 + 1);
                    break;

                case int n when n >= 20: // товары, которые торговец поставляет массово
                    traderGood.Count += Random.Range(traderGood.MaxCount * 3 / 5, traderGood.MaxCount * 3 / 4 + 2);
                    break;
            }

            if (traderGood.Count > traderGood.MaxCount)
                traderGood.Count = traderGood.MaxCount;

            // TODO
            // еще хочу сделать так, чтобы с небольшим шансом торговец начал торговать каким-то новым предметом
            // Который подходит его специализации. Но для этого надо с базой данных работать
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
