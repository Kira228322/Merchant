using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindApples : Quest
{
    [SerializeField] private Item TEST_requiredItemType; //что надо собрать, в будущем сделаю через айди. 

    void Start()
    {
        QuestName = "Apple Finding";
        Description = "Find apples, ok?";
        ExperienceReward = 100;
        MoneyReward = 69;

        Goals.Add(new CollectItemsGoal(this, TEST_requiredItemType, "собери 3 яблока", false, 0, 3)); 

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
        }
    }

}
