using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCrossroads : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("������� ������");
        ButtonsLabel.Add("������� �������");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                _eventWindow.ChangeDescription("�� ���������� ���� ���� �� ����� ������");
                break;
            case 1:
                _eventWindow.ChangeDescription("�� ���������� ���� ���� �� ������ ������");
                break;
        }

        FindObjectOfType<TravelEventHandler>().ChangeTravelTime(Random.Range(1,4));
    }
}
