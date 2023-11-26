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

    public int questCompletionDelay;

    public int questGiverID;

    public int dayStartedOn;
    public int hourStartedOn;

    public int dayFinishedOn;
    public int hourFinishedOn;

    public int experienceReward;
    public int moneyReward;
    public List<ItemReward> itemRewards;

    public List<Goal> goals;
}
