using UnityEngine;

public class UsableBerryBush : UsableEnvironment
{
    [SerializeField] private Item _berry;
    protected override bool IsFunctionalComplete()
    {
        if (InventoryController.Instance.TryCreateAndInsertItem
                (_berry, 1, 0) != null)
            return true;
        return false;
    }
}
