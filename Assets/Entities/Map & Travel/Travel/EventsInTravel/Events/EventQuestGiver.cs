using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQuestGiver : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("����� �������");
        ButtonsLabel.Add("����������");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        // TODO ���� �������� ����� �� ���� � �������� ��������� ������-�� ���������� ��� �� ��������� �������.
        switch (n)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }
}
