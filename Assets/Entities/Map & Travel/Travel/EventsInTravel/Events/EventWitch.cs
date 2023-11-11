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
        ButtonsLabel.Add("�����������");
        ButtonsLabel.Add("����������");
        SetInfoButton($"������ ����� �������� ������������� ������� � ������ {TravelEventHandler.GetProbability(_probabilityOfGoodCast, Player.Instance.Statistics.Luck)}%." +
                      $"\n���� ������ ������� �� ����� �����");
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
                    _eventWindow.ChangeDescription($"�� ���� ��� ��� ������� � ������ �������� �� ��� {status.StatusName}");
                }
                else
                {
                    status = _negativeStatuses[Random.Range(0, _negativeStatuses.Count)];
                    StatusManager.Instance.AddStatusForPlayer(status);
                    _eventWindow.ChangeDescription($"��� �� ������� � ������ �������� �� ��� {status.StatusName}." +
                                                   $"������ �������������, ��� ����� ����� ���� � �����.");
                }
                    
                break;
            case 1:
                _eventWindow.ChangeDescription("�� ����� �������� �� ������������� ��  ������� ����������� �������� ������.");
                break;
        }
    }
}
