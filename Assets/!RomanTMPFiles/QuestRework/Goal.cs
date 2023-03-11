using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Roman.Rework
{
    [Serializable]
    public class Goal
    {

        public enum State { Active, Completed, Failed}

        private State _currentState;

        public State CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                GoalChangedState?.Invoke(this);
            }
        }
        public string Description;
        public int CurrentAmount;
        public int RequiredAmount;


        public event UnityAction<Goal> GoalUpdated;
        public event UnityAction<Goal> GoalChangedState;


        [Serializable]
        public class GoalParams
        {
            public string description;
            public State currentState;
            public int currentAmount;
            public int requiredAmount;
        }
        
        public Goal(GoalParams goalParams)
        {
            Description = goalParams.description;
            CurrentState = goalParams.currentState;
            CurrentAmount = goalParams.currentAmount;
            RequiredAmount = goalParams.requiredAmount;
        }

        protected void UpdateGoal()
        {
            GoalUpdated?.Invoke(this);
        }

        protected virtual void Evaluate()
        {
            UpdateGoal();

            if (CurrentState == State.Active)
            {
                if (CurrentAmount >= RequiredAmount)
                {
                    CurrentState = State.Completed;
                }
            }
            else if (CurrentState == State.Completed)  //Развыполнить. Пример: Было три яблока из трёх => Выполнился.
                                                       //Чел выкинул одно => Развыполнился.
            {
                if (CurrentAmount < RequiredAmount)
                {
                    CurrentState = State.Active;
                }
            }
        }

        public void Fail()
        {
            CurrentState = State.Failed;
        }

        public virtual void Initialize()
        {

        }
        public virtual void Deinitialize()
        {

        }

    }
}
