using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private void Start()
    {
        //TODO: убрать проверку двух одинаковых предметов перед релизом
        var duplicateGroups = Instance.Items.ItemList.GroupBy(item => item.Name).Where(group => group.Count() > 1);
        foreach (var group in duplicateGroups)
        {
            Debug.LogWarning($" {group.Key} содержитс€ в базе данных два раза!");
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
        List<Item> itemsOfThisType = Instance.Items.ItemList.Where(item => item.TypeOfItem == itemType).ToList();
        return itemsOfThisType[Random.Range(0, itemsOfThisType.Count)]; 
    }
    public static Item GetRandomItem()
    {
        return Instance.Items.ItemList[Random.Range(0, Instance.Items.ItemList.Count)];
    }
}
