using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Location : MonoBehaviour
{
    [Header("Village information")]
    [SerializeField] private Region _region;

    public Region Region => _region;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField] private string _villageName;
    public string VillageName => _villageName;
    [SerializeField] [TextArea(2,4)] private string _description;
    public string Description => _description;
    
    [Space(12)]
    [Header("Economy")]
    [HideInInspector] public Dictionary<string , int[]> ItemEconomyParams = new (); // ������� �� ����� �� dictionary ������ ������� Dictionary<string (ItemName), List<int (Q;A;C)>>
    [SerializeField] private int _populationOfVillage; // ����� �������������� dictionary ������, ������� ��������� Q � C 
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;
    private int _lastRestockDay;
    
    [Space(12)]
    [Header("Initialization")]

    [SerializeField]private string _sceneName;
    public string SceneName => _sceneName;
    [SerializeField] private List<Location> _relatedPlaces;
    // ����� � ������� ����� ������� �� ������� ����� (��� ����� � �����) 
    public List<Location> RelatedPlaces => _relatedPlaces;
    [HideInInspector] public List<Road> _roads = new();
    
    
    private void Start()
    {
        Road[] roads = FindObjectsOfType<Road>();
        foreach (var road in roads)
        {
            if (road.Points[0] == this)
            {
                _roads.Add(road);
                continue;
            }
            if (road.Points[1] == this)
                _roads.Add(road);
        }
        
    }

    public void FillDictionary()
    {
        //����� ����������, ��� � ������� ��� �������� ������� �� �����.
        //������� ���� ����� ���������� ����� ��������, � �� � Start
        float coef = 1.1f * _populationOfVillage / _region.AveragePopulation;
        foreach (var EconomyParam in _region.ItemEconomyParams)
        {
            ItemEconomyParams.Add(EconomyParam.Key, new[]
                    {Convert.ToInt32(Math.Round(EconomyParam.Value[0] * coef)),
                    EconomyParam.Value[1],
                    Convert.ToInt32(Math.Round(EconomyParam.Value[2] * coef))});
        }
    }

    public void Initialize() //���������� ������ ��� ������ ����� ����, ������� �������
    {
        //���������������, ��� FillDictionary ��� ��� ������ �����   
        CountOfEachItem = new();
        foreach (var item in ItemEconomyParams)
        { // ������������� ������� ����� ���������� � ���� 
            CountOfEachItem.Add(item.Key, item.Value[0]); // item.Value[0] ����������� �����
        }

    }

    public void CountAllItemsOnScene()
    {
        NpcTrader[] traders = FindObjectsOfType<NpcTrader>(); // ����� ���� ��������� �� ���� 
        
        foreach (var item in ItemEconomyParams)
            CountOfEachItem[item.Key] = 0; // ��������

        for (int i = 0; i < traders.Length; i++) // ��������� ������� ����
        {
            for (int j = 0; j < traders[i].Goods.Count; j++)
            {
                CountOfEachItem[traders[i].Goods[j].Good.Name] += traders[i].Goods[j].CurrentCount;
            }

            for (int j = 0; j < traders[i].AdditiveGoods.Count; j++)
            {
                CountOfEachItem[traders[i].AdditiveGoods[j].Good.Name] += traders[i].AdditiveGoods[j].CurrentCount;
            }
            
        }
            
    }

    public void OnPlaceClick()
    {
        GameObject win = Instantiate(MapManager.VillageWindow, MapManager.Canvas.transform);
        win.GetComponent<VillageWindow>().Init(this);
    }

    public void Restock()
    {
        if (GameTime.CurrentDay < _lastRestockDay + 2)
            return;
        
        NpcTrader[] traders = FindObjectsOfType<NpcTrader>();
        RestockBuyCoefficients(traders);
        RestockMainGoods(traders);
        AddAdditiveGoods(traders);
        CountAllItemsOnScene();
        _lastRestockDay = GameTime.CurrentDay;
    }

    public void OnEnterOnLocation()
    {
        GameTime.HourChanged += CheckRestock;
    }

    public void OnLeaveLocation()
    {
        GameTime.HourChanged -= CheckRestock;
    }

    private void CheckRestock()
    {
        if (GameTime.Hours == 1)
            if (_lastRestockDay + 2 < GameTime.CurrentDay)
            {
                Restock();
            }
    }
    private void RestockBuyCoefficients(NpcTrader[] traders)
    {
        foreach (var trader in traders)
            trader.RestockCoefficients();
    }

    private void AddAdditiveGoods(NpcTrader[] traders)
    {
        foreach (var trader in traders)
        {
            if (trader.AdditiveGoods.Count > 9)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 7)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));
            if (trader.AdditiveGoods.Count > 4)
                trader.AdditiveGoods.RemoveAt(Random.Range(0, trader.AdditiveGoods.Count));

            if (Random.Range(0,3) == 0)
                return;
            
            int count = Random.Range(1, Player.Instance.Statistics.TotalDiplomacy / 3 + 2); // �� ������ 3 ���������� ���� �� +1
            // �������������� ������ � ��������
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

    private void RestockMainGoods(NpcTrader[] traders)
    {
        List<NpcTrader> activeTraders = new List<NpcTrader>(); // �������� ������� ����� ������������ � ��������� �������� foreach
        int gainCount; 
        
        foreach (var item in ItemDatabase.Instance.Items.ItemList)
        {
            // ������� ������� ������ �������� ����� ��������� �� �������
            // budget (��������� �������� ������) ������������ �� ��������� ������� � �� �������� � ���� ���� �������� �������� (CoefsForItemTypes)
            // ��� ������� � ���� �������� ������ ������ ��� �� ���������� � ��� ���������� ������.
            gainCount = _region.CalculateGainOnMarket(CountOfEachItem[item.Name], item.Price,
                ItemEconomyParams[item.Name][0], ItemEconomyParams[item.Name][1],
                ItemEconomyParams[item.Name][2], (int)(_populationOfVillage * (2 - _region.CoefsForItemTypes[item.TypeOfItem])));
            
            if (gainCount == 0)
                continue;

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
                if (activeTraders.Count == 0)
                {
                    // ���� �� ������� ������ ��� ���������, ������� ������� ���� 
                    // �� ������� ����������� � 25% ������ � ������ �� �������� �� ������������ �����.
                    if (Random.Range(0, 4) == 0)
                    {
                        gainCount /= 2;
                        if (gainCount > 0)
                            traders[Random.Range(0,traders.Length)].AdditiveGoods.Add(new NpcTrader.TraderGood(item.Name, 
                                gainCount, gainCount, item.Price + Random.Range(1, item.Price/10 + 2)));
                    }
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
                        if (traderGood.CurrentCount > 0)
                        {
                            traderGood.CurrentCount--;
                            gainCount++;
                            trader.NpcData.CurrentMoney += traderGood.CurrentPrice;
                        }
                        if (gainCount == 0)
                            break;
                    }
                }
                
            }
        }
    }
    
    
}
