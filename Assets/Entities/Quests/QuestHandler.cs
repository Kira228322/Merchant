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
    public static QuestHandler Instance;

    [SerializeField] private QuestLog _questLog; //UI-��������
    
    public List<Quest> Quests = new(); // �������� ��� ������, � ��� ����� ����������� ��� �����������
    public List<Quest> ActiveQuests = new(); // �������� ���� ����� � AddQuest � OnQuestChangedState

    public List<AwaitingQuest> AwaitingQuests = new(); //������, ������� ���� �������, ����� ������ ����.

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
    private void OnQuestChangedState(Quest quest, Quest.State oldState, Quest.State newState)
    {
        ActiveQuests.Remove(quest); //����� �� ����� �������� ��� ��������� �� ��������,
                                    //�� ������ �������� � ����� ���������.
                                    //(Quest.ctor �� �������� ���� �����)

        if (newState == Quest.State.RewardUncollected || (!quest.HasRewards() && newState == Quest.State.Completed))
        {

            //� ������ ���������� ������ ����� �������� �� ���� ���� �������:
            //�������� �� ��� ������ �� ������, ������� ��� �� ���� ����� � ������� �����-�� ���������� 
            //�������� �� ��� ������ �� ������, � ������� ��������� ��� ����������
            //�������� ��� ������������ ������.

            foreach (var pregenQuest in PregenQuestDatabase.QuestList.ScriptedQuests
            .Where(quest => quest.PrerequisiteQuests?.Count > 0 && !HasQuestBeenTaken(quest.QuestSummary))
            .Where(quest => quest.PrerequisiteQuests.All(prerequisite => HasQuestBeenCompleted(prerequisite.QuestSummary)))
            .Select(quest => quest.GenerateQuestParams()))
            {
                if (quest.QuestCompletionDelay > 0)
                {
                    AwaitingQuest awaitingQuest = new(pregenQuest, quest.QuestCompletionDelay);
                    awaitingQuest.AwaitingQuestGiven += OnAwaitingQuestGiven;
                    awaitingQuest.Initialize();
                    AwaitingQuests.Add(awaitingQuest);
                }
                else
                    AddQuest(pregenQuest);
            }
        }

        if (newState == Quest.State.Completed || newState == Quest.State.Failed)
            quest.QuestChangedState -= Instance.OnQuestChangedState;
        QuestChangedState?.Invoke(quest);
        
    }

    private void OnAwaitingQuestGiven(AwaitingQuest quest)
    {
        quest.AwaitingQuestGiven -= OnAwaitingQuestGiven;
        AwaitingQuests.Remove(quest);
    }
    #endregion
    #region ������ ��������� ���������� � �������
    public static List<Quest> GetActiveQuestsForThisNPC(int ID)
    {
        List<Quest> result = new();
        foreach (Quest quest in Instance.ActiveQuests)
        {
            if (quest.IsNpcTargetOfQuest(ID) || quest.IsNpcSourceOfQuest(ID))
                result.Add(quest);
        }
        return result;
    }
    public static List<Quest> GetActiveQuests()
    {
        List<Quest> result = new();
        foreach (Quest quest in Instance.ActiveQuests)
        {
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
        return Instance.Quests.Any(quest => quest.QuestSummary == summary) 
            || Instance.AwaitingQuests.Any(quest => quest.questParams.questSummary == summary);
    }
    public static bool HasQuestBeenCompleted(string summary)
    {
        return Instance.Quests.Any
            (quest => quest.QuestSummary == summary &&
            (quest.CurrentState == Quest.State.RewardUncollected
            || quest.CurrentState == Quest.State.Completed));
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
                questCompletionDelay = quest.QuestCompletionDelay,
                questGiverID = quest.QuestGiverID,
                dayStartedOn = quest.DayStartedOn,
                hourStartedOn = quest.HourStartedOn,
                dayFinishedOn = quest.DayFinishedOn,
                hourFinishedOn = quest.HourFinishedOn,
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
                    case KeepItemsGoal oldGoal:
                        newGoal = new KeepItemsGoal(oldGoal.CurrentState, oldGoal.Description,
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredItemName);
                        break;
                    case StayOnSceneGoal oldGoal:
                        newGoal = new StayOnSceneGoal(oldGoal.CurrentState, oldGoal.Description,
                            oldGoal.CurrentAmount, oldGoal.RequiredAmount, oldGoal.RequiredSceneName);
                        break;
                    default:
                        Debug.LogError("��� ������ ���� Goal");
                        break;
                }
                questParams.goals.Add(newGoal);
            }

            saveData.savedQuestParams.Add(questParams);
        }
        foreach (AwaitingQuest awaitingQuest in Instance.AwaitingQuests)
        {
            saveData.awaitingQuests.Add(new(awaitingQuest.questParams, awaitingQuest.delay));
        }
        return saveData;
    }

    public void LoadData(QuestSaveData data)
    {
        foreach (QuestParams questParams in data.savedQuestParams)
        {
            AddQuest(questParams);
        }
        foreach (AwaitingQuest awaitingQuest in data.awaitingQuests)
        {
            AwaitingQuest loadedQuest = new(awaitingQuest.questParams, awaitingQuest.delay);
            AwaitingQuests.Add(loadedQuest);
            loadedQuest.AwaitingQuestGiven += OnAwaitingQuestGiven;
            loadedQuest.Initialize();
        }
    }
    #endregion
}