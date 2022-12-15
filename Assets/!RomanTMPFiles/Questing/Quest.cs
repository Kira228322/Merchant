using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest
{
    private Quest _nextQuest;
    public List<Goal> Goals { get; set; } = new();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public int MoneyReward { get; set; }
    public bool IsCompleted { get; set; }

    public void CheckGoals()
    {
        foreach (var goal in Goals)
        {
            if (!goal.IsCompleted)
            {
                break;
            }
        }
        Complete();

    }
    private void Complete()
    {
        IsCompleted = true;
        GiveReward();
    }

    private void GiveReward()
    {
        Player.Singleton.AddExperience(ExperienceReward);
        Player.Singleton.Money += MoneyReward;
    }
}
