using System;
using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector]public int MinMoneyReward;
    [HideInInspector]public int MaxMoneyReward;

   /* 
    private int _experienceReward;
    private int _moneyReward;
   */

   public PregenQuestSO NextQuest;
    public List<CompactedGoal> goals;
    
   [HideInInspector]public List<ItemReward> ItemRewards = new();

   
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
        questParams.experienceReward = Random.Range(MinExperienceReward, MaxExperienceReward + 1);
        questParams.moneyReward = Random.Range(MinMoneyReward, MaxMoneyReward + 1);
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

    [CustomEditor(typeof(PregenQuestSO))]
    class EditorPregenQuest : Editor
    {
        private bool randomExp;
        private bool randomReward;
        private PregenQuestSO _quest;
        private void OnEnable()
        {
            _quest = (PregenQuestSO)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Вознаграждение");
            GUILayout.Space(5);
            randomExp = GUILayout.Toggle(randomExp, "Random experience");
            
            if (randomExp)
            {
                EditorGUILayout.BeginHorizontal();
                _quest.MinExperienceReward = EditorGUILayout.IntField("Min exp", _quest.MinExperienceReward);
                GUILayout.FlexibleSpace();
                _quest.MaxExperienceReward = EditorGUILayout.IntField("Max exp", _quest.MaxExperienceReward);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                _quest.MinExperienceReward = EditorGUILayout.IntField("Experience", _quest.MinExperienceReward);
                _quest.MaxExperienceReward = _quest.MinExperienceReward;
            }
            
            randomReward = GUILayout.Toggle(randomReward, "Random money");
            
            if (randomReward)
            {
                EditorGUILayout.BeginHorizontal();
                _quest.MinMoneyReward = EditorGUILayout.IntField("Min money", _quest.MinMoneyReward);
                GUILayout.FlexibleSpace();
                _quest.MaxMoneyReward = EditorGUILayout.IntField("Max money", _quest.MaxMoneyReward);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                _quest.MinMoneyReward = EditorGUILayout.IntField("Money", _quest.MinMoneyReward);
                _quest.MaxMoneyReward = _quest.MinMoneyReward;
            }
            
            
                
            GUILayout.Space(20);
            
            if (GUILayout.Button("Add item reward"))
                _quest.ItemRewards.Add(new ItemReward("", 0, 0));
            
            if (_quest.ItemRewards.Count > 0)
            {
                foreach (var item in _quest.ItemRewards)
                {
                    GUILayout.Space(-1);
                    EditorGUILayout.BeginVertical("box");

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(-20);
                    if (GUILayout.Button("X", GUILayout.Height(17), GUILayout.Width(18)))
                    {
                        _quest.ItemRewards.Remove(item);
                        break;
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Name", GUILayout.MaxWidth(38));
                    item.itemName = EditorGUILayout.TextField(item.itemName);
                    GUILayout.Label("Amount", GUILayout.MaxWidth(50));
                    item.amount = EditorGUILayout.IntField( item.amount);
                    GUILayout.Label("DayBoughtAg", GUILayout.MaxWidth(80));
                    item.daysBoughtAgo = EditorGUILayout.FloatField( item.daysBoughtAgo);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
            }
            
        }
    }
}