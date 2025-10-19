using System.Collections.Generic;

[System.Serializable]
public class QuestParams
{
    public enum State { Active, RewardUncollected, Completed, Failed }
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

    // Отдельные списки для каждого типа целей
    public List<CollectItemsGoal> collectItemsGoals = new();
    public List<TalkToNPCGoal> talkToNPCGoals = new();
    public List<WaitingGoal> waitingGoals = new();
    public List<TimedGoal> timedGoals = new();
    public List<GiveItemsGoal> giveItemsGoals = new();
    public List<DeliveryGoal> deliveryGoals = new();
    public List<UseItemsGoal> useItemsGoals = new();
    public List<KeepItemsGoal> keepItemsGoals = new();
    public List<StayOnSceneGoal> stayOnSceneGoals = new();
    
    public List<Goal> AllGoals
    {
        get
        {
            List<Goal> allGoals = new List<Goal>();
            allGoals.AddRange(collectItemsGoals);
            allGoals.AddRange(talkToNPCGoals);
            allGoals.AddRange(waitingGoals);
            allGoals.AddRange(timedGoals);
            allGoals.AddRange(giveItemsGoals);
            allGoals.AddRange(deliveryGoals);
            allGoals.AddRange(useItemsGoals);
            allGoals.AddRange(keepItemsGoals);
            allGoals.AddRange(stayOnSceneGoals);

            allGoals.Sort((a, b) => a.IntendedIndex.CompareTo(b.IntendedIndex));

            return allGoals;
        }
    }
}