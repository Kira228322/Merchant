using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBandits : EventInTravel
{
    [SerializeField] private float _minWeight;
    [SerializeField] private float _maxWeight;
    [SerializeField] private int _money;

    public override void SetButtons()
    {
        ButtonsLabel.Add("���������� ��������� �������");
        ButtonsLabel.Add("������ ����������� ������");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                // TODO �������� �������� �� Rand(min,max) ���� � ������ 
                break;
            case 1:
                // TODO �������� ������ � ������
                break;
        }
        _eventHandler.EventEnd();
    }
}
