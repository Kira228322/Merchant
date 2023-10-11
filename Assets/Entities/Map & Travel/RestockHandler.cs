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
        //TODO: ������ �� ���� ������� �������� ����� foreach ������ ���� ���������.
        //������� ������ ��� ������ �������� � ��������� ��� ����� foreach (var trader in location)?
        
        if (location.NpcTraders.Count <= 0)
            return;
        if (location.NpcTraders[0].Name != "Pidor Pidorovich")
            return;
        
        RestockBuyCoefficients(location.NpcTraders);
        StartCoroutine(RestockMainGoods(location.NpcTraders, location));
        
        
        // TODO ����� � ���� ��������� ����� ���������� ��� -- ������������ �������! 
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
                    isMainGood = false; // �� ���� ��� ������ �������� 
                else
                    isMainGood = true; // ���� ��� ������ ��������
    
                while (true)
                {
                    reallyNew = true;
                    traderBuyCoefficient = trader.BuyCoefficients[Random.Range(0, trader.BuyCoefficients.Count)];
                    // � ���� ������ ���� 1
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
        
        List<NpcTraderData> activeTraders = new List<NpcTraderData>(); // �������� ������� ����� ������������ � ��������� �������� foreach
        int gainCount;

        
        foreach (var item in ItemDatabase.Instance.Items.ItemList)
        {

            // ������� ������� ������ �������� ����� ��������� �� �������
            // budget (��������� �������� ������) ������������ �� ��������� ������� � �� �������� � ���� ���� �������� �������� (CoefsForItemTypes)
            // ��� ������� � ���� �������� ������ ������ ��� �� ���������� � ��� ���������� ������.
            activeTraders.Clear();
            
            Debug.Log(item.Name);
            yield return waitForEndOfFrame;
            
            foreach (var trader in traders) // �� ���� ��������� ��������� ���, ��� ������� ����� �������
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

            // ���������� �������� ����� ������������ ���� � ������ � ������ �������� ����� (���� ��������)(� �������� ��������)
            // ���������� �� �����, ��� ��� ������ ��������� ��������, ����� ����������� ������� ����� � �������
            // (���� �� ��������� � ���� ���������, ����� ���������� �����) 
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

            activeTraders.Shuffle(); // ��������� ��������� ������ ������ ���������� ��� List 
                                     // ���� ����� ������������ �������� �������� � ����� � ��������� �������
                                     // ��� ���� ��� �����? ����� ������ �� ��������� � ������ � ���� �� �������� ������ ��� 



            if (gainCount > 0) // ������� ������
            {
                if (activeTraders.Count == 0)
                {
                    // ���� �� ������� ������ ��� ���������, ������� ������� ���� 
                    // �� ������� ����������� ������ �� �������� �� ������������ �����.
                    gainCount /= 2;
                    if (gainCount > 0)
                        traders[Random.Range(0, traders.Count)].AdditiveGoods.Add(new NpcTrader.TraderGood(item.Name,
                            gainCount, gainCount, item.Price + Random.Range(1, item.Price / 10 + 2)));
                    
                }
                else
                {
                    // 5 ��� ��������� �� ���� ��������� � ������� ���� ���� �������. ����������� �����, ��� ��� ���� ������ ���
                    // ����� while(gainCount > 0), �� ����� ���������� �������� ����� gainCount > 0 � � ���� ���������
                    // CurrentCount ����� = MaxCount � ���� ���������, ����� ����� �������� �������� �� ��������� ��������������
                    // ���������� ��� �������� � ���� ��������. ��� ������� ��������� ����� ������ ������ ����� for 5
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

                    while (gainCount > 0) // ���� ����� ����� ��� ���� �������� ��������� �� ��������� �� ��� ����� Max 
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

            else // �������� ������
            {
                foreach (var trader in traders)
                    foreach (var traderGood in trader.AdditiveGoods) // ������� � ���� � �������������� ���� �����
                        if (item.Name == traderGood.Good.Name)
                        {
                            //TODO ��������� ����� �� ���� �����, ��� ������� ������������ ��������� � � mainGoods � � additive
                            activeTraders.Add(trader);
                            break;
                        }
            
                // � ��� ��� �� ���� �� ����� ��������� ����� ��������, ����� gaincount != 0 
                // � �������� � ��������� �����������, ��� ��� gainCount ������ ������� ����� � ������������ �����.
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
