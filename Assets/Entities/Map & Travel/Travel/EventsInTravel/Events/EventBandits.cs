using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBandits : EventInTravel
{
    [SerializeField][Range(0, 1)] private float _minWealthTaken;
    [SerializeField][Range(0, 1)] private float _maxWealthTaken;
    private float _wealthTaken;
    private float _playerWealthDistribution;
    private int _playerTotalWealth;
    public override void SetButtons()
    {
        _wealthTaken = Random.Range(_minWealthTaken, _maxWealthTaken);
        int itemTotalWealth = 0;
        foreach (InventoryItem item in Player.Instance.Inventory.ItemList)
        {
            if (item.ItemData.IsQuestItem) 
                continue; //квестовые предметы не учитываются в подсчете общей стоимости
            itemTotalWealth += item.ItemData.Price * item.CurrentItemsInAStack;
        }
        _playerTotalWealth = itemTotalWealth + Player.Instance.Money;
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
                if (_playerWealthDistribution >= _wealthTaken)
                    TakePlayersMoney((int)(_playerTotalWealth * _wealthTaken));
                else
                    TakePlayersItems((int)(_playerTotalWealth * _wealthTaken));
                break;
            case 1:
                TakePlayersItems((int)(_playerTotalWealth * _wealthTaken));
                break;
        }
        _eventHandler.EventEnd();
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
