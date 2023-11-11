using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGraveyard : EventInTravel
{
    [SerializeField] private Item _item;
    [SerializeField] private Status _curse;

    private int _probabilityNotCursed = 15;
    public override void SetButtons()
    {
        ButtonsLabel.Add("����� ��������");
        ButtonsLabel.Add("����� ������");
        SetInfoButton($"����������� ����, ��� �������� �� ����� �������� " +
                      $"{TravelEventHandler.GetProbability(_probabilityNotCursed, Player.Instance.Statistics.Luck)}%." +
                      $"\n���� ������ ������� �� ����� �����");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (TravelEventHandler.EventFire(_probabilityNotCursed, true, Player.Instance.Statistics.Luck))
                {
                    if (InventoryController.Instance.TryCreateAndInsertItem
                            (_item, 1, 0) != null)
                    {
                        _eventWindow.ChangeDescription("��� ���������� ������� � ��� ������ ���-�� ����� ��������� �������� �� ��������. �� ���������� ���.");
                    }
                    else
                    {
                        _eventWindow.ChangeDescription("� ��� �� ���� ����� ��� �������� � �� ������ �� ����������� � ���� �������� �����");
                    }
                }
                else
                {
                    StatusManager.Instance.AddStatusForPlayer(_curse);
                    _eventWindow.ChangeDescription("�������� ��������� ��������! �� ����� ��� � ���� � ��� ��� ��� �� �����������. " +
                                                   "�� ������� ������������� �����. ������ �� ����������.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("�� ������ �������� ��� ��������� ����� � �����.");
                break;
        }
    }
}
