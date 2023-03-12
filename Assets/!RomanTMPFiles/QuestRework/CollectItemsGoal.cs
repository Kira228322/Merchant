using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CollectItemsGoal : Goal
{

    //������ ���������, ������ ���� ���������

    public string RequiredItemName;
    public Item RequiredItem;
    public CollectItemsGoal(State currentState, string description, int currentAmount, int requiredAmount, string requiredItemName) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItemName = requiredItemName;
        RequiredItem = ItemDatabase.GetItem(requiredItemName);
    }

    public override void Initialize()
    {
        //���� �����, ������� ����� ����� ��� ���� � ������ � ���������
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
