using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //������, �� ������� ����� ��� ������. ��������, ���� ������.
    [SerializeField] private QuestLog _questLog; //UI-��������

    public static QuestLog QuestLog => Singleton._questLog;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
        }
    }

    public static void AddQuest(string questName)
    {
        Quest quest = (Quest)Singleton._questsGameObject.AddComponent(System.Type.GetType(questName));
        quest.QuestUpdatedEvent += Singleton.OnQuestUpdated;
        quest.QuestCompletedEvent += Singleton.OnQuestCompleted;
        QuestLog.AddToActiveQuests(quest);
        if (quest.IsCompleted) Singleton.OnQuestCompleted(quest); //���� ���������� �����������, � ��� �� ����� ��� ��� ����
    }
    public static void AddQuestAsCompleted (string questName)
    {
        Quest quest = (Quest)Singleton._questsGameObject.AddComponent(System.Type.GetType(questName));
        quest.IsCompleted = true;
        foreach (Goal goal in quest.Goals)
        {
            goal.CurrentAmount = goal.RequiredAmount;
            goal.IsCompleted = true;
        }
        QuestLog.AddToActiveQuests(quest);
        quest.questPanel.OnComplete();
        QuestLog.MoveToCompletedQuests(quest);

    }
    public static void RemoveQuest(System.Type questType)
    {
        //��� ������ ������ ���� � �������, �� �� ������ ����� ������
        Destroy(Singleton._questsGameObject.GetComponent(questType));
    }
    public static void MoveToCompleted(Quest quest)
    {
        QuestLog.MoveToCompletedQuests(quest);
    }
    private void OnQuestUpdated(Quest quest)
    {
        quest.questPanel.Refresh();
    }
    private void OnQuestCompleted(Quest quest)
    {
        quest.questPanel.OnComplete();
        if(quest.NextQuestName != null)
        {
            AddQuest(quest.NextQuestName);
        }
        QuestLog.MoveToCompletedQuests(quest);
    }
    
}
