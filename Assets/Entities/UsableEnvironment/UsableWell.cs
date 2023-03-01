using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableWell : UsableEnvironment
{
    protected override bool IsFunctionalComplete()
    {
        foreach (var item in Player.Instance.Inventory.ItemList)
        {
            if (item.ItemData.Name == "Empty bottle")
            {
                //заменить предмет "Empty bottle" на "Water bottle" через БД 
                InventoryController.Instance.DestroyItem(Player.Instance.ItemGrid, item);
                InventoryController.Instance.TryCreateAndInsertItem(
                    Player.Instance.ItemGrid,
                    ItemDatabase.GetItem("WaterBottle"),
                    1, 0, true);
                return true;
            }
        }

        return false;
    }
}
