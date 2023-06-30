using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannedItemsHandler : MonoBehaviour, ISaveable<BannedItemsSaveData>
{
    public List<Item> BannedItems { get; private set; }

    private void Awake()
    {
        BannedItems = new();
    }
    public void BanItem(Item item)
    {
        if (BannedItems.Contains(item))
        {
            Debug.LogWarning("This item is already banned");
            return;
        }
        BannedItems.Add(item);
    }


    public void UnbanItem(Item item)
    {
        if (!BannedItems.Contains(item))
        {
            Debug.LogWarning("This item wasn't banned before");
            return;
        }
        BannedItems.Remove(item);
    }
    public void LoadData(BannedItemsSaveData data)
    {
        foreach (string itemName in data.BannedItemNames)
        {
            BannedItems.Add(ItemDatabase.GetItem(itemName));
        }
    }

    public BannedItemsSaveData SaveData()
    {
        BannedItemsSaveData saveData = new(BannedItems);
        return saveData;
    }
}
