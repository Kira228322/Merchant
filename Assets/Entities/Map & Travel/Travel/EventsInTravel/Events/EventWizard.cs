using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWizard : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("�������� ��������� (n ������)"); // Todo
        ButtonsLabel.Add("�������, �� �����");
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
