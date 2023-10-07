using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UsableWell : UsableEnvironment
{
    [SerializeField] private Status _wellBuff;
    protected override bool IsFunctionalComplete()
    {
        foreach (var item in Player.Instance.Inventory.ItemList)
        {
            if (item.ItemData.Name == "Пустая бутылка")
            {
                if (StatusManager.Instance.ActiveStatuses.FirstOrDefault(status =>
                        status.StatusData.StatusName == "Всезнание колодца") != null)
                {
                    int exp = Random.Range(3, 5);
                    Player.Instance.Experience.AddExperience(exp);
                    CanvasWarningGenerator.Instance.CreateWarning("Мудрость старого колодца", $"Вы получили {exp} опыта");
                }
                if (TravelEventHandler.EventFire(20, true, Player.Instance.Statistics.Luck))
                {
                    StatusManager.Instance.AddStatusForPlayer(_wellBuff);
                }
                InventoryController.Instance.DestroyItem(Player.Instance.ItemGrid, item);
                InventoryController.Instance.TryCreateAndInsertItem(
                    Player.Instance.ItemGrid,
                    ItemDatabase.GetItem("Бутылка с водой"),
                    1, 0, true);
                return true;
            }
        }

        return false;
    }
}
