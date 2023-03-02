using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayersInventory : MonoBehaviour
{
    [SerializeField] private ItemGrid _inventoryItemGrid;
    [SerializeField] private float _startingMaxTotalWeight;

    private List<InventoryItem> _inventory = new();

    private float _currentTotalWeight;
    private float _maxTotalWeight;

    public GameObject InventoryPanel; //� ���������� ����� ������ ������ �� ������� ������, ���������� ���� ���������

    public List<InventoryItem> ItemList => _inventory;
    public ItemGrid ItemGrid => _inventoryItemGrid;

    public bool IsOverencumbered; //������ �� ������ ������ ������� � ������ ������?

    public event UnityAction<float, float> WeightChanged;

    public float CurrentTotalWeight
    {
        get
        {
            return _currentTotalWeight;
        }
        set
        {
            if (value > _maxTotalWeight)
            {
                IsOverencumbered = true;
            }
            else IsOverencumbered = false;
            _currentTotalWeight = value;
            WeightChanged?.Invoke(CurrentTotalWeight, MaxTotalWeight);
        }
    }
    public float MaxTotalWeight
    {
        get => _maxTotalWeight;
        set
        {
            _maxTotalWeight = value;
            WeightChanged?.Invoke(CurrentTotalWeight, MaxTotalWeight);
        }
    }

    private void Awake()
    {
        ItemGrid.Init();
    }

    private void OnEnable() 
    { 
        GameTime.HourChanged += OnHourChanged;
        _inventoryItemGrid.ItemPlacedInTheGrid += AddItemInInventory;
        _inventoryItemGrid.ItemUpdated += ItemUpdated;
        _inventoryItemGrid.ItemRemovedFromTheGrid += RemoveItemInInventory;

    }
    private void OnDisable() 
    { 
        GameTime.HourChanged -= OnHourChanged;
        _inventoryItemGrid.ItemPlacedInTheGrid -= AddItemInInventory;
        _inventoryItemGrid.ItemUpdated -= ItemUpdated;
        _inventoryItemGrid.ItemRemovedFromTheGrid -= RemoveItemInInventory;
    }
    private void Start()
    {
        MaxTotalWeight = _startingMaxTotalWeight;
        CurrentTotalWeight = CalculateWeight();
    }
    public void AddItemInInventory(InventoryItem item)
    {
        ItemList.Add(item);
        CurrentTotalWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
    }
    public void RemoveItemInInventory(InventoryItem item)
    {
        ItemList.Remove(item);
        CurrentTotalWeight -= item.ItemData.Weight * item.CurrentItemsInAStack;
    }
    private void ItemUpdated(InventoryItem item, int howManyWereChanged)
    {
        CurrentTotalWeight += howManyWereChanged * item.ItemData.Weight;
    }

    private void OnHourChanged()
    {
        CheckSpoilItems();
    }
    private float CalculateWeight()
    {
        float totalWeight = 0;
        foreach (var item in ItemList)
        {
            totalWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
        }
        return totalWeight;
    }

    private void CheckSpoilItems()
    {
        foreach (var item in ItemList)
        {
            if (item.ItemData.IsPerishable)
            {
                item.BoughtDaysAgo += 1f / 24f; // +1 ��� � �������������
                item.RefreshSliderValue();
            }
        }
    }
}
