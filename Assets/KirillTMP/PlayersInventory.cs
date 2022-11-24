using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGrid))] //Договорились, что будем это вешать на сетку инвентаря
public class PlayersInventory : MonoBehaviour
{
    private List<InventoryItem> _inventory = new();

    private void OnEnable()
    {
        GameTime.HourChanged += OnHourChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
    }
    public void AddItemInInventory(InventoryItem item)
    {
        _inventory.Add(item);
    }
    public void RemoveItemInInventory(InventoryItem item)
    {
        _inventory.Remove(item);
    }

    private void OnHourChanged()
    {
        CheckSpoilItems();
    }

    private void CheckSpoilItems()
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
