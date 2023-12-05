using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BannedItemEventController : MonoBehaviour, IEventController<GlobalEvent_BannedItem>, ISaveable<EventControllerSaveData>
{

    public int LastEventDay { get; set; }
    public int DurationOfEvent { get; set; }
    public Item ItemToBan { get; private set; }
    public int MinDelayToNextEvent => 7;

    public int MaxDelayToNextEvent => 9;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }

    public void PrepareEvent()
    {
        AddEvent();
        PredictNextEvent();
    }
    public GlobalEvent_BannedItem AddEvent()
    {
        GlobalEvent_BannedItem newEvent = new()
        {
            DurationHours = DurationOfEvent * 24,
            BannedItemName = ItemToBan.Name
        };
        var eventToAdd = GlobalEventHandler.Instance.AddGlobalEvent(newEvent);
        LastEventDay = GameTime.CurrentDay;
        
        return eventToAdd;
    }

    public void RemoveEvent()
    {

    }

    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24);
        DurationOfEvent = Random.Range(5, 9); //дней
        ItemToBan = ItemDatabase.GetRandomItem();
        if (BannedItemsHandler.Instance.IsItemBanned(ItemToBan))
        {
            ItemToBan = ItemDatabase.GetRandomItem(); 
            //ѕовторно роллитс€, чтобы уменьшить шансы на бан двух одинаковых предметов.
            //Ўанс и так невысок, а глубокие проверки чтобы достать точно не содержащийс€ рандомный айтем,
            //кажутс€ невыгодными. ѕоэтому просто роллитс€ два раза в этом случае
        }
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
