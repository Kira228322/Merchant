using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestockHandler : MonoBehaviour
{
    // TODO запрещенные предметы правильный ресток.
    
    private int _lastRestockDay;
    private Location[] _locations;
    
    private void OnEnable()
    {
        GameTime.HourChanged += CheckRestock;
    }

    private void OnDisable()
    {
        GameTime.HourChanged -= CheckRestock;
    }

    private void Start()
    {
        _locations = FindObjectsOfType<Location>();
    }

    private void CheckRestock()
    {
        if (GameTime.Hours == 1)
            if (_lastRestockDay + 2 < GameTime.CurrentDay)
            {
                Restock();
            }
    }
    
    private void RestockBuyCoefficients(List<NpcTraderData> traders)
    {
        foreach (var trader in traders)
            trader.RestockCoefficients();
    }

    private void AddAdditiveGoods(List<NpcTraderData> traders)
    {
        foreach (var trader in traders)
        {
            if (trader.AdditiveGoods.Count > 9)
                trader.AdditiveGoods.RemoveAt(Random.Range(8, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 7)
                trader.AdditiveGoods.RemoveAt(Random.Range(6, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 4)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));

            if (Random.Range(0,3) == 0)
                return;
            
            int count = Random.Range(1, 3); 
            for (int i = 0; i < count; i++)
            {
                NpcTrader.BuyCoefficient traderBuyCoefficient;
                bool isMainGood;
                Item newItem;
                bool reallyNew;

                if (Random.Range(0, 4) == 0)
                    isMainGood = false; // не мейн тип шмотки торговца 
                else
                    isMainGood = true; // мейн тип шмотки торговца

                while (true)
                {
                    reallyNew = true;
                    traderBuyCoefficient = trader.BuyCoefficients[Random.Range(0, trader.BuyCoefficients.Count)];
                    // у мейн шмоток коэф 1
                    if ((traderBuyCoefficient.Coefficient == 1) == isMainGood)
                    {
                        newItem = ItemDatabase.GetRandomItemOfThisType(traderBuyCoefficient.itemType);

                        foreach (var goods in trader.Goods)
                        {
                            if (goods.Good.Name == newItem.Name)
                            {
                                reallyNew = false;
                                break;
                            }
                        }

                        if (!reallyNew)
                            continue;


                        int randomCount = Random.Range(1, 3);
                        
                        if (newItem.Price < 50)
                            randomCount++;
                        
                        NpcTrader.TraderGood newGood = new NpcTrader.TraderGood(newItem.Name, randomCount, 
                            randomCount, newItem.Price + Random.Range(1, newItem.Price/12 + 2));

                        trader.AdditiveGoods.Add(newGood);
                        break;
                    }
                }
            }
        }
    }

    private void RestockMainGoods(List<NpcTraderData> traders, Location location)
    {
        List<NpcTraderData> activeTraders = new List<NpcTraderData>(); // трейдеры которые будут учавствовать в очередной итерации foreach
        int gainCount; 
        
        foreach (var item in ItemDatabase.Instance.Items.ItemList)
        {
            
            // считаем сколько должно прирасти таких предметов на локации
            // budget (последний параметр метода) основываетс€ на населении локации и на интересе в этом типе предмета локацией (CoefsForItemTypes)
            // чем интерес в этом предмете больше значит тем он редкостнее и его прирастает меньше.
            gainCount = location.Region.CalculateGainOnMarket(location.CountOfEachItem[item.Name], item.Price, 
                location.ItemEconomyParams[item.Name][0], location.ItemEconomyParams[item.Name][1],
                location.ItemEconomyParams[item.Name][2],
                (int)(location.PopulationOfVillage * (2 - location.Region.CoefsForItemTypes[item.TypeOfItem])));
            
            if (gainCount == 0)
                continue;

            activeTraders.Clear();
            foreach (var trader in traders) // из всех трейдеров выбирамем тех, кто торгует таким товаром
            foreach (var traderGood in trader.Goods)
                if (item.Name == traderGood.Good.Name)
                {
                    activeTraders.Add(trader);
                    break;
                }
            
            activeTraders.Shuffle(); // Ѕлагодар€ бездушной машине сделал расширени€ дл€ List 
            // этот метод перемешивает мместами элементы в листе в случайном пор€дке
            // ƒл€ чего это нужно? чтобы ресток не начиналс€ с одного и того же трейдера каждый раз 

            
            
            if (gainCount > 0) // прирост товара
            {
                if (activeTraders.Count == 0)
                {
                    // ≈сли на локации вообще нет торговцев, которые торгует этим 
                    // то предмет рестокнетс€ с 25% шансом и только на половину от необходимого числа.
                    if (Random.Range(0, 4) == 0)
                    {
                        gainCount /= 2;
                        if (gainCount > 0)
                            traders[Random.Range(0,traders.Count)].AdditiveGoods.Add(new NpcTrader.TraderGood(item.Name, 
                                gainCount, gainCount, item.Price + Random.Range(1, item.Price/10 + 2)));
                    }
                }
                
                else
                {
                    // 5 раз прохожусь по всем трейдерам у которых есть этот предмет. ќпределнное число, так как если делать это
                    // через while(gainCount > 0), то может возникнуть ситуаци€ когда gainCount > 0 и у всех трейдеров
                    // CurrentCount будет = MaxCount и комп возрветс€, чтобы этого избежать пришлось бы добавл€ть дополнительную
                    // переменную дл€ проверки и саму проверку. ћне кажетс€ экономнее будет делать именно через for 5
                    for (int i = 0; i < 5; i++) 
                    {
                        foreach (var trader in activeTraders)
                        {
                            NpcTrader.TraderGood traderGood =
                                trader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                            if (traderGood.CurrentCountIsLessMaxCount())
                            {
                                traderGood.CurrentCount++;
                                gainCount--;
                            }
                            if (gainCount == 0)
                                break;
                        }
                        
                        if (gainCount == 0)
                            break;
                    }

                    while (gainCount > 0) // если после этого еще надо добавить предметов то добавл€ем их уже сверх Max 
                    {
                        foreach (var trader in activeTraders)
                        {
                            NpcTrader.TraderGood traderGood =
                                trader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                            
                            traderGood.CurrentCount++;
                            gainCount--;
                            
                            if (gainCount == 0)
                                break;
                        }
                    }
                }
            }
            
            else // убывание товара
            {
                foreach (var trader in traders) 
                foreach (var traderGood in trader.AdditiveGoods) // смотрим у кого в дополнительных есть товар
                    if (item.Name == traderGood.Good.Name)
                    {
                        activeTraders.Add(trader);
                        break;
                    }
                
                // а вот тут по идеи не может случитьс€ такой ситуации, когда gaincount != 0 
                // а предметы у торговцев закончились, так как gainCount всегда толкает числа к равновесному числу.
                while (gainCount != 0)
                {
                    foreach (var trader in activeTraders)
                    {
                        NpcTrader.TraderGood traderGood =
                            trader.AdditiveGoods.FirstOrDefault(good => good.Good.Name == item.Name);
                        if (traderGood.CurrentCount > 0)
                        {
                            traderGood.CurrentCount--;
                            gainCount++;
                            trader.CurrentMoney += traderGood.CurrentPrice;
                        }
                        if (gainCount == 0)
                            break;
                    }
                }
                
            }
        }
    }
    
    
    public void Restock()
    {
        if (GameTime.CurrentDay < _lastRestockDay + 2)
            return;
        
        // перебрать все локации и на них всех трейдеров 
        foreach (var location in _locations)
        {
            RestockBuyCoefficients(location.NpcTraders);
            RestockMainGoods(location.NpcTraders, location);
            AddAdditiveGoods(location.NpcTraders);
            location.CountAllItemsOnScene();
        }
        
        _lastRestockDay = GameTime.CurrentDay;
    }

    
}
