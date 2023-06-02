using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Location : MonoBehaviour
{
    [Header("Village information")]
    [SerializeField] private Region _region;
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    [SerializeField] private string _villageName;
    public string VillageName => _villageName;
    [SerializeField] [TextArea(2,4)] private string _description;
    public string Description => _description;
    
    [Space(12)]
    [Header("Economy")]
    [HideInInspector] public Dictionary<string , int[]> ItemEconomyParams = new (); // TODO ������� �� ����� �� dictionary ������ ������� Dictionary<string (ItemName), List<int (Q;A;C)>>
    [SerializeField] private int _populationOfVillage; // TODO ����� �������������� dictionary ������, ������� ��������� Q � C 
    
    
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
            if (road.Points[0] = this)
            {
                _roads.Add(road);
                continue;
            }
            if (road.Points[1] = this)
                _roads.Add(road);
        }
        
        // TODO ����� ��� ����� ������ - ������ ��������
        if (_region == null)
            return;
        
        float coef = 1.1f * _populationOfVillage / _region.AveragePopulation;
        
        foreach (var EconomyParam in _region.ItemEconomyParams)
        {
            ItemEconomyParams.Add(EconomyParam.Key, new []
                    {Convert.ToInt32(Math.Round(EconomyParam.Value[0] * coef)), 
                    EconomyParam.Value[1], 
                    Convert.ToInt32(Math.Round(EconomyParam.Value[2] * coef))});
        }
    }

    public void OnPlaceClick()
    {
        GameObject win = Instantiate(MapManager.VillageWindow, MapManager.Canvas.transform);
        win.GetComponent<VillageWindow>().Init(this);
    }
}
