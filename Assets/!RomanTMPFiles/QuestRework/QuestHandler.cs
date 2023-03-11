using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roman.Rework
{
    public class QuestHandler : MonoBehaviour
    {
        private static QuestHandler Singleton;
        [SerializeField] private QuestLog _questLog; //UI-��������

        public static QuestLog QuestLog => Singleton._questLog;

        public List<Quest> Quests = new(); // �������� ��� ������, � ��� ����� ����������� ��� �����������

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
                Debug.LogWarning("��������� ������� �����, �������� �� ���� � ������");
        }

        //public static bool IsQuestActive()
        //public static bool IsQuestCompleted()  � ��� � ������� ��������: ��� ���������� ���������� � ������?
        //public static bool IsQuestFailed()     ��� ������ ���� � ���������� ���� �������?

        private void OnQuestUpdated(Quest quest)
        {
            //������ � �����-�����
        }

        private void OnQuestChangedState(Quest quest)
        {
            //������ � �����-�����
        }
    }
}
