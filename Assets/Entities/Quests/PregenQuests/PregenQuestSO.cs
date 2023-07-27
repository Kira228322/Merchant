using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Pre-generated Quest", menuName = "Quest/Pre-generated Quest")]
public class PregenQuestSO : ScriptableObject
{
    public QuestParams.State StartingState;
    public string QuestName;
    public string QuestSummary;
    [TextArea(2,5)] public string Description;
    [HideInInspector] public int MinExperienceReward; 
    [HideInInspector] public int MaxExperienceReward; 
    [HideInInspector] public int MinMoneyReward;
    [HideInInspector] public int MaxMoneyReward;
    

   public PregenQuestSO NextQuest;
   [HideInInspector] public List<CompactedGoal> goals = new();
    
   [HideInInspector] public List<ItemReward> ItemRewards = new();

    public QuestParams GenerateQuestParams()
    {

        QuestParams questParams = new()
        {
            currentState = StartingState,
            questName = QuestName,
            questSummary = QuestSummary,
            description = Description,
            experienceReward = Random.Range(MinExperienceReward, MaxExperienceReward + 1),
            moneyReward = Random.Range(MinMoneyReward, MaxMoneyReward + 1),

            itemRewards = ItemRewards
        };

        if (NextQuest != null)
        {
            questParams.nextQuestParams = NextQuest.GenerateQuestParams();
        }

        questParams.goals = new();
        foreach (CompactedGoal pregenGoal in goals)
        {
            Goal newGoal = new();
            int additiveRewardItemCount;
            switch (pregenGoal.goalType)
            {
                case CompactedGoal.GoalType.CollectItemsGoal:
                    additiveRewardItemCount = Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount);
                    
                    newGoal = new CollectItemsGoal(pregenGoal.goalState, pregenGoal.description, 
                        pregenGoal.currentAmount, additiveRewardItemCount, pregenGoal.RequiredItemName);
                    
                    if (pregenGoal.AdditiveMoneyReward)
                        questParams.moneyReward += ItemDatabase.GetItem(pregenGoal.RequiredItemName).Price * additiveRewardItemCount;
                    
                    break;

                case CompactedGoal.GoalType.TalkToNPCGoal:
                    newGoal = new TalkToNPCGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount),
                        pregenGoal.RequiredIDofNPC, pregenGoal.RequiredLine, pregenGoal.FailingLine);
                    break;

                case CompactedGoal.GoalType.TimedGoal:
                    newGoal = new TimedGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount));
                    break;

                case CompactedGoal.GoalType.WaitingGoal:
                    newGoal = new WaitingGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount));
                    break;

                case CompactedGoal.GoalType.GiveItemsGoal:

                    additiveRewardItemCount = Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount);

                    newGoal = new GiveItemsGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, additiveRewardItemCount,
                        pregenGoal.RequiredItemName, pregenGoal.RequiredIDofNPC, pregenGoal.RequiredLine);

                    if (pregenGoal.AdditiveMoneyReward)
                        questParams.moneyReward += ItemDatabase.GetItem(pregenGoal.RequiredItemName).Price * additiveRewardItemCount;

                    break;
                default:
                    Debug.LogError("Нет такого типа Goal");
                    break;
            }
            questParams.goals.Add(newGoal);

        }
        return questParams;
    }
    [Serializable]
    public class CompactedGoal
    {
        public enum GoalType { CollectItemsGoal, TalkToNPCGoal, WaitingGoal, TimedGoal, GiveItemsGoal}

        public GoalType goalType;
        public Goal.State goalState;
        public string description;
        public int currentAmount;
        
        public bool randomAmount;
        public int minRequiredAmount;
        public int maxRequiredAmount;
        //Опциональные поля
        public bool AdditiveMoneyReward;
        public int RequiredIDofNPC;
        public string RequiredLine;
        public string FailingLine;

        public string RequiredItemName;
    }

}