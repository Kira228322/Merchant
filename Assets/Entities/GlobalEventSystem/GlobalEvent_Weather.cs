using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_Weather : GlobalEvent_Base
{
    public override string GlobalEventName => null;
    public override string Description => null;
    public int StrengthOfWeather { get; set; }
    public override void Execute()
    {
        //Ну по логике, дождь включается уже и так в WeatherController.
        //Ничего делать не нужно, иначе всё-таки странно выходит.
    }

    public override void Terminate() 
    {
        UnityEngine.Object.FindObjectOfType<WeatherController>().RemoveEvent();
    }
}
