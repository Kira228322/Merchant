using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CollectItemsGoal : Goal 
{
    public Item _requiredItemType;

    public CollectItemsGoal(Quest quest, string requiredItemTypeName, string description, bool isCompleted, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        _requiredItemType = ItemDatabase.GetItem(requiredItemTypeName);
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
        Evaluate();

        ItemGrid playerInventoryItemGrid = Player.Singleton.Inventory.ItemGrid;
        playerInventoryItemGrid.ItemPlacedInTheGrid += ItemCollected;
        playerInventoryItemGrid.ItemUpdated += ItemUpdated;
        playerInventoryItemGrid.ItemRemovedFromTheGrid += ItemRemoved;
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        ItemGrid playerInventoryItemGrid = Player.Singleton.Inventory.ItemGrid;
        playerInventoryItemGrid.ItemPlacedInTheGrid -= ItemCollected;
        playerInventoryItemGrid.ItemUpdated -= ItemUpdated;
        playerInventoryItemGrid.ItemRemovedFromTheGrid -= ItemRemoved;
    }

    private void ItemCollected(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount += item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemRemoved(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount -= item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemUpdated(InventoryItem item, int updatedAmount)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount = updatedAmount;
            Evaluate();
        }
    }

}
