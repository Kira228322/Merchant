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
        ButtonsLabel.Add("Взять ожерелье");
        ButtonsLabel.Add("Ехать дальше");
        SetInfoButton($"Вероятность того, что ожерелье НЕ будет проклято " +
                      $"{TravelEventHandler.GetProbability(_probabilityNotCursed, Player.Instance.Statistics.Luck)}%." +
                      $"\nШанс успеха зависит от вашей удачи");
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
                        _eventWindow.ChangeDescription("Вам несказанно повезло и это просто кто-то забыл дорогущее ожерелье на кладбище. Вы подбираете его.");
                    }
                    else
                    {
                        _eventWindow.ChangeDescription("У вас не было места для ожерелья и вы решили не прикосаться к этой зловещей штуке");
                    }
                }
                else
                {
                    StatusManager.Instance.AddStatusForPlayer(_curse);
                    _eventWindow.ChangeDescription("Ожерелья оказалось проклято! Вы взяли его в руки и оно тот час же рассыпалось. " +
                                                   "Вы ощутили пронзительный холод. Теперь вы осквернены.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("Вы решили оставить это проклятое место в покое.");
                break;
        }
    }
}
