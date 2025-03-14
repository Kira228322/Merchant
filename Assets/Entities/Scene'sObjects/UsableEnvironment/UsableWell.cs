using System.Linq;
using UnityEngine;

public class UsableWell : UsableEnvironment
{
    [SerializeField] private Status _wellBuff;
    protected override bool IsFunctionalComplete()
    {
        foreach (var item in Player.Instance.Inventory.BaseItemList)
        {
            if (item.ItemData.Name == "������ �������")
            {
                if (StatusManager.Instance.ActiveStatuses.FirstOrDefault(status =>
                        status.StatusData.StatusName == "��������� �������") != null)
                {
                    int exp = Random.Range(3, 5);
                    Player.Instance.Experience.AddExperience(exp);
                    CanvasWarningGenerator.Instance.CreateWarning("�������� ������� �������", $"�� �������� {exp} �����");
                }
                if (TravelEventHandler.EventFire(20, true, Player.Instance.Statistics.Luck))
                {
                    StatusManager.Instance.AddStatusForPlayer(_wellBuff);
                }
                InventoryController.Instance.DestroyItem(Player.Instance.BaseItemGrid, item);
                InventoryController.Instance.TryCreateAndInsertItem(ItemDatabase.GetItem("������� � �����"), 1, 0);
                return true;
            }
        }

        return false;
    }
}
