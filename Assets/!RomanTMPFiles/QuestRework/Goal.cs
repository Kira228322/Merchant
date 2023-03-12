using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Goal
{

    public enum State { Active, Completed, Failed }

    [SerializeField] private State _currentState;

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

    public Goal(State currentState, string description, int currentAmount, int requiredAmount)
    {
        CurrentState = currentState;
        Description = description;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
    }

    public Goal()
    {

    }

    protected void UpdateGoal()
    {
        GoalUpdated?.Invoke(this);
    }

    protected virtual void Evaluate()
    {
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
        UpdateGoal();
    }

    public void Fail()
    {
        CurrentState = State.Failed;
    }

    public virtual void Initialize() { }
    public virtual void Deinitialize() { }

    public static T CreateInstance<T>() where T : Goal // метод-фабрика для создания наследуемых Goal
    {
        return Activator.CreateInstance(typeof(T)) as T;
    }

}
