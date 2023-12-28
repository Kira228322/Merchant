using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoadKeeper : EventInTravel
{
    private int money;
    private float _reduceQuality = 0.06f;
    public override void SetButtons()
    {
        money = Random.Range(28, 38) - Player.Instance.Statistics.Diplomacy.Total - Player.Instance.Statistics.Diplomacy.Total/2;
        ButtonsLabel.Add("���������");
        ButtonsLabel.Add("������������");
        SetInfoButton($"��������� ������ ������� {money} ������.\n���� ���������� ��������� ��������� �����.");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (Player.Instance.Money < money)
                {
                    FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                    _eventWindow.ChangeDescription("� ��� �� ���� ���������� ������. " +
                                                   "��������� ���������� � ������ �� ������ ������ �������. " +
                                                   "��� ����� �������� � ������������ ���������� ��������� ��������� ����� �������");
                }
                else
                {
                    Player.Instance.Money -= money;
                    _eventWindow.ChangeDescription("��������� ������ ��� ��� ������� ����� �� ������. �� �����������," +
                                                   " ������� � �������� ��� ��������");
                }
                break;
            case 1:
                FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                _eventWindow.ChangeDescription("��������� ���������� � ������ �� ������ ������ �������. " +
                                               "��� ����� �������� � ������������ ���������� ��������� ��������� ����� �������");
                break;
        }
    }
}
