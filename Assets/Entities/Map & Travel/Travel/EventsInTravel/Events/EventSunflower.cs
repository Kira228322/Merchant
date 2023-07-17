using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSunflower : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("—обрать подсолнухи"); // Todo добавить 2-3 подсолнуха игроку
        ButtonsLabel.Add("≈хать дальше");
    }

    public override void OnButtonClick(int n)
    {
        // TODO
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
