using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class GlobalEventManager : MonoBehaviour
{
    public static GlobalEventManager Instance;

    private GlobalEventFactory _eventFactory = new();

    public List<GlobalEvent> ActiveGlobalEvents = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }


    public void CreateRandomEvent()
    {
        GlobalEvent.GlobalEventType eventType = GetRandomEventType();
        GlobalEvent newEvent = _eventFactory.CreateEvent(eventType, GetEventParameters(eventType));
        ActiveGlobalEvents.Add(newEvent);
        newEvent.Execute();
    }

    private GlobalEvent.GlobalEventType GetRandomEventType()
    {
        return (GlobalEvent.GlobalEventType)Random.Range(0, 2);
    }

    private object[] GetEventParameters(GlobalEvent.GlobalEventType eventType)
    {
        switch (eventType)
        {
            case GlobalEvent.GlobalEventType.Weather:
                return new object[] { Random.Range(0, 3), FindObjectOfType<WeatherController>() };
            default:
                Debug.LogError("Такого типа ивента нет в этом Switch");
                return null;
        }
    }
}
