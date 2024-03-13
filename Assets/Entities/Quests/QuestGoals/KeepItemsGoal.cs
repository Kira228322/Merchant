using System;

[Serializable]
public class KeepItemsGoal : Goal
{
    //Goal, который фейлится, как только игрок юзает требуемый предмет

    [NonSerialized] public UsableItem RequiredItem;
    public string RequiredItemName;

    public KeepItemsGoal(State currentState, string description, int currentAmount, int requiredAmount, string requiredItemName) : base(currentState, description, currentAmount, requiredAmount)
    {
        RequiredItemName = requiredItemName;
    }

    public override void Initialize()
    {
        RequiredItem = (UsableItem)ItemDatabase.GetItem(RequiredItemName);

        Player.Instance.Inventory.ItemUsed += OnItemUse;

        Evaluate();
    }

    public override void Deinitialize()
    {
        Player.Instance.Inventory.ItemUsed -= OnItemUse;
    }

    private void OnItemUse(UsableItem usedItem)
    {
        if (usedItem.Name == RequiredItemName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
    protected override void Evaluate()
    {
        if (CurrentState == State.Completed)
        {
            if (CurrentAmount >= RequiredAmount)
            {
                CurrentState = State.Failed;
                Deinitialize(); //Можно перестать считать, ведь ошибок прошлого уже не исправить
            }
        }
        UpdateGoal();
    }
}
