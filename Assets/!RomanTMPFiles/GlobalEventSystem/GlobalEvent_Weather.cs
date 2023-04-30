using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Weather : IGlobalEvent
{

    [NonSerialized] private WeatherController _weatherController;
    public int DurationHours { get; set; }

    public int StrengthOfWeather;
    public void Execute()
    {
        _weatherController = UnityEngine.Object.FindObjectOfType<WeatherController>();
        _weatherController.StartWeather();
    }

    public void Terminate()
    {
        _weatherController.StopWeather();
    }
}
