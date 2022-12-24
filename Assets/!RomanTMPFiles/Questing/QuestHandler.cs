using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //���� ������ ����� ��������� ������. ���� ����� ������� ��� ��������. ��������, �� ���� �� ������� ����� ������������� ��� ������� ������ � ���� ����������� 
    //������ ��������� ����� ���� �� ������� ������ � GameManager, ��� ����� ������ QuestManager

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //������, �� ������� ����� ��� ������. ��������, ���� ������.
    [SerializeField] private QuestLog _questLog; //UI-��������

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
        quest.QuestCompletedEvent += Singleton.OnQuestComplete;
        Singleton._questLog.AddToActiveQuests(quest);
        if (quest.IsCompleted) Singleton.OnQuestComplete(quest); //���� ���������� �����������, � ��� �� ����� ��� ��� ����
    }
    public static void RemoveQuest(System.Type questType)
    {
        Quest quest = (Quest)Singleton._questsGameObject.GetComponent(questType);
        Singleton._questLog.RemoveFromActiveQuests(quest);
        Destroy(Singleton._questsGameObject.GetComponent(questType));
    }
    private void OnQuestComplete(Quest quest)
    {
        if(quest.NextQuestName != null)
        {
            AddQuest(quest.NextQuestName);
        }
        RemoveQuest(quest.GetType());
    }
    
}
