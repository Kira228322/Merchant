using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Flood : GlobalEvent_Base
{
    public override string GlobalEventName => $"���������� � {Location.VillageName}!";

    public override string Description => $"� {Location.VillageName} ��������� ����������. ��������� ��������� ������ �������.";

    public Location Location;
    public string ItemToDeleteName => "��������� ������"; //TODO: ���� ������� ���, ������ ��������� ������.
    public override void Execute()
    {
        Location.DeleteItemsFromTraders(ItemToDeleteName);
      //TODO ����� ����� ��������� ��������������� � ��������:
      //ItemDatabase.GetRandomItemOfThisType(Item.ItemType.GrownFood)
    }

    public override void Terminate()
    {
        //����������� �����.
        //����� �������� ������� ��� �������� ������ ������������ ���� ��-�� ������� ���������.
        //������� ������ �� ���������� ����� ����� �������������.
    }
}
