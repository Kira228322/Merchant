using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EventBumpyRoad : EventInTravel
{
    [SerializeField] private float _reduceQuality;
    [SerializeField] private int _minTimeToGoAround;
    [SerializeField] private int _maxTimeToGoAround;
    public override void SetButtons()
    {
        ButtonsLabel.Add("����� �� ��������� ������");
        ButtonsLabel.Add("����� � ������");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                _eventWindow.ChangeDescription("�� ������ ������� ��������, �������������� � ����� �����������.");
                break;
            case 1:
                FindObjectOfType<TravelEventHandler>().ChangeTravelTime(Random.Range(_minTimeToGoAround, _maxTimeToGoAround +1));
                _eventWindow.ChangeDescription("�� ������� � ������. ���� ����� - ������ ������");
                break;
        }
        
    }
}
