using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EventBumpyRoad : EventInTravel
{
    [SerializeField] private float _reduceQuality;
    [SerializeField] private int _minTimeToGoAround;
    [SerializeField] private int _maxTimeToGoAround;
    public override void SetButtons()
    {
        ButtonsLabel.Add("Ехать по ухабистой дороге");
        ButtonsLabel.Add("Ехать в объезд");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                FindObjectOfType<TravelEventHandler>().RoadBadnessMultiplier += _reduceQuality;
                _eventWindow.ChangeDescription("Вы решили поехать напрямую, незадерживаясь в своем путешествии.");
                break;
            case 1:
                FindObjectOfType<TravelEventHandler>().ChangeTravelTime(Random.Range(_minTimeToGoAround, _maxTimeToGoAround +1));
                _eventWindow.ChangeDescription("Вы поехали в объезд. Тише едешь - дальше будешь");
                break;
        }
        
    }
}
