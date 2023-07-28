using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Quest
{
    public enum State { Active, RewardUncollected, Completed, Failed }

    private State _currentState;

    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            QuestChangedState?.Invoke(this);
        }
    }

    public string QuestName;
    public string QuestSummary;
    public string Description;

    public NpcData QuestGiver;

    public int ExperienceReward;
    public int MoneyReward;
    public List<ItemReward> ItemRewards;

    public QuestParams NextQuestParams;

    public List<Goal> Goals;

    public QuestPanel questPanel = null; //она сама себя назначит

    public event UnityAction<Quest> QuestUpdated;
    public event UnityAction<Quest> QuestChangedState;

    public Quest(QuestParams questParams)
    {
        CurrentState = (State)questParams.currentState;

        QuestName = questParams.questName;
        QuestSummary = questParams.questSummary;
        Description = questParams.description;

        if (questParams.questGiverID != 0)
            QuestGiver = NpcDatabase.GetNPCData(questParams.questGiverID);

        ExperienceReward = questParams.experienceReward;
        MoneyReward = questParams.moneyReward;
        ItemRewards = questParams.itemRewards;

        NextQuestParams = questParams.nextQuestParams;

        Goals = questParams.goals;

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += OnGoalUpdated;
        }
        CheckGoals();
    }

    public void CheckGoals()
    {
        QuestUpdated?.Invoke(this);

        bool allComplete = Goals.All(goal => goal.CurrentState == Goal.State.Completed);

        if (allComplete)
        {
            Complete();
        }
        else if (Goals.Any(goal => goal.CurrentState == Goal.State.Failed))
        {
            Fail();
        }
    }

    public bool HasRewards()
    {
        return ExperienceReward != 0 || MoneyReward != 0 || ItemRewards.Count != 0;
    }

    private void Complete()
    {
        foreach (Goal goal in Goals)
        {
            goal.Deinitialize();
        }
        if (HasRewards()) //Если есть хоть какие-то награды
        CurrentState = State.RewardUncollected;
        else
        {
            CurrentState = State.Completed;
        }
    }
    private void Fail()
    {
        foreach (Goal goal in Goals)
        {
            goal.Deinitialize();
        }

        CurrentState = State.Failed;
    }

    public void CompleteManually()
    {
        foreach (Goal goal in Goals)
        {
            goal.CurrentAmount = goal.RequiredAmount;
        }
        CheckGoals();
    }

    public void GiveReward()
    {
        Player.Instance.AddExperience(ExperienceReward);
        Player.Instance.Money += MoneyReward;

        if (ItemRewards.Count != 0)
        {
            foreach (var item in ItemRewards)
            {
                InventoryController.Instance.TryCreateAndInsertItem(Player.Instance.Inventory.ItemGrid, ItemDatabase.GetItem(item.itemName), item.amount, item.daysBoughtAgo, true);
            }
        }

        CurrentState = State.Completed;
    }
    private void OnGoalUpdated(Goal goal)
    {
        CheckGoals();
    }

}