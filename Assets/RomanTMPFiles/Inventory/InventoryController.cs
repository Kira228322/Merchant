using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    #region Поля и свойства

    [HideInInspector] private ItemGrid _selectedItemGrid;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] Transform _canvasTransform;
    [SerializeField] ItemInfo _itemInfoPanel;

    [SerializeField] private List<Item> items; //Для тестирования, список предметов, которые могут вылезти рандомно по нажатию ПКМ

    private Vector2 _mousePositionOnPress;

    private InventoryItem _selectedItem;
    private RectTransform _rectTransform;

    private InventoryHighlight _inventoryHighlight;
    private InventoryItem _itemToHighlight;
    private Vector2Int _previousHighlightPosition;

    private float _pressAndHoldTime = 0f;
    private Vector2Int _currentTileGridPosition;
    private ItemGrid _gridPickedUpFrom;
    private Vector2Int _itemPickedUpFromPosition;

    public InventoryItem CurrentSelectedItem { 
        get 
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

    #endregion

    private void Awake()
    {
        _inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();
        
        if (SelectedItemGrid != null)
        {
            _currentTileGridPosition = GetTileGridPosition();
        }
        if (Input.GetMouseButtonDown(1)) //Ради тестирования
        {
            InsertRandomItem();
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButtonPress();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnLeftMouseButtonRelease();
        }
        if (Input.GetMouseButton(0))
        {
            WhileLeftMouseButtonIsPressed();
        }

        HighlightUpdate();

    }

    #region Методы, связанные с инпутом
    private void OnLeftMouseButtonPress()
    {
        _mousePositionOnPress = Input.mousePosition;
    }
    private void WhileLeftMouseButtonIsPressed()
    {
        if (IsHoveringOverItem())
        {
            if (_selectedItem == null)
            {
                _pressAndHoldTime += Time.deltaTime;
                if (_pressAndHoldTime >= 0.3f && Vector2.Distance(Input.mousePosition, _mousePositionOnPress) < 30)
                {
                    PickUp(_currentTileGridPosition);
                    SelectedItemGrid.GetComponentInParent<ScrollRect>().StopMovement();
                    SelectedItemGrid.GetComponentInParent<ScrollRect>().enabled = false;
                    _pressAndHoldTime = 0;
                }
            }
        }
    }
    private void OnLeftMouseButtonRelease()
    {
        _pressAndHoldTime = 0;
        if (CurrentSelectedItem != null)
        {
            _gridPickedUpFrom.GetComponentInParent<ScrollRect>().enabled = true;
            if (SelectedItemGrid == null)
            {
                SelectedItemGrid = _gridPickedUpFrom;
                PlaceDown(_itemPickedUpFromPosition);
                SelectedItemGrid = null;
            }
            else
            {
                PlaceDown(_currentTileGridPosition);
            }
        }
        else if (Vector2.Distance(Input.mousePosition, _mousePositionOnPress) < 30)
        {
            LeftMouseButtonPress();
        }
    }
    private void LeftMouseButtonPress()
    {
        if (IsHoveringOverItem())
        {
            ShowItemStats(SelectedItemGrid.GetItem(GetTileGridPosition().x, GetTileGridPosition().y));
        }
    }
    private bool IsHoveringOverItem()
    {
        if (SelectedItemGrid != null && _selectedItem == null)
        {
            if (SelectedItemGrid.GetItem(GetTileGridPosition().x, GetTileGridPosition().y) != null)
            {
                return true;
            }
        }
        return false;
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

    #endregion

    #region Методы, связанные с взаимодействием с айтемом
    public bool TryCreateAndInsertItem(ItemGrid itemGrid, Item item, int amount, bool isFillingStackFirst)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;


        CreateItem(item, amount);
        InventoryItem itemToInsert = CurrentSelectedItem;
        SelectedItemGrid = itemGrid;

        if (TryInsertItem(itemToInsert, isFillingStackFirst))
        {
            CurrentSelectedItem = null;
            SelectedItemGrid = initialItemGridState;
            return true;
        }
        //Не получилось поместить
        Destroy(itemToInsert.gameObject);
        CurrentSelectedItem = null;
        SelectedItemGrid = initialItemGridState;
        return false;
    }
    public bool TryCreateAndInsertItemRotated(ItemGrid itemGrid, Item item, int amount, bool isFillingStackFirst)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;

        CreateItemRotated(item, amount);
        InventoryItem itemToInsert = CurrentSelectedItem;
        SelectedItemGrid = itemGrid;

        if (TryInsertItem(itemToInsert, isFillingStackFirst))
        {
            CurrentSelectedItem = null;
            SelectedItemGrid = initialItemGridState;
            //itemGrid обнуляется после установки предмета
            return true;
        }
        //Не получилось поместить
        Destroy(itemToInsert.gameObject);
        CurrentSelectedItem = null;
        SelectedItemGrid = initialItemGridState;
        return false;
    }
    private void ShowItemStats(InventoryItem item)
    {
        _itemInfoPanel.Show(item, SelectedItemGrid);
    }
    private void PickUp(Vector2Int tileGridPosition)
    {
        CurrentSelectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        _gridPickedUpFrom = SelectedItemGrid;
        _itemPickedUpFromPosition = new Vector2Int (CurrentSelectedItem.XPositionOnTheGrid, CurrentSelectedItem.YPositionOnTheGrid);
        if (CurrentSelectedItem != null)
        {
            _rectTransform = CurrentSelectedItem.GetComponent<RectTransform>();
            _rectTransform.SetAsLastSibling();
        }
    }
    private void PlaceDown(Vector2Int tileGridPosition)
    {
        bool isSuccessful = SelectedItemGrid.TryPlaceItem(CurrentSelectedItem, tileGridPosition.x, tileGridPosition.y);
        if (isSuccessful)
        {
            CurrentSelectedItem = null;
        }
        else
        {
            PlaceDown(_itemPickedUpFromPosition);
        }
    }
    private void ItemIconDrag()
    {
        if (CurrentSelectedItem != null)
        {
            _rectTransform.position = Input.mousePosition;
        }
    }
    private void CreateItem(Item item, int amount)
    {
        InventoryItem spawnedItem = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        CurrentSelectedItem = spawnedItem;

        spawnedItem.CurrentItemsInAStack = amount;

        _rectTransform = spawnedItem.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

        spawnedItem.SetItemFromData(item);

    }
    private void CreateItemRotated(Item item, int amount)
    {
        InventoryItem spawnedItem = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        spawnedItem.Rotate();
        CurrentSelectedItem = spawnedItem;

        spawnedItem.CurrentItemsInAStack = amount;

        _rectTransform = spawnedItem.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

        spawnedItem.SetItemFromData(item);
    }
    private void CreateRandomItem() //Для тестирования
    {
        InventoryItem item = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        CurrentSelectedItem = item;

        item.CurrentItemsInAStack = 1;

        _rectTransform = item.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        item.SetItemFromData(items[selectedItemID]);
    }
    private void InsertRandomItem() //Для тестирования
    {
        if (SelectedItemGrid == null) { return; }
        CreateRandomItem();
        InventoryItem itemToInsert = CurrentSelectedItem;
        CurrentSelectedItem = null;
        TryInsertItem(itemToInsert, true);
    }
    public bool PickUpRotateInsert(InventoryItem itemInInventory, ItemGrid itemGrid)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;
        SelectedItemGrid = itemGrid;

        Vector2Int itemPosition = new(itemInInventory.XPositionOnTheGrid, itemInInventory.YPositionOnTheGrid);
        PickUp(itemPosition);
        InventoryItem pickedUpItem = CurrentSelectedItem;
        if (itemInInventory.IsRotated)
        {
            if (TryCreateAndInsertItem(itemGrid, itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, isFillingStackFirst: false))
            {
                Destroy(pickedUpItem.gameObject);
                SelectedItemGrid = initialItemGridState;
                return true;
            }
            else PlaceDown(itemPosition);
            SelectedItemGrid = initialItemGridState;
            return false;
        }
        else
        {
            if (TryCreateAndInsertItemRotated(itemGrid, itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, isFillingStackFirst: false))
            {
                Destroy(pickedUpItem.gameObject);
                SelectedItemGrid = initialItemGridState;
                return true;
            }
            else PlaceDown(itemPosition);
            SelectedItemGrid = initialItemGridState;
            return false;
        }

    }
    private bool TryInsertItem (InventoryItem itemToInsert, bool isFillingStackFirst)
    {
        //itemToInsert.transform.localScale = canvasTransform.localScale;
        //^^возможно пригодится в будущем в качестве подсказки, когда будем делать спавн объектов 

        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForItemInsertion(itemToInsert, isFillingStackFirst);

        if (posOnGrid == null) 
        {
            return false; 
        }
        SelectedItemGrid.TryPlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        return true;
    }

    #endregion
    private void HighlightUpdate()
    {
        if (SelectedItemGrid == null) 
        {
            _inventoryHighlight.Show(false);
            return; 
        }

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
