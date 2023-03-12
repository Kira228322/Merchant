using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����, ������� ������������� �� ���������� RequiredAmount �����
///�� ������ � WaitingGoal <��. cref ="WaitingGoal" />
/// </summary>
[System.Serializable]
public class TimedGoal : Goal
{
    private int _timeCounter;
    public TimedGoal(State currentState, string description, int currentAmount, int requiredAmount) : base(currentState, description, currentAmount, requiredAmount)
    {
        //���������� �������� ������������
    }
    public override void Initialize()
    {
        Evaluate();

        GameTime.MinuteChanged += OnMinuteChanged;
    }
    public override void Deinitialize()
    {
        GameTime.MinuteChanged -= OnMinuteChanged;
    }

    private void OnMinuteChanged()
    {
        _timeCounter++;
        if (_timeCounter >= 60)
        {
            _timeCounter = 0;
            CurrentAmount++;
            Evaluate();
        }
    }

    protected override void Evaluate()
    {
        if (CurrentState == State.Completed) //TimedGoal ���������� ��������, �� ����� ��������� ������� �� ������
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Failed;
                Deinitialize(); //����� ��������� �������, ���� ����� �� ����� �����
            }

        }
        UpdateGoal();
    }
}
