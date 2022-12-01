using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGrid))]
public class PlayersInventory : MonoBehaviour
{
    private List<InventoryItem> _inventory = new();

    public GameObject InventoryPanel; //в инспекторе нужно задать ссылку на главную панель, содержащую весь инвентарь

    private ItemGrid _inventoryItemGrid;

    private void Awake()
    {
        _inventoryItemGrid = GetComponent<ItemGrid>();
    }

    private void OnEnable() 
    { 
        GameTime.HourChanged += OnHourChanged;
        _inventoryItemGrid.ItemPlacedInTheGrid += AddItemInInventory;
        _inventoryItemGrid.ItemRemovedFromTheGrid += RemoveItemInInventory;

    }
    private void OnDisable() 
    { 
        GameTime.HourChanged -= OnHourChanged;
        _inventoryItemGrid.ItemPlacedInTheGrid -= AddItemInInventory;
        _inventoryItemGrid.ItemRemovedFromTheGrid -= RemoveItemInInventory;
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
