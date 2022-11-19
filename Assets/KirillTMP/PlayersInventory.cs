using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInventory : MonoBehaviour
{
    private List<InventoryItem> _inventory;

    public void AddItemInInventory(InventoryItem item)
    {
        _inventory.Add(item);
    }

    public void RemoveItemInInventory(InventoryItem item)
    {
        _inventory.Remove(item);
    }

    public void CheckSpoilItems()
    {
        foreach (var item in _inventory)
        {
            if (item.ItemData.IsPerishable)
            {
                item.BoughtDaysAgo += 1f / 24f; // +1 час к испорченности
                item.RefreshSliderValue();
            }
        }
    }
}
