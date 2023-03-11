using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Цель, которая проваливается по прошествии RequiredAmount часов
///Не путать с WaitingGoal <см. cref ="WaitingGoal" />
/// </summary>
[System.Serializable]
public class TimedGoal : Goal
{
    private int _timeCounter;
    public TimedGoal(State currentState, string description, int currentAmount, int requiredAmount) : base(currentState, description, currentAmount, requiredAmount)
    {

    }

    public override void Initialize()
    {
        base.Initialize();

        Evaluate();

        GameTime.MinuteChanged += OnMinuteChanged;
    }
    public override void Deinitialize()
    {
        base.Deinitialize();

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
        if (CurrentState == State.Completed) //TimedGoal изначально выполнен, но после истечения времени он фейлен
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Failed;
                Deinitialize(); //Можно перестать считать, ведь время не уйдет назад
            }

        }
        UpdateGoal();
    }

}
