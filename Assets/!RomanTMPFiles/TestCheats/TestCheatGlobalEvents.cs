using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestCheatGlobalEvents : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown newEventSelector;
    [SerializeField] private TMP_Text informationField;
    [SerializeField] private TMP_Text currentTime;
    private List<IEventController> eventControllers = new();

    private void OnEnable()
    {
        GameTime.HourChanged += Refresh;
    }
    private void OnDisable()
    {
        GameTime.HourChanged -= Refresh;
    }

    private void Start()
    {
        foreach (IEventController eventController in GlobalEventHandler.Instance.EventControllers)
        {
            eventControllers.Add(eventController);
        }

        List<string> controllerNames = new();
        foreach (IEventController eventController in eventControllers)
        {
            controllerNames.Add(eventController.GetType().ToString());
        }
        newEventSelector.AddOptions(controllerNames);
        Refresh();
    }

    public void Refresh()
    {
        currentTime.text = $"День {GameTime.CurrentDay}, {GameTime.Hours} часов";
        informationField.text = "";
        foreach (IEventController eventController in eventControllers)
        {
            informationField.text += $"<color=red>{eventController.GetType()}</color>: " +
                $"Следующий ивент запланирован на день " +
                $"{eventController.DateOfNextEvent}, час " +
                $"{eventController.HourOfNextEvent}. Запланированная " +
                $"длительность ивента: {eventController.DurationOfEvent}. " +
                $"Задержка между ивентами: {eventController.MinDelayToNextEvent}-" +
                $"{eventController.MaxDelayToNextEvent} дней. " +
                $"Последний ивент этого типа был в день {eventController.LastEventDay}\n";
        }
        informationField.text += "\n <color=green>Активные ивенты:</color> \n";
        foreach (GlobalEvent_Base globalEvent in GlobalEventHandler.Instance.ActiveGlobalEvents)
        {
            informationField.text += $"" +
                $"{(globalEvent.GlobalEventName == "" || globalEvent.GlobalEventName == null? "Безымянный": globalEvent.GlobalEventName)}. " +
                $"Осталось {globalEvent.DurationHours} часов. {globalEvent.Description}\n";
        }
    }

    public void OnAddEventButtonClick()
    {
        eventControllers[newEventSelector.value].PrepareEvent();
        Refresh();
    }

    public void OnNpcDatabaseCheckClick()
    {
        //Это не тот скрипт, ну да мне лень делать отдельный для одного метода. Похуй, это же чит-панель
        List<NpcData> sortedList = new(NpcDatabase.Instance.NpcDatabaseSO.NpcList);
        sortedList = sortedList.OrderBy(npc => npc.ID).ToList();
        foreach (var npc in sortedList)
        {
            Debug.Log($"ID {npc.ID}: {npc.Name}. Денег: Сейчас - {npc.CurrentMoney}, старт игры: {npc.GameStartMoney}");
        }

    }
}
