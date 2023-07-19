using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GlobalEvent_BannedItem : GlobalEvent_Base
{
    public string BannedItemName;
    public override string GlobalEventName => "Предмет запрещён";

    public override string Description => $"Начиная с этого дня, власти запрещают предмет {BannedItemName}";

    public override void Execute()
    {
        BannedItemsHandler.Instance.BanItem(ItemDatabase.GetItem(BannedItemName));
    }

    public override void Terminate()
    {
        BannedItemsHandler.Instance.UnbanItem(ItemDatabase.GetItem(BannedItemName));
    }
}
