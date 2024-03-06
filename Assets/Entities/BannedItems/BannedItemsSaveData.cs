using System;
using System.Collections.Generic;

[Serializable]
public class BannedItemsSaveData
{
    public List<string> BannedItemNames;

    public BannedItemsSaveData(List<Item> bannedItems)
    {
        BannedItemNames = new();
        foreach (Item item in bannedItems)
        {
            BannedItemNames.Add(item.Name);
        }
    }

}
