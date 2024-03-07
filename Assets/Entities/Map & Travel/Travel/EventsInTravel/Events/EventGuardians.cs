using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGuardians : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Попытаться обмануть");
        ButtonsLabel.Add("Попытаться договориться");
        SetInfoButton($"Вероятность обмануть охрану {TravelEventHandler.GetProbability(50, Player.Instance.Statistics.Luck)}%. Эта вероятность зависит от вашей удачи." +
                      $"\nВероятность договориться с охраной {TravelEventHandler.GetProbability(50, Player.Instance.Statistics.Diplomacy)}%. Эта вероятность зависит от вашей дипломатии.");
    }

    public override void OnButtonClick(int n)
    {
        int lostMoney = Random.Range(45, 56);
        switch (n)
        {
            case 0:
                if (TravelEventHandler.EventFire(50, true, Player.Instance.Statistics.Luck))
                    _eventWindow.ChangeDescription("Вам удалось обхитрить стражников. Они пропускают вас, до конца не поняв, что происходит.");
                else
                {
                    if (Player.Instance.Money > lostMoney)
                        Player.Instance.Money -= lostMoney;
                    else
                    {
                        lostMoney = Player.Instance.Money;
                        Player.Instance.Money = 0;
                    }
                    _eventWindow.ChangeDescription($"Вам не удалось обхитрить стражу. Вы были вынуждены оплатить штраф в размере {lostMoney} золота.");
                }
                break;
            case 1:
                if (TravelEventHandler.EventFire(50, true, Player.Instance.Statistics.Diplomacy))
                    _eventWindow.ChangeDescription("Вам удалось договориться со стражниками. Они пропускают вас, даже не зная, как вам возразить.");
                else
                {
                    if (Player.Instance.Money > lostMoney)
                        Player.Instance.Money -= lostMoney;
                    else
                    {
                        lostMoney = Player.Instance.Money;
                        Player.Instance.Money = 0;
                    }
                    _eventWindow.ChangeDescription($"Вам не удалось договориться со стражей. Вы были вынуждены оплатить штраф в размере {lostMoney} золота.");
                }
                break;
        }
    }
}
