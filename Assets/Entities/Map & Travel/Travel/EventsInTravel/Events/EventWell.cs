using System.Linq;
using UnityEngine;

public class EventWell : EventInTravel
{
    [SerializeField] private Status _wellBuff;
    private bool _haveBottle = false;
    private int _probabilityOfWater = 55;
    public override void SetButtons()
    {
        SetInfoButton($"����������� ����, ��� � ������� ����� ���� {TravelEventHandler.GetProbability(_probabilityOfWater, Player.Instance.Statistics.Luck)}%." +
                      $"\n���� ������ ������� �� ����� �����");
        foreach (var item in Player.Instance.Inventory.BaseItemList)
        {
            if (item.ItemData.Name is "������ �������" or "������� � �����")
            {
                ButtonsLabel.Add("��������� ������");
                _haveBottle = true;
                return;
            }
        }
        if (_haveBottle == false)
            ButtonsLabel.Add("��� �������");

    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (_haveBottle == false)
                    _eventWindow.ChangeDescription("� ��� �� ���� ������� ��� ���� ����� ��������� ����. �� ���������� ���� �����������.");
                else
                {
                    if (TravelEventHandler.EventFire(_probabilityOfWater, true, Player.Instance.Statistics.Luck))
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

                        foreach (var item in Player.Instance.Inventory.BaseItemList)
                        {
                            if (item.ItemData.Name == "������ �������")
                            {
                                InventoryController.Instance.DestroyItem(Player.Instance.BaseItemGrid, item);
                                InventoryController.Instance.TryCreateAndInsertItem(ItemDatabase.GetItem("������� � �����"), 1, 0);
                                _eventWindow.ChangeDescription("� ������� ��������� ���� � �� ��������� ������.");
                                return;
                            }
                        }

                        UsableItem bottle = (UsableItem)ItemDatabase.GetItem("������� � �����");
                        Player.Instance.Needs.CurrentHunger += bottle.UsableValue;
                        _eventWindow.ChangeDescription("� ������� ��������� ����, �� � ��� �� ��������� ������ �������. " +
                                                       "�� ������ ���� �� ����� �������, � ����� ��������� ��.");
                    }
                    else
                        _eventWindow.ChangeDescription("� ���������, ������� �������� ��������.");
                }
                break;
        }
    }
}
