using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public InventoryItemGrid SelectedItemGrid;

    private void Update()
    {
        if (SelectedItemGrid == null) { return; }
    }
}
