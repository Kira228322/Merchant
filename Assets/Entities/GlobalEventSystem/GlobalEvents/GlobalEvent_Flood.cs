using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Flood : GlobalEvent_Base
{
    public override string GlobalEventName => $"���������� � ������� {LocationVillageName}!";
    public override string Description => $"� ������� {LocationVillageName} ��������� ����������. ������ ����� ���������� ������ �������� {ItemToMultiplyName}.";

    public string LocationSceneName;
    public string LocationVillageName;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName;

    public override void Execute()
    {
        Location location = MapManager.GetLocationBySceneName(LocationSceneName);
        location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //����������� �����.
        //����� ��������/������� �������� ��� ��� �������� ������ ������������ ���� ��-�� ������� ���������.
        //������� ������ �� ���������� ����� ����� �������������.
    }
}
