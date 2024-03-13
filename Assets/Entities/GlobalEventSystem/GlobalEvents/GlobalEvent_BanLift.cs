using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GlobalEvent_BanLift : GlobalEvent_Base
{
    public override string GlobalEventName => $"������ ������� �� {string.Join(", ", CurrentEventItemNames)}";

    public override string Description => $"�������� ���� ������� ������� ����� ������������ ������ �� ��������� ��������: {string.Join(", ", CurrentEventItemNames)}. ������ ��� �������� ��������� ������� � ���������.";

    public string BanLiftedItemName;

    public List<string> CurrentEventItemNames;

    public override void Execute()
    {
        GlobalEvent_BannedItem foundEvent = (GlobalEvent_BannedItem)GlobalEventHandler.Instance.ActiveGlobalEvents.FirstOrDefault
            (globalEvent => globalEvent is GlobalEvent_BannedItem bannedItemEvent
            && bannedItemEvent.BannedItemNames.Contains(BanLiftedItemName));
        if (foundEvent == null)
        {
            Debug.LogError("�� ���� ������ ����������� �������� ����� ��� ������");
            return;
        }
        CurrentEventItemNames = new(foundEvent.BannedItemNames);
        foundEvent.DurationHours = -1; //������ � ��� ���������� GlobalEventHandler
    }

    public override void Terminate()
    {
        //����������� �����. ������ �� ����������
    }
}
