using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    //Этот типчик будет раздавать квесты. Пока думаю сделать ему синглтон. Вероятно, на этом же объекте будут отслеживаться все текущие квесты в виде компонентов 
    //Вместо синглтона можно было бы сделать ссылку в GameManager, или какой нибудь QuestManager

    private static QuestHandler Singleton;

    [SerializeField] private GameObject _questsGameObject; //Объект, на котором висят все квесты. Возможно, этот объект.
    [SerializeField] private QuestLog _questLog; //UI-КвестЛог

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
        Singleton._questLog.AddToActiveQuests(quest);
        if (quest.IsCompleted) Singleton.OnQuestCompleted(quest); //Если выполнился моментально, в тот же фрейм как был взят
    }
    public static void RemoveQuest(System.Type questType)
    {
        Quest quest = (Quest)Singleton._questsGameObject.GetComponent(questType);
        Destroy(Singleton._questsGameObject.GetComponent(questType));
    }
    public static void MoveToCompleted(Quest quest)
    {
        Singleton._questLog.MoveToCompletedQuests(quest);
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
        Singleton._questLog.MoveToCompletedQuests(quest);
    }
    
}
