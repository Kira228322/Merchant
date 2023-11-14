using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWitch : EventInTravel
{
    [SerializeField] private List<Status> _positiveStatuses;
    [SerializeField] private List<Status> _negativeStatuses;
    
    private int _probabilityOfGoodCast = 50;
    public override void SetButtons()
    {
        ButtonsLabel.Add("Согласиться");
        ButtonsLabel.Add("Отказаться");
        SetInfoButton($"Ведьма может наложить положительное заклять с шансом {TravelEventHandler.GetProbability(_probabilityOfGoodCast, Player.Instance.Statistics.Luck)}%." +
                      $"\nШанс успеха зависит от вашей удачи");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                Status status;
                if (TravelEventHandler.EventFire(_probabilityOfGoodCast, true, Player.Instance.Statistics.Luck))
                {
                    status = _positiveStatuses[Random.Range(0, _positiveStatuses.Count)];
                    StatusManager.Instance.AddStatusForPlayer(status);
                    _eventWindow.ChangeDescription($"На этот раз вам повезло и ведьма наложила на вас {status.StatusName}");
                }
                else
                {
                    status = _negativeStatuses[Random.Range(0, _negativeStatuses.Count)];
                    StatusManager.Instance.AddStatusForPlayer(status);
                    _eventWindow.ChangeDescription($"Вам не повезло и ведьма наложила на вас {status.StatusName}." +
                                                   $"Ведьма предупреждала, что исход может быть и таков.");
                }
                    
                break;
            case 1:
                _eventWindow.ChangeDescription("От греха подальше вы отказываетесь от  щедрого предложения странной ведьмы.");
                break;
        }
    }
}
