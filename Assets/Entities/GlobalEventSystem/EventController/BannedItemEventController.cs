using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BannedItemEventController : MonoBehaviour, IEventController<GlobalEvent_BannedItem>, ISaveable<EventControllerSaveData>
{

    public int LastEventDay { get; set; }
    public int DurationOfEvent { get; set; }
    public List<Item> ItemsToBan { get; private set; }
    public int MinDelayToNextEvent => 5;

    public int MaxDelayToNextEvent => 7;

    public int DateOfNextEvent { get; set; }
    public int HourOfNextEvent { get; set; }

    public void PrepareEvent()
    {
        AddEvent();
        PredictNextEvent();
    }
    public GlobalEvent_BannedItem AddEvent()
    {
        GlobalEvent_BannedItem newEvent = new()
        {
            DurationHours = DurationOfEvent * 24,
            BannedItemNames = ItemsToBan.Select(item => item.Name).ToList(),
        };
        var eventToAdd = GlobalEventHandler.Instance.AddGlobalEvent(newEvent);
        LastEventDay = GameTime.CurrentDay;

        return eventToAdd;
    }

    public void RemoveEvent()
    {

    }

    public void PredictNextEvent()
    {
        DateOfNextEvent = LastEventDay + Random.Range(MinDelayToNextEvent, MaxDelayToNextEvent + 1);
        HourOfNextEvent = Random.Range(1, 24);
        DurationOfEvent = Random.Range(5, 15); //����
        int itemsToBanAmount = Random.Range(1, 4);
        Item randomItem = ItemDatabase.GetRandomItem(); //���� ��������� ������� ��� �� �������.
                                                        //������? ������ ��� �� ������ ���� ������������� ������������
                                                        //������� � �� ��� ������� ������� �������� ���� �� ����,
                                                        //������ ���� ����� ������� ���� ��������� ��� ��������.
                                                        //��� �������� � ���, ��� � ��� ���� ��� ����, ������� ������ ������� - ��� "Null" � "Chemicals" (�� ������ 26.12.23 � Chemicals ��� ���������)
        ItemsToBan = new(ItemDatabase.GetRandomUnbannedItemsOfThisType(randomItem.TypeOfItem, itemsToBanAmount));

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
