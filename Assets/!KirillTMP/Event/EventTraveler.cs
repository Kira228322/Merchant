using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTraveler : EventInTravel
{
    // Путник может либо отблагодарить игрока, за то, что тот ему помог, либо оказаться вором
    [SerializeField] private int _chanceTravelerIsChief; // Вероятность, что вор (веротность, что не вор q = 1 - p)
    [SerializeField] private int _money;
    [SerializeField] private int _experience;
    public override void SetButtons()
    {
        ButtonsLabel.Add("Подвезти путника");
        ButtonsLabel.Add("Ехать дальше");
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
