using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GlobalEvent_BanLift : GlobalEvent_Base
{
    public override string GlobalEventName => $"������ ������� �� {BanLiftedItemName}";

    public override string Description => $"�������� ���� ������� ������� ����� ������������ ������ �� {BanLiftedItemName}. ������ ���� ������� ��������� ������� � ���������.";

    public string BanLiftedItemName;

    public override void Execute()
    {
        GlobalEvent_BannedItem foundEvent = (GlobalEvent_BannedItem)GlobalEventHandler.Instance.ActiveGlobalEvents.FirstOrDefault
            (globalEvent => globalEvent is GlobalEvent_BannedItem bannedItemEvent 
            && bannedItemEvent.BannedItemName == BanLiftedItemName);
        if (foundEvent == null)
        {
            Debug.LogError("�� ���� ������ ����������� �������� ����� ��� ������");
            return;
        }
        foundEvent.DurationHours = -1; //������ � ��� ���������� GlobalEventHandler
    }

    public override void Terminate()
    {
        //����������� �����. ������ �� ����������
    }
}
