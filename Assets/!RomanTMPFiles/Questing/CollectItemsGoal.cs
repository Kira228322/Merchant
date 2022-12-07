using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemsGoal : Goal 
{
    public Item _requiredItemType;

    public CollectItemsGoal(Item requiredItemType, string description, bool isCompleted, int currentAmount, int requiredAmount)
    {
        _requiredItemType = requiredItemType;
        Description = description;
        IsCompleted = isCompleted;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;   
    }

    public override void Initialize()
    {
        base.Initialize();
        Player.Singleton.Inventory.GetComponent<ItemGrid>().ItemPlacedInTheGrid += ItemCollected;
        Player.Singleton.Inventory.GetComponent<ItemGrid>().ItemRemovedFromTheGrid += ItemRemoved;
    }

    private void ItemCollected(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
    private void ItemRemoved(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount--;
            Evaluate();
        }
    }

}
