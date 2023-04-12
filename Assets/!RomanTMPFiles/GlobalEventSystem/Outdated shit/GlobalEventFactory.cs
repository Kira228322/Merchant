using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventFactory
{
    public GlobalEvent CreateEvent(GlobalEvent.GlobalEventType eventType, object[] parameters)
    {
        switch (eventType)
        {
            case GlobalEvent.GlobalEventType.Weather:
                return new GlobalEvent_Weather((int)parameters[0], (WeatherController)parameters[1]);
            default:
                Debug.LogError("������ ���� ������ ��� � ���� Switch");
                return null;
        }
    }
}
