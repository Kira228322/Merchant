using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayersInventory : MonoBehaviour, ISaveable<PlayersInventorySaveData>
{
    [SerializeField] private ItemGrid _inventoryItemGrid;
    [SerializeField] private QuestItemHolder _questItemHolder;
    [SerializeField] private float _startingMaxTotalWeight;

    private List<InventoryItem> _inventory = new();
    private List<InventoryItem> _questInventory = new();

    private float _currentTotalWeight;
    private float _maxTotalWeight;

    public GameObject InventoryPanel; //в инспекторе нужно задать ссылку на главную панель, содержащую весь инвентарь

    public List<InventoryItem> ItemList => _questInventory.Concat(_inventory).ToList();
    public List<InventoryItem> BaseItemList => _inventory;
    public List<InventoryItem> QuestItemList => _questInventory;

    public ItemGrid BaseItemGrid => _inventoryItemGrid;
    public ItemGrid QuestItemGrid => _questItemHolder.ItemGrid;

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
        BaseItemGrid.Init();
    }

    private void OnEnable() 
    { 
        GameTime.HourChanged += OnHourChanged;
        GameTime.TimeSkipped += OnTimeSkipped;
        _inventoryItemGrid.ItemPlacedInTheGrid += AddItemInInventory;
        _inventoryItemGrid.ItemUpdated += ItemUpdated;
        _inventoryItemGrid.ItemRemovedFromTheGrid += RemoveItemInInventory;
        QuestItemGrid.ItemPlacedInTheGrid += AddItemInQuestItems;
        QuestItemGrid.ItemUpdated += ItemUpdated;
        QuestItemGrid.ItemRemovedFromTheGrid += RemoveItemInQuestItems;

    }
    private void OnDisable() 
    { 
        GameTime.HourChanged -= OnHourChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
        _inventoryItemGrid.ItemPlacedInTheGrid -= AddItemInInventory;
        _inventoryItemGrid.ItemUpdated -= ItemUpdated;
        _inventoryItemGrid.ItemRemovedFromTheGrid -= RemoveItemInInventory;
        QuestItemGrid.ItemPlacedInTheGrid -= AddItemInQuestItems;
        QuestItemGrid.ItemUpdated -= ItemUpdated;
        QuestItemGrid.ItemRemovedFromTheGrid -= RemoveItemInQuestItems;
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
        int result = ItemList
            .Where(inventoryItem => inventoryItem.ItemData.Name == item.Name)
            .Sum(inventoryItem => inventoryItem.CurrentItemsInAStack);

        return result;
    }
    public List<InventoryItem> GetInventoryItemsOfThisData(Item itemData)
    {
        return ItemList.Where(item => item.ItemData == itemData).ToList();
    }
    public bool HasEnoughItemsOfThisItemData(Item itemData, int amount)
    {
        //TODO Протестировать оба инвентаря, чтобы не было сомнений что здесь не возникнет бага никогда
        //Debug.Log($"GetCount {itemData.Name}: base {GetCount(itemData)}, additional {_questItemHolder.GetCount(itemData)}");
        if (GetCount(itemData) >= amount)
            return true;
        return false;
    }
    public void RemoveItemsByPrice(int price)
    {
        //Удалить предметов на такую стоимость
        //Квестовые предметы игнорируются
        /*UPD 18.10.23 (делаем доп.инвентарь): Решил, что из доп.инвентаря
        не будут убираться предметы в этом методе. На данный момент мы решили,
        что предметы в тот инвентарь будут попадать только из-за диалога,
        пусть даже не все из них квестовые.
        То есть если нпс дал тебе предмет, то он для чего-то всё таки нужен. Бандиты его не спиздят
        Зачем так? Потому что посчитал, что тогда метод сильно усложнится - нужно будет
        отсортировать по цене в двух инвентарях совместно, а потом запомнить какому инвентарю какой предмет принадлежит
        (ну потому что не получится убрать сначала все из основного,
        потом все из дополнительного, надо как бы комбинировать. 
        Надеюсь хоть что-то из вышеописанного имеет смысл)
        */
        int priceLeftToRemove = price;
        List<InventoryItem> sortedItems = BaseItemList.Where(item => !item.ItemData.IsQuestItem).OrderBy(item => item.TotalPrice).ToList();
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
            BaseItemGrid.RemoveItemsFromAStack(itemsToRemove[i], itemsToRemove[i].CurrentItemsInAStack);
        }
        if (partiallyRemovedItem != null)
        {
            int items = (int)Mathf.Ceil((float)priceLeftToRemove / partiallyRemovedItem.ItemData.Price);
            BaseItemGrid.RemoveItemsFromAStack(partiallyRemovedItem, items);
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

        for (int i = QuestItemList.Count - 1; i >= 0; i--)
        {
            if (QuestItemList[i].ItemData.Name == itemType.Name)
            {
                if (QuestItemList[i].CurrentItemsInAStack <= leftToRemove)
                {
                    leftToRemove -= QuestItemList[i].CurrentItemsInAStack;
                    QuestItemGrid.RemoveItemsFromAStack(QuestItemList[i], QuestItemList[i].CurrentItemsInAStack);
                }
                else
                {
                    QuestItemGrid.RemoveItemsFromAStack(QuestItemList[i], leftToRemove);
                    return;
                }
            }
        }
        for (int i = BaseItemList.Count - 1; i >= 0; i--)
        {
            if (BaseItemList[i].ItemData.Name == itemType.Name)
            {
                if (BaseItemList[i].CurrentItemsInAStack <= leftToRemove)
                {
                    leftToRemove -= BaseItemList[i].CurrentItemsInAStack;
                    BaseItemGrid.RemoveItemsFromAStack(BaseItemList[i], BaseItemList[i].CurrentItemsInAStack);
                }
                else
                {
                    BaseItemGrid.RemoveItemsFromAStack(BaseItemList[i], leftToRemove);
                    return;
                }
            }
        }
    }
    public void RemoveAllItemsOfThisItemData(Item itemType)
    {
        for (int i = QuestItemList.Count - 1; i >= 0; i--)
        {
            if (QuestItemList[i].ItemData.Name == itemType.Name)
            {
                QuestItemGrid.RemoveItemsFromAStack(QuestItemList[i], QuestItemList[i].CurrentItemsInAStack);
            }
        }
        for (int i = BaseItemList.Count - 1; i >= 0; i--)
        {
            if (BaseItemList[i].ItemData.Name == itemType.Name)
            {
                BaseItemGrid.RemoveItemsFromAStack(BaseItemList[i], BaseItemList[i].CurrentItemsInAStack);
            }
        }
    }
    private void AddItemInInventory(InventoryItem item)
    {
        BaseItemList.Add(item);
        CurrentTotalWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
    }
    private void RemoveItemInInventory(InventoryItem item)
    {
        BaseItemList.Remove(item);
        CurrentTotalWeight -= item.ItemData.Weight * item.CurrentItemsInAStack;
    }
    private void AddItemInQuestItems(InventoryItem item)
    {
        QuestItemList.Add(item);
        CurrentTotalWeight += item.ItemData.Weight * item.CurrentItemsInAStack;
    }
    private void RemoveItemInQuestItems(InventoryItem item)
    {
        QuestItemList.Remove(item);
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

                if (item.ItemData.IsQuestItem && item.BoughtDaysAgo > item.ItemData.DaysToSpoil)
                    InventoryController.Instance.DestroyItem(QuestItemGrid, item);
                //Чтобы полностью прогнившие квестовые предметы, которые игрок не может удалить, исчезали сами.
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

                if (item.ItemData.IsQuestItem && item.BoughtDaysAgo > item.ItemData.DaysToSpoil)
                    InventoryController.Instance.DestroyItem(QuestItemGrid, item);
                //Чтобы полностью прогнившие квестовые предметы, которые игрок не может удалить, исчезали сами.
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
            InventoryItem inventoryItem = InventoryController.Instance.TryCreateAndInsertItem(
                ItemDatabase.GetItem(item.itemName), item.currentItemsInAStack, item.boughtDaysAgo);
            inventoryItem.RefreshSliderValue();
        }
    }
}
