using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TravelEventHandler : MonoBehaviour
{
    // Скрипт, который будет отвечать за генерацию событий в поездке
    [SerializeField] private EventWindow _eventWindow;
    [SerializeField] private Mover _mover;
    [SerializeField] private RandomBGGenerator _generator;

    private bool _banditEvent;
    
    
    
    private void EventStart(EventInTravel eventInTravel)
    {
        _generator.enabled = false;
        foreach (var cloud in _generator.CloudsOnScene)
        {
            cloud.enabled = false;
        }
        _mover.enabled = false;
        GameTime.SetTimeScale(0);
        _eventWindow.gameObject.SetActive(true);
        _eventWindow.Init(eventInTravel);
    }

    private void EventEnd()
    {
        _generator.enabled = true;
        foreach (var cloud in _generator.CloudsOnScene)
        {
            cloud.enabled = true;
        }
        _mover.enabled = true;
        GameTime.SetTimeScale(GameTime.TimeScaleInTravel);
        _eventWindow.gameObject.SetActive(false);
    }

    private void OnTravelSceneEnter()
    {
        if (EventFire(MapManager.CurrentRoad.Danger, false, true))
            _banditEvent = true;
        else _banditEvent = false;
        
        // Еще думаю каким образом будет работать определение ивентов в дороге
        // Что точно должно быть, это вычисление будет ли нападение бандитов или нет
        // TODO
    }

    private bool EventFire(float value, bool positiveEvent = true, bool luckImpl = false)  
    {
        if (luckImpl)
        {
            if (positiveEvent)
            {
                if (Random.Range(0, 101) <= value * Player.Singleton.Statistics.GetCoefForPositiveEvent())
                    return true;
                return false;
            }
            if (Random.Range(0, 101) <= value * Player.Singleton.Statistics.GetCoefForNegativeEvent())
                return true;
            return false;
        }
        if (Random.Range(0, 101) <= value)
            return true;
        return false;
    }
}
