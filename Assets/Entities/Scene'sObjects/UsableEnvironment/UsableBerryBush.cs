using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableBerryBush : UsableEnvironment
{
    [SerializeField] private Item _berry;
    protected override bool IsFunctionalComplete()
    {
        // TODO проверить есть ли место свободное в инвентаре и добавить ягоду какую-нибудь
        if (InventoryController.Instance.TryCreateAndInsertItem
                (_berry, 1, 0) != null) 
            return true;
        return false;
    }
}
