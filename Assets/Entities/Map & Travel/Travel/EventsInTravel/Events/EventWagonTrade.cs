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
        
        // Romaga, 19.07.23: сделал тут сет каких-то кнопок пока что,
        // потому что кнопок не было и не на что было нажимать => игра входила в затруднительное положение
        ButtonsLabel.Add("Ударить торговцу с правой руки");
        ButtonsLabel.Add("Ударить торговцу с левой руки");
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
