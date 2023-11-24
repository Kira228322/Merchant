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
            State oldState = _currentState;
            _currentState = value;
            QuestChangedState?.Invoke(this, oldState, _currentState);

        }
    }

    public string QuestName;
    public string QuestSummary;
    public string Description;

    public int QuestGiverID;

    public int ExperienceReward;
    public int MoneyReward;
    public List<ItemReward> ItemRewards;

    public List<Goal> Goals;

    public QuestPanel questPanel = null; //она сама себ€ назначит

    public int DayStartedOn;
    public int HourStartedOn;

    public int DayFinishedOn;  
    public int HourFinishedOn;

    public event UnityAction<Quest> QuestUpdated;
    public event UnityAction<Quest, State, State> QuestChangedState;

    public Quest(QuestParams questParams)
    {
        _currentState = (State)questParams.currentState; //в конструкторе не происходит Invoke QuestChangedState

        QuestName = questParams.questName;
        QuestSummary = questParams.questSummary;
        Description = questParams.description;

        QuestGiverID = questParams.questGiverID;

        ExperienceReward = questParams.experienceReward;
        MoneyReward = questParams.moneyReward;
        ItemRewards = questParams.itemRewards;

        DayStartedOn = questParams.dayStartedOn;
        HourStartedOn = questParams.hourStartedOn;

        DayFinishedOn = questParams.dayFinishedOn;
        HourFinishedOn = questParams.hourFinishedOn;

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
        if (HasRewards() && CurrentState != State.Completed) //≈сли есть хоть какие-то награды
            CurrentState = State.RewardUncollected;
        else
            CurrentState = State.Completed;

        DayFinishedOn = GameTime.CurrentDay;
        HourFinishedOn = GameTime.Hours;
    }
    private void Fail()
    {
        foreach (Goal goal in Goals)
        {
            goal.Deinitialize();
        }

        CurrentState = State.Failed;

        DayFinishedOn = GameTime.CurrentDay;
        HourFinishedOn = GameTime.Hours;
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
                InventoryController.Instance.TryCreateAndInsertItem(ItemDatabase.GetItem(item.itemName), item.amount, item.daysBoughtAgo);
            }
        }

        CurrentState = State.Completed;
    }
    private void OnGoalUpdated(Goal goal)
    {
        CheckGoals();
    }
    public bool IsNpcSourceOfQuest(int ID)
        //Ќужно дл€ проверки на активные квесты, чтобы пон€ть относитс€ ли этот квест к этому Ќѕ—.
        //Ќо не св€зано с восклицательным знаком, потому что то что он выдал этот квест ещЄ не €вл€етс€
        //основанием того что с ним об€зательно надо взаимодействовать
    {
        if (QuestGiverID == ID)
            return true;
        return false;
    }
    public bool IsNpcTargetOfQuest(int ID) 
        //«аслуживает ли NPC с этим айди, чтобы над ним горел восклицательный знак
    {
        return Goals.Any(goal =>
        {
            if (goal is TalkToNPCGoal talkToNpcGoal)
            {
                return talkToNpcGoal.RequiredIDOfNPC == ID;
            }
            if (goal is GiveItemsGoal giveItemsGoal)
            {
                return giveItemsGoal.RequiredIDOfNPC == ID;
            }
            if (goal is DeliveryGoal deliveryGoal)
            {
                return deliveryGoal.RequiredIDOfNPC == ID;
            }
            return false;
        });
    }

}