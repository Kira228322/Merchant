using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTime
{
    private static int _days = 1;
    private static int _hours = 0;
    private static float _minutes = 0f;
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
        }
    }

    public static void TimeSet(int days, int hours, int minutes)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
    }
}
