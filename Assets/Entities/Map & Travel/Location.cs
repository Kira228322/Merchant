using System;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] [TextArea(2,4)] private string _description;
    public string Description => _description;
    
    [Space(12)]
    [Header("Economy")]
    [HideInInspector] public Dictionary<string , int[]> ItemEconomyParams = new (); // основан на таком же dictionary своего региона Dictionary<string (ItemName), List<int (Q;A;C)>>
    [SerializeField] private int _populationOfVillage; // будет корректировать dictionary сверху, завава€ параметры Q и C 
    [HideInInspector] public Dictionary<string, int> CountOfEachItem;

    [Space(12)]
    [Header("Initialization")]

    [SerializeField]private string _sceneName;
    public string SceneName => _sceneName;
    [SerializeField] private List<Location> _relatedPlaces;
    // места в которые можно попасть из данного места (как ребра в графе) 
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
        
    }

    public void FillDictionary()
    {
        //«десь полагаетс€, что в регионе уже подсосан словарь из файла.
        //ѕоэтому этот метод вызываетс€ самим регионом, а не в Start
        float coef = 1.1f * _populationOfVillage / _region.AveragePopulation;
        foreach (var EconomyParam in _region.ItemEconomyParams)
        {
            ItemEconomyParams.Add(EconomyParam.Key, new[]
                    {Convert.ToInt32(Math.Round(EconomyParam.Value[0] * coef)),
                    EconomyParam.Value[1],
                    Convert.ToInt32(Math.Round(EconomyParam.Value[2] * coef))});
        }
    }

    public void Initialize() //¬ызываетс€ только при начале новой игры, создает словарь
    {
        //ѕодразумеваетс€, что FillDictionary уже был вызван ранее   
        CountOfEachItem = new();
        foreach (var item in ItemEconomyParams)
        { // инициализаци€ словар€ всеми предметами в игре 
            CountOfEachItem.Add(item.Key, item.Value[0]); // item.Value[0] равновесное число
        }

    }

    public void CountAllItemsOnScene()
    {
        // TODO сначала провести n-ое кол-во рестоков 
        
        
        NpcTrader[] traders = FindObjectsOfType<NpcTrader>(); // берем всех трейдеров на локе 
        
        foreach (var item in ItemEconomyParams)
            CountOfEachItem[item.Key] = 0; // занулили

        for (int i = 0; i < traders.Length; i++) // посчитали сколько чего
        for (int j = 0; j < traders[i].Goods.Count; j++)
        {
            CountOfEachItem[traders[i].Goods[j].Good.Name] += traders[i].Goods[j].CurrentCount;
        }
            
    }

    public void OnPlaceClick()
    {
        GameObject win = Instantiate(MapManager.VillageWindow, MapManager.Canvas.transform);
        win.GetComponent<VillageWindow>().Init(this);
    }
}
