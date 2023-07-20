using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_BannedItem : GlobalEvent_Base
{
    public string BannedItemName;
    public override string GlobalEventName => "������� ��������";

    public override string Description => $"������� � ����� ���, ������ ��������� ������� {BannedItemName}. ���� ��������� �������: {GetAddedTime(DurationHours)}";

    private string GetAddedTime(int hoursToAdd)
    {
        //����� � ����� ���������� ���������� ����� ����� ������, � �� ������� ����� ��������
        int daysToDisplay = GameTime.CurrentDay;
        int hoursToDisplay = GameTime.Hours;
        while (hoursToAdd >= 24)
        {
            daysToDisplay++;
            hoursToAdd -= 24;
        }
        hoursToDisplay += hoursToAdd;
        if (hoursToDisplay >= 24)
        {
            daysToDisplay++;
            hoursToDisplay -= 24;
        }

        return $"���� {daysToDisplay}, {(hoursToDisplay < 10 ? $"0{hoursToDisplay}": $"{hoursToDisplay}")}:00";
    }

    public override void Execute()
    {
        BannedItemsHandler.Instance.BanItem(ItemDatabase.GetItem(BannedItemName));
    }

    public override void Terminate()
    {
        BannedItemsHandler.Instance.UnbanItem(ItemDatabase.GetItem(BannedItemName));
    }
}
