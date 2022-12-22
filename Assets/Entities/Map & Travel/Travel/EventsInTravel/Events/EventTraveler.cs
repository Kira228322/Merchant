using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTraveler : EventInTravel
{
    // ������ ����� ���� ������������� ������, �� ��, ��� ��� ��� �����, ���� ��������� �����
    [SerializeField] private int _chanceTravelerIsThief; // �����������, ��� ��� (�����������, ��� �� ��� q = 1 - p)
    [SerializeField] private int _money;
    [SerializeField] private int _experience;
    private bool _isThief;
    
    public override void SetButtons()
    {
        ButtonsLabel.Add("�������� �������");
        ButtonsLabel.Add("����� ������");
    }

    private void Start()
    {
        _isThief = TravelEventHandler.EventFire(_chanceTravelerIsThief, false, true);
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                break;
            case 1:
                break;
        }
        _eventHandler.EventEnd();
    }
}
