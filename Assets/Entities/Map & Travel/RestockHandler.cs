using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestockHandler : MonoBehaviour
{
    private int _lastRestockDay;
    private Location[] _locations;
    
    private void OnEnable()
    {
        GameTime.HourChanged += CheckRestock;
        GameTime.TimeSkipped += OnTimeSkipped;
    }

    private void OnDisable()
    {
        GameTime.HourChanged -= CheckRestock;
        GameTime.TimeSkipped -= OnTimeSkipped;
    }

    private void Start()
    {
        _locations = FindObjectsOfType<Location>(true);
        foreach (var location in _locations)
            location.CountAllItemsOnScene();
    }

    private void CheckRestock()
    {
        if (_lastRestockDay + 2 < GameTime.CurrentDay)
        { 
            Restock();
        }
    }
    private void OnTimeSkipped(int skippedDays, int skippedHours, int skippedMinutes)
    {
        CheckRestock();
    }
    
    public void Restock()
    {
        foreach (var location in _locations)
        {
            RestockTraders(location);
            RestockWagonUpgraders(location);
        }


        _lastRestockDay = GameTime.CurrentDay;
    }
    private void RestockTraders(Location location)
    {
        //TODO: Каждый из этих методов проходит через foreach заново всех трейдеров.
        //Сделать методы для одного трейдера и запихнуть под общий foreach (var trader in location)?
        
        if (location.NpcTraders.Count <= 0)
            return;
        if (location.NpcTraders[0].Name != "Pidor Pidorovich")
            return;
        
        RestockBuyCoefficients(location.NpcTraders);
        StartCoroutine(RestockMainGoods(location.NpcTraders, location));
        
        
        // TODO когда у всех трейдеров будет установлен тип -- раскоммитить строчку! 
        // AddAdditiveGoods(location.NpcTraders);
        location.CountAllItemsOnScene();
    }
    private void RestockWagonUpgraders(Location location)
    {
        foreach (NpcWagonUpgraderData wagonUpgrader in location.WagonUpgraders)
        {
            wagonUpgrader.RestockParts();
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
            if (!trader.HaveAdditiveGoods)
                continue;

            if (trader.AdditiveGoods.Count > 10)
                trader.AdditiveGoods.RemoveAt(Random.Range(9, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 7)
                trader.AdditiveGoods.RemoveAt(Random.Range(6, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 4)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
            else
            {
                if (Random.Range(0, 2) == 0)
                    trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
            }

            if (Random.Range(0, 3) == 0)
                continue;

            int count = Random.Range(1, 3);
            for (int i = 0; i < count; i++)
            {
                NpcTrader.BuyCoefficient traderBuyCoefficient;
                bool isMainGood;
                Item newItem;
                bool reallyNew;

                if (Random.Range(0, 3) == 0)
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
                        newItem = ItemDatabase.GetRandomItemOfThisType(traderBuyCoefficient.ItemType);

                        if (BannedItemsHandler.Instance.IsItemBanned(newItem))
                            continue;

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
                            randomCount, newItem.Price + Random.Range(1, newItem.Price / 12 + 2));

                        trader.AdditiveGoods.Add(newGood);
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator RestockMainGoods(List<NpcTraderData> traders, Location location)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        
        List<NpcTraderData> activeTraders = new List<NpcTraderData>(); // трейдеры которые будут учавствовать в очередной итерации foreach
        int gainCount;

        
        foreach (var item in ItemDatabase.Instance.Items.ItemList)
        {

            // считаем сколько должно прирасти таких предметов на локации
            // budget (последний параметр метода) основывается на населении локации и на интересе в этом типе предмета локацией (CoefsForItemTypes)
            // чем интерес в этом предмете больше значит тем он редкостнее и его прирастает меньше.
            activeTraders.Clear();
            
            Debug.Log(item.Name);
            yield return waitForEndOfFrame;
            
            foreach (var trader in traders) // из всех трейдеров выбирамем тех, кто торгует таким товаром
            foreach (var traderGood in trader.Goods)
                if (item.Name == traderGood.Good.Name)
                {
                    activeTraders.Add(trader);
                    break;
                }
            if (activeTraders.Count == 0)
                continue;
            
            gainCount = location.Region.CalculateGainOnMarket(location.CountOfEachItem[item.Name], item.Price,
                location.ItemEconomyParams[item.Name][0], location.ItemEconomyParams[item.Name][1],
                location.ItemEconomyParams[item.Name][2],
                (int)(location.PopulationOfVillage * (2 - location.Region.CoefsForItemTypes[item.TypeOfItem])));

            if (gainCount == 0)
                continue;

            // забаненные предметы будут прибавляться мало и только у одного торговца разом (типо поставка)(и экономия ресурсов)
            // убавляться не будут, так как тяжело придумать ситуацию, когда запрещенный предмет будет в избытке
            // (ведь он пропадает у всех торговцев, когда появляется ивент) 
            if (BannedItemsHandler.Instance.IsItemBanned(item))
            {
                activeTraders.Clear();
                foreach (var trader in traders)
                    if (trader.IsBlackMarket)
                        activeTraders.Add(trader);

                if (activeTraders.Count == 0)
                    continue;

                if (gainCount > 0)
                {
                    gainCount = (gainCount + 1) / 2;
                    NpcTraderData targetTrader = activeTraders[Random.Range(0, activeTraders.Count)];
                    NpcTrader.TraderGood traderGood =
                        targetTrader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                    if (traderGood != null)
                        traderGood.CurrentCount += gainCount;
                    else
                        targetTrader.AdditiveGoods.Add(new NpcTrader.TraderGood(item.Name,
                            gainCount, gainCount, item.Price + Random.Range(1, item.Price / 10 + 2)));
                    continue;
                }
            }

            activeTraders.Shuffle(); // Благодаря бездушной машине сделал расширения для List 
                                     // этот метод перемешивает мместами элементы в листе в случайном порядке
                                     // Для чего это нужно? чтобы ресток не начинался с одного и того же трейдера каждый раз 



            if (gainCount > 0) // прирост товара
            {
                if (activeTraders.Count == 0)
                {
                    // Если на локации вообще нет торговцев, которые торгует этим 
                    // то предмет рестокнется только на половину от необходимого числа.
                    gainCount /= 2;
                    if (gainCount > 0)
                        traders[Random.Range(0, traders.Count)].AdditiveGoods.Add(new NpcTrader.TraderGood(item.Name,
                            gainCount, gainCount, item.Price + Random.Range(1, item.Price / 10 + 2)));
                    
                }
                else
                {
                    // 5 раз прохожусь по всем трейдерам у которых есть этот предмет. Определнное число, так как если делать это
                    // через while(gainCount > 0), то может возникнуть ситуация когда gainCount > 0 и у всех трейдеров
                    // CurrentCount будет = MaxCount и комп возрвется, чтобы этого избежать пришлось бы добавлять дополнительную
                    // переменную для проверки и саму проверку. Мне кажется экономнее будет делать именно через for 5
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

                    while (gainCount > 0) // если после этого еще надо добавить предметов то добавляем их уже сверх Max 
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
                            //TODO проверить может ли быть такое, что предмет одновременно находится и в mainGoods и в additive
                            activeTraders.Add(trader);
                            break;
                        }
            
                // а вот тут по идеи не может случиться такой ситуации, когда gaincount != 0 
                // а предметы у торговцев закончились, так как gainCount всегда толкает числа к равновесному числу.
                while (gainCount != 0)
                {
                    foreach (var trader in activeTraders)
                    {
                        NpcTrader.TraderGood traderGood =
                            trader.AdditiveGoods.FirstOrDefault(good => good.Good.Name == item.Name);
                        if (traderGood == null)
                            traderGood = trader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                        
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

}
