using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestsSaveData
{
    public List<string> activeQuestTypes = new();
    public List<string> rewardUncollectedQuestTypes = new();
    public List<SavedQuestPanel> finishedQuestPanels = new();

    [Serializable]
    public class SavedQuestPanel
    {
        public string questName;
        public string description;
        public List<string> goals = new();
        public List<string> rewards = new();

        public SavedQuestPanel(QuestPanel panel)
        {
            questName = panel.QuestName;
            description = panel.QuestDescription;
            for (int i = 0; i < panel.GoalCount; i++) 
            {
                goals.Add(panel.GoalTexts[i].text);
            }
            for (int i = 0; i < panel.RewardCount; i++)
            {
                goals.Add(panel.RewardTexts[i].text);
            }
        }
    }
}
