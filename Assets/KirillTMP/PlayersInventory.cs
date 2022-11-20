using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGrid))] //Договорились, что будем это вешать на сетку инвентаря
public class PlayersInventory : MonoBehaviour
{
    private List<InventoryItem> _inventory = new();
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
