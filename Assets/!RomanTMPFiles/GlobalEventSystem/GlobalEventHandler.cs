using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class GlobalEventHandler : MonoBehaviour
{
    public static GlobalEventHandler Instance;
    [SerializeField] private List<string> _globalEventTypeNames;
    private List<Type> _globalEventTypes = new();

    public List<IGlobalEvent> ActiveGlobalEvents = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        foreach(string typeName in _globalEventTypeNames)
        {
            _globalEventTypes.Add(Type.GetType(typeName));
        }
        GameTime.HourChanged += OnHourChanged;
        GameTime.DayChanged += OnDayChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
        GameTime.DayChanged -= OnDayChanged;
    }

    private Type RollRandomEventType()
    {
        List<Type> uncheckedTypes = new(_globalEventTypes);
        while (uncheckedTypes.Count > 0)
        {
            int randomIndex = Random.Range(0, uncheckedTypes.Count);
            Type selectedType = uncheckedTypes[randomIndex];

            bool isConcurrent = (bool)selectedType.GetProperty("IsConcurrent").GetValue(null); //Если таких ивентов может быть несколько
            bool isActive = ActiveGlobalEvents.Exists(type => type.GetType() == selectedType); //Если ивента того же типа ещё нет в активных
            if (!isActive || isConcurrent)
            {
                return selectedType;
            }
            uncheckedTypes.RemoveAt(randomIndex); //не подходящий, ищем другие
        }
        return null; //не найдено подходящих глобалИвентов
    }
    public IGlobalEvent AddGlobalEvent(Type globalEventType, int durationHours)
    {
        if (globalEventType == null) return null;
        IGlobalEvent globalEvent = (IGlobalEvent)Activator.CreateInstance(globalEventType);
        globalEvent.DurationHours = durationHours;
        globalEvent.Execute();
        ActiveGlobalEvents.Add(globalEvent);
        return globalEvent;
    }
    

    private void OnHourChanged() //уменьшить счётчик у каждого элемента и остановить истёкшие
    {
        ActiveGlobalEvents.ForEach(globalEvent =>
        {
            globalEvent.DurationHours--;

            if (globalEvent.DurationHours <= 0)
            {
                globalEvent.Terminate();
            }
        });
        ActiveGlobalEvents.RemoveAll(globalEvent => globalEvent.DurationHours <= 0);
    }
    private void OnDayChanged()
    {
        AddRandomEvent();
    }
    public void AddRandomEvent()
    {
        AddGlobalEvent(RollRandomEventType(), Random.Range(0, 12));
    }

    public GlobalEventHandlerSaveData SaveData()
    {
        GlobalEventHandlerSaveData saveData = new(ActiveGlobalEvents);
        return saveData;
    }
    public void LoadData(GlobalEventHandlerSaveData saveData)
    {
        foreach (IGlobalEvent globalEvent in saveData.SavedGlobalEvents)
        {
            ActiveGlobalEvents.Add(globalEvent);
            globalEvent.Execute();
        }
    }

}
