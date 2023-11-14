using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWagonTrade : EventInTravel
{
    [SerializeField] private List<Item> _cheapItems;
    [SerializeField] private List<Item> _expensiveItems;

    private int RareItemprobability = 39;
    public override void SetButtons()
    {
        RareItemprobability += Random.Range(0, 6);
        ButtonsLabel.Add("������ ����� � ��������");
        ButtonsLabel.Add("���������� �� �����������");
        SetInfoButton($"� �������� � ������������ {TravelEventHandler.GetProbability(RareItemprobability, Player.Instance.Statistics.Diplomacy)}% " +
                      $"�������� ������ ������� \n���� ������ ������� �� ����� ����������");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < 100)
                {
                    _eventWindow.ChangeDescription("� ��� ���� �� ���������� ������. �������� ��������� �� ��� � ���������!");
                    return;
                }

                Item item;
                if (TravelEventHandler.EventFire(RareItemprobability, true, Player.Instance.Statistics.Diplomacy))
                {
                    item = _expensiveItems[Random.Range(0, _expensiveItems.Count)];
                    if (InventoryController.Instance.TryCreateAndInsertItem
                            (item, 1, 0) != null)
                    {
                        _eventWindow.ChangeDescription($"������ �������� ������ �����: {item.Name}. �� � ������������� ��������� ��� ����, ����� �������� ��� 100 �������");
                        Player.Instance.Money -= 100;
                    }
                    else
                    {
                        _eventWindow.ChangeDescription($"������ �������� ������ �����: {item.Name}. �� � ��� �� ���� ����� � ���������, ����� ��������� ���. �������� ����� �������� ���� ����� ����. (�� ������ �� ���������)");
                    }
                }
                else
                {
                    item = _cheapItems[Random.Range(0, _cheapItems.Count)];
                    if (InventoryController.Instance.TryCreateAndInsertItem
                            (item, 1, 0) != null)
                    {
                        _eventWindow.ChangeDescription($"������ ������ �� ������ �����: {item.Name}. �� � �������������� ��������� ��� ����, ����� �������� ��� ������� 100 �������");
                        Player.Instance.Money -= 100;
                    }
                    else
                    {
                        _eventWindow.ChangeDescription($"������ ������ �� ������ �����: {item.Name}. �� � ��� �� ���� ����� � ���������, ����� ��������� ���. �������� ����� �������� ���� ����� ����. (�� ������ �� ���������)");
                    }
                }
                
                break;
            case 1:
                _eventWindow.ChangeDescription("������� ������� ������ �������������� �������� �� ����� ��������...");
                break;
        }
        
    }
}
