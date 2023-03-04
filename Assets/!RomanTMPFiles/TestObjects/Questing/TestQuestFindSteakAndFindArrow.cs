using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindSteakAndFindArrow : Quest
{

    void Awake()
    {
        QuestName = "SteakThenArrowFinding";
        Description = "Find 1 steak and find 1 arrow";
        ExperienceReward = 100;
        MoneyReward = 69;

        Goals.Add(new CollectItemsGoal(this, "Steak", "собери 1 steak", false, 0, 1));
        Goals.Add(new CollectItemsGoal(this, "Arrow", "собери 1 стрелу", false, 0, 1));

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += QuestUpdated;
        }
    }
    /*public override void CheckGoals()
    {
        
        /* Отказался от этого подхода развыполнения, он громоздкий и глупый.
         * Задумка была в том, что внутри одного квеста нужно сначала сделать X, потом сделать Y
         * Пока не сделан X, нельзя сделать Y
         * Лучше делать через 2 связанных квеста: сделайте X, следующий квест сделайте Y
         
          if (!Goals[1].IsCompleted)
        {
            Goals[1].Initialize();
        }
        else Complete();
        */
        /* Для двух шагов всё просто: Если вызвали CheckGoals, значит какой то Goal точно выполнился. Если это был не второй, значит был первый 
         Норм, но если будет длинная цепь шагов то это ведет к большому нагромождению, например так для трёх шагов:
         if (!Goals[2].IsCompleted && Goals[1].IsCompleted) 
        {
            Goals[2].Initialize();
        }
        else if (!Goals[1].IsCompleted)
        {
            Goals[1].Initialize();
        }
        else Complete();
        
      } */
}
