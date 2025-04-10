using System;
using UnityEngine;
using UnityEngine.Events;

//������ �� ������ Goal �����������? ������ ��� �� ������ �����������������, � ����, ����������� �� �����������, ��������������� ������.
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
            GoalUpdated?.Invoke(this);
        }
    }
    public string Description;
    public int CurrentAmount;
    public int RequiredAmount;


    public event UnityAction<Goal> GoalUpdated;

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
        else if (CurrentState == State.Completed)  //������������. ������: ���� ��� ������ �� ��� => ����������.
                                                   //��� ������� ���� => �������������.
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

    public virtual void Initialize() { }
    public virtual void Deinitialize() { }

}
