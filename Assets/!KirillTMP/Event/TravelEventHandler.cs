using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelEventHandler : MonoBehaviour
{
    // Скрипт, который будет отвечать за генерацию событий в поездке
    [SerializeField] private EventWindow _eventWindow;
    [SerializeField] private Mover _mover;
    [SerializeField] private RandomBGGenerator _generator;

    public EventInTravel TEST;
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
        GameTime.SetTimeScale(TravelManager.TimeScale);
        _eventWindow.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            EventStart(TEST);
        }
    }
}
