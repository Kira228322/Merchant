using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayersInventorySaveData
{
    [System.Serializable]
    public class SavedItem
    {
        public string itemName;
        public float boughtDaysAgo;
        public int currentItemsInAStack;

        public SavedItem(InventoryItem item)
        {
            itemName = item.ItemData.Name;
            boughtDaysAgo = item.BoughtDaysAgo;
            currentItemsInAStack = item.CurrentItemsInAStack;
        }
    }

    public List<SavedItem> items = new();
    public PlayersInventorySaveData(PlayersInventory inventory)
    {
        foreach (InventoryItem item in inventory.ItemList)
        {
            items.Add(new SavedItem(item));
        }
    }
}
