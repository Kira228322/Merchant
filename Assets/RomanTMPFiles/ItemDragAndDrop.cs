using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class ItemDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _canvas;

    private Transform _previousParent;
    private Vector3 _previousPosition;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private InventoryItem _thisInventoryItem;


    [SerializeField] private InventoryItemGrid _startingInventory;
    [SerializeField] private Vector2Int _startingPositionInInventory;

    private void Awake()
    {
        _thisInventoryItem = GetComponent<InventoryItem>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        transform.SetParent(_startingInventory.transform);
        PlaceInGrid(_startingInventory, _startingPositionInInventory);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _previousParent = transform.parent;
        _previousPosition = transform.position;

        transform.SetParent(_canvas.transform);

        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         var result = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventData, result);
          foreach(var raycast in result)
         {
             if (raycast.gameObject.TryGetComponent(out InventoryItemGrid itemGrid))
             {
                itemGrid.RemoveItemFromGrid(itemGrid.GetTileGridPosition(_previousPosition));

                transform.SetParent(raycast.gameObject.transform);
                PlaceInGrid(itemGrid, eventData.position);
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.alpha = 1f;
                return;
             }
         }
        transform.SetParent(_previousParent);
        transform.position = _previousPosition;
        
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

    }

    private void PlaceInGrid(InventoryItemGrid itemGrid, Vector2 mousePosition)
    {
        Vector2Int tilePositionWhereToPlace = itemGrid.GetTileGridPosition(mousePosition);
        itemGrid.PlaceItemInGrid(_thisInventoryItem, tilePositionWhereToPlace);

        Vector2 position = new();
        position.x = tilePositionWhereToPlace.x * InventoryItemGrid.TileSizeWidth + InventoryItemGrid.TileSizeWidth / 2;
        position.y = tilePositionWhereToPlace.y * InventoryItemGrid.TileSizeHeight + InventoryItemGrid.TileSizeHeight / 2;

        transform.localPosition = position;
    }
    private void PlaceInGrid(InventoryItemGrid itemGrid, Vector2Int tilePosition)
    {
        itemGrid.PlaceItemInGrid(_thisInventoryItem, tilePosition);
        Vector2 position = new();
        position.x = tilePosition.x * InventoryItemGrid.TileSizeWidth + InventoryItemGrid.TileSizeWidth / 2;
        position.y = tilePosition.y * InventoryItemGrid.TileSizeHeight + InventoryItemGrid.TileSizeHeight / 2;

        transform.localPosition = position;
    }
}
