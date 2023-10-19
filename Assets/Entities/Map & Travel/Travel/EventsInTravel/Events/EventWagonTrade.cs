using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWagonTrade : EventInTravel
{
    [SerializeField] private List<Item> _cheapItems;
    [SerializeField] private List<Item> _expensiveItems;
    public override void SetButtons()
    {
        ButtonsLabel.Add("Купить товар у торговца");
        ButtonsLabel.Add("Отказаться от предложения");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < 100)
                {
                    _eventWindow.ChangeDescription("У вас было не достаточно золота. Торговец посмотрел на вас и посмеялся!");
                    return;
                }

                Item item;
                if (TravelEventHandler.EventFire(40, true, Player.Instance.Statistics.Diplomacy))
                {
                    item = _expensiveItems[Random.Range(0, _expensiveItems.Count)];
                    if (InventoryController.Instance.TryCreateAndInsertItem
                            (item, 1, 0) != null)
                    {
                        _eventWindow.ChangeDescription($"Внутри оказался ценный товар: {item.Name}. Вы с удовольствием забираете его себе, платя торговцу его 100 золотых");
                        Player.Instance.Money -= 100;
                    }
                    else
                    {
                        _eventWindow.ChangeDescription($"Внутри оказался ценный товар: {item.Name}. Но у вас не было места в инвентаре, чтобы поместить его. Торговец решил оставить этот товар себе");
                    }
                }
                else
                {
                    item = _cheapItems[Random.Range(0, _cheapItems.Count)];
                    if (InventoryController.Instance.TryCreateAndInsertItem
                            (item, 1, 0) != null)
                    {
                        _eventWindow.ChangeDescription($"Внутри совсем не ценный товар: {item.Name}. Вы с забираете его себе, платя торговцу его грязные 100 золотых");
                        Player.Instance.Money -= 100;
                    }
                    else
                    {
                        _eventWindow.ChangeDescription($"Внутри совсем не ценный товар: {item.Name}. Но у вас не было места в инвентаре, чтобы поместить его. Торговец решил оставить этот товар себе");
                    }
                }
                
                break;
            case 1:
                _eventWindow.ChangeDescription("Загадка тайного товара странствующего торговца не будет раскрыта...");
                break;
        }
        
    }
}
