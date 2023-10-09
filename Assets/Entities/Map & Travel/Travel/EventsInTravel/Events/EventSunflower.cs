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
        // “вои методы перемотки времени будут полезны не только дл€ дебага и тестов! 
        // Ќадо проверить следующее :
        // 1. ѕри перемотке не должно мен€тьс€ оставшеес€ врем€ проездки.
        // 2. ѕри перемотке должны портитьс€ продукты на соответсвующее врем€.
        // 3. Ќебесные тела должны передвигатьс€.
        switch (n)
        {
            case 0:
                break;
            case 1:
                break;
        }
        
    }
}
