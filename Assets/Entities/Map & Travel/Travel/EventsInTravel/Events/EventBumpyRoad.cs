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
        ButtonsLabel.Add("����� �� ��������� ������");
        ButtonsLabel.Add("����� � ������");
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
