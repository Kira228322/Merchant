using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController _inventoryController;
    private ItemGrid _itemGrid;

    private void Awake()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        _itemGrid = GetComponent<ItemGrid>();
    }

    //Я надеюсь, это будет нормально работать для пальцев, касающихся мобильника
    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryController.SelectedItemGrid = _itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryController.SelectedItemGrid = null;
    }

}
