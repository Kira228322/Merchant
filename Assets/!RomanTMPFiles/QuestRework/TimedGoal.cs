using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÷ель, котора€ проваливаетс€ по прошествии RequiredAmount часов
///Ќе путать с WaitingGoal <см. cref ="WaitingGoal" />
/// </summary>
[System.Serializable]
public class TimedGoal : Goal
{
    private int _timeCounter;
    public TimedGoal(State currentState, string description, int currentAmount, int requiredAmount) : base(currentState, description, currentAmount, requiredAmount)
    {
        //аналогичен базовому конструктору
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
        if (CurrentState == State.Completed) //TimedGoal изначально выполнен, но после истечени€ времени он фейлен
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Failed;
                Deinitialize(); //ћожно перестать считать, ведь врем€ не уйдет назад
            }

        }
        UpdateGoal();
    }
}
