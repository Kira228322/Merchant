using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestHandler : MonoBehaviour, ISaveable<QuestSaveData>
{

    private static QuestHandler Instance;
    [SerializeField] private QuestLog _questLog; //UI- вестЋог

    public static QuestLog QuestLog => Instance._questLog;

    public List<Quest> Quests = new(); // —одержит все квесты, в том числе проваленные или выполненные


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public static void AddQuest(Quest.QuestParams questParams)
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

    public QuestSaveData SaveData()
    {

        QuestSaveData saveData = new();

        foreach (Quest quest in Quests) 
        {
            Quest.QuestParams questParams = new();
            questParams.currentState = quest.CurrentState;
            questParams.questName = quest.QuestName;
            questParams.questSummary = quest.QuestSummary;
            questParams.description = quest.Description;
            questParams.experienceReward = quest.ExperienceReward;
            questParams.moneyReward = quest.MoneyReward;
            questParams.itemRewards = quest.ItemRewards;
            questParams.goals = quest.Goals;
        }

        return saveData;
    }

    public void LoadData(QuestSaveData data)
    {
        foreach (Quest.QuestParams questParams in data.savedQuestParams)
        {
            AddQuest(questParams);
        }
    }
}