using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableBerryBush : UsableEnvironment
{
    protected override bool IsFunctionalComplete()
    {
        // TODO проверить есть ли место свободное в инвентаре и добавить ягоду какую-нибудь
        if (InventoryController.Instance.TryCreateAndInsertItem(
            Player.Instance.ItemGrid,
            ItemDatabase.GetItem("Sugus"),
            1, 0, true)
            != null) 
            return true;
        return false;
    }
}
