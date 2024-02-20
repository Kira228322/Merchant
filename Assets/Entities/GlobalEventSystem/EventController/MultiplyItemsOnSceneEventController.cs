using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplyItemsOnSceneEventController : MonoBehaviour, IEventController<GlobalEvent_MultiplyItemsOnScene>, ISaveable<EventControllerSaveData>
{
    public int MinDelayToNextEvent => 5;
    public int MaxDelayToNextEvent => 9;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }
    public int DurationOfEvent { get; set; }
    public int LastEventDay { get; set; }

    [HideInInspector] public Location Location;
    [HideInInspector] public float MultiplyCoefficient;
    [HideInInspector] public string SelectedItem;

    [SerializeField] private bool _isPositive;

    public List<Location> PossibleLocations = new();
    public GlobalEvent_MultiplyItemsOnScene AddEvent()
    {
        GlobalEvent_MultiplyItemsOnScene newEvent = new()
        {
            DurationHours = DurationOfEvent,
            LocationSceneName = Location.SceneName,
            LocationVillageName = Location.VillageName,
            MultiplyCoefficient = MultiplyCoefficient,
            ItemToMultiplyName = SelectedItem,
            IsPositive = _isPositive,
        };
        var eventToAdd = GlobalEventHandler.Instance.AddGlobalEvent(newEvent);
        LastEventDay = GameTime.CurrentDay;

        return eventToAdd;
    }

    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24);
        DurationOfEvent = 24; //Этот ивент одноразовый, но у игрока будет 24 часа чтобы заметить объявление на доске

        if (_isPositive)
            MultiplyCoefficient = Random.Range(1.4f, 1.6f);
        else
            MultiplyCoefficient = Random.Range(0.6f, 0.8f);

        List<Location> sameRegionLocations = PossibleLocations.Where(location => location.Region == MapManager.CurrentRegion).ToList(); 
        Location = sameRegionLocations[Random.Range(0, sameRegionLocations.Count)];
        SelectedItem = SelectApplicableItem();
    }

    public void PrepareEvent()
    {
        AddEvent();
        PredictNextEvent();
    }

    public void RemoveEvent()
    {

    }

    private string SelectApplicableItem()
    {
        var sortedItems = Location.NpcTraders.SelectMany(trader => trader.Goods)
            .GroupBy(item => item.Good)
            .Select(group => new
            {
                Item = group.Key,
                TotalCount = group.Sum(item => item.MaxCount * item.Good.Price)
            })
            .OrderByDescending(group => group.TotalCount)
            .ToList();
        
        return sortedItems[Random.Range(0, 5)].Item.Name;
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
