using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_MultiplyItemsOnScene : GlobalEvent_Base
{
    public override string GlobalEventName => IsPositive? ($"������� ������� ��������� � {Location.VillageName}!")
                                                         :($"������ �������� � {Location.VillageName}");

    public override string Description => IsPositive? ($"��������� ��������� �������, � {Location.VillageName} ��������� ����������� ������� ������� �������� {ItemToMultiplyName}.")
                                                     :($"��-�� ����������� ������� ����� �� ������� {Location.VillageName}, � ��� ��������� ������ �������� {ItemToMultiplyName}");

    public bool IsPositive;
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
