using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAdvertisement : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Посмотреть представление");
        ButtonsLabel.Add("Проехать мимо");
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
