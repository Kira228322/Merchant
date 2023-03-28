using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public ItemDatabaseSO Items;
    private static ItemDatabase Instance; //—инглтон приватный, потому что обращение с базой только через методы GetItem (статические, это важно),
                                           //другим челам не нужен доступ именно к синглтону

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
        
        Debug.LogWarning("“акого айтема не существует!");
        return null;
    }

    public static Item GetRandomItemOfThisType(Item.ItemType itemType)
    {
        List<Item> itemsOfThisType = Instance.Items.ItemList.Where(item => item.TypeOfItem == itemType).ToList();
        return itemsOfThisType[Random.Range(0, itemsOfThisType.Count)]; 
    }
}
