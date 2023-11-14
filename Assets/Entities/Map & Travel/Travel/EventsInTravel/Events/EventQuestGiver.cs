using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQuestGiver : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Взять задание");
        ButtonsLabel.Add("Отказаться");
        SetInfoButton("");
    }

    public override void OnButtonClick(int n)
    {
        // TODO надо выдавать квест на сбор и доставку предметов какому-то случайному нпс со случайной локации.
        switch (n)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }
}
