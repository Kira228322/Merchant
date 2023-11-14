using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCrossroads : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Поехать налево");
        ButtonsLabel.Add("Поехать направо");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                _eventWindow.ChangeDescription("Вы продолжили свой путь по левой дороге");
                break;
            case 1:
                _eventWindow.ChangeDescription("Вы продолжили свой путь по правой дороге");
                break;
        }

        FindObjectOfType<TravelEventHandler>().ChangeTravelTime(Random.Range(1,4));
    }
}
