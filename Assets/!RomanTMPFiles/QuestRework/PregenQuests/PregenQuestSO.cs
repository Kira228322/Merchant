using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pre-generated Quest", menuName = "Quest/Pre-generated Quest")]
public class PregenQuestSO : ScriptableObject
{

    public QuestParams.State StartingState;
    public string QuestName;
    public string QuestSummary;
    public string Description;
    public int ExperienceReward;
    public int MoneyReward;

   /* 
    private int _experienceReward;
    private int _moneyReward;
   */

    public PregenQuestSO NextQuest;

    public List<ItemReward> ItemRewards = new();

    public List<CompactedGoal> goals;
    /*private int AssignRandomValue(string line)
    {
        string[] strArr = line.Substring(line.IndexOf("(") + 1, line.IndexOf(")") - line.IndexOf("(") - 1).Split(',');
        int[] intArr = Array.ConvertAll(strArr, int.Parse);

        int result = UnityEngine.Random.Range(intArr[0], intArr[1]);
        return result;
    }*/
    public QuestParams GenerateQuestParams()
    {
       /*
        if (ExperienceReward.ToLower().Contains("random"))
        {
            _experienceReward = AssignRandomValue(ExperienceReward);
        }
        if (MoneyReward.ToLower().Contains("random"))
        {
            _moneyReward = AssignRandomValue(MoneyReward);
        }
       */

        QuestParams questParams = new();

        questParams.currentState = StartingState;
        questParams.questName = QuestName;
        questParams.questSummary = QuestSummary;
        questParams.description = Description;
        questParams.experienceReward = ExperienceReward;
        questParams.moneyReward = MoneyReward;
        questParams.itemRewards = ItemRewards;

        if (NextQuest != null)
        {
            questParams.nextQuestParams = NextQuest.GenerateQuestParams();
        }

        questParams.goals = new();
        foreach (CompactedGoal pregenGoal in goals)
        {
            Goal newGoal = new();
            switch (pregenGoal.goalType)
            {
                case CompactedGoal.GoalType.CollectItemsGoal:
                    newGoal = new CollectItemsGoal(pregenGoal.goalState, pregenGoal.description, 
                        pregenGoal.currentAmount, pregenGoal.requiredAmount, 
                        pregenGoal.RequiredItemName);
                    
                    break;

                case CompactedGoal.GoalType.TalkToNPCGoal:
                    newGoal = new TalkToNPCGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, pregenGoal.requiredAmount,
                        pregenGoal.RequiredIDofNPC, pregenGoal.RequiredLine, pregenGoal.FailingLine);
                    break;

                case CompactedGoal.GoalType.TimedGoal:
                    newGoal = new TimedGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, pregenGoal.requiredAmount);
                    break;

                case CompactedGoal.GoalType.WaitingGoal:
                    newGoal = new WaitingGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, pregenGoal.requiredAmount);
                    break;
            }
            questParams.goals.Add(newGoal);

        }
        return questParams;
    }
    [Serializable]
    public class CompactedGoal
    {
        public enum GoalType { CollectItemsGoal, TalkToNPCGoal, WaitingGoal, TimedGoal}

        public GoalType goalType;
        public Goal.State goalState;
        public string description;
        public int currentAmount;
        public int requiredAmount;

        //Опциональные поля

        public int RequiredIDofNPC;
        public string RequiredLine;
        public string FailingLine;

        public string RequiredItemName;
    }
}