using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;

public class ItemDatabase : MonoBehaviour
{
    public ItemDatabaseSO Items;
    public static ItemDatabase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static Item GetItem(string name)
    {
        Item result = Instance.Items.ItemList.FirstOrDefault(item => item.Name.ToLower() == name.ToLower());
        //¬ыражение Linq, аналогичное foreach (var item in itemlist) { if (item.Name == name) return item; else return null; } 
        if (result != null)
        {
            return result;
        }
        //≈сли не получилось достать из обычного листа, значит это или квестовый или несуществующий предмет.
        //»з листа квестовых предметов они будут доставатьс€ например при загрузке игры -- если у игрока в инвентаре лежал квестовый предмет
        result = Instance.Items.QuestItemList.FirstOrDefault(item => item.Name.ToLower() == name.ToLower());
        if (result != null)
        {
            return result;
        }
        Debug.LogWarning($"“акого айтема {name} не существует!");
        return null;
    }

    public static Item GetRandomItemOfThisType(Item.ItemType itemType)
    {
        return GetRandomItems(Instance.Items.ItemList.Where(item => item.TypeOfItem == itemType).ToList(), 1)[0];
    }
    public static List<Item> GetRandomItemsOfThisType(Item.ItemType itemType, int amount)
    {
        return GetRandomItems(Instance.Items.ItemList.Where(item => item.TypeOfItem == itemType).ToList(), amount);
    }
    public static Item GetRandomItem()
    {
        return GetRandomItems(Instance.Items.ItemList, 1)[0];
    }
    public static List<Item> GetRandomUnbannedItems(int amount)
    {
        return GetRandomItems(Instance.Items.ItemList.Except(BannedItemsHandler.Instance.BannedItems).ToList(), amount);
    }
    public static List<Item> GetRandomUnbannedItemsOfThisType(Item.ItemType itemType, int amount)
    {
        return GetRandomItems(Instance.Items.ItemList.Except(BannedItemsHandler.Instance.BannedItems).Where(item => item.TypeOfItem == itemType).ToList(), amount);
    }
    private static List<Item> GetRandomItems(List<Item> itemList, int amount)
    {
        if (amount == 1)
        {
            return new() { itemList[Random.Range(0, itemList.Count)] };
        }
        List<Item> result = new();
        List<int> indices = Enumerable.Range(0, itemList.Count).ToList();
        indices.Shuffle();
        foreach (int index in indices.Take(amount))
        {
            result.Add(itemList[index]);
        }
        return result;
    }
}
