using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventWell : EventInTravel
{
    [SerializeField] private Status _wellBuff;
    private bool _haveBottle = false;
    private int _probabilityOfWater = 40;
    public override void SetButtons()
    {
        SetInfoButton($"����������� ����, ��� � ������� ����� ���� {TravelEventHandler.GetProbability(_probabilityOfWater, Player.Instance.Statistics.Luck)}%." +
                      $"\n���� ������ ������� �� ����� �����");
        foreach (var item in Player.Instance.Inventory.BaseItemList)
        {
            if (item.ItemData.Name == "������ �������" || item.ItemData.Name == "������� � �����")
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
