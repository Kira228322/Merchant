using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTraveler : EventInTravel
{
    // ������ ����� ���� ������������� ������, �� ��, ��� ��� ��� �����, ���� ��������� �����
    [SerializeField] private int _chanceTravelerIsChief; // �����������, ��� ��� (����������, ��� �� ��� q = 1 - p)
    [SerializeField] private int _money;
    [SerializeField] private int _experience;
    public override void SetButtons()
    {
        ButtonsLabel.Add("�������� �������");
        ButtonsLabel.Add("����� ������");
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
