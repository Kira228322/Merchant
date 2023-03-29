using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    #region ���� � ��������

    public static InventoryController Instance;

    [HideInInspector] private ItemGrid _selectedItemGrid;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _itemInfoPanelPrefab;
    [SerializeField] private Transform _canvasTransform;

    [SerializeField] private List<Item> items; //��� ������������, ������ ���������, ������� ����� ������� �������� �� ������� ���

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
    #region ������������� � Update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();
        
        if (SelectedItemGrid != null)
        {
            _currentTileGridPosition = GetTileGridPosition();
        }
        if (Input.GetMouseButtonDown(1)) //���� ������������
        {
            InsertRandomItem();
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(2)) //���� ������������
        {

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

    #endregion
    #region ������, ��������� � �������
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
                    var parentScrollRect = SelectedItemGrid.GetComponentInParent<ScrollRect>();
                    if (parentScrollRect != null)
                    {
                        parentScrollRect.StopMovement();
                        parentScrollRect.enabled = false;
                    }
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
            var gridPickedUpFromScrollRect = _gridPickedUpFrom.GetComponentInParent<ScrollRect>();
            if (gridPickedUpFromScrollRect != null) 
            {
                gridPickedUpFromScrollRect.enabled = true; 
            }
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
        if (Input.touchCount > 0)
            //��� ������ OnPointerExit ����������� � ��� ����� ���� ����� ��������� �������. ��� ����� �� ���. 
            //������� � GridInteract.OnPointerExit � �������� ������ �������� ItemGrid, ���� ������� ���� ��������
            //�����, ��������, ����� ��������. ����� ������� �� ����������, ��� OnPointerExit ����� ����������� ������ ���� �����
            //������������� ����� �� ������� ItemGrid. (24.03.23)
            SelectedItemGrid = null;
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
    #region ������, ��������� � ��������������� � �������
    private class CompactedItem
    {
        public Item item;
        public int amount;
        public float daysBoughtAgo;
    }
    public bool IsThereAvailableSpaceForInsertingMultipleItems(ItemGrid itemGrid, List<ItemReward> rewardItems)
    {
        List<CompactedItem> items = new();
        foreach (var item in rewardItems)
        {
            CompactedItem newItemReward = new();
            newItemReward.item = ItemDatabase.GetItem(item.itemName);
            newItemReward.amount = item.amount;
            newItemReward.daysBoughtAgo = item.daysBoughtAgo;
        }
        /*
         ����� ���������, ���� �� ����� ��� ���������� ���������� ���������. ��� �� ��������� (������ - ����������� �����)
        */
        if (items.Count == 0)
        {
            return true;
        }

        //������ � ����, ��� ����������� ���� ���������� ��������� �� ������ �������� � ������ ����������
        //���� ��������� ������� ������, ��� ������� ����� �� �������� �����.

        items.Sort((x, y) => (y.item.CellSizeWidth * y.item.CellSizeHeight).CompareTo(x.item.CellSizeWidth * x.item.CellSizeHeight));
        //^ ���������� �� ���������� ������� https://stackoverflow.com/a/3309292

        Dictionary<InventoryItem, int> placedInventoryItems = new();
        //^����������� ��� ����, ����� ���������, ����� ���� ���� �������� � ������� ������ ���� ���� ��������.
        //��� �����, ����� ���� �� ��������� ��������� ���, ����� ������� ���� � ���� ���� �������� � ������ �������, ������� �����.
        //(���� ��������� ���� �������� � ����, �� ����� ������ �� ���� ����, � �������, ������� � ���� ��������, ��� ����?)

        foreach(CompactedItem itemReward in items)
        {
            InventoryItem placedItem = TryCreateAndInsertItem(itemGrid, itemReward.item, itemReward.amount, itemReward.daysBoughtAgo, true);
            if (placedItem != null)
            {
                placedInventoryItems.Add(placedItem, itemReward.amount); 
            }
            else
            {
                foreach (var item in placedInventoryItems)
                {
                    itemGrid.RemoveItemsFromAStack(item.Key, item.Value);
                }
                return false;
            }
        }
        foreach (var item in placedInventoryItems)
        {
            itemGrid.RemoveItemsFromAStack(item.Key, item.Value);
        }
        return true;

        /*������ ��������� ������ ��� ����������� �� ����������:
        ������ ����� ����� ���� *���������*, ���� �� ����������� ��������� ��������� �����.
        �� ��� �� �� �������� � ���������. ��������� �������, ��� ��� ������������, ������
        ����� ���������� ����� ��� ���������, �������� � Quest ���� ����� GiveReward().
        �� ������ �����, ����� � ������, ���� ������� ����. ���� �� �����, � ������� ���� �����������,
        ������� ����, �� �� ���� �� �������� ������� � � ��� ������, �������������� �������� ��� ������ ������ � ����.
        ��� ���� �� ���������, ������ ��� ���-������ ����� �� ����� ��������� ������� � �� ����� ������ ��� �� �������� ��������.

        ������� ������ ��������������� �����������, � ���� �� ��� ������ ������� ����������� ������ �� ������� ������ ���������� ������,
        ��� � ���� �� ����� ����� �����������. (26.01.23)
        */

    }
    public InventoryItem TryCreateAndInsertItem(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        InventoryItem result = TryCreateAndInsertItemUnrotated(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
        if (result == null)
        {
            result = TryCreateAndInsertItemRotated(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
        }
        return result;
    }
    private InventoryItem TryCreateAndInsertItemUnrotated(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;


        CreateItem(item, amount, daysBoughtAgo);
        InventoryItem itemToInsert = CurrentSelectedItem;
        SelectedItemGrid = itemGrid;

        InventoryItem result = TryInsertItem(itemToInsert, isFillingStackFirst);
        if (result != null)
        {
            CurrentSelectedItem = null;
            SelectedItemGrid = initialItemGridState;
            return result;
        }
        //�� ���������� ���������
        Destroy(itemToInsert.gameObject);
        CurrentSelectedItem = null;
        SelectedItemGrid = initialItemGridState;
        return null;
    }
    private InventoryItem TryCreateAndInsertItemRotated(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;

        CreateItemRotated(item, amount, daysBoughtAgo);
        InventoryItem itemToInsert = CurrentSelectedItem;
        SelectedItemGrid = itemGrid;

        InventoryItem result = TryInsertItem(itemToInsert, isFillingStackFirst);
        if (result != null)
        {
            CurrentSelectedItem = null;
            SelectedItemGrid = initialItemGridState;
            return result;
        }
        //�� ���������� ���������
        Destroy(itemToInsert.gameObject);
        CurrentSelectedItem = null;
        SelectedItemGrid = initialItemGridState;
        return null;
    }
    public void DestroyItem(ItemGrid itemGrid, InventoryItem item)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;
        SelectedItemGrid = itemGrid;

        SelectedItemGrid.DestroyItem(item);

        SelectedItemGrid = initialItemGridState;
    }
    private void ShowItemStats(InventoryItem item)
    {
        ItemInfo itemInfoPanel = Instantiate(_itemInfoPanelPrefab, _canvasTransform).GetComponent<ItemInfo>();
        itemInfoPanel.Initialize(item, SelectedItemGrid);
    }
    private void PickUp(Vector2Int tileGridPosition)
    {
        CurrentSelectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        _gridPickedUpFrom = SelectedItemGrid;
        _itemPickedUpFromPosition = new Vector2Int (CurrentSelectedItem.XPositionOnTheGrid, CurrentSelectedItem.YPositionOnTheGrid);
        if (CurrentSelectedItem != null)
        {
            _rectTransform = CurrentSelectedItem.GetComponent<RectTransform>();
            _rectTransform.SetParent(_canvasTransform);
            _rectTransform.SetAsLastSibling();
        }
    }
    private void PlaceDown(Vector2Int tileGridPosition)
    {
        InventoryItem itemPlaced = SelectedItemGrid.TryPlaceItem(CurrentSelectedItem, tileGridPosition.x, tileGridPosition.y);
        if (itemPlaced != null)
        {
            CurrentSelectedItem = null;
        }
        else
        {
            if (_gridPickedUpFrom != SelectedItemGrid)
            {
                ItemGrid tempItemGrid = SelectedItemGrid;
                SelectedItemGrid = _gridPickedUpFrom;
                PlaceDown(_itemPickedUpFromPosition);
                SelectedItemGrid = tempItemGrid;
            }
            else PlaceDown(_itemPickedUpFromPosition);
        }
    }
    private void ItemIconDrag()
    {
        if (CurrentSelectedItem != null)
        {
            _rectTransform.position = Input.mousePosition;
        }
    }
    private void CreateItem(Item item, int amount, float boughtDaysAgo)
    {
        InventoryItem spawnedItem = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        spawnedItem.SetItemFromData(item);
        CurrentSelectedItem = spawnedItem;

        spawnedItem.CurrentItemsInAStack = amount;
        spawnedItem.BoughtDaysAgo = boughtDaysAgo;

        _rectTransform = spawnedItem.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;


    }
    private void CreateItemRotated(Item item, int amount, float boughtDaysAgo)
    {
        InventoryItem spawnedItem = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        spawnedItem.SetItemFromData(item);
        spawnedItem.Rotate();
        CurrentSelectedItem = spawnedItem;

        spawnedItem.CurrentItemsInAStack = amount;
        spawnedItem.BoughtDaysAgo = boughtDaysAgo;

        _rectTransform = spawnedItem.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

    }
    private void CreateRandomItem() //��� ������������
    {
        InventoryItem item = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        CurrentSelectedItem = item;

        item.CurrentItemsInAStack = 1;

        _rectTransform = item.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

        int selectedItemID = Random.Range(0, items.Count);
        item.SetItemFromData(items[selectedItemID]); 
    }
    private InventoryItem InsertRandomItem() //��� ������������
    {
        if (SelectedItemGrid == null) { return null; }
        CreateRandomItem();
        InventoryItem itemToInsert = CurrentSelectedItem;
        CurrentSelectedItem = null;
        InventoryItem result = TryInsertItem(itemToInsert, true);
        if (result == null)
        {
            Destroy(itemToInsert.gameObject);
        }
        return result;
    }
    public InventoryItem TryPickUpRotateInsert(InventoryItem itemInInventory, ItemGrid itemGrid)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;
        SelectedItemGrid = itemGrid;

        Vector2Int itemPosition = new(itemInInventory.XPositionOnTheGrid, itemInInventory.YPositionOnTheGrid);
        PickUp(itemPosition);
        InventoryItem pickedUpItem = CurrentSelectedItem;
        if (itemInInventory.IsRotated)
        {
            InventoryItem result = TryCreateAndInsertItem(itemGrid, itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, itemInInventory.BoughtDaysAgo, isFillingStackFirst: false);
            if (result != null)
            {
                Destroy(pickedUpItem.gameObject);
                SelectedItemGrid = initialItemGridState;
                return result;
            }
            else
            {
                PlaceDown(itemPosition);
                SelectedItemGrid = initialItemGridState;
                return null;
            }
        }
        else
        {
            InventoryItem result = TryCreateAndInsertItemRotated(itemGrid, itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, itemInInventory.BoughtDaysAgo, isFillingStackFirst: false);
            if (result != null)
            {
                Destroy(pickedUpItem.gameObject);
                SelectedItemGrid = initialItemGridState;
                return result;
            }
            else
            {
                PlaceDown(itemPosition);
                SelectedItemGrid = initialItemGridState;
                return null;
            }
        }

    }
    private InventoryItem TryInsertItem (InventoryItem itemToInsert, bool isFillingStackFirst)
    {
        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForItemInsertion(itemToInsert, isFillingStackFirst);
        if (posOnGrid == null) 
        {
            return null; 
        }
        InventoryItem result = SelectedItemGrid.TryPlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        return result;
    }

    #endregion
}
