using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegionHandler : MonoBehaviour, ISaveable<RegionSaveData>
{
    //Будет иметь лист всех регионов и триггерить в них инициализацию, сохранение и загрузку когда надо

    public List<Region> Regions = new();

    public void InitializeAll()
    {
        foreach (Region region in Regions)
        {
            region.FillCoefsForItemTypesDictionary();
            region.FillEconomyParamsDictionary();
            region.Initialize();
        }
    }

    public RegionSaveData SaveData()
    {
        RegionSaveData saveData = new();
        for (int i = 0; i < Regions.Count; i++)
        {
            List<Dictionary<string, int>> locationCount = new();
            for (int j = 0; j < Regions[i].Locations.Count; j++)
            {
                locationCount.Add(new(Regions[i].Locations[j].CountOfEachItem));
            }
            saveData.savedRegions.Add(new(Regions[i].CountOfEachItem, locationCount));
        }
        return saveData;
    }
    public void LoadData(RegionSaveData data)
    {
        for (int i = 0; i < Regions.Count; i++)
        {
            Regions[i].CountOfEachItem = new(data.savedRegions[i].countOfEachItem);
            for (int j = 0; j < Regions[i].Locations.Count; j++)
            {
                Regions[i].Locations[j].CountOfEachItem = new(data.savedRegions[i].locationCounts[j]);
            }
        }
    }

}
