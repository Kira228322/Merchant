using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_BannedItem : GlobalEvent_Base
{
    public string BannedItemName;
    public override string GlobalEventName => "Предмет запрещён";

    public override string Description => $"Начиная с этого дня, власти запрещают предмет {BannedItemName}. Дата окончания запрета: {GetAddedTime(DurationHours)}";

    private string GetAddedTime(int hoursToAdd)
    {
        //Чтобы в доске объявлений отображось время конца ивента, а не сколько часов осталось
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

        return $"День {daysToDisplay}, {(hoursToDisplay < 10 ? $"0{hoursToDisplay}": $"{hoursToDisplay}")}:00";
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
