using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTime
{
    private static int _days = 1;
    private static int _hours = 0;
    private static int _minutes = 0;
    private static float _seconds = 0f;
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
            _hours = value;
            if (_hours >= 24) 
            {
                _hours = 0;
                Days++;
            }
        }
    }
    public static int Minutes
    {
        get
        { return _minutes; }
        set
        {
            _minutes = value;
            if (_minutes >= 60)
            {
                _minutes = 0;
                Hours++;
            }
        }
    }
    public static float Seconds
    {
        get
        { return _seconds; }
        set
        {
            _seconds = value;
            if (_seconds >= 60f)
            {
                _seconds = 0f;
                Minutes++;
            }
        }
    }

    public static void TimeSet(int days, int hours, int minutes, int seconds)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }
}
