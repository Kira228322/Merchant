using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestFindApples : Quest
{
    [SerializeField] private Item TEST_requiredItemType; //��� ���� �������, � ������� ������ ����� ����. 

    void Start()
    {
        QuestName = "Apple Finding";
        Description = "Find apples, ok?";
        ExperienceReward = 100;
        MoneyReward = 69;

        Goals.Add(new CollectItemsGoal(this, TEST_requiredItemType, "������ 3 ������", false, 0, 3)); 

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
        }
    }

}
