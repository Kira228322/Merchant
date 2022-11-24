using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class GameTime
{
    private static Timeflow _timeflow;
    private static int _days = 1;
    private static int _hours = 0;
    private static float _minutes = 0f;

    public static event UnityAction HourChanged;
    public static event UnityAction MinuteChanged;

    public static int Days
    {
        get
        { return _days; }
        set
        {
            _days = value;
        }
    }
    public static int Hours
    {
        get
        { return _hours; }
        set
        {
            //TradeManager.PlayersInventory.CheckSpoilItems(); “еперь сам PlayersInventory занимаетс€ этим по событию HourChanged
            _hours = value;
            if (_hours >= 24) 
            {
                _hours = 0;
                Days++;
            }
            HourChanged.Invoke();
        }
    }
    public static float Minutes
    {
        get
        { return _minutes; }
        set
        {
            _minutes = value;
            if (_minutes >= 60f)
            {
                _minutes = 0f;
                Hours++;
            }
            MinuteChanged.Invoke();
        }
    }

    public static void TimeSet(int days, int hours, int minutes)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
    }

    public static void Init(Timeflow timeflow)
    {
        _timeflow = timeflow;
    }

    public static void SetTimeScale(float value)
    {
        _timeflow.TimeScale = value;
    }

    public static float GetTimeScale()
    {
        return _timeflow.TimeScale;
    }
}
