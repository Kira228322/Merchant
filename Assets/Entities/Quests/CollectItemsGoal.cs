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

        ItemGrid playerInventoryItemGrid = Player.Singleton.Inventory.GetComponent<ItemGrid>();
        playerInventoryItemGrid.ItemPlacedInTheGrid += ItemCollected;
        playerInventoryItemGrid.ItemPlacedInTheStack += ItemCollected;
        playerInventoryItemGrid.ItemRemovedFromTheGrid += ItemRemoved;
        playerInventoryItemGrid.ItemsRemovedFromTheStack += ItemRemoved;
    }

    public override void Deinitialize()
    {
        base.Deinitialize();

        ItemGrid playerInventoryItemGrid = Player.Singleton.Inventory.GetComponent<ItemGrid>();
        playerInventoryItemGrid.ItemPlacedInTheGrid -= ItemCollected;
        playerInventoryItemGrid.ItemPlacedInTheStack -= ItemCollected;
        playerInventoryItemGrid.ItemRemovedFromTheGrid -= ItemRemoved;
        playerInventoryItemGrid.ItemsRemovedFromTheStack -= ItemRemoved;
    }

    private void ItemCollected(InventoryItem item)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount += item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemCollected(InventoryItem item, int howManyWereInserted)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount += howManyWereInserted;
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
    private void ItemRemoved(InventoryItem item, int amount)
    {
        if (item.ItemData == _requiredItemType)
        {
            CurrentAmount -= amount;
            Evaluate();
        }
    }

}
