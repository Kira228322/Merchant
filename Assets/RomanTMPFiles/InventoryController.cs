using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] private ItemGrid _selectedItemGrid;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] Transform _canvasTransform;

    [SerializeField] private List<Roman.Item> items; //Для тестирования

    private InventoryItem _selectedItem;
    private RectTransform _rectTransform;

    private InventoryHighlight _inventoryHighlight;
    private InventoryItem _itemToHighlight;
    private Vector2Int _previousHighlightPosition;

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


        if (Input.GetKeyDown(KeyCode.Q))  //Ради тестирования
        {
            CreateRandomItem();
        }
        if (Input.GetKeyDown(KeyCode.W)) //Ради тестирования (..что такое юнит тесты?)
        {
            InsertRandomItem();
        }
        if (Input.GetKeyDown(KeyCode.R)) //Позже сделать UI менюшку с этой кнопкой
        {
            RotateItem();
        }

        if (SelectedItemGrid == null) 
        {
            _inventoryHighlight.Show(false);
            return; 
        }

        HighlightUpdate(); //Обновить информацию о подсветке предметов

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (_selectedItem == null)
        {
            PickUp(tileGridPosition);
        }
        else
        {
            PlaceDown(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (_selectedItem != null)
        {
            position.x -= (_selectedItem.Width - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (_selectedItem.Height - 1) * ItemGrid.TileSizeHeight / 2;
        }
 
        return SelectedItemGrid.GetTileGridPosition(position, _canvasTransform.localScale);
    }

    private void PickUp(Vector2Int tileGridPosition)
    {
        _selectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (_selectedItem != null)
        {
            _rectTransform = _selectedItem.GetComponent<RectTransform>();
        }
    }
    private void PlaceDown(Vector2Int tileGridPosition)
    {
        bool isSuccessful = SelectedItemGrid.TryPlaceItem(_selectedItem, tileGridPosition.x, tileGridPosition.y);
        if (isSuccessful)
        {
            _selectedItem = null;
        }
    }
    private void RotateItem()
    {
        if (_selectedItem == null) { return; }
        if (_selectedItem.ItemData.CellSizeHeight == 1 && _selectedItem.ItemData.CellSizeWidth == 1) { return; }

        _selectedItem.Rotate();
    }


    private void ItemIconDrag()
    {
        if (_selectedItem != null)
        {
            _rectTransform.position = Input.mousePosition;
        }
    }

    private void CreateRandomItem() //Для тестирования
    {
        InventoryItem item = Instantiate(_itemPrefab).GetComponent<InventoryItem>();
        _selectedItem = item;

        _rectTransform = item.GetComponent<RectTransform>();
        _rectTransform.SetParent(_canvasTransform);
        _rectTransform.SetAsLastSibling();

        _selectedItem.transform.localScale = Vector2.one;

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        item.SetItemFromData(items[selectedItemID]);
    }
    private void InsertRandomItem() //Для тестирования, но пригодится в дальнейшем
    {
        if (SelectedItemGrid == null) { return; }
        CreateRandomItem();
        InventoryItem itemToInsert = _selectedItem;
        _selectedItem = null;
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
        if (_selectedItem == null)
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
                _selectedItem.Width, 
                _selectedItem.Height)
                );
            _inventoryHighlight.SetSize(_selectedItem);
            _inventoryHighlight.SetPosition(SelectedItemGrid, _selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }
}
