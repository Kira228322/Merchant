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
        ButtonsLabel.Add($"�������� ��������� ({cost} ������)");
        ButtonsLabel.Add("�������, �� �����");
        SetInfoButton($"���������� {TravelEventHandler.GetProbability(_rareBuffProbability, Player.Instance.Statistics.Diplomacy)}% " +
                      $"����, ��� ��������� ������� ������ ���������� \n���� ������ ������� �� ����� ����������");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                int index;
                if (Player.Instance.Money < cost)
                {
                    _eventWindow.ChangeDescription("� ��� �� ���� ���������� ������. ��� ��������� �� ��� ���������.");
                    return;
                }

                Player.Instance.Money -= cost;

                if (TravelEventHandler.EventFire(_rareBuffProbability, true, Player.Instance.Statistics.Diplomacy))
                {
                    index = Random.Range(0, _rareStatuses.Count);
                    StatusManager.Instance.AddStatusForPlayer(_rareStatuses[index]);
                    _eventWindow.ChangeDescription($"������� ������� �� ��� ������ ����������, �������� ������ {_rareStatuses[index].StatusName}. ��� ��������, ��� ��� ������ ��, ��� �� ������.");
                }
                else
                {
                    index = Random.Range(0, _commonStatuses.Count);
                    StatusManager.Instance.AddStatusForPlayer(_commonStatuses[index]);
                    _eventWindow.ChangeDescription($"������� ������� �� ��� ����������, �������� ������ {_commonStatuses[index].StatusName}. ��� �������, ��� ������� �� ���������.");

                }
                break;
            case 1:
                _eventWindow.ChangeDescription("������� ������������, ��� �� �������������� ����������� �����������...");
                break;
        }

    }
}
