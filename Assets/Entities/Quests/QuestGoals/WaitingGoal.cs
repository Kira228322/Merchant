using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Цель, которая выполняется по прошествии RequiredAmount часов
///Не путать с TimedGoal <см. cref ="TimedGoal" />
/// </summary>
[System.Serializable]
public class WaitingGoal : Goal
{
    private int _timeCounter;
    public WaitingGoal(State currentState, string description, int currentAmount, int requiredAmount) : base(currentState, description, currentAmount, requiredAmount)
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

        if (CurrentState == State.Active)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Completed;
                Deinitialize(); //Можно перестать считать, ведь время не уйдет назад
            }
        }

        UpdateGoal();
    }
}