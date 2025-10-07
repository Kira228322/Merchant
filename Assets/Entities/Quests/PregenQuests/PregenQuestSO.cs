using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Pre-generated Quest", menuName = "Quest/Pre-generated Quest")]
public class PregenQuestSO : ScriptableObject
{
    public QuestParams.State StartingState;
    public string QuestName;
    public string QuestSummary;

    [Tooltip("Айди нпс, который выдал квест. Используется для проверкам в " +
        "диалогах, но не влияет на ExclamationMark над головой")]
    public int QuestGiverID;

    [TextArea(2, 5)] public string Description;

    [HideInInspector] public bool RandomExp;
    [HideInInspector] public bool RandomReward;

    [HideInInspector] public int MinExperienceReward;
    [HideInInspector] public int MaxExperienceReward;
    [HideInInspector] public int MinMoneyReward;
    [HideInInspector] public int MaxMoneyReward;

    [Tooltip("Назначается автоматически в QuestLine.OnEnable, здесь это поле лучше не трогать")]
    public QuestLine QuestLine;

    [Tooltip("Те квесты, которые должны быть выполнены, чтобы этот автоматически заспавнился")]
    public List<PregenQuestSO> PrerequisiteQuests = new();

    [Tooltip("Время в часах, которое пройдет после выполнения этого квеста, перед тем как выдастся следующий")]
    public int QuestCompletionDelay;

    [HideInInspector] public List<CompactedGoal> goals = new();

    [HideInInspector] public List<ItemReward> ItemRewards = new();

    public QuestParams GenerateQuestParams()
    {
        QuestParams questParams = new()
        {
            currentState = StartingState,
            questName = QuestName,
            questSummary = QuestSummary,
            questGiverID = QuestGiverID,
            description = Description,
            questCompletionDelay = QuestCompletionDelay,
            experienceReward = Random.Range(MinExperienceReward, MaxExperienceReward + 1),
            moneyReward = Random.Range(MinMoneyReward, MaxMoneyReward + 1),
            dayStartedOn = GameTime.CurrentDay,
            hourStartedOn = GameTime.Hours,
            itemRewards = ItemRewards,
            // Инициализируем все списки целей
            collectItemsGoals = new List<CollectItemsGoal>(),
            talkToNPCGoals = new List<TalkToNPCGoal>(),
            waitingGoals = new List<WaitingGoal>(),
            timedGoals = new List<TimedGoal>(),
            giveItemsGoals = new List<GiveItemsGoal>(),
            deliveryGoals = new List<DeliveryGoal>(),
            useItemsGoals = new List<UseItemsGoal>(),
            keepItemsGoals = new List<KeepItemsGoal>(),
            stayOnSceneGoals = new List<StayOnSceneGoal>()
        };

        foreach (CompactedGoal pregenGoal in goals)
        {
            int additiveRewardItemCount;
            
            switch (pregenGoal.goalType)
            {
                case CompactedGoal.GoalType.CollectItemsGoal:
                    additiveRewardItemCount = Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount);

                    var collectGoal = new CollectItemsGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, additiveRewardItemCount, pregenGoal.RequiredItemName);

                    questParams.collectItemsGoals.Add(collectGoal);

                    if (pregenGoal.AdditiveMoneyReward)
                        questParams.moneyReward += ItemDatabase.GetItem(pregenGoal.RequiredItemName).Price * additiveRewardItemCount;
                    break;

                case CompactedGoal.GoalType.TalkToNPCGoal:
                    var talkGoal = new TalkToNPCGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount),
                        pregenGoal.RequiredIDofNPC, pregenGoal.RequiredLine, pregenGoal.FailingLine);

                    questParams.talkToNPCGoals.Add(talkGoal);
                    break;

                case CompactedGoal.GoalType.TimedGoal:
                    var timedGoal = new TimedGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount));

                    questParams.timedGoals.Add(timedGoal);
                    break;

                case CompactedGoal.GoalType.WaitingGoal:
                    var waitingGoal = new WaitingGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount));

                    questParams.waitingGoals.Add(waitingGoal);
                    break;

                case CompactedGoal.GoalType.GiveItemsGoal:
                    additiveRewardItemCount = Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount);

                    var giveGoal = new GiveItemsGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, additiveRewardItemCount,
                        pregenGoal.RequiredItemName, pregenGoal.RequiredIDofNPC, pregenGoal.RequiredLine);

                    questParams.giveItemsGoals.Add(giveGoal);

                    if (pregenGoal.AdditiveMoneyReward)
                        questParams.moneyReward += ItemDatabase.GetItem(pregenGoal.RequiredItemName).Price * additiveRewardItemCount;
                    break;

                case CompactedGoal.GoalType.DeliveryGoal:
                    var deliveryGoal = new DeliveryGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, 1, pregenGoal.RequiredIDofNPC, pregenGoal.RequiredItemCategories,
                        pregenGoal.QuestItemsBehaviour, (float)Math.Round(Random.Range(pregenGoal.MinRequiredDeliveryWeight, pregenGoal.MaxRequiredDeliveryWeight), 1),
                        Random.Range(pregenGoal.MinRequiredDeliveryCount, pregenGoal.MaxRequiredDeliveryCount), pregenGoal.RequiredRotThreshold);

                    questParams.deliveryGoals.Add(deliveryGoal);
                    break;

                case CompactedGoal.GoalType.UseItemsGoal:
                    var useGoal = new UseItemsGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount), pregenGoal.RequiredItemName);

                    questParams.useItemsGoals.Add(useGoal);
                    break;

                case CompactedGoal.GoalType.KeepItemsGoal:
                    var keepGoal = new KeepItemsGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount), pregenGoal.RequiredItemName);

                    questParams.keepItemsGoals.Add(keepGoal);
                    break;

                case CompactedGoal.GoalType.StayOnSceneGoal:
                    var sceneGoal = new StayOnSceneGoal(pregenGoal.goalState, pregenGoal.description,
                        pregenGoal.currentAmount, Random.Range(pregenGoal.minRequiredAmount, pregenGoal.maxRequiredAmount), pregenGoal.RequiredSceneName);

                    questParams.stayOnSceneGoals.Add(sceneGoal);
                    break;

                default:
                    Debug.LogError("Нет такого типа Goal");
                    break;
            }
        }

        return questParams;
    }

    [Serializable]
    public class CompactedGoal
    {
        public enum GoalType { CollectItemsGoal, TalkToNPCGoal, WaitingGoal, TimedGoal, GiveItemsGoal, DeliveryGoal, UseItemsGoal, KeepItemsGoal, StayOnSceneGoal }

        public GoalType goalType;
        public Goal.State goalState;
        public string description;
        public int currentAmount;

        public bool randomAmount;
        public int minRequiredAmount;
        public int maxRequiredAmount;
        //Опциональные поля
        public bool AdditiveMoneyReward;
        public int RequiredIDofNPC;
        public string RequiredLine;
        public string FailingLine;

        public string RequiredItemName;

        public string RequiredSceneName;

        public List<Item.ItemType> RequiredItemCategories;
        public ItemContainer.QuestItemsBehaviourEnum QuestItemsBehaviour;
        public bool RandomDeliveryWeight;
        public bool RandomDeliveryCount;
        public float MinRequiredDeliveryWeight;
        public float MaxRequiredDeliveryWeight;
        public int MinRequiredDeliveryCount;
        public int MaxRequiredDeliveryCount;
        public float RequiredRotThreshold;
    }
}