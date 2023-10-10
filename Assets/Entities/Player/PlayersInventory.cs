using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayersInventory : MonoBehaviour, ISaveable<PlayersInventorySaveData>
{
    [SerializeField] private ItemGrid _inventoryItemGrid;
    [SerializeField] private float _startingMaxTotalWeight;

    private List<InventoryItem> _inventory = new();

    private float _currentTotalWeight;
    private float _maxTotalWeight;

    public GameObject InventoryPanel; //в инспекторе нужно задать ссылку на главную панель, содержащую весь инвентарь

    public List<InventoryItem> ItemList => _inventory;
    public ItemGrid ItemGrid => _inventoryItemGrid;

    public bool IsOverencumbered; //тяжело ли ослику тащить повозку в данный момент? //upd. Какая прелесть

    public event UnityAction<float, float> WeightChanged;

    public event UnityAction<UsableItem> ItemUsed;

    public float CurrentTotalWeight
    {
        get
        {
            return _currentTotalWeight;
        }
        set
        {
            if (value > _maxTotalWeight)
                IsOverencumbered = true;
            else 
                IsOverencumbered = false;
            
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
        GameTime.TimeSkipped += OnTimeSkipped;
        _inventoryItemGrid.ItemPlacedInTheGrid += AddItemInInventory;
        _inventoryItemGrid.ItemUpdated += ItemUpdated;
        _inventoryItemGrid.ItemRemovedFromTheGrid += RemoveItemInInventory;

    }
    private void OnDisable() 
    { 
        GameTime.HourChanged -= OnHourChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
        _inventoryItemGrid.ItemPlacedInTheGrid -= AddItemInInventory;
        _inventoryItemGrid.ItemUpdated -= ItemUpdated;
        _inventoryItemGrid.ItemRemovedFromTheGrid -= RemoveItemInInventory;
    }
    private void Start()
    {
        MaxTotalWeight = _startingMaxTotalWeight;
        CurrentTotalWeight = CalculateWeight();
    }
    public void OnItemUsed(UsableItem usableItem)
    {
        ItemUsed?.Invoke(usableItem);
    }
    public int GetCount(Item item)
    {
        int result = 0;
        foreach (InventoryItem inventoryItem in ItemList)
        {
            if (inventoryItem.ItemData.Name == item.Name)
            {
                result += inventoryItem.CurrentItemsInAStack;
            }
        }
        return result;
    }
    public List<InventoryItem> GetInventoryItemsOfThisData(Item itemData)
    {
        return ItemList.Where(item => item.ItemData == itemData).ToList();
    }
    public bool HasEnoughItemsOfThisItemData(Item itemData, int amount)
    {
        if (GetCount(itemData) >= amount)
            return true;
        return false;
    }
    public void RemoveItemsByPrice(int price)
    {
        //Удалить предметов на такую стоимость
        //Квестовые предметы игнорируются
        int priceLeftToRemove = price;
        List<InventoryItem> sortedItems = ItemList.Where(item => !item.ItemData.IsQuestItem).OrderBy(item => item.TotalPrice).ToList();
        List<InventoryItem> itemsToRemove = new();
        InventoryItem partiallyRemovedItem = null;
        foreach (InventoryItem item in sortedItems)
        {
            if (priceLeftToRemove - item.TotalPrice <= 0)
            {
                partiallyRemovedItem = item;
                break;
            }
            priceLeftToRemove -= item.TotalPrice;
            itemsToRemove.Add(item);        
        }
        for (int i = itemsToRemove.Count - 1; i >= 0; i--)
        {
            ItemGrid.RemoveItemsFromAStack(itemsToRemove[i], itemsToRemove[i].CurrentItemsInAStack);
        }
        if (partiallyRemovedItem != null)
        {
            int items = (int)Mathf.Ceil((float)priceLeftToRemove / partiallyRemovedItem.ItemData.Price);
            ItemGrid.RemoveItemsFromAStack(partiallyRemovedItem, items);
        }
    }
    public void RemoveItemsOfThisItemData(Item itemType, int amount)
    {
        if (GetCount(itemType) < amount)
        {
            Debug.LogError("Попытался убрать из инвентаря больше чем есть");
            return;
        }
        int leftToRemove = amount;

        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            if (ItemList[i].ItemData.Name == itemType.Name)
            {
                if (ItemList[i].CurrentItemsInAStack <= leftToRemove)
                {
                    leftToRemove -= ItemList[i].CurrentItemsInAStack;
                    ItemGrid.RemoveItemsFromAStack(ItemList[i], ItemList[i].CurrentItemsInAStack);
                }
                else
                {
                    ItemGrid.RemoveItemsFromAStack(ItemList[i], leftToRemove); 
                }
            }
        }
    }
    public void RemoveAllItemsOfThisItemData(Item itemType)
    {
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            if (ItemList[i].ItemData.Name == itemType.Name)
            {
                ItemGrid.RemoveItemsFromAStack(ItemList[i], ItemList[i].CurrentItemsInAStack);
            }
        }
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
    private void OnTimeSkipped(int daysSkipped, int hoursSkipped, int minutesSkipped)
    {
        float totalHoursSkipped = daysSkipped * 24 + hoursSkipped + (float)minutesSkipped / 60;
        CheckSpoilItems(totalHoursSkipped);
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
                item.BoughtDaysAgo += 1f / 24f; // +1 час к испорченности
                item.RefreshSliderValue();
            }
        }
    }
    private void CheckSpoilItems(float hoursSkipped)
    {
        foreach (var item in ItemList)
        {
            if (item.ItemData.IsPerishable)
            {
                item.BoughtDaysAgo += hoursSkipped / 24f; // +hoursSkipped часов к испорченности
                item.RefreshSliderValue();
            }
        }
    }


    public PlayersInventorySaveData SaveData()
    {
        PlayersInventorySaveData saveData = new(this);
        return saveData;
    }

    public void LoadData(PlayersInventorySaveData saveData)
    {
        foreach(var item in saveData.items)
        {
            InventoryItem inventoryItem = InventoryController.Instance.TryCreateAndInsertItem(ItemGrid,
                ItemDatabase.GetItem(item.itemName), item.currentItemsInAStack, item.boughtDaysAgo, true);
            inventoryItem.RefreshSliderValue();
        }
    }
}
