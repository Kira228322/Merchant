using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CollectItemsGoal : Goal 
{
    public Item _requiredItemType;

    public CollectItemsGoal(Quest quest, Item requiredItemType, string description, bool isCompleted, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        _requiredItemType = requiredItemType;
        Description = description;
        IsCompleted = isCompleted;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;   
    }

    public override void Initialize()
    {
        base.Initialize();

        //Надо найти, сколько таких вещей уже есть у игрока в инвентаре
        foreach (InventoryItem item in Player.Singleton.Inventory.ItemList)
        {
            if (item.ItemData == _requiredItemType)
            {
                CurrentAmount += item.CurrentItemsInAStack;
            }
        }

        ItemGrid playerInventoryItemGrid = Player.Singleton.Inventory.GetComponent<ItemGrid>();
        playerInventoryItemGrid.ItemPlacedInTheGrid += ItemCollected;
        playerInventoryItemGrid.ItemPlacedInTheStack += ItemCollected;
        playerInventoryItemGrid.ItemRemovedFromTheGrid += ItemRemoved;
        playerInventoryItemGrid.ItemsRemovedFromTheStack += ItemRemoved;
    }

    private void ItemCollected(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount += item.CurrentItemsInAStack;
            Debug.Log($"Шото подобрал, щас у меня {CurrentAmount}");
            Evaluate();
        }
    }
    private void ItemRemoved(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount -= item.CurrentItemsInAStack;
            Debug.Log($"Шото выкинул, щас у меня {CurrentAmount}");
            Evaluate();
        }
    }
    private void ItemRemoved(InventoryItem item, int amount)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount -= amount;
            Debug.Log($"Шото выкинул, щас у меня {CurrentAmount}");
            Evaluate();
        }
    }

}
