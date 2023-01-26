using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    #region Поля и свойства

    public static InventoryController Instance;

    [HideInInspector] private ItemGrid _selectedItemGrid;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _itemInfoPanelPrefab;
    [SerializeField] private Transform _canvasTransform;

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
    #region Инициализация и Update
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
        if (Input.GetMouseButtonDown(1)) //Ради тестирования
        {
            InsertRandomItem();
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(2)) //Ради тестирования
        {
            QuestHandler.AddQuest("TestQuestFindApples");
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
    public bool IsThereASpaceForMultipleItemsInsertion(ItemGrid itemGrid, List<Quest.ItemReward> items)
    {
        //Суть: этот метод просто *проверяет*, есть ли свободное место для размещения нескольких айтемов.
        //Проверяет тем образом, что создает дубликат сетки инвентаря и помещает в неё.
        //Если всё норм, значит место есть. Если не всё норм, то игрок пошел нахуй.

        if (items.Count == 0)
        {
            return false;
        }

        GameObject theoreticalItemGridGameObject = Instantiate(itemGrid).gameObject;
        ItemGrid theoreticalItemGrid = theoreticalItemGridGameObject.GetComponent<ItemGrid>();
        for (int i = 0; i < items.Count; i++)
        {
            if(!TryCreateAndInsertItem(theoreticalItemGrid, items[i].item, items[i].amount, items[i].daysBoughtAgo, true))
            {
                Destroy(theoreticalItemGrid.gameObject);
                Debug.Log("not unfalse");
                return false;
            }
        }
        Destroy(theoreticalItemGrid.gameObject);
        Debug.Log("true");
        return true;

    }
    public bool TryCreateAndInsertItem(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        if (!TryCreateAndInsertItemUnrotated(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst))
        {
            if (!TryCreateAndInsertItemRotated(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst))
            {
                return false;
            }
            return true;
        }
        return true;
    }
    private bool TryCreateAndInsertItemUnrotated(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;


        CreateItem(item, amount, daysBoughtAgo);
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
    private bool TryCreateAndInsertItemRotated(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;

        CreateItemRotated(item, amount, daysBoughtAgo);
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
        bool isSuccessful = SelectedItemGrid.TryPlaceItem(CurrentSelectedItem, tileGridPosition.x, tileGridPosition.y);
        if (isSuccessful)
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
        if (!TryInsertItem(itemToInsert, true)) Destroy(itemToInsert.gameObject);
    }
    public bool TryPickUpRotateInsert(InventoryItem itemInInventory, ItemGrid itemGrid)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;
        SelectedItemGrid = itemGrid;

        Vector2Int itemPosition = new(itemInInventory.XPositionOnTheGrid, itemInInventory.YPositionOnTheGrid);
        PickUp(itemPosition);
        InventoryItem pickedUpItem = CurrentSelectedItem;
        if (itemInInventory.IsRotated)
        {
            if (TryCreateAndInsertItem(itemGrid, itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, itemInInventory.BoughtDaysAgo, isFillingStackFirst: false))
            {
                Destroy(pickedUpItem.gameObject);
                SelectedItemGrid = initialItemGridState;
                return true;
            }
            else
            {
                PlaceDown(itemPosition);
                SelectedItemGrid = initialItemGridState;
                return false;
            }
        }
        else
        {
            if (TryCreateAndInsertItemRotated(itemGrid, itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, itemInInventory.BoughtDaysAgo, isFillingStackFirst: false))
            {
                Destroy(pickedUpItem.gameObject);
                SelectedItemGrid = initialItemGridState;
                return true;
            }
            else
            {
                PlaceDown(itemPosition);
                SelectedItemGrid = initialItemGridState;
                return false;
            }
        }

    }
    private bool TryInsertItem (InventoryItem itemToInsert, bool isFillingStackFirst)
    {
        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForItemInsertion(itemToInsert, isFillingStackFirst);
        if (posOnGrid == null) 
        {
            return false; 
        }
        SelectedItemGrid.TryPlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        return true;
    }

    #endregion
}
