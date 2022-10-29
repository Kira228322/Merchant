using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryItemGrid))]
public class InventoryGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;
    InventoryItemGrid itemGrid;

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }

    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        itemGrid = GetComponent<InventoryItemGrid>();
    }
}
