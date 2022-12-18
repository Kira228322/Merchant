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
        else if (CurrentAmount < RequiredAmount) //–азвыполнение цели: ≈сли например квест состоит из двух целей
                                                 //1) собрать 3 €блока
                                                 //2) поговорить с челом (ну типа отнести ему 3 €блока)
                                                 //“о если собрал 3 €блока а потом одно выкинул то нельз€ ему их принести
                                                            
        {
            IsCompleted = false; //¬ызвать CheckGoals, в нЄм предусмотреть деинициализацию?  онечно, только в тех Quest, где это нужно.
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
}
