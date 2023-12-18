using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EventBandits : EventInTravel
{
    [SerializeField][Range(0, 1)] private float _minWealthTaken;
    [SerializeField][Range(0, 1)] private float _maxWealthTaken;
    private float _wealthTaken;
    private float _playerWealthDistribution;
    private int _playerTotalWealth;
    private int _itemTotalWealth;

    private int _runningAwayProbability = 10;
    public override void SetButtons()
    {
        ButtonsLabel.Add("���������� ������");
        SetInfoButton($"���������� ������ - ���� ������ " +
                      $"{TravelEventHandler.GetProbability(_runningAwayProbability, Player.Instance.Statistics.Toughness)}% \n" +
                      $"���� ������� �� ����� ������������");

        _wealthTaken = Random.Range(_minWealthTaken, _maxWealthTaken);
        _itemTotalWealth = 0;
        foreach (InventoryItem item in Player.Instance.Inventory.BaseItemList)
        {
            if (item.ItemData.IsQuestItem) 
                continue; //��������� �������� �� ����������� � �������� ����� ���������
            _itemTotalWealth += item.ItemData.Price * item.CurrentItemsInAStack;
        }
        _playerTotalWealth = _itemTotalWealth + Player.Instance.Money;
        _playerWealthDistribution = (float)Player.Instance.Money / _playerTotalWealth;
        if (_playerWealthDistribution >= _wealthTaken)
        {
            ButtonsLabel.Add("������ ����������� ������");
        }
        if ((1 - _playerWealthDistribution) >= _wealthTaken)
        {
            ButtonsLabel.Add("���������� ��������� �������");
        }
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                TryRunningAway();
                break;
            case 1:
                if (_playerWealthDistribution >= _wealthTaken)
                {
                    TakePlayersMoney((int)(_playerTotalWealth * _wealthTaken));
                    _eventWindow.ChangeDescription($"����������. ������� ������� {(int)(_playerTotalWealth * _wealthTaken)} ������");
                }
                else
                {
                    TakePlayersItems((int)(_playerTotalWealth * _wealthTaken));
                    _eventWindow.ChangeDescription($"����������. ������� ������� ���� �� ����� {(int)(_playerTotalWealth * _wealthTaken)} ������");
                }
                break;
            case 2:
                TakePlayersItems((int)(_playerTotalWealth * _wealthTaken));
                _eventWindow.ChangeDescription($"����������. ������� ������� ���� �� ����� {(int)(_playerTotalWealth * _wealthTaken)} ������");
                break;
        }
        
    }

    private void TryRunningAway()
    {
        if (!TravelEventHandler.EventFire(_runningAwayProbability, true, Player.Instance.Statistics.Toughness))
        {
            int itemsTaken = (int)(_itemTotalWealth * _wealthTaken);
            int moneyTaken = (int)(Player.Instance.Money * _wealthTaken);
            TakePlayersItems(itemsTaken);
            TakePlayersMoney(moneyTaken);
            _eventWindow.ChangeDescription($"�������. ������� ������� ���� �� ����� {itemsTaken} ������, � ����� {moneyTaken} ������");
        }
        else
        {
            _eventWindow.ChangeDescription("��� ����� ������� ������� �� ��������. ���� ������ � ��������� ����");
        }
    }
    private void TakePlayersMoney(int amount)
    {
        Player.Instance.Money -= amount;
    }
    private void TakePlayersItems(int price)
    {
        Player.Instance.Inventory.RemoveItemsByPrice(price);
    }
}
