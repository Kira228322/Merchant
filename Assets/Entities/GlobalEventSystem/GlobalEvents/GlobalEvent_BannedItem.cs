using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_BannedItem : GlobalEvent_Base
{
    public List<string> BannedItemNames;
    public override string GlobalEventName => "������� ��������";
    public override string Description => $"������� � ����� ���, ������ ��������� ��������� ��������: {string.Join(", ", BannedItemNames)}. ���� ��������� �������: {GetAddedTime(DurationHours)}";

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
        foreach (string BannedItemName in BannedItemNames)
            BannedItemsHandler.Instance.BanItem(ItemDatabase.GetItem(BannedItemName));
    }

    public override void Terminate()
    {
        foreach (string BannedItemName in BannedItemNames)
            BannedItemsHandler.Instance.UnbanItem(ItemDatabase.GetItem(BannedItemName));
    }
}
