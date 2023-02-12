using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGoal : Goal
{
    private int _timeCounter;
    public TimedGoal(Quest quest, string description, bool isCompleted, int currentAmountHours, int requiredAmountHours)
    {
        Quest = quest;
        Description = description;
        IsCompleted = isCompleted;
        CurrentAmount = currentAmountHours;
        RequiredAmount = requiredAmountHours;
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
}
