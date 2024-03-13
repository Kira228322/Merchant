using System;
using System.Collections.Generic;

[Serializable]
public class RegionSaveData
{
    [Serializable]
    public class RegionSave
    {
        public Dictionary<string, int> countOfEachItem;
        public List<Dictionary<string, int>> locationCounts;
        public RegionSave(Dictionary<string, int> countOfEachItem, List<Dictionary<string, int>> locationCounts)
        {
            this.countOfEachItem = new(countOfEachItem);
            this.locationCounts = new(locationCounts);
        }
    }

    public List<RegionSave> savedRegions = new();
}
