using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegionSaveData
{
    [Serializable]
    public class RegionSave
    {
        public List<string> countOfEachItemKey = new List<string>();
        public List<int> countOfEachItemValue = new List<int>();
        public List<LocationData> locationCounts = new List<LocationData>();

        [Serializable]
        public class LocationData
        {
            public List<string> keys = new List<string>();
            public List<int> values = new List<int>();
        }

        // Конструктор по умолчанию для сериализации
        public RegionSave() { }

        public RegionSave(Dictionary<string, int> countOfEachItem, List<Dictionary<string, int>> locationCounts)
        {
            // Инициализация списков
            countOfEachItemKey = new List<string>();
            countOfEachItemValue = new List<int>();
            this.locationCounts = new List<LocationData>();

            // Заполнение countOfEachItem
            if (countOfEachItem != null)
            {
                foreach (var pair in countOfEachItem)
                {
                    countOfEachItemKey.Add(pair.Key);
                    countOfEachItemValue.Add(pair.Value);
                }
            }

            // Заполнение locationCounts
            if (locationCounts != null)
            {
                foreach (var locationDict in locationCounts)
                {
                    var locationData = new LocationData();
                    
                    if (locationDict != null)
                    {
                        foreach (var pair in locationDict)
                        {
                            locationData.keys.Add(pair.Key);
                            locationData.values.Add(pair.Value);
                        }
                    }
                    
                    this.locationCounts.Add(locationData);
                }
            }
        }

        public Dictionary<string, int> GetCountOfEachItemDict()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            
            // Проверка на null и соответствие размеров
            if (countOfEachItemKey != null && countOfEachItemValue != null && 
                countOfEachItemKey.Count == countOfEachItemValue.Count)
            {
                for (int i = 0; i < countOfEachItemKey.Count; i++)
                {
                    if (countOfEachItemKey[i] != null)
                    {
                        dictionary[countOfEachItemKey[i]] = countOfEachItemValue[i];
                    }
                }
            }

            return dictionary;
        }

        public List<Dictionary<string, int>> GetLocationsCountList()
        {
            List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
            
            if (locationCounts != null)
            {
                for (int i = 0; i < locationCounts.Count; i++)
                {
                    var locationData = locationCounts[i];
                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                    
                    if (locationData != null && locationData.keys != null && locationData.values != null &&
                        locationData.keys.Count == locationData.values.Count)
                    {
                        for (int j = 0; j < locationData.keys.Count; j++)
                        {
                            if (locationData.keys[j] != null)
                            {
                                dictionary[locationData.keys[j]] = locationData.values[j];
                            }
                        }
                    }
                    
                    list.Add(dictionary);
                }
            }

            return list;
        }
    }

    public List<RegionSave> savedRegions = new List<RegionSave>();

    // Вспомогательные методы для удобства
    public void AddRegionSave(Dictionary<string, int> countOfEachItem, List<Dictionary<string, int>> locationCounts)
    {
        if (savedRegions == null)
            savedRegions = new List<RegionSave>();
            
        savedRegions.Add(new RegionSave(countOfEachItem, locationCounts));
    }

    public RegionSave GetRegionSave(int index)
    {
        if (savedRegions != null && index >= 0 && index < savedRegions.Count)
            return savedRegions[index];
        return null;
    }
}