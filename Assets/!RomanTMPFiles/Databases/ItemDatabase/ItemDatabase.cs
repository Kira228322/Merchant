using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public ItemDatabaseSO Items;
    private static ItemDatabase Singleton; //—инглтон приватный, потому что обращение с базой только через методы GetItem (статические, это важно),
                                           //другим челам не нужен доступ именно к синглтону

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
        }
    }

    public static Item GetItem(string name)
    {
        Item result = Singleton.Items.ItemList.FirstOrDefault(item => item.Name == name);
        //¬ыражение Linq, аналогичное foreach (var item in itemlist) { if (item.Name == name) return item; else return null; } 
        if (result != null)
        {
            return result;
        }
        
        Debug.LogWarning("“акого айтема не существует!");
        return null;
        
    }
}
