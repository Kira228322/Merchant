using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public Quest Quest { get; set; } //к какому квесту принадлежит эта цель
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public void Evaluate()
    {
        if (!IsCompleted)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                Complete();
            }
        }
    }
    private void Complete()
    {
        IsCompleted = true;
        Debug.Log("Goal completed");
        Quest.CheckGoals();
    }
    public virtual void Initialize()
    {

    }
    public virtual void Deinitialize()
    {

    }
}
