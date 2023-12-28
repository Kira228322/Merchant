using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMansCompetition : EventInTravel
{
    private int _probabilityToWin = 18;
    public override void SetButtons()
    {
        _probabilityToWin += Player.Instance.Statistics.Toughness.Total + Random.Range(0,5);
        ButtonsLabel.Add("�����������");
        ButtonsLabel.Add("�������� ����");
        SetInfoButton($"����������� �������� � ����������� {TravelEventHandler.GetProbability(_probabilityToWin, Player.Instance.Statistics.Toughness)}%." +
                      $"\n����������� �������� ������� �� ����� ������������");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < 10)
                {
                    _eventWindow.ChangeDescription("� ��� ���� �� ���������� ������. ������ ������� � ��� ����������!");
                    return;
                }

                if (TravelEventHandler.EventFire(_probabilityToWin, true, Player.Instance.Statistics.Toughness))
                {
                    Player.Instance.Experience.AddExperience(10);
                    _eventWindow.ChangeDescription("�� �������� � �����������!!! ��� ������� �������������� �����. � ��� ���������� ���������! " +
                                                   "�� �������� 10 �����");
                }
                else
                {
                    Player.Instance.Money -= 10;
                    _eventWindow.ChangeDescription("�� ��������� ��� �����, �� �������� �� ����������. ��� ���� ����, ��� �� ������� �������.");
                }
                break;
            case 1:
                _eventWindow.ChangeDescription("������ ������� �������� ��� �������������, �� �� ������ ������.");
                break;
        }   
    }
}
