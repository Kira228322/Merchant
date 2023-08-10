using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class EventInTravel : MonoBehaviour
{
    public string EventName;
    public int Weight;
    [TextArea(2,6)]public string Description;
    [HideInInspector] public List<string> ButtonsLabel; // задается количество кнопок + их описание 
    [SerializeField] private Transform _celestialBodies;
    protected EventWindow _eventWindow;
    

    protected virtual void Start()
    {
        _celestialBodies.rotation = Quaternion.Euler(_celestialBodies.rotation.x, _celestialBodies.rotation.y, 
            -(float)(GameTime.Hours * 60 + GameTime.Minutes)/4); // 4 - это _convertTimeToRotation в DayNightCycle
    }

    public void Init(EventWindow eventWindow)
    {
        _eventWindow = eventWindow;
    }

    public abstract void SetButtons();
    
    public abstract void OnButtonClick(int n);
}
