using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindSteakThenFindArrow : Quest
{
    //Смысл в том, что это цепь заданий: сначала нужно найти steak, и только после этого (пока одно steak есть в инвентаре) нужно найти стрелу

    void Start()
    {
        QuestName = "SteakThenArrowFinding";
        Description = "Find 1 steak, after that find 1 arrow";
        ExperienceReward = 100;
        MoneyReward = 69;

        Goals.Add(new CollectItemsGoal(this, "Steak", "собери 1 steak", false, 0, 1));
        Goals.Add(new CollectItemsGoal(this, "Arrow", "собери 1 стрелу", false, 0, 1));

        Goals[0].Initialize();
        //По идее, нужно инициализировать Goal2 после того, как выполнился Goal1

    }
    public override void CheckGoals()
    {
        if (!Goals[1].IsCompleted)
        {
            Goals[1].Initialize();
        }
        else Complete();

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
        */
    }
}
