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
        ButtonsLabel.Add("Попытаться уехать");
        SetInfoButton($"Попытаться уехать - шанс успеха " +
                      $"{TravelEventHandler.GetProbability(_runningAwayProbability, Player.Instance.Statistics.Toughness)}% \n" +
                      $"Шанс зависит от вашей выносливости");

        _wealthTaken = Random.Range(_minWealthTaken, _maxWealthTaken);
        _itemTotalWealth = 0;
        foreach (InventoryItem item in Player.Instance.Inventory.BaseItemList)
        {
            if (item.ItemData.IsQuestItem) 
                continue; //квестовые предметы не учитываются в подсчете общей стоимости
            _itemTotalWealth += item.ItemData.Price * item.CurrentItemsInAStack;
        }
        _playerTotalWealth = _itemTotalWealth + Player.Instance.Money;
        _playerWealthDistribution = (float)Player.Instance.Money / _playerTotalWealth;
        if (_playerWealthDistribution >= _wealthTaken)
        {
            ButtonsLabel.Add("Отдать разбойникам золото");
        }
        if ((1 - _playerWealthDistribution) >= _wealthTaken)
        {
            ButtonsLabel.Add("Разбойники разграбят повозку");
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
                    _eventWindow.ChangeDescription($"Ограбление. Бандиты забрали {(int)(_playerTotalWealth * _wealthTaken)} золота");
                }
                else
                {
                    TakePlayersItems((int)(_playerTotalWealth * _wealthTaken));
                    _eventWindow.ChangeDescription($"Ограбление. Бандиты забрали вещи на сумму {(int)(_playerTotalWealth * _wealthTaken)} золота");
                }
                break;
            case 2:
                TakePlayersItems((int)(_playerTotalWealth * _wealthTaken));
                _eventWindow.ChangeDescription($"Ограбление. Бандиты забрали вещи на сумму {(int)(_playerTotalWealth * _wealthTaken)} золота");
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
            _eventWindow.ChangeDescription($"Неудача. Бандиты забрали вещи на сумму {itemsTaken} золота, а также {moneyTaken} золота");
        }
        else
        {
            _eventWindow.ChangeDescription("Вам чудом удалось сбежать от бандитов. Ваши деньги и имущество цело");
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
