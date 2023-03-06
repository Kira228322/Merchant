using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CollectItemsGoal : Goal 
{
    public Item RequiredItemType;

    public CollectItemsGoal(Quest quest, string requiredItemTypeName, string description, bool isCompleted, int currentAmount, int requiredAmount)
    {
        Quest = quest;
        RequiredItemType = ItemDatabase.GetItem(requiredItemTypeName);
        Description = description;
        IsCompleted = isCompleted;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;   
    }

    public override void Initialize()
    {
        base.Initialize();
        //Надо найти, сколько таких вещей уже есть у игрока в инвентаре
        foreach (InventoryItem item in Player.Instance.Inventory.ItemList)
        {
            if (item.ItemData == RequiredItemType)
            {
                CurrentAmount += item.CurrentItemsInAStack;
            }
        }
        Evaluate();

        ItemGrid playerInventoryItemGrid = Player.Instance.Inventory.ItemGrid;
        playerInventoryItemGrid.ItemPlacedInTheGrid += ItemCollected;
        playerInventoryItemGrid.ItemUpdated += ItemUpdated;
        playerInventoryItemGrid.ItemRemovedFromTheGrid += ItemRemoved;
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        ItemGrid playerInventoryItemGrid = Player.Instance.Inventory.ItemGrid;
        playerInventoryItemGrid.ItemPlacedInTheGrid -= ItemCollected;
        playerInventoryItemGrid.ItemUpdated -= ItemUpdated;
        playerInventoryItemGrid.ItemRemovedFromTheGrid -= ItemRemoved;
    }

    private void ItemCollected(InventoryItem item)
    {
        if (item.ItemData == RequiredItemType)
        {
            CurrentAmount += item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemRemoved(InventoryItem item)
    {
        if (item.ItemData == RequiredItemType)
        {
            CurrentAmount -= item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemUpdated(InventoryItem item, int updatedAmount)
    {
        if (item.ItemData == RequiredItemType)
        {
            CurrentAmount += updatedAmount;
            Evaluate();
        }
    }

}
