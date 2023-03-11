using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Roman.Rework
{
    public class QuestHandler : MonoBehaviour, ISaveable<QuestSaveData>
    {

        private static QuestHandler Instance;
        [SerializeField] private QuestLog _questLog; //UI-КвестЛог

        public static QuestLog QuestLog => Instance._questLog;

        public List<Quest> Quests = new(); // Содержит все квесты, в том числе проваленные или выполненные


        public static void AddQuest(Quest.QuestParams questParams)
        {
            Quest quest = new(questParams);
            Instance.Quests.Add(quest);
            QuestLog.AddQuest(quest);
        }
        public static void RemoveQuest(Quest quest)
        {
            if (!Instance.Quests.Remove(quest))
                Debug.LogWarning("Попытался удалить квест, которого не было в списке");
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
            //В этом методе была использована помощь ChatGPT касаемо LINQ-выражений

            QuestSaveData saveData = new()
            {
                savedQuestParams = Quests.Select(quest => new Quest.QuestParams
                {
                    currentState = quest.CurrentState,
                    questName = quest.QuestName,
                    description = quest.Description,
                    experienceReward = quest.ExperienceReward,
                    moneyReward = quest.MoneyReward,
                    itemRewards = quest.ItemRewards,

                    goals = quest.Goals.Select(goal => new Goal.GoalParams
                    {
                        description = goal.Description,
                        currentState = goal.CurrentState,
                        currentAmount = goal.CurrentAmount,
                        requiredAmount = goal.RequiredAmount

                    }).ToList()
                }).ToList()
            };

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
}
