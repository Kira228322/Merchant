using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TravelEventHandler : MonoBehaviour
{
    // —крипт, который будет отвечать за генерацию событий в поездке
    [SerializeField] private TravelTimeCounter _timeCounter;
    [SerializeField] private EventWindow _eventWindow;
    [SerializeField] private Mover _mover;
    [SerializeField] private RandomBGGenerator _generator;

    [SerializeField] private List<EventInTravel> _eventsInTravels = new ();
    [SerializeField] private EventInTravel _eventInTravelBandits;
    private EventInTravel _nextEvent;

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
        
        RollNextEvent();
    }

    private void RollNextEvent()
    {
        int delayToNextEvent = 2; // событи€ максимум каждые 2 часа или реже  
        while (delayToNextEvent < _timeCounter.Duration-1) // «а 2 часа до конца поездки ивента быть не может
        {
            if (_banditEvent) // ролим событие разбiйники, если оно должно случитьс€
                if (Random.Range(0, _timeCounter.Duration-2 - delayToNextEvent) == 0)
                {
                    _banditEvent = false;
                    _nextEvent = _eventInTravelBandits;
                    break;
                }

            if (Random.Range(0, Convert.ToInt32(Math.Floor(
                    28/(Math.Pow(delayToNextEvent, 0.2f) + Math.Pow(delayToNextEvent, 0.8f)))) + 1) == 0)
            {
                _nextEvent = ChooseEvent();
                break;
            }

            delayToNextEvent++;
        }
    }

    private EventInTravel ChooseEvent()
    {
        List<int> index = new List<int>();
        int max = 0;
        for (int i = 0; i < _eventsInTravels.Count; i++)
            if (max < _eventsInTravels[i].Weight)
                max = _eventsInTravels[i].Weight;

        for (int i = 0; i < _eventsInTravels.Count; i++)
            if (max == _eventsInTravels[i].Weight)
                index.Add(i);

        return _eventsInTravels[index[Random.Range(0, index.Count)]];
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
