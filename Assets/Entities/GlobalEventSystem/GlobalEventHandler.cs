using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GlobalEventHandler : MonoBehaviour
{
    public static GlobalEventHandler Instance;
    [SerializeField] private List<GameObject> EventControllerGameObjects;
    [HideInInspector] public List<GlobalEvent_Base> ActiveGlobalEvents = new();
    private List<IEventController<GlobalEvent_Base>> _eventControllers = new();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        foreach (GameObject eventControllerObject in EventControllerGameObjects)
        {
            /* Type componentType = typeof(IEventController<>).MakeGenericType(typeof(GlobalEvent_Base));
             //����� ������� Reflection, 90% ���� ������� � ���� ������� ���� ��-�� ����� �����
             var eventController = (IEventController<GlobalEvent_Base>)eventControllerObject.GetComponent(componentType);
             _eventControllers.Add(eventController);
            */
        }
    }
    private void OnEnable()
    {
        GameTime.HourChanged += OnHourChanged;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;   
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
    public T AddGlobalEvent<T>(T createdEvent) where T : GlobalEvent_Base
        //��������������, ��� ������� ����� ������� ����� ����� ����� new � ��������� ��� ��� ����,
        //�.� Duration � ���������. ����� ����� �� ������ �����������
    {
        createdEvent.Execute();
        ActiveGlobalEvents.Add(createdEvent);
        return createdEvent;
    }
    public void StopEvent(GlobalEvent_Base activeEvent)
    {
        if (!ActiveGlobalEvents.Contains(activeEvent))
            return;
        activeEvent.DurationHours = 0;
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

    private void OnHourChanged()
    {
        CheckControllersDelay();
        DecreaseDurations();
    }

    private IEventController<T> GetControllerByType<T>() where T: GlobalEvent_Base
        //�� ���� �� �����, ����� ����� ����������
    {
        Type eventType = typeof(T);
        var controllerType = typeof(IEventController<>).MakeGenericType(eventType);
        return (IEventController<T>)_eventControllers.FirstOrDefault(controller => controller.GetType() == controllerType);
    }
    private IEventController<GlobalEvent_Base> GetControllerFromEvent(GlobalEvent_Base globalEvent)
    {
        Type eventType = globalEvent.GetType();
        Type controllerType = typeof(IEventController<>).MakeGenericType(eventType);
        //^ ��� ������ ���� Reflection, ��������� ������ �� ���� ����� ��������� ���-�� �����
        return _eventControllers.FirstOrDefault(controller => controller.GetType() == controllerType);
    }

    private void CheckControllersDelay()
    {
        int currentDay = GameTime.CurrentDay;
        int currentHour = GameTime.Hours;
        foreach (var controller in _eventControllers)
        {
            if (controller.DateOfNextEvent >= currentDay)
                if (controller.HourOfNextEvent >= currentHour)
                    controller.AddEvent();
        }
    }
    private void DecreaseDurations() //��������� ������� � ������� �������� � ���������� �������
    {
        ActiveGlobalEvents.ForEach(globalEvent =>
        {
            globalEvent.DurationHours--;

            if (globalEvent.DurationHours <= 0)
            {
                globalEvent.Terminate();

                IEventController<GlobalEvent_Base> eventController = GetControllerFromEvent(globalEvent);
                eventController?.RemoveEvent();
            }
        });
        ActiveGlobalEvents.RemoveAll(globalEvent => globalEvent.DurationHours <= 0);
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
