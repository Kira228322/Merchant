using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeliveryGoal : Goal
{
  
    public int RequiredIDOfNPC;
    public List<Item.ItemType> RequiredItemCategories;
    public ItemContainer.QuestItemsBehaviourEnum QuestItemsBehaviour;
    public float RequiredWeight;
    public int RequiredCount;
    public float RequiredRotThreshold;

    public DeliveryGoal(State currentState, string description, int currentAmount, int requiredAmount, int requiredIdOfNpc, List<Item.ItemType> requiredItemCategories, ItemContainer.QuestItemsBehaviourEnum questItemsBehaviour, float requiredWeight, int requiredCount, float requiredRotThreshold) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredIDOfNPC = requiredIdOfNpc;
        RequiredItemCategories = requiredItemCategories;
        QuestItemsBehaviour = questItemsBehaviour;
        RequiredWeight = requiredWeight;
        RequiredCount = requiredCount;
        RequiredRotThreshold = requiredRotThreshold;
    }
    public override void Initialize()
    {
        Evaluate();
    }
    public override void Deinitialize()
    {
        //Ничего? Этот Goal не подписывается на ивенты, отписываться тоже не надо.
        //Кстати почему не подписывается - потому что DialogueManagerу бы пришлось инвокать такой ивент с
        //каким-то особым идентификатором, чтобы не выполнились все активные DeliveryGoal.
        //У него уже есть прямая ссылка на этот Goal, поэтому я подумал что так будет эффективнее. (08.09.23)
    }

}
