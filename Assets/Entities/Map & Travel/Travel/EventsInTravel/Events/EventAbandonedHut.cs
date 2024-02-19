using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAbandonedHut : EventInTravel
{
    [SerializeField] private List<Item> _loot;
    [SerializeField] private List<Item> _rareLoot;
    
    private int _probabilityOfGoodResult = 50;
    public override void SetButtons()
    {
        ButtonsLabel.Add("����� ������");
        ButtonsLabel.Add("�� ���������");
        SetInfoButton("� ������ ��� �� ����� ����� ��������� � ������������ " +
                      $"{TravelEventHandler.GetProbability(_probabilityOfGoodResult, Player.Instance.Statistics.Luck)}%." +
                      "\n���� ������ ������� �� ����� �����");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (TravelEventHandler.EventFire(_probabilityOfGoodResult, true, Player.Instance.Statistics.Luck))
                {
                    Loot();
                }
                else
                {
                    int stoleMoney = Player.Instance.Money / 2 + Player.Instance.Money % 2;
                    if (stoleMoney < 20)
                        stoleMoney = Player.Instance.Money;
                    if (stoleMoney > 200)
                        stoleMoney = 200;
                    Player.Instance.Money -= stoleMoney;
                    _eventWindow.ChangeDescription("������ �������� ���������. �� ��� ������ ��� " +
                                                   "������� ������, ������� ���� � ���� ������. �� ������� ���, ����� " + stoleMoney + " ������.");
                }

                break;
            case 1:
                _eventWindow.ChangeDescription("�� ������ �� ��������� � ������� ������.");
                break;
        }
    }

    private void Loot()
    {
        int countOfCommonLoot = Random.Range(3, 5);
        int money = Random.Range(3, 8);
        string loot = "";
        string missedLoot = "";
        Item item;
        
        for (int i = 0; i < countOfCommonLoot - 1; i++)
        {
            item = _loot[Random.Range(0, _loot.Count)];
            if (InventoryController.Instance.TryCreateAndInsertItem(item, 1, 0))
            {
                loot += item.Name + ", ";
            }
            else
            {
                missedLoot += item.Name + ", ";
                money += Random.Range(4, 7);
            }
        }

        if (Random.Range(0, 4) == 0)
        {
            item = _rareLoot[Random.Range(0, _rareLoot.Count)];
            if (InventoryController.Instance.TryCreateAndInsertItem(item, 1, 0))
            {
                if (missedLoot != "")
                {
                    _eventWindow.ChangeDescription(
                        "� �������, � ������ ������ �� ���������, � �� �������� �. ����� �������� ����� �� �����: "
                        + loot + "� ��� �� ������ ������� " + item.Name +$". � ���� �� �� ����� {money} ������. " +
                        $"����� �������� ����� ��� �� ����: {missedLoot} �� �� �� ������ �� �����, ��-�� �������� ���������� ����� � ���������.");
                }
                else
                {
                    _eventWindow.ChangeDescription(
                        "� �������, � ������ ������ �� ���������, � �� �������� �. ����� �������� ����� �� �����: "
                        + loot + "� ��� �� ������ ������� " + item.Name +$". � ���� �� �� ����� {money} ������.");
                }
            }
            else
            {
                missedLoot += item.Name + ", ";
                money += item.Price/2 + 1;
                if (missedLoot != "")
                {
                    _eventWindow.ChangeDescription("� �������, � ������ ������ �� ���������, � �� �������� �. ����� �������� ����� �� �����: " +
                                                   loot + $"� ��� �� {money} ������" + $" ����� �������� ����� ��� �� ����: {missedLoot} �� �� �� ������ �� ����� ��-�� �������� ���������� ����� � ���������.");
                }
                else 
                    _eventWindow.ChangeDescription("� �������, � ������ ������ �� ���������, � �� �������� �. ����� �������� ����� �� �����: " +
                                                   loot + $"� ��� �� {money} ������.");
            }
        }
        else
        {
            item = _loot[Random.Range(0, _loot.Count)];
            if (InventoryController.Instance.TryCreateAndInsertItem(item, 1, 0))
            {
                loot += item.Name + ", ";
            }
            else
            {
                missedLoot += item.Name + ", ";
                money += item.Price/2 + 1;
            }

            if (missedLoot != "")
            {
                _eventWindow.ChangeDescription("� �������, ������ ��������� �����, � �� �������� ��. ����� �������� ����� �� �����: "+
                                               loot + $"� ��� �� {money} ������" + $" ����� �������� ����� ��� �� ����: {missedLoot} �� �� �� ������ �� �����, ��-�� �������� ���������� ����� � ���������.");
            }
            else 
                _eventWindow.ChangeDescription("� �������, ������ ��������� �����, � �� �������� ��. ����� �������� ����� �� �����: "+
                                                loot + $"� ��� �� {money} ������.");
        }
    }
}
