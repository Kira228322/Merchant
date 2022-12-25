using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal
{
    public Quest Quest { get; set; } //к какому квесту принадлежит эта цель
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public event UnityAction GoalUpdated;

    public void Evaluate()
    {
        GoalUpdated?.Invoke();
        if (!IsCompleted)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                Complete();
            }
        }
        else if (CurrentAmount < RequiredAmount)
        {
            Uncomplete();
        }
    }
    private void Complete()
    {
        IsCompleted = true;
        Debug.Log("Goal completed");
        Quest.CheckGoals();
    }
    private void Uncomplete()
    {
        IsCompleted = false;
        Debug.Log("Goal uncompleted");
        Quest.CheckGoals();
    }
    public virtual void Initialize()
    {

    }
    public virtual void Deinitialize()
    {

    }
}
