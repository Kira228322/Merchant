using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanLiftEventController : MonoBehaviour, IEventController<GlobalEvent_BanLift>, ISaveable<EventControllerSaveData>
{
    public int MinDelayToNextEvent => 27;

    public int MaxDelayToNextEvent => 35;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }
    public int DurationOfEvent { get; set; }
    public int LastEventDay { get; set; }

    public GlobalEvent_BanLift AddEvent()
    {
        LastEventDay = GameTime.CurrentDay;
        int bannedItemsCount = BannedItemsHandler.Instance.BannedItems.Count;
        if (bannedItemsCount == 0)
            return null;
        string itemToUnBanName = BannedItemsHandler.Instance.BannedItems[Random.Range(0, bannedItemsCount)].Name;
        GlobalEvent_BanLift newEvent = new()
        {
            DurationHours = DurationOfEvent,
            BanLiftedItemName = itemToUnBanName,
        };
        var eventToAdd = GlobalEventHandler.Instance.AddGlobalEvent(newEvent);
        return eventToAdd;
    }

    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24);
        DurationOfEvent = 24; //одноразовый ивент, но у игрока будет 24 часа чтобы заметить объявление на доске.
    }

    public void PrepareEvent()
    {
        AddEvent();
        PredictNextEvent();
    }

    public void RemoveEvent()
    {

    }

    public EventControllerSaveData SaveData()
    {
        EventControllerSaveData saveData = new(LastEventDay);
        return saveData;
    }

    public void LoadData(EventControllerSaveData data)
    {
        LastEventDay = data.LastEventDay;
    }
}
