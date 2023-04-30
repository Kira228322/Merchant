using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class GameTime
{

    private static Timeflow _timeflow;
    private static float _timeScaleInTravel = 15; 
    public static float TimeScaleInTravel => _timeScaleInTravel; 
    private static int _currentDay = 1;
    private static int _hours = 0;
    private static int _minutes = 0;

    public static event UnityAction HourChanged;
    public static event UnityAction MinuteChanged;
    public static event UnityAction DayChanged;

    public static int CurrentDay
    {
        get => _currentDay;
        set
        {
            _currentDay = value;
        }
    }
    public static int Hours
    {
        get { return _hours;}
        set
        {
            _hours = value;
            if (_hours >= 24) 
            {
                _hours = 0;
                CurrentDay++;
                DayChanged?.Invoke();
            }
        }
    }
    public static int Minutes
    {
        get => _minutes; 
        set
        {
            _minutes = value;
            if (_minutes >= 60)
            {
                _minutes = 0;
                Hours++;
                HourChanged?.Invoke();
            }
            MinuteChanged?.Invoke();
        }
    }

    public static void TimeSet(int days, int hours, int minutes)
    {
        CurrentDay = days;
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

    public static TimeFlowSaveData SaveData()
    {
        return _timeflow.SaveData();
    }
    public static void LoadData(TimeFlowSaveData data)
    {
        _timeflow.LoadData(data);
    }
}
