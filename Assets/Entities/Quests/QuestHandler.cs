using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class QuestHandler : MonoBehaviour, ISaveable<QuestSaveData>
{
    #region ����, �������� � �������
    
    public static QuestLog QuestLog => Instance._questLog;
    public static event UnityAction<Quest> QuestChangedState; 
    private static QuestHandler Instance;

    [SerializeField] private QuestLog _questLog; //UI-��������
    
    public List<Quest> Quests = new(); // �������� ��� ������, � ��� ����� ����������� ��� �����������
    public List<Quest> ActiveQuests = new(); // �������� ���� ����� � AddQuest � OnQuestChangedState
    #endregion
    #region ������ �������������
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion
    #region ������ ������ � �������� (��������, �������� ���������)
    public static Quest AddQuest(QuestParams questParams)
    {
        Quest quest = new(questParams);
        Instance.Quests.Add(quest);
        if (quest.CurrentState == Quest.State.Active)
            Instance.ActiveQuests.Add(quest);
        QuestLog.AddQuest(quest);
        quest.QuestChangedState += Instance.OnQuestChangedState;
        QuestChangedState?.Invoke(quest);
        return quest;
    }
    public static Quest AddQuest(string questSummary)
    {
        return AddQuest(PregenQuestDatabase.GetQuestParams(questSummary));
    }
    public static Quest AddQuest(QuestParams questParams, NpcData questGiver)
    {
        Quest quest = new(questParams);
        Instance.Quests.Add(quest);
        if (quest.CurrentState == Quest.State.Active)
            Instance.ActiveQuests.Add(quest);
        QuestLog.AddQuest(quest);
        quest.QuestChangedState += Instance.OnQuestChangedState;
        QuestChangedState?.Invoke(quest);
        return quest;
    }
    private void OnQuestChangedState(Quest quest, Quest.State oldState, Quest.State newState)
    {

        ActiveQuests.Remove(quest); //����� �� ����� �������� ��� ��������� �� ��������,
                                    //�� ������ �������� � ����� ���������.
                                    //(Quest.ctor �� �������� ���� �����)

        if (newState == Quest.State.Completed || newState == Quest.State.Failed)
            quest.QuestChangedState -= Instance.OnQuestChangedState;
        QuestChangedState?.Invoke(quest);
    }
    #endregion
    #region ������ ��������� ���������� � �������
    public static List<Quest> GetActiveQuestsForThisNPC(int ID)
    {
        //null propagation ��������� ��� unity��������, ������� �� ��������� LINQ (��� �������� questgiver != null)
        List<Quest> result = new();
        foreach (Quest quest in Instance.ActiveQuests)
        {
            if (quest.IsNpcTargetOfQuest(ID) || quest.IsNpcSourceOfQuest(ID))
                result.Add(quest);
        }
        return result;
    }
    public static bool IsNpcTargetOfAnyActiveQuest(int ID)
    {
        return Instance.ActiveQuests.Any(quest => quest.IsNpcTargetOfQuest(ID));
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
    public static bool AnyUncollectedRewards()
    {
        return Instance.Quests.Any(quest => quest.CurrentState == Quest.State.RewardUncollected);
    }
    #endregion
    #region ���������� � �������� 
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
                    case DeliveryGoal oldGoal:
                        newGoal = new DeliveryGoal(oldGoal.CurrentState, oldGoal.Description,
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredIDOfNPC, oldGoal.RequiredItemCategories,
                            oldGoal.QuestItemsBehaviour, oldGoal.RequiredWeight, oldGoal.RequiredCount,
                            oldGoal.RequiredRotThreshold);
                        break;
                    case UseItemsGoal oldGoal:
                        newGoal = new UseItemsGoal(oldGoal.CurrentState, oldGoal.Description,
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredItemName);
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
    #endregion
}