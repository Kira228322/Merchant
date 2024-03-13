using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalEventHandler : MonoBehaviour
{
    public static GlobalEventHandler Instance;
    [SerializeField] private List<MonoBehaviour> EventControllerGameObjects;
    [HideInInspector] public List<GlobalEvent_Base> ActiveGlobalEvents = new();
    private List<IEventController> _eventControllers = new();
    public List<IEventController> EventControllers => _eventControllers;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    public void Initialize()
    {
        foreach (MonoBehaviour eventControllerObject in EventControllerGameObjects)
        {
            IEventController controller = (IEventController)eventControllerObject;
            controller.PredictNextEvent();
            _eventControllers.Add(controller);
        }
    }
    public void ResetEvents()
    {
        //Удаляет имеющиеся ивенты и обновляет предикты. Используется при загрузке/начале игры

        for (int i = ActiveGlobalEvents.Count - 1; i >= 0; i--)
        {
            ActiveGlobalEvents[i].Terminate();
            ActiveGlobalEvents.RemoveAt(i);
        }

        foreach (IEventController controller in EventControllers)
        {
            controller.LastEventDay = 0;
            controller.PredictNextEvent();
        }
    }
    private void OnEnable()
    {
        GameTime.HourChanged += OnHourChanged;
        GameTime.TimeSkipped += OnTimeSkipped;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= OnHourChanged;
        GameTime.TimeSkipped -= OnTimeSkipped;
    }
    public T AddGlobalEvent<T>(int durationHours) where T : GlobalEvent_Base, new()
    {
        T globalEvent = new()
        {
            DurationHours = durationHours
        };
        if (SceneManager.GetActiveScene().name == "MainMenu" && globalEvent is not GlobalEvent_Weather)
        {
            return null;
        }
        globalEvent.Execute();
        ActiveGlobalEvents.Add(globalEvent);
        return globalEvent;
    }
    public T AddGlobalEvent<T>(T createdEvent) where T : GlobalEvent_Base
        //Предполагается, что сначала нужно создать такой ивент через new и назначить ему все поля,
        //т.е Duration и остальные. После этого он просто запускается
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" && createdEvent is not GlobalEvent_Weather)
        {
            Debug.LogWarning("В главном меню ивенты кроме дождя не запускаются");
            return null;
        }
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
    private void OnTimeSkipped(int skippedDays, int skippedHours, int skippedMinutes)
    {
        int totalSkippedHours = skippedDays * 24 + skippedHours + skippedMinutes / 60;
        CheckControllersDelay();
        DecreaseDurations(totalSkippedHours);
    }

    private void CheckControllersDelay()
    {
        int currentDay = GameTime.CurrentDay;
        int currentHour = GameTime.Hours;
        foreach (var controller in _eventControllers)
        {
            if (currentDay >= controller.DateOfNextEvent)
                if (currentHour >= controller.HourOfNextEvent)
                {
                    controller.PrepareEvent();
                }
        }
    }
    private void DecreaseDurations() //уменьшить счётчик у каждого элемента и остановить истёкшие
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
    private void DecreaseDurations(int skippedHours)
    {
        ActiveGlobalEvents.ForEach(globalEvent =>
        {
            globalEvent.DurationHours -= skippedHours;

            if (globalEvent.DurationHours <= 0)
            {
                globalEvent.Terminate();

            }
        });
        ActiveGlobalEvents.RemoveAll(globalEvent => globalEvent.DurationHours <= 0);
    }

    public GlobalEventHandlerSaveData SaveData()
    {
        GlobalEventHandlerSaveData saveData = new(ActiveGlobalEvents, _eventControllers);
        return saveData;
    }
    public void LoadData(GlobalEventHandlerSaveData saveData)
    {
        foreach (GlobalEvent_Base globalEvent in saveData.SavedGlobalEvents)
        {
            ActiveGlobalEvents.Add(globalEvent);
            globalEvent.Execute();
        }
        for (int i = 0; i < saveData.EventControllerSaveDatas.Count; i++)
        {
            //Порядок контроллеров неизменный, что в сейве, что актуально
            _eventControllers[i].LastEventDay = saveData.EventControllerSaveDatas[i].LastEventDay;

            //После загрузки стоит перепредиктить ивенты
            _eventControllers[i].PredictNextEvent();
        }
    }
}
