[System.Serializable]
public class ItemReward
{
    public string itemName;
    public int amount;
    public float daysBoughtAgo;
    public ItemReward(string itemName, int amount, float daysBoughtAgo)
    {
        this.itemName = itemName;
        this.amount = amount;
        this.daysBoughtAgo = daysBoughtAgo;
    }
}
