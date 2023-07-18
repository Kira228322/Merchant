using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvent_BannedItem : GlobalEvent_Base
{
    public Item BannedItem;
    public override string GlobalEventName => "������� ��������";

    public override string Description => $"������� � ����� ���, ������ ��������� ������� {BannedItem.Name}";

    public override void Execute()
    {
        BannedItemsHandler.Instance.BanItem(BannedItem);
    }

    public override void Terminate()
    {
        BannedItemsHandler.Instance.UnbanItem(BannedItem);
    }
}
