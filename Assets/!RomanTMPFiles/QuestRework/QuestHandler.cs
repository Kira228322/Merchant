using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman.Rework
{
    public class QuestHandler : MonoBehaviour
    {
        private static QuestHandler Singleton;
        [SerializeField] private QuestLog _questLog; //UI-КвестЛог

        public static QuestLog QuestLog => Singleton._questLog;

        public List<Quest> Quests = new(); // Содержит все квесты, в том числе проваленные или выполненные

        public static void AddQuest(Quest.QuestParams questParams)
        {
            Quest quest = new(questParams);
            quest.QuestUpdated += Singleton.OnQuestUpdated;
            quest.QuestChangedState += Singleton.OnQuestChangedState;
            Singleton.Quests.Add(quest);
        }
        public static void RemoveQuest(Quest quest)
        {
            if (!Singleton.Quests.Remove(quest))
                Debug.LogWarning("Попытался удалить квест, которого не было в списке");
        }

        //public static bool IsQuestActive()
        //public static bool IsQuestCompleted()  А вот и главная проблема: как однозначно обратиться к квесту?
        //public static bool IsQuestFailed()     Что должно быть в параметрах этих методов?

        private void OnQuestUpdated(Quest quest)
        {
            //Работа с квест-логом
        }

        private void OnQuestChangedState(Quest quest)
        {
            //Работа с квест-логом
        }
    }
}
