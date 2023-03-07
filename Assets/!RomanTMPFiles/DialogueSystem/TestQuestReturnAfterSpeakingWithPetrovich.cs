using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestReturnAfterSpeakingWithPetrovich : Quest
{
    void Awake()
    {
        QuestName = "��������� �������";
        Description = "� ��������� � ����������. ���� ������������ �������";
        ExperienceReward = 100;
        MoneyReward = 69;
        ItemRewards.Add(new ItemReward(ItemDatabase.GetItem("Sugus"), 3, 0f));
        ItemRewards.Add(new ItemReward(ItemDatabase.GetItem("Bazugus"), 1, 0f));
        NextQuestName = null;


        Goals.Add(new TalkToNPCGoal(this, 55, 
            "returned_after_talking_about_buhlo",
            "��������� �������", 
            false, currentAmount: 0, requiredAmount: 1));

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += QuestUpdated;
        }
    }
}
