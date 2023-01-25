using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindApples : Quest
{
    void Awake()
    {
        QuestName = "Apple Finding";
        Description = "Find apples, ok?";
        ExperienceReward = 100;
        MoneyReward = 69;
        ItemRewards = new()
        {
            { ItemDatabase.GetItem("Sugoma"), 1 },
            { ItemDatabase.GetItem("Arrow"), 3 }
        };
        NextQuestName = "TestQuestFindSteakAndFindArrow";


        Goals.Add(new CollectItemsGoal(this, "Sugus", "собери 3 €блока", false, 0, 3)); 

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += QuestUpdated;
        }
    }

}
