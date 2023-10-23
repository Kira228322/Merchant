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
    
    // TODO ��� ������� ����� ����� ������� ����� ������ �� ������ ��� �������� ���,� ������ ����� ���� ��� ��������.
    // ��� ��� ���� ������ ������ � �����, �� ����� �� �������������� ���� ����� ������ � ����� �������, ��� � �����.
    // ���� ������ ��� ���, �� �� ����� ������ �����, �� ��� ����� �� ������, ����� ������ �� �����.
    
    public override void SetButtons()
    {
        ButtonsLabel.Add("�������� �������");
        ButtonsLabel.Add("����� ������");
        SetInfoButton("");
    }

    protected override void Start()
    {
        base.Start();
        _isThief = TravelEventHandler.EventFire(_chanceTravelerIsThief, false, Player.Instance.Statistics.Luck);
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
        
    }
}
