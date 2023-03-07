using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestTalkToPetrovichAboutBuhlo : Quest
{
    void Awake()
    {
        QuestName = "Поговорить с Петровичем";
        Description = "Торговец хочет, чтобы я передал его слова насчет попойки Петровичу";
        ExperienceReward = 100;
        MoneyReward = 69;
        NextQuestName = "TestQuestReturnAfterSpeakingWithPetrovich";


        Goals.Add(new TalkToNPCGoal(this, 33, "talked_about_buhlo", "Поговори с Петровичем", false, currentAmount: 0, requiredAmount: 1));

        foreach (Goal goal in Goals)
        {
            goal.Initialize();
            goal.GoalUpdated += QuestUpdated;
        }
    }
}
