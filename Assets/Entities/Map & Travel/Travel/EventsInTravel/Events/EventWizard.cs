using System.Collections.Generic;
using UnityEngine;

public class EventWizard : EventInTravel
{
    [SerializeField] private List<Status> _commonStatuses;
    [SerializeField] private List<Status> _rareStatuses;

    private int _rareBuffProbability = 20;
    private int cost;
    public override void SetButtons()
    {
        cost = Random.Range(75, 111);
        ButtonsLabel.Add($"Получить улучшение ({cost} золота)");
        ButtonsLabel.Add("Спасибо, не нужно");
        SetInfoButton($"Существует {TravelEventHandler.GetProbability(_rareBuffProbability, Player.Instance.Statistics.Diplomacy)}% " +
                      $"шанс, что волшебник наложит редкое заклинание \nШанс успеха зависит от вашей дипломатии");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                int index;
                if (Player.Instance.Money < cost)
                {
                    _eventWindow.ChangeDescription("У вас не было достаточно золота. Маг посмотрел на вас презренно.");
                    return;
                }

                Player.Instance.Money -= cost;

                if (TravelEventHandler.EventFire(_rareBuffProbability, true, Player.Instance.Statistics.Diplomacy))
                {
                    index = Random.Range(0, _rareStatuses.Count);
                    StatusManager.Instance.AddStatusForPlayer(_rareStatuses[index]);
                    _eventWindow.ChangeDescription($"Чародей наложил на вас редкое заклинание, дарующее эффект {_rareStatuses[index].StatusName}. Маг надеется, что это именно то, что вы хотели.");
                }
                else
                {
                    index = Random.Range(0, _commonStatuses.Count);
                    StatusManager.Instance.AddStatusForPlayer(_commonStatuses[index]);
                    _eventWindow.ChangeDescription($"Чародей наложил на вас заклинание, дарующее эффект {_commonStatuses[index].StatusName}. Маг говорит, что сегодня не колдуется.");

                }
                break;
            case 1:
                _eventWindow.ChangeDescription("Чародей раздосадован, что не предоставилось возможности поколдовать...");
                break;
        }

    }
}
