using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_PlentifulHarvest : GlobalEvent_Base
{
    public override string GlobalEventName => $"����������� � {Location.VillageName}!";

    public override string Description => $"��������� ��������� �������, � {Location.VillageName} ��������� ����������� ������� ������.";


    [NonSerialized] public Location Location;
    public float MultiplyCoefficient;
    public string ItemToMultiplyName => "��������� ������"; //TODO: ������, ����� ����� ����������� ������ � �����������.

    public override void Execute()
    {
        Location.MultiplyItemsInTraders(ItemToMultiplyName, MultiplyCoefficient);
    }

    public override void Terminate()
    {
        //����������� �����.
        //����� �������� �������� ��� ��� �������� ������ ������������ ���� ��-�� ������� ���������.
        //������� ������ �� ���������� ����� ����� �������������.
    }
}
