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
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }
    private void Complete()
    {
        IsCompleted = true;
        Quest.CheckGoals();
        Debug.Log("Goal completed");
    }
    public virtual void Initialize()
    {

    }
}
