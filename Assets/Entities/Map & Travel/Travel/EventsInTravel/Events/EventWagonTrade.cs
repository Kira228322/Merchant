using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWagonTrade : EventInTravel
{
    public override void SetButtons()
    {
        // TODO 
        // �� ���� ��� �� ����� ��������� ���� ������, ��� � ����� ��� 
        // ����� ���������� ������ ������ 2-3 ������ �������� � ������ ����� ��� ����� ������ _eventHandler.EventEnd();
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
