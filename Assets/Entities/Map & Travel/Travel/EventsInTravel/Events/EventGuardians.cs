using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGuardians : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("���������� ��������");
        ButtonsLabel.Add("���������� ������������");
        SetInfoButton($"����������� �������� ������ {TravelEventHandler.GetProbability(50, Player.Instance.Statistics.Luck)}%. ��� ����������� ������� �� ����� �����." +
                      $"\n����������� ������������ � ������� {TravelEventHandler.GetProbability(50, Player.Instance.Statistics.Diplomacy)}%. ��� ����������� ������� �� ����� ����������.");
    }

    public override void OnButtonClick(int n)
    {
        int lostMoney = Random.Range(45, 56);
        switch (n)
        {
            case 0:
                if (TravelEventHandler.EventFire(50, true, Player.Instance.Statistics.Luck))
                    _eventWindow.ChangeDescription("��� ������� ��������� ����������. ��� ���������� ���, �� ����� �� �����, ��� ����������.");
                else
                {
                    if (Player.Instance.Money > lostMoney)
                        Player.Instance.Money -= lostMoney;
                    else
                    {
                        lostMoney = Player.Instance.Money;
                        Player.Instance.Money = 0;
                    }
                    _eventWindow.ChangeDescription($"��� �� ������� ��������� ������. �� ���� ��������� �������� ����� � ������� {lostMoney} ������.");
                }
                break;
            case 1:
                if (TravelEventHandler.EventFire(50, true, Player.Instance.Statistics.Diplomacy))
                    _eventWindow.ChangeDescription("��� ������� ������������ �� �����������. ��� ���������� ���, ���� �� ����, ��� ��� ���������.");
                else
                {
                    if (Player.Instance.Money > lostMoney)
                        Player.Instance.Money -= lostMoney;
                    else
                    {
                        lostMoney = Player.Instance.Money;
                        Player.Instance.Money = 0;
                    }
                    _eventWindow.ChangeDescription($"��� �� ������� ������������ �� �������. �� ���� ��������� �������� ����� � ������� {lostMoney} ������.");
                }
                break;
        }
    }
}
