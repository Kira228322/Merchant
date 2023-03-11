using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestHandler : MonoBehaviour, ISaveable<QuestSaveData>
{

    private static QuestHandler Instance;
    [SerializeField] private QuestLog _questLog; //UI-КвестЛог

    public static QuestLog QuestLog => Instance._questLog;

    public List<Quest> Quests = new(); // Содержит все квесты, в том числе проваленные или выполненные


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public static void AddQuest(QuestParams questParams)
    {
        Quest quest = new(questParams);
        Instance.Quests.Add(quest);
        QuestLog.AddQuest(quest);
    }
    public static Quest GetQuestBySummary(string summary)
    {
        return Instance.Quests.FirstOrDefault(quest => quest.QuestSummary == summary);
    }

    public static bool IsQuestActive(string summary)
    {
        return Instance.Quests.Any(quest => quest.QuestSummary == summary && quest.CurrentState == Quest.State.Active);
    }
    public static bool IsQuestCompleted(string summary)
    {
        return Instance.Quests.Any(quest => quest.QuestSummary == summary &&
        (quest.CurrentState == Quest.State.Completed || quest.CurrentState == Quest.State.RewardUncollected));
    }
    public static bool IsQuestFailed(string summary)
    {
        return Instance.Quests.Any(quest => quest.QuestSummary == summary && quest.CurrentState == Quest.State.Failed);
    }

    public static QuestSaveData SaveQuests()
    {
        return Instance.SaveData();
    }
    public static void LoadQuests(QuestSaveData data)
    {
        Instance.LoadData(data);
    }

    public QuestSaveData SaveData()
    {

        QuestSaveData saveData = new();

        foreach (Quest quest in Instance.Quests) 
        {
            QuestParams questParams = new();
            questParams.currentState = (QuestParams.State)quest.CurrentState;
            questParams.questName = quest.QuestName;
            questParams.questSummary = quest.QuestSummary;
            questParams.description = quest.Description;
            questParams.experienceReward = quest.ExperienceReward;
            questParams.moneyReward = quest.MoneyReward;
            questParams.itemRewards = quest.ItemRewards;

            questParams.goals = new();
            foreach (Goal goal in quest.Goals)
            {
                //Я всегда топлю за обобщение и на самом деле не люблю делать подобные switch с перечислением всех возможных типов,
                //но если делать обобщенно, то начинается полнейшая дичь с использованием словарей и System.Reflection.
                //Очень волосато и не мой уровень. Я спрашивал у ChatGPT, очень-очень волосатый код.
                //Главное когда будут новые Goal не забыть их добавить вот сюда, иначе не будет сейвиться игра...

                Goal newGoal = null;
                switch (goal)
                {
                    case CollectItemsGoal oldGoal:
                        newGoal = new CollectItemsGoal(oldGoal.CurrentState, oldGoal.Description, oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredItem);
                        break;
                    case TalkToNPCGoal oldGoal:
                        newGoal = new TalkToNPCGoal(oldGoal.CurrentState, oldGoal.Description, oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredNPC.ID, oldGoal.RequiredLine, oldGoal.FailingLine);
                        break;
                    case WaitingGoal oldGoal:
                        newGoal = new WaitingGoal(oldGoal.CurrentState, oldGoal.Description, oldGoal.CurrentAmount, oldGoal.RequiredAmount);
                        break;
                    case TimedGoal oldGoal:
                        newGoal = new TimedGoal(oldGoal.CurrentState, oldGoal.Description, oldGoal.CurrentAmount, oldGoal.RequiredAmount);
                        break;
                }
                questParams.goals.Add(newGoal);
            }

            saveData.savedQuestParams.Add(questParams);
        }
        return saveData;
    }

    public void LoadData(QuestSaveData data)
    {
        foreach (QuestParams questParams in data.savedQuestParams)
        {
            AddQuest(questParams);
        }
    }
}