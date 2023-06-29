using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeatherControllerSaveData
{
    public int LastEventDay;
    public int DateOfPrecipitation;
    public int HourOfPrecipitation;
    public int StrengthOfWeather;

    public WeatherControllerSaveData(int lastEventDay, int dateOfPrecipitation, int hourOfPrecipitation, int strengthOfWeather)
    {
        LastEventDay = lastEventDay;
        DateOfPrecipitation = dateOfPrecipitation;
        HourOfPrecipitation = hourOfPrecipitation;
        StrengthOfWeather = strengthOfWeather;
    }
}
