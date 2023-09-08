using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeliveryGoal : Goal
{
    /// <summary>
    /// WIP
    /// </summary>
  
    public List<Item.ItemType> RequiredItemCategories;
    public ItemContainer.QuestItemsBehaviourEnum QuestItemsBehaviour;
    public float RequiredWeight;
    public int RequiredCount;
    public float RequiredRotThreshold;

    public DeliveryGoal(State currentState, string description, int currentAmount, int requiredAmount, List<Item.ItemType> requiredItemCategories, ItemContainer.QuestItemsBehaviourEnum questItemsBehaviour, float requiredWeight, int requiredCount, float requiredRotThreshold) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItemCategories = requiredItemCategories;
        QuestItemsBehaviour = questItemsBehaviour;
        RequiredWeight = requiredWeight;
        RequiredCount = requiredCount;
        RequiredRotThreshold = requiredRotThreshold;
    }
    public override void Initialize()
    {
        //Container.AcceptedItems(string summary) += OnAcceptedItems;

        Evaluate();
    }
    public override void Deinitialize()
    {
        //container.AcceptedItems(string summary) -= OnAcceptedItems;
    }

}
