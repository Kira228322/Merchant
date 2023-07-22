using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlentifulHarvestEventController : MonoBehaviour, IEventController<GlobalEvent_PlentifulHarvest>
{
    public int MinDelayToNextEvent => 6;
    public int MaxDelayToNextEvent => 13;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }
    public int DurationOfEvent { get; set; }
    public int LastEventDay { get; set; }

    [HideInInspector] public Location Location;
    [HideInInspector] public float MultiplyCoefficient;

    public List<Location> PossibleLocations = new();
    //TODO: по готовности локаций вставить в инспекторе те, которые имеют хорошие условия для роста продуктов
    public GlobalEvent_PlentifulHarvest AddEvent()
    {
        GlobalEvent_PlentifulHarvest newEvent = new()
        {
            DurationHours = DurationOfEvent,
            Location = Location,
            MultiplyCoefficient = MultiplyCoefficient,
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
        MultiplyCoefficient = Random.Range(1.2f, 1.6f); //TODO баланс коэффициента
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
