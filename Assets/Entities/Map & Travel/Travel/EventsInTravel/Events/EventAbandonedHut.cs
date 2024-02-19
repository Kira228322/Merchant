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
        ButtonsLabel.Add("Войти внутрь");
        ButtonsLabel.Add("Не рисковать");
        SetInfoButton("В хижине вас НЕ будет ждать опасность с вероятностью " +
                      $"{TravelEventHandler.GetProbability(_probabilityOfGoodResult, Player.Instance.Statistics.Luck)}%." +
                      "\nШанс успеха зависит от вашей удачи");
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
                    _eventWindow.ChangeDescription("Внутри оказался разбойник. Он был весьма рад " +
                                                   "увидеть добычу, которая сама к нему пришла. Он ограбил вас, украв " + stoleMoney + " золота.");
                }

                break;
            case 1:
                _eventWindow.ChangeDescription("Вы решили не рисковать и поехали дальше.");
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
                        "К счастью, в хижине никого не оказалось, и вы обыскали её. Среди полезных вещей вы нашли: "
                        + loot + "а так же ценный предмет " + item.Name +$". К тому же вы нашли {money} золота. " +
                        $"Среди полезных вещей так же были: {missedLoot} но вы не смогли их взять, из-за нехватки свободного места в инвентаре.");
                }
                else
                {
                    _eventWindow.ChangeDescription(
                        "К счастью, в хижине никого не оказалось, и вы обыскали её. Среди полезных вещей вы нашли: "
                        + loot + "а так же ценный предмет " + item.Name +$". К тому же вы нашли {money} золота.");
                }
            }
            else
            {
                missedLoot += item.Name + ", ";
                money += item.Price/2 + 1;
                if (missedLoot != "")
                {
                    _eventWindow.ChangeDescription("К счастью, в хижине никого не оказалось, и вы обыскали её. Среди полезных вещей вы нашли: " +
                                                   loot + $"а так же {money} золота" + $" Среди полезных вещей так же были: {missedLoot} но вы не смогли их взять из-за нехватки свободного места в инвентаре.");
                }
                else 
                    _eventWindow.ChangeDescription("К счастью, в хижине никого не оказалось, и вы обыскали её. Среди полезных вещей вы нашли: " +
                                                   loot + $"а так же {money} золота.");
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
                _eventWindow.ChangeDescription("К счастью, хижина оказалась пуста, и вы обыскали ее. Среди полезных вещей вы нашли: "+
                                               loot + $"а так же {money} золота" + $" Среди полезных вещей так же были: {missedLoot} но вы не смогли их взять, из-за нехватки свободного места в инвентаре.");
            }
            else 
                _eventWindow.ChangeDescription("К счастью, хижина оказалась пуста, и вы обыскали ее. Среди полезных вещей вы нашли: "+
                                                loot + $"а так же {money} золота.");
        }
    }
}
