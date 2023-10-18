using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CollectItemsGoal : Goal
{

    //Нельзя зафейлить, просто сбор предметов
    [NonSerialized] public Item RequiredItem;
    public string RequiredItemName;

    public CollectItemsGoal(State currentState, string description, int currentAmount, int requiredAmount, string requiredItemName) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItemName = requiredItemName;
    }

    public override void Initialize()
    {
        RequiredItem = ItemDatabase.GetItem(RequiredItemName);

        //Надо найти, сколько таких вещей уже есть у игрока в инвентаре
        foreach (InventoryItem item in Player.Instance.Inventory.ItemList)
        {
            if (item.ItemData == RequiredItem)
            {
                CurrentAmount += item.CurrentItemsInAStack;
            }
        }
        Evaluate();

        ItemGrid playerInventoryItemGrid = Player.Instance.Inventory.BaseItemGrid;
        playerInventoryItemGrid.ItemPlacedInTheGrid += ItemCollected;
        playerInventoryItemGrid.ItemUpdated += ItemUpdated;
        playerInventoryItemGrid.ItemRemovedFromTheGrid += ItemRemoved;
    }

    public override void Deinitialize()
    {
        ItemGrid playerInventoryItemGrid = Player.Instance.Inventory.BaseItemGrid;
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
