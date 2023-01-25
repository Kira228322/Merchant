using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Quest: MonoBehaviour
{
    public string NextQuestName;
    public List<Goal> Goals { get; set; } = new();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public int MoneyReward { get; set; }
    public Dictionary<Item, int> ItemRewards { get; set; } = new();
    public bool IsCompleted { get; set; }

    public QuestPanel questPanel = null; //она сама себя назначит

    public event UnityAction<Quest> QuestUpdatedEvent;
    public event UnityAction<Quest> QuestCompletedEvent;

    public void CheckGoals()
    {
        foreach (Goal goal in Goals)
        {
            if (!goal.IsCompleted)
            {
                return;
            }
        }
        QuestUpdatedEvent?.Invoke(this);
        Complete();

    }
    protected void Complete()
    {
        IsCompleted = true;
        //GiveReward(); //Пусть QuestHandler выдает награду потом мб а не здесь? А нахуя?

        foreach (Goal goal in Goals)
        {
            goal.Deinitialize();
        }
        QuestCompletedEvent?.Invoke(this);
    }

    public virtual void GiveReward()
    {
        Player.Singleton.AddExperience(ExperienceReward);
        Player.Singleton.Money += MoneyReward;
        if (ItemRewards.Count != 0)
        foreach (var item in ItemRewards)
        {
            InventoryController.Instance.TryCreateAndInsertItem(Player.Singleton.Inventory.GetComponent<ItemGrid>(), item.Key, item.Value, 0, true);
        }
    }
    protected void QuestUpdated()
    {
        QuestUpdatedEvent?.Invoke(this);
    }
}
