using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWizard : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("Получить улучшение (n золота)"); // Todo
        ButtonsLabel.Add("Спасибо, не нужно");
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
        
    }
}
