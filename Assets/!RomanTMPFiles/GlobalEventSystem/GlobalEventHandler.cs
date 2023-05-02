using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GlobalEventHandler : MonoBehaviour
{
    public static GlobalEventHandler Instance;
    public List<GlobalEvent_Base> ActiveGlobalEvents = new();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    public T AddGlobalEvent<T>(int durationHours) where T : GlobalEvent_Base, new()
    {
        T globalEvent = new()
        {
            DurationHours = durationHours
        };
        globalEvent.Execute();
        ActiveGlobalEvents.Add(globalEvent);
        return globalEvent;
    }
    public bool IsEventActive<T>() where T : GlobalEvent_Base
    {
        return ActiveGlobalEvents.Any(globalEvent => globalEvent.GetType() == typeof(T));
    }
    public T GetActiveEventByType<T>() where T : GlobalEvent_Base
    {
        return (T)ActiveGlobalEvents.FirstOrDefault(globalEvent => globalEvent.GetType() == typeof(T));
    }
    public T[] GetActiveEventsByType<T>() where T : GlobalEvent_Base
    {
        return ActiveGlobalEvents.OfType<T>().ToArray();
    }
    public GlobalEventHandlerSaveData SaveData()
    {
        GlobalEventHandlerSaveData saveData = new(ActiveGlobalEvents);
        return saveData;
    }
    public void LoadData(GlobalEventHandlerSaveData saveData)
    {
        foreach (GlobalEvent_Base globalEvent in saveData.SavedGlobalEvents)
        {
            ActiveGlobalEvents.Add(globalEvent);
            globalEvent.Execute();
        }
    }
}
