using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestParams
{
    public enum State { Active, RewardUncollected, Completed, Failed}
    public State currentState;

    public string questName;
    public string questSummary;
    public string description;

    public int questGiverID;

    public int experienceReward;
    public int moneyReward;
    public List<ItemReward> itemRewards;

    public List<Goal> goals;

    public QuestParams nextQuestParams;
}
