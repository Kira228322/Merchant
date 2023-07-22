using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloodEventController : MonoBehaviour, IEventController<GlobalEvent_Flood>
{
    public int MinDelayToNextEvent => 6;
    public int MaxDelayToNextEvent => 13;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }
    public int DurationOfEvent { get; set; }
    public int LastEventDay { get; set; }

    [HideInInspector] public Location Location;

    public List<Location> PossibleLocations = new();
    //TODO: по готовности локаций вставить в инспекторе те, которые близко к воде и имеют риск наводнения
    public GlobalEvent_Flood AddEvent()
    {
        GlobalEvent_Flood newEvent = new()
        {
            DurationHours = DurationOfEvent,
            Location = Location,
        };
        var eventToAdd = GlobalEventHandler.Instance.AddGlobalEvent(newEvent);
        LastEventDay = GameTime.CurrentDay;

        return eventToAdd;
    }

    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24);
        DurationOfEvent = 1; //Этот ивент одноразовый, а не длящийся
        List<Location> sameRegionLocations = PossibleLocations.Where(location => location.Region == MapManager.CurrentRegion).ToList();
        Location = sameRegionLocations[Random.Range(0, sameRegionLocations.Count)];
    }

    public void PrepareEvent()
    {
        AddEvent();
        PredictNextEvent();
    }

    public void RemoveEvent()
    {
        
    }
}
