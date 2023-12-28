using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMansCompetition : EventInTravel
{
    private int _probabilityToWin = 18;
    public override void SetButtons()
    {
        _probabilityToWin += Player.Instance.Statistics.Toughness.Total + Random.Range(0,5);
        ButtonsLabel.Add("Участвовать");
        ButtonsLabel.Add("Проехать мимо");
        SetInfoButton($"Вероятность победить в состязаниях {TravelEventHandler.GetProbability(_probabilityToWin, Player.Instance.Statistics.Toughness)}%." +
                      $"\nВероятность серьезно зависит от вашей выносливости");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < 10)
                {
                    _eventWindow.ChangeDescription("У вас было не достаточно золота. Мужики вдоволь с вас посмеялись!");
                    return;
                }

                if (TravelEventHandler.EventFire(_probabilityToWin, true, Player.Instance.Statistics.Toughness))
                {
                    Player.Instance.Experience.AddExperience(10);
                    _eventWindow.ChangeDescription("Вы победили в состязаниях!!! Вам вернули первоначальный взнос. К вам прониклись уважением! " +
                                                   "Вы получили 10 опыта");
                }
                else
                {
                    Player.Instance.Money -= 10;
                    _eventWindow.ChangeDescription("Вы старались как могли, но победить не получилось. Все были рады, что вы приняли участие.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("Жители деревни зазывали вас поучаствовать, но вы просто уехали.");
                break;
        }   
    }
}
