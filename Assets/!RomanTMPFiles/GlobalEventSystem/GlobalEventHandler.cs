using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;



public class GlobalEventHandler : MonoBehaviour
{
    [Serializable]
    class EventTypeInformation
    {
        public EventTypeInformation(Type type) //TODO System.Type не является сериализуемым...
                                               //...но можно конвертировать в строку и обратно
        {
            Type = type;
            LastHappenedDay = -(int)Type.GetProperty("CooldownDays").GetValue(null); //Первый раз не может быть кд
            TimesNotRolled = 0;
        }
        public Type Type;
        public int LastHappenedDay;
        public int TimesNotRolled;
    }

    public static GlobalEventHandler Instance;

    [SerializeField] private List<string> _randomEventTypeNames;

    private List<EventTypeInformation> _randomEventTypeInformations = new();

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
        foreach(string typeName in _randomEventTypeNames)
        {
            _randomEventTypeInformations.Add(new(Type.GetType(typeName)));
        }
        GameTime.HourChanged += OnHourChanged;
        GameTime.DayChanged += OnDayChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
        GameTime.DayChanged -= OnDayChanged;
    }

    private List<Type> RollRandomEventTypes()
    {
        List<Type> result = new();

        foreach (EventTypeInformation selectedTypeInfo in _randomEventTypeInformations)
        {
            bool isOnCooldown = (selectedTypeInfo.LastHappenedDay + (int)selectedTypeInfo.Type
                .GetProperty("CooldownDays").GetValue(null)) > GameTime.CurrentDay;
            bool isConcurrent = (bool)selectedTypeInfo.Type.GetProperty("IsConcurrent").GetValue(null); //Если таких ивентов может быть несколько
            bool isActive = ActiveGlobalEvents.Exists(type => type.GetType() == selectedTypeInfo.Type); //Если ивента того же типа ещё нет в активных
            if (!isOnCooldown && (!isActive || isConcurrent))
            {
                //Попробовать рольнуть его шанс. Если не получится, добавить timesNotRolled.
                //Если получится, обнулить timesNotRolled.
                float baseChance = (float)selectedTypeInfo.Type.GetProperty("BaseChance").GetValue(null);
                bool isRolled = Random.Range(0f, 1f) < baseChance * (selectedTypeInfo.TimesNotRolled + 1);
                if (isRolled)
                {
                    selectedTypeInfo.LastHappenedDay = GameTime.CurrentDay;
                    selectedTypeInfo.TimesNotRolled = 0;
                    result.Add(selectedTypeInfo.Type);
                }
                else
                {
                    selectedTypeInfo.TimesNotRolled++;
                }
            }
        }
        return result;
    }
    public IGlobalEvent AddGlobalEvent(Type globalEventType, int durationHours)
    {
        if (globalEventType == null) return null;
        IGlobalEvent globalEvent = (IGlobalEvent)Activator.CreateInstance(globalEventType);
        globalEvent.DurationHours = durationHours;
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
        AddRandomEvents();
    }
    public void AddRandomEvents()
    {
        foreach (Type type in RollRandomEventTypes())
        {
            AddGlobalEvent(type, durationHours: Random.Range(0, 13));
        }
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
