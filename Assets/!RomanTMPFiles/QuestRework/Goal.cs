using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Goal
{

    public enum State { Active, Completed, Failed }

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

    public Goal(State currentState, string description, int currentAmount, int requiredAmount)
    {
        CurrentState = currentState;
        Description = description;
        CurrentAmount = currentAmount;
        RequiredAmount = requiredAmount;
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
        else if (CurrentState == State.Completed)  //������������. ������: ���� ��� ������ �� ��� => ����������.
                                                   //��� ������� ���� => �������������.
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

    public virtual void Initialize()
    {

    }
    public virtual void Deinitialize()
    {

    }

}
