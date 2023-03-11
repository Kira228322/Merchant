using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectItemsGoal : Goal
{

    //Нельзя зафейлить, просто сбор предметов

    public Item RequiredItem;
    public CollectItemsGoal(State currentState, string description, int currentAmount, int requiredAmount, Item requiredItem) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItem = requiredItem;
    }

    public override void Initialize()
    {
        base.Initialize();

        //Надо найти, сколько таких вещей уже есть у игрока в инвентаре
        foreach (InventoryItem item in Player.Instance.Inventory.ItemList)
        {
            if (item.ItemData == RequiredItem)
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
        if (item.ItemData == RequiredItem)
        {
            CurrentAmount += item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemRemoved(InventoryItem item)
    {
        if (item.ItemData == RequiredItem)
        {
            CurrentAmount -= item.CurrentItemsInAStack;
            Evaluate();
        }
    }
    private void ItemUpdated(InventoryItem item, int updatedAmount)
    {
        if (item.ItemData == RequiredItem)
        {
            CurrentAmount += updatedAmount;
            Evaluate();
        }
    }
}
