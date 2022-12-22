using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TravelEventHandler : MonoBehaviour
{
    // Скрипт, который будет отвечать за генерацию событий в поездке
    [SerializeField] private TravelTimeCounter _timeCounter;
    [SerializeField] private EventWindow _eventWindow;
    [SerializeField] private Mover _mover;
    [SerializeField] private RandomBGGenerator _generator;

    [SerializeField] private List<EventInTravel> _eventsInTravels = new ();
    [SerializeField] private EventInTravel _eventInTravelBandits;
    private EventInTravel _nextEvent;
    private int _delayToNextEvent;

    private bool _banditEvent;


    public void StartEventIfTimerExpired() 
    {
        _delayToNextEvent--;
        if (_delayToNextEvent == 0)
        {
            EventStart(_nextEvent);
            RollNextEvent();
        }
    }
    
    private void EventStart(EventInTravel eventInTravel)
    {
        _generator.enabled = false;
        foreach (var cloud in _generator.CloudsOnScene)
        {
            if (cloud != null)
                cloud.enabled = false;
        }
        _mover.enabled = false;
        GameTime.SetTimeScale(0);
        _eventWindow.gameObject.SetActive(true);
        _eventWindow.Init(eventInTravel);
    }

    public void EventEnd()
    {
        _generator.enabled = true;
        foreach (var cloud in _generator.CloudsOnScene)
        {
            if (cloud != null)
                cloud.enabled = true;
        }
        _mover.enabled = true;
        GameTime.SetTimeScale(GameTime.TimeScaleInTravel);
        
        StartCoroutine(_eventWindow.EventEnd());
    }

    public void OnTravelSceneEnter()
    {
        if (EventFire(MapManager.CurrentRoad.Danger, false, true))
            _banditEvent = true;
        else _banditEvent = false;
        
        RollNextEvent();
    }

    private void RollNextEvent()
    {
        _delayToNextEvent = 2; // события максимум каждые 2 часа или реже  
        while (_delayToNextEvent < _timeCounter.Duration-2) // За 2 часа до конца поездки ивента быть не может
        {
            if (_banditEvent) // ролим событие разбiйники, если оно должно случиться
                if (Random.Range(0, _timeCounter.Duration-2 - _delayToNextEvent) == 0)
                {
                    _banditEvent = false;
                    _nextEvent = _eventInTravelBandits;
                    break;
                }

            if (Random.Range(0, Convert.ToInt32(Math.Floor(
                    28/(Math.Pow(_delayToNextEvent, 0.2f) + Math.Pow(_delayToNextEvent, 0.8f)))) -2) == 0)
            {
                _nextEvent = ChooseEvent();
                break;
            }
            
            _delayToNextEvent++;
        }

        if (_delayToNextEvent == _timeCounter.Duration - 1)
            _delayToNextEvent += 10; // событие не случится 
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

    public static bool EventFire(float probability, bool positiveEvent = true, bool luckMultiplier = false)  
    {
        if (luckMultiplier)
        {
            if (positiveEvent)
            {
                if (Random.Range(0, 101) <= probability * Player.Singleton.Statistics.GetCoefForPositiveEvent())
                    return true;
                return false;
            }
            if (Random.Range(0, 101) <= probability * Player.Singleton.Statistics.GetCoefForNegativeEvent())
                return true;
            return false;
        }
        if (Random.Range(0, 101) <= probability)
            return true;
        return false;
    }
}
