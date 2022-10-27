using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] InventoryItemGrid selectedItemGrid;

    private void Update()
    {
        if (selectedItemGrid == null) { return; }

        Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
    }
}
