using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DangerousRoadEventController : MonoBehaviour, IEventController<GlobalEvent_DangerousRoad>
{
    public int MinDelayToNextEvent => 3;

    public int MaxDelayToNextEvent => 5;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }
    public int DurationOfEvent { get; set; }
    public int LastEventDay { get; set; }
    public float MultiplyCoefficient { get; private set; }
    public Road TargetRoad { get; private set; }
    [HideInInspector] public List<Road> PossibleRoads = new();

    private void Awake()
    {
        PossibleRoads = FindObjectsByType<Road>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public GlobalEvent_DangerousRoad AddEvent()
    {
        GlobalEvent_DangerousRoad newEvent = new()
        {
            DurationHours = DurationOfEvent,
            FirstPointLocationSceneName = TargetRoad.Points[0].SceneName,
            SecondPointLocationSceneName = TargetRoad.Points[1].SceneName,
            TargetRoadName = TargetRoad.RoadName,
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
        DurationOfEvent = 24;
        MultiplyCoefficient = Random.Range(1.5f, 1.8f); 
        List<Road> sameRegionRoads = PossibleRoads.Where(
            road => 
            road.Points[0].Region == MapManager.CurrentRegion || 
            road.Points[1].Region == MapManager.CurrentRegion).ToList();
        TargetRoad = sameRegionRoads[Random.Range(0, sameRegionRoads.Count)];
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
