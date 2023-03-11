using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Roman.Rework
{
    public class Quest
    {
        public enum State { Active, RewardUncollected, Completed, Failed }

        private State _currentState;

        public State CurrentState
        {
            get => _currentState;
            set
            {
                QuestChangedState?.Invoke(this);
                _currentState = value;
            }
        }

        public string QuestName;
        public string Description;

        public int ExperienceReward;
        public int MoneyReward;
        public List<ItemReward> ItemRewards;

        public QuestParams NextQuestParams;

        public List<Goal> Goals;

        public event UnityAction<Quest> QuestUpdated;
        public event UnityAction<Quest> QuestChangedState;

        [Serializable]
        public struct ItemReward
        {
            public string itemName;
            public int amount;
            public float daysBoughtAgo;
            public ItemReward(Item item, int amount, float daysBoughtAgo)
            {
                itemName = item.Name;
                this.amount = amount;
                this.daysBoughtAgo = daysBoughtAgo;
            }
        }
        [Serializable]
        public class QuestParams
        {
            public string questName;
            public string description;

            public int experienceReward;
            public int moneyReward;
            public List<ItemReward> itemRewards;

            public List<Goal> goals;
        }
        public Quest(QuestParams questParams)
        {
            CurrentState = State.Active;

            QuestName = questParams.questName;
            Description = questParams.description;

            ExperienceReward = questParams.experienceReward;
            MoneyReward = questParams.moneyReward;
            ItemRewards = questParams.itemRewards;

            Goals = questParams.goals;
        }

        public void CheckGoals()
        {
            foreach (Goal goal in Goals)
            {
                if (goal.CurrentState != Goal.State.Completed)
                {
                    if (goal.CurrentState == Goal.State.Failed)
                    {
                        Fail();
                    }
                    return;
                }
            }
            QuestUpdated?.Invoke(this);
            Complete();
        }
        
        private void Complete()
        {
            foreach (Goal goal in Goals)
            {
                goal.Deinitialize();
            }

            CurrentState = State.RewardUncollected;
        }
        private void Fail()
        {
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

    }
}
