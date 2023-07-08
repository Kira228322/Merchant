using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWagonTrade : EventInTravel
{
    public override void SetButtons()
    {
        // TODO 
        // по сути тут не будет открывать окно трейда, как с любым нпс 
        // думаю предложить игроку купить 2-3 редких предмета и кнопку выход она будет делать _eventHandler.EventEnd();
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
