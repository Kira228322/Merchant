using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class UseItemsGoal : Goal
{
    [NonSerialized] public UsableItem RequiredItem;
    public string RequiredItemName;

    public UseItemsGoal(State currentState, string description, int currentAmount, int requiredAmount, string requiredItemName) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItemName = requiredItemName;
    }

    public override void Initialize()
    {
        RequiredItem = (UsableItem)ItemDatabase.GetItem(RequiredItemName);

        Player.Instance.Inventory.ItemUsed += OnItemUse;

        Evaluate();
    }

    public override void Deinitialize()
    {
        Player.Instance.Inventory.ItemUsed -= OnItemUse;
    }

    private void OnItemUse(UsableItem usedItem)
    {
        if (usedItem.Name == RequiredItemName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
