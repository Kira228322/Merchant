using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvent_Weather : GlobalEvent
{
    public int StrengthOfWeather;
    private WeatherController _weatherController;

    public GlobalEvent_Weather(int strengthOfWeather, WeatherController weatherController)
    {
        StrengthOfWeather = strengthOfWeather;
        _weatherController = weatherController;
    }
    public override void Execute()
    {
        Debug.Log("Rain tipa started");
    }
    public override void Terminate()
    {
        Debug.Log("Rain tipa stopped");
    }

}
