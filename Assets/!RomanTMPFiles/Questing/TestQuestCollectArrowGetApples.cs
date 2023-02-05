using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestCollectArrowGetApples : Quest
{
    void Awake()
    {
        QuestName = "Arrow Finding";
        Description = "1 arrow is what I need";
        ExperienceReward = 100;
        MoneyReward = 69;
        ItemRewards.Add(new ItemReward(ItemDatabase.GetItem("Sugus"), 3, 0f));
        ItemRewards.Add(new ItemReward(ItemDatabase.GetItem("Bazugus"), 1, 0f));
        NextQuestName = null;


        Goals.Add(new CollectItemsGoal(this, "Arrow", "собери 1 стрелу", false, currentAmount: 0, requiredAmount: 1));

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += QuestUpdated;
        }
    }

}

