using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Flood : GlobalEvent_Base
{
    public override string GlobalEventName => $"���������� � ������� {Location.VillageName}!";
    public override string Description => $"� ������� {Location.VillageName} ��������� ����������. ������ ����� ���������� ������ �������� {ItemToMultiplyName}.";

    [NonSerialized] public Location Location;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName;

    public override void Execute()
    {
        Location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //����������� �����.
        //����� ��������/������� �������� ��� ��� �������� ������ ������������ ���� ��-�� ������� ���������.
        //������� ������ �� ���������� ����� ����� �������������.
    }
}
