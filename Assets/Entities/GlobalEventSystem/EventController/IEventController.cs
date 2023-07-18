using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventController
{
    int MinDelayToNextEvent { get; }
    int MaxDelayToNextEvent { get; }

    int DateOfNextEvent { get; set; }
    int HourOfNextEvent { get; set; }
    int DurationOfEvent { get; set; }
    int LastEventDay { get; set; }

    void PrepareEvent();
    void PredictNextEvent();
    void RemoveEvent();
}
public interface IEventController<T> : IEventController where T : GlobalEvent_Base
{
    T AddEvent();
}

