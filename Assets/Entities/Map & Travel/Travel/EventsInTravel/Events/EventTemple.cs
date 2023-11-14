using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTemple : EventInTravel
{
    [SerializeField] private Status MerickasBlessing;

    private int _probability = 60;
    public override void SetButtons()
    {
        ButtonsLabel.Add("Помолиться");
        SetInfoButton($"Вы можете получить благославение с шансом {TravelEventHandler.GetProbability(_probability, Player.Instance.Statistics.Luck)}%." +
                      $"\nШанс успеха зависит от вашей удачи");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (TravelEventHandler.EventFire(_probability, true, Player.Instance.Statistics.Luck))
                {
                    StatusManager.Instance.AddStatusForPlayer(MerickasBlessing);
                    _eventWindow.ChangeDescription("Богиня одарила вас своим благославлением!");
                }
                else
                    _eventWindow.ChangeDescription("Богиня не услышала ваши молитвы...");
                
                break;
        }
    }
}
