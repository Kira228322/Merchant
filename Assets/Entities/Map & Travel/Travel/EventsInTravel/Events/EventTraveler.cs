using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTraveler : EventInTravel
{
    // Путник может либо отблагодарить игрока, за то, что тот ему помог, либо оказаться вором
    [SerializeField] private int _chanceTravelerIsThief; // Вероятность, что вор (вероятность, что не вор q = 1 - p)
    [SerializeField] private int _money;
    [SerializeField] private int _experience;
    private bool _isThief;
    
    public override void SetButtons()
    {
        ButtonsLabel.Add("Подвезти путника");
        ButtonsLabel.Add("Ехать дальше");
    }

    private void Start()
    {
        _isThief = TravelEventHandler.EventFire(_chanceTravelerIsThief, false, true);
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                break;
            case 1:
                break;
        }
        _eventHandler.EventEnd();
    }
}
