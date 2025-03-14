using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Location : MonoBehaviour
{
    [Header("Village information")]
    [SerializeField] private Region _region;

    public Region Region => _region;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField] private string _villageName;
    public string VillageName => _villageName;
    [SerializeField][TextArea(2, 4)] private string _description;
    public string Description => _description;

    [Space(12)]
    [Header("Economy")]
    [HideInInspector] public Dictionary<string, int[]> ItemEconomyParams = new(); // ������� �� ����� �� dictionary ������ ������� Dictionary<string (ItemName), List<int (Q;A;C)>>
    [SerializeField] private int _populationOfVillage; // ����� �������������� dictionary ������, ������� ��������� Q � C 
    public int PopulationOfVillage => _populationOfVillage;
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;


    [Space(12)]
    [Header("Initialization")]

    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;
    [SerializeField] private List<Location> _relatedPlaces;
    // ����� � ������� ����� ������� �� ������� ����� (��� ����� � �����) 
    public List<Location> RelatedPlaces => _relatedPlaces;
    [HideInInspector] public List<Road> _roads = new();
    public List<NpcTraderData> NpcTraders = new();
    public List<NpcWagonUpgraderData> WagonUpgraders = new();

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
        float coef = 1.05f * _populationOfVillage / _region.AveragePopulation;
        foreach (var EconomyParam in _region.ItemEconomyParams)
        {
            ItemEconomyParams.Add(EconomyParam.Key, new[]
                    {Convert.ToInt32(Math.Round(EconomyParam.Value[0] * coef)),
                        Convert.ToInt32(Math.Round(EconomyParam.Value[1] * coef)),
                        Convert.ToInt32(Math.Round(EconomyParam.Value[2] * coef))});
        }
    }

    public void Initialize() //���������� ������ ��� ������ ����� ����, ������� �������
    {
        //���������������, ��� FillDictionary ��� ��� ������ �����   
        CountOfEachItem = new();
        foreach (var item in ItemEconomyParams)
        { // ������������� ������� ����� ���������� � ���� 
            CountOfEachItem.Add(item.Key, item.Value[0]);
        }
    }

    public void CountAllItemsOnScene()
    {
        foreach (var item in ItemEconomyParams)
            CountOfEachItem[item.Key] = 0; // ��������
        for (int i = 0; i < NpcTraders.Count; i++) // ��������� ������� ����
        {
            for (int j = 0; j < NpcTraders[i].Goods.Count; j++)
            {
                CountOfEachItem[NpcTraders[i].Goods[j].Good.Name] += NpcTraders[i].Goods[j].CurrentCount;
            }

            for (int j = 0; j < NpcTraders[i].AdditiveGoods.Count; j++)
            {
                CountOfEachItem[NpcTraders[i].AdditiveGoods[j].Good.Name] += NpcTraders[i].AdditiveGoods[j].CurrentCount;
            }
        }
    }

    public void ChangeCountOfCurrentItemOnScene(string itemName, int value)
    {
        CountOfEachItem[itemName] += value;
        _region.CountOfEachItem[itemName] += value;
    }

    public void DeleteItemsFromTraders(string itemToDeleteName)
    {
        foreach (var trader in NpcTraders)
        {
            trader.Goods.RemoveAll(item => item.Good.Name == itemToDeleteName);
            trader.AdditiveGoods.RemoveAll(item => item.Good.Name == itemToDeleteName);
        }
        CountAllItemsOnScene();
        _region.CountAllItemsInRegion();
    }

    public void MultiplyItemsInTraders(string itemToMultiplyName, float coef)
    {
        var targetGoods = NpcTraders
            .SelectMany(trader => trader.Goods.Where(item => item.Good.Name == itemToMultiplyName)).ToList();

        foreach (var traderGood in targetGoods)
        {
            traderGood.CurrentCount = (int)(traderGood.CurrentCount * coef);
        }

        CountAllItemsOnScene();
        _region.CountAllItemsInRegion();
    }

    public void OnPlaceClick()
    {
        GameObject win = Instantiate(MapManager.VillageWindow, MapManager.Canvas.transform);
        win.GetComponent<VillageWindow>().Init(this);
    }

}
