using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GlobalEvent_BanLift : GlobalEvent_Base
{
    public override string GlobalEventName => $"Отмена запрета на {string.Join(", ", CurrentEventItemNames)}";

    public override string Description => $"Властями было принято решение снять существующий запрет на следующие предметы: {string.Join(", ", CurrentEventItemNames)}. Теперь эти предметы разрешено хранить и провозить.";

    public string BanLiftedItemName;

    public List<string> CurrentEventItemNames;

    public override void Execute()
    {
        GlobalEvent_BannedItem foundEvent = (GlobalEvent_BannedItem)GlobalEventHandler.Instance.ActiveGlobalEvents.FirstOrDefault
            (globalEvent => globalEvent is GlobalEvent_BannedItem bannedItemEvent
            && bannedItemEvent.BannedItemNames.Contains(BanLiftedItemName));
        if (foundEvent == null)
        {
            Debug.LogError("Не было ивента забаненного предмета чтобы его убрать");
            return;
        }
        CurrentEventItemNames = new(foundEvent.BannedItemNames);
        foundEvent.DurationHours = -1; //дальше с ним разберется GlobalEventHandler
    }

    public override void Terminate()
    {
        //Одноразовый ивент. Ничего не происходит
    }
}
