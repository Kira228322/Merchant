using System;
using System.Collections.Generic;

[Serializable]
public class TravelEventsSaveData
{
    public List<int> SavedIndexes;

    public TravelEventsSaveData(List<int> savedIndexes)
    {
        SavedIndexes = savedIndexes;
    }
}
