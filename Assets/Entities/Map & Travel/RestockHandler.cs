using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RestockHandler : MonoBehaviour, ISaveable<RestockSaveData>
{
    private int _lastRestockDay;
    private Location[] _locations;
    [SerializeField] private RegionHandler _regionHandler;

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
        foreach (var region in _regionHandler.Regions)
            region.CountAllItemsInRegion();
        
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
        _lastRestockDay = GameTime.CurrentDay;
        foreach (var location in _locations)
        {
            RestockTraders(location);
            RestockWagonUpgraders(location);
        }
        foreach (var region in _regionHandler.Regions)
            region.CountAllItemsInRegion();
    }
    private void RestockTraders(Location location)
    {
        //TODO: ������ �� ���� ������� �������� ����� foreach ������ ���� ���������.
        //������� ������ ��� ������ �������� � ��������� ��� ����� foreach (var trader in location)?
        
        if (location.NpcTraders.Count <= 0)
            return;
        
        
        RestockMainGoods(location.NpcTraders, location);
        AddAdditiveGoods(location.NpcTraders);
        RestockBuyCoefficients(location.NpcTraders);
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
            if (trader.AdditiveGoods.Count != 0)
            {
                if (trader.AdditiveGoods.Count > 6)
                    trader.AdditiveGoods.RemoveAt(Random.Range(5, trader.AdditiveGoods.Count));
                if (trader.AdditiveGoods.Count > 3)
                    trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
                else
                {
                    if (Random.Range(0, 2) == 0)
                        trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
                }
            }
            
            if (!trader.HaveAdditiveGoods)
                continue;

            if (Random.Range(0, 4) == 0)
                continue;

            int count = Random.Range(1, 3);
            for (int i = 0; i < count; i++)
            {
                Item newItem;
                bool reallyNew;
    
                while (true)
                {
                    reallyNew = true;
                    
                    newItem = ItemDatabase.GetRandomItem();

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
                    
                    foreach (var goods in trader.AdditiveGoods)
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
                    if (newItem.Price < 25)
                        randomCount++;

                    NpcTrader.TraderGood newGood = new NpcTrader.TraderGood(newItem.Name, randomCount,
                        randomCount, newItem.Price + Random.Range(1, newItem.Price / 20 + 2));

                    trader.AdditiveGoods.Add(newGood);
                    break;
                }
            }
        }
    }

    private void RestockMainGoods(List<NpcTraderData> traders, Location location)
    {
        List<NpcTraderData> activeTraders = new List<NpcTraderData>(); // �������� ������� ����� ������������ � ��������� �������� foreach
        int gainCount;
        
        foreach (var item in ItemDatabase.Instance.Items.ItemList)
        {
            // ������� ������� ������ �������� ����� ��������� �� �������
            // budget (��������� �������� ������) ������������ �� ��������� ������� � �� �������� � ���� ���� �������� �������� (CoefsForItemTypes)
            // ��� ������� � ���� �������� ������ ������ ��� �� ���������� � ��� ���������� ������.
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

            activeTraders.Clear();
            
            foreach (var trader in traders) // �� ���� ��������� ��������� ���, ��� ������� ����� �������
            foreach (var traderGood in trader.Goods)
                if (item.Name == traderGood.Good.Name)
                {
                    activeTraders.Add(trader);
                    break;
                }
            
            activeTraders.Shuffle(); // ��������� ��������� ������ ������ ���������� ��� List 
                                     // ���� ����� ������������ �������� �������� � ����� � ��������� �������
                                     // ��� ���� ��� �����? ����� ������ �� ��������� � ������ � ���� �� �������� ������ ��� 



            if (gainCount > 0) // ������� ������
            {
                if (activeTraders.Count != 0)
                {
                    // 8 ��� ��������� �� ���� ��������� � ������� ���� ���� �������. ����������� �����, ��� ��� ���� ������ ���
                    // ����� while(gainCount > 0), �� ����� ���������� �������� ����� gainCount > 0 � � ���� ���������
                    // CurrentCount ����� = MaxCount � ���� ���������, ����� ����� �������� �������� �� ��������� ��������������
                    // ���������� ��� �������� � ���� ��������. ��� ������� ��������� ����� ������ ������ ����� for 8
                    for (int i = 0; i < 8; i++)
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
                            activeTraders.Add(trader);
                            break;
                        }
                if (activeTraders.Count == 0)
                { 
                    continue;
                }
                // � ��� ��� �� ���� �� ����� ��������� ����� ��������, ����� gaincount != 0 
                // � �������� � ��������� �����������, ��� ��� gainCount ������ ������� ����� � ������������ �����.
                for (int i = 0; i < 12; i++)
                {
                    foreach (var trader in activeTraders)
                    {
                        NpcTrader.TraderGood traderGood =
                            trader.AdditiveGoods.FirstOrDefault(good => good.Good.Name == item.Name);
                        
                        
                        if (traderGood != null)
                        {
                            traderGood.CurrentCount--;
                            if (traderGood.CurrentCount <= 0)
                                trader.AdditiveGoods.Remove(traderGood);
                            gainCount++;
                            trader.CurrentMoney += traderGood.CurrentPrice;
                        }
                        else
                        {
                            traderGood = trader.Goods.FirstOrDefault(good => good.Good.Name == item.Name);
                            if (traderGood != null)
                            {
                                if (traderGood.CurrentCount > 0)
                                {
                                    traderGood.CurrentCount--;
                                    gainCount++;
                                    trader.CurrentMoney += traderGood.CurrentPrice;
                                }
                            }
                        }
                        if (gainCount == 0)
                            break;
                    }
                    if (gainCount == 0)
                        break;
                }
            }
        }
    }

    public RestockSaveData SaveData()
    {
        RestockSaveData saveData = new(_lastRestockDay);
        return saveData;   
    }



    public void LoadData(RestockSaveData data)
    {
        _lastRestockDay = data.LastRestockDay;
    }
}
