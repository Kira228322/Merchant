using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBumpyRoad : EventInTravel
{
    [SerializeField] private int _reduceQuality;
    [SerializeField] private int _minTimeToGoRound;
    [SerializeField] private int _maxTimeToGoRound;
    public override void SetButtons()
    {
        ButtonsLabel.Add("≈хать по ухабистой дороге");
        ButtonsLabel.Add("≈хать в объезд");
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
        
    }
}
