using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvent_Weather : IGlobalEvent
{
    public static bool IsConcurrent { get; } = false;

    private WeatherController _weatherController;
    public int DurationHours { get; set; }

    public int StrengthOfWeather;

    public void Execute()
    {
        _weatherController = Object.FindObjectOfType<WeatherController>();
        _weatherController.StartWeather();
    }

    public void Terminate()
    {
        _weatherController.StopWeather();
    }
}
