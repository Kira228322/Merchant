using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] private ItemGrid _selectedItemGrid;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] Transform _canvasTransform;
    [SerializeField] ItemInfo _itemInfoPanel;


    [SerializeField] private List<Item> items; //Для тестирования

    private InventoryItem _selectedItem;
    private RectTransform _rectTransform;

    private InventoryHighlight _inventoryHighlight;
    private InventoryItem _itemToHighlight;
    private Vector2Int _previousHighlightPosition;

    public InventoryItem CurrentSelectedItem { get 
        { return _selectedItem; }
        private set 
        {
            _selectedItem = value;
        } 
    }

    public ItemGrid SelectedItemGrid
    {
        get => _selectedItemGrid;
        set
        {
            _selectedItemGrid = value;
            _inventoryHighlight.SetParent(value);
        }
    }

    private void Awake()
    {
        _inventoryHighlight = GetComponent<InventoryHighlight>();

    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.touchCount == 1 && Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
        if (Input.touchCount == 3 && CurrentSelectedItem == null) //Ради тестирования (..что такое юнит тесты?)
        {
            InsertRandomItem();
        }

        if (SelectedItemGrid == null) 
        {
            _inventoryHighlight.Show(false);
            return; 
        }

        HighlightUpdate(); //Обновить информацию о подсветке предметов

    }

    private void LeftMouseButtonPress()
    {
        /* Переработать под мобильный инпут, поднятие по удержанию, PlaceDown как сейчас, убрать в другой метод
         * Vector2Int tileGridPosition = GetTileGridPosition();

        if (CurrentSelectedItem == null)
        {
            PickUp(tileGridPosition);
        }
        else
        {
            PlaceDown(tileGridPosition);
        }
        */
        if (SelectedItemGrid.GetItem(GetTileGridPosition().x, GetTileGridPosition().y) != null)
        {
            ShowItemStats(SelectedItemGrid.GetItem(GetTileGridPosition().x, GetTileGridPosition().y));
        }
    }

    private void ShowItemStats(InventoryItem item)
    {
            _itemInfoPanel.Show(item);
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (CurrentSelectedItem != null)
        {
            position.x -= (CurrentSelectedItem.Width - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (CurrentSelectedItem.Height - 1) * ItemGrid.TileSizeHeight / 2;
        }
 
        return SelectedItemGrid.GetTileGridPosition(position, _canvasTransform.localScale);
    }

    private void PickUp(Vector2Int tileGridPosition)
    {
        CurrentSelectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (CurrentSelectedItem != null)
        {
            _rectTransform = CurrentSelectedItem.GetComponent<RectTransform>();
        }
    }
    private void PlaceDown(Vector2Int tileGridPosition)
    {
        bool isSuccessful = SelectedItemGrid.TryPlaceItem(CurrentSelectedItem, tileGridPosition.x, tileGridPosition.y);
        if (isSuccessful)
        {
            CurrentSelectedItem = null;
        }
    }
    private void RotateItem()
    {
        if (CurrentSelectedItem == null) { return; }
        if (CurrentSelectedItem.ItemData.CellSizeHeight == CurrentSelectedItem.ItemData.CellSizeWidth) { return; }

        CurrentSelectedItem.Rotate();
    }


    private void ItemIconDrag()
    {
        if (CurrentSelectedItem != null)
        {
            _rectTransform.position = Input.mousePosition;
        }
    }

    private void CreateRandomItem() //Для тестирования
    {
        InventoryItem item = Instantiate(_itemPrefab).GetComponent<InventoryItem>();
        CurrentSelectedItem = item;

        item.CurrentItemsInAStack = 1;

        _rectTransform = item.GetComponent<RectTransform>();
        _rectTransform.SetParent(_canvasTransform);
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        item.SetItemFromData(items[selectedItemID]);
    }
    private void InsertRandomItem() //Для тестирования, но пригодится в дальнейшем
    {
        if (SelectedItemGrid == null) { return; }
        CreateRandomItem();
        InventoryItem itemToInsert = CurrentSelectedItem;
        CurrentSelectedItem = null;
        InsertItem(itemToInsert);
    }
    private void InsertItem (InventoryItem itemToInsert)
    {
        //itemToInsert.transform.localScale = canvasTransform.localScale;
        //^^возможно пригодится в будущем в качестве подсказки, когда будем делать спавн объектов 

        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null) 
        { 
            return; 
        }
        SelectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void HighlightUpdate()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (_previousHighlightPosition == positionOnGrid) { return; }
        _previousHighlightPosition = positionOnGrid;
        if (CurrentSelectedItem == null)
        {
            _itemToHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (_itemToHighlight != null)
            {
                _inventoryHighlight.Show(true);
                _inventoryHighlight.SetSize(_itemToHighlight);
                _inventoryHighlight.SetPosition(SelectedItemGrid, _itemToHighlight);
            }
            else
            {
                _inventoryHighlight.Show(false);
            }
        }
        else
        {
            _inventoryHighlight.Show(SelectedItemGrid.IsInCorrectPosition(
                positionOnGrid.x, 
                positionOnGrid.y,
                CurrentSelectedItem.Width,
                CurrentSelectedItem.Height)
                );
            _inventoryHighlight.SetSize(CurrentSelectedItem);
            _inventoryHighlight.SetPosition(SelectedItemGrid, CurrentSelectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }
}
