using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;
using UnityEngine.Events;

public class QuestHandler : MonoBehaviour, ISaveable<QuestSaveData>
{

    private static QuestHandler Instance;
    [SerializeField] private QuestLog _questLog; //UI-��������

    public static QuestLog QuestLog => Instance._questLog;

    
    public List<Quest> Quests = new(); // �������� ��� ������, � ��� ����� ����������� ��� �����������
    public List<Quest> ActiveQuests => Quests.Where(quest => quest.CurrentState == Quest.State.Active).ToList();

    public static event UnityAction QuestChangedState; 
    public static List<Quest> GetActiveQuestsForThisNPC(int ID)
    {
        // ����� ��������������.
        return Instance.ActiveQuests.Where(quest => quest.QuestGiver.ID == ID).ToList();
    }
    
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
        quest.QuestChangedState += Instance.OnQuestChangedState;
        QuestChangedState?.Invoke();
    }
    public static void AddQuest(QuestParams questParams, NpcData questGiver)
    {
        Quest quest = new(questParams);
        quest.QuestGiver = questGiver;
        Instance.Quests.Add(quest);
        QuestLog.AddQuest(quest);
        quest.QuestChangedState += Instance.OnQuestChangedState;
        QuestChangedState?.Invoke();
    }

    private void OnQuestChangedState(Quest quest)
    {
        QuestChangedState?.Invoke();
        if (quest.CurrentState == Quest.State.Completed || quest.CurrentState == Quest.State.Failed)
            quest.QuestChangedState -= Instance.OnQuestChangedState;
    }
    
    public static Quest GetQuestBySummary(string summary)
    {
        return Instance.Quests.FirstOrDefault(quest => quest.QuestSummary == summary);
    }
    public static Quest GetActiveQuestBySummary(string summary)
    {
        return Instance.ActiveQuests.FirstOrDefault(quest => quest.QuestSummary == summary);
    }

    public static bool HasQuestBeenTaken(string summary)
    {
        return Instance.Quests.Any(quest => quest.QuestSummary == summary);
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
            QuestParams questParams = new()
            {
                currentState = (QuestParams.State)quest.CurrentState,
                questName = quest.QuestName,
                questSummary = quest.QuestSummary,
                description = quest.Description,
                questGiverID = quest.QuestGiver.ID,
                experienceReward = quest.ExperienceReward,
                moneyReward = quest.MoneyReward,
                itemRewards = quest.ItemRewards,

                goals = new()
            };
            foreach (Goal goal in quest.Goals)
            {
                //� ������ ����� �� ��������� � �� ����� ���� �� ����� ������ �������� switch � ������������� ���� ��������� �����,
                //�� ���� ������ ���������, �� ���������� ��������� ���� � �������������� �������� � System.Reflection.
                //����� �������� � �� ��� �������. � ��������� � ChatGPT, �����-����� ��������� ���.
                //������� ����� ����� ����� Goal �� ������ �� �������� ��� ����, ����� �� ����� ��������� ����...

                Goal newGoal = null;
                switch (goal)
                {
                    case CollectItemsGoal oldGoal:
                        newGoal = new CollectItemsGoal(oldGoal.CurrentState, oldGoal.Description, 
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredItemName);
                        break;
                    case TalkToNPCGoal oldGoal:
                        newGoal = new TalkToNPCGoal(oldGoal.CurrentState, oldGoal.Description, 
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredIDOfNPC, oldGoal.RequiredLine, oldGoal.FailingLine);
                        break;
                    case WaitingGoal oldGoal:
                        newGoal = new WaitingGoal(oldGoal.CurrentState, oldGoal.Description, 
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount);
                        break;
                    case TimedGoal oldGoal:
                        newGoal = new TimedGoal(oldGoal.CurrentState, oldGoal.Description, 
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount);
                        break;
                    case GiveItemsGoal oldGoal:
                        newGoal = new GiveItemsGoal(oldGoal.CurrentState, oldGoal.Description,
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredItemName,
                            oldGoal.RequiredIDOfNPC, oldGoal.RequiredLine);
                        break;
                    default:
                        Debug.LogError("��� ������ ���� Goal");
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