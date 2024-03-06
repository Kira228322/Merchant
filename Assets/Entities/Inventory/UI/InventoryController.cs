using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    #region Поля и свойства

    public static InventoryController Instance;

    [SerializeField] private QuestItemHolder _questItemHolder;
    [HideInInspector] private ItemGrid _selectedItemGrid;

    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _itemInfoPanelPrefab;
    [SerializeField] private Transform _canvasTransform;

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

    public InventoryItem CurrentSelectedItem
    {
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
    private bool IsItemAcceptable()
    {
        if (SelectedItemGrid == null)
            return false;

        if (SelectedItemGrid.gameObject.TryGetComponent(out ItemContainer container)) //Работаем с правым контейнером
        {
            if (container.RequiredItemTypes.Count != 0)
            {
                if (!container.RequiredItemTypes.Contains(CurrentSelectedItem.ItemData.TypeOfItem))
                    return false;
            }
            if (container.ItemRotThreshold != 0 && CurrentSelectedItem.ItemData.IsPerishable)
            {
                if (container.ItemRotThreshold > 0)
                {
                    if (1 - (CurrentSelectedItem.BoughtDaysAgo / CurrentSelectedItem.ItemData.DaysToSpoil) < container.ItemRotThreshold)
                    {
                        return false;
                    }
                }
                else
                {
                    if (1 - (CurrentSelectedItem.BoughtDaysAgo / CurrentSelectedItem.ItemData.DaysToSpoil) > -container.ItemRotThreshold)
                    {
                        return false;
                    }
                }

            }
            switch (container.QuestItemsBehaviour)
            {
                case ItemContainer.QuestItemsBehaviourEnum.NotQuestItems:
                    if (CurrentSelectedItem.ItemData.IsQuestItem)
                        return false;
                    break;

                case ItemContainer.QuestItemsBehaviourEnum.OnlyQuestItems:
                    if (!CurrentSelectedItem.ItemData.IsQuestItem)
                        return false;
                    break;

                default:
                    break;
            }
        }
        if (SelectedItemGrid == _questItemHolder.ItemGrid) //Работаем с левым доп.контейнером
        {
            //Здесь можно было бы даже не делать проверку и всегда возвращать false,
            //потому что в этот контейнер вообще ничего нельзя положить, только достать.
            //Но я всё-таки добавлю, чтобы в него можно было ложить квестовые айтемы,
            //на случай если они всё-же как-то оказались в основном инвентаре
            if (!CurrentSelectedItem.ItemData.IsQuestItem)
                return false;
        }
        if (CurrentSelectedItem.ItemData.IsQuestItem)   //Работаем с основным инвентарем
            return false;

        return true;
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
            if (!IsItemAcceptable())
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
            //Для пальца OnPointerExit срабатывает в том числе если палец отпускает нажатие. Для мышки не так. 
            //Поэтому в GridInteract.OnPointerExit я запретил пальцу обнулять ItemGrid, если нажатие было отпущено
            //Здесь, наоборот, нужно обнулить. Таким образом мы добиваемся, что OnPointerExit будет срабатывать только если палец
            //действительно вышел за пределы ItemGrid. (24.03.23)
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
    #region Методы, связанные с взаимодействием с айтемом
    private class CompactedItem
    {
        public Item item;
        public int amount;
        public float daysBoughtAgo;

        public CompactedItem(InventoryItem itemToCompact)
        {
            item = itemToCompact.ItemData;
            amount = itemToCompact.CurrentItemsInAStack;
            daysBoughtAgo = itemToCompact.BoughtDaysAgo;
        }
        public CompactedItem()
        {

        }
    }
    #region CanInsertMultipleItems
    public bool CanInsertMultipleItems(ItemGrid itemGrid, List<ItemReward> rewardItems)
    {
        List<CompactedItem> items = new();
        foreach (var item in rewardItems)
        {
            CompactedItem newItemReward = new()
            {
                item = ItemDatabase.GetItem(item.itemName),
                amount = item.amount,
                daysBoughtAgo = item.daysBoughtAgo
            };
        }
        /*
         Метод проверяет, есть ли место для размещения нескольких предметов. Сам не размещает (почему - комментарий внизу)
        */
        if (items.Count == 0)
        {
            return true;
        }

        //Начнем с того, что отсортируем лист помещаемых предметов от самого крупного к самому маленькому
        //Если поместить сначала мелкие, для крупных может не остаться места.

        items.Sort((x, y) => (y.item.CellSizeWidth * y.item.CellSizeHeight).CompareTo(x.item.CellSizeWidth * x.item.CellSizeHeight));
        //^ сортировка по занимаемой площади https://stackoverflow.com/a/3309292

        Dictionary<InventoryItem, int> placedInventoryItems = new();
        //^Дикционарий для того, чтобы запомнить, какие вещи были помещены и сколько каждой вещи было помещено.
        //Это нужно, чтобы если не получится поместить все, знать сколько чего и куда было помещено и убрать столько, сколько нужно.
        //(Если некоторые были помещены в стак, то нужно убрать не весь стак, а столько, сколько в него положили, так ведь?)

        foreach (CompactedItem itemReward in items)
        {
            if (itemReward.item.IsQuestItem)
                continue; //Квестовые айтемы отправляются в другой инвентарь, поэтому их можно не проверять

            InventoryItem placedItem = TryCreateAndInsertItem(itemReward.item, itemReward.amount, itemReward.daysBoughtAgo, itemGrid);
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

        /*Почему удаляются айтемы вне зависимости от результата:
        Данный метод всего лишь *проверяет*, есть ли возможность поставить несколько вещей.
        Он сам их не помещает в инвентарь. Прекрасно осознаю, что это неэффективно, однако
        такое разделение проще для понимания, например в Quest есть метод GiveReward().
        Он выдает экспу, рубли и айтемы, если таковые есть. Если бы метод, в котором этот комментарий,
        выдавал вещи, то об этом бы пришлось помнить и в том методе, соответственно выдавать там только деньги и опыт.
        Это было бы запутанно, потому что кто-нибудь зашел бы через несколько месяцев и не понял почему там не выдаются предметы.

        Поэтому считаю неэффективность оправданной, к тому же эта группа методов срабатывает только по нажатию кнопки завершения квеста,
        что в игре не будет часто происходить. (26.01.23)
        */

    }
    public bool CanInsertMultipleItems(ItemGrid itemGrid, List<InventoryItem> inventoryItems)
    {
        List<InventoryItem> items = new(inventoryItems);

        /*
         Метод проверяет, есть ли место для размещения нескольких предметов. Сам не размещает (почему - комментарий внизу)
        */
        if (items.Count == 0)
        {
            return true;
        }

        //Начнем с того, что отсортируем лист помещаемых предметов от самого крупного к самому маленькому
        //Если поместить сначала мелкие, для крупных может не остаться места.

        items.Sort((x, y) => (y.ItemData.CellSizeWidth * y.ItemData.CellSizeHeight).CompareTo(x.ItemData.CellSizeWidth * x.ItemData.CellSizeHeight));
        //^ сортировка по занимаемой площади https://stackoverflow.com/a/3309292

        Dictionary<InventoryItem, int> placedInventoryItems = new();
        //^Дикционарий для того, чтобы запомнить, какие вещи были помещены и сколько каждой вещи было помещено.
        //Это нужно, чтобы если не получится поместить все, знать сколько чего и куда было помещено и убрать столько, сколько нужно.
        //(Если некоторые были помещены в стак, то нужно убрать не весь стак, а столько, сколько в него положили, так ведь?)

        foreach (InventoryItem item in items)
        {
            if (item.ItemData.IsQuestItem)
                continue; //Квестовые айтемы отправляются в другой инвентарь, поэтому их можно не проверять

            InventoryItem placedItem = TryCreateAndInsertItem(item.ItemData, item.CurrentItemsInAStack, item.BoughtDaysAgo, itemGrid);
            if (placedItem != null)
            {
                placedInventoryItems.Add(placedItem, item.CurrentItemsInAStack);
            }
            else
            {
                foreach (var dictionaryItem in placedInventoryItems)
                {
                    itemGrid.RemoveItemsFromAStack(dictionaryItem.Key, dictionaryItem.Value);
                }
                return false;
            }
        }
        foreach (var item in placedInventoryItems)
        {
            itemGrid.RemoveItemsFromAStack(item.Key, item.Value);
        }
        return true;
    }
    #endregion
    public bool CanInsertItem(Item item, int amount, ItemGrid itemGrid = null)
    {
        if (itemGrid == null)
            itemGrid = Player.Instance.Inventory.BaseItemGrid;

        InventoryItem placedItem = TryCreateAndInsertItem(item, amount, 0, itemGrid);
        if (placedItem == null)
        {
            return false;
        }
        itemGrid.RemoveItemsFromAStack(placedItem, amount);
        return true;
    }
    public void Sort(ItemGrid itemGrid)
    {
        //Сортирует выбранную itemGrid по размеру айтемов. Запоминает и удаляет все предметы из неё,
        //а потом инсертит от больших к маленьким. 

        List<InventoryItem> oldItems = itemGrid.GetAllItemsInTheGrid();
        List<CompactedItem> items = oldItems.Select(item => new CompactedItem(item)).ToList(); //копирует, запоминает старые предметы
        foreach (InventoryItem item in oldItems)
            DestroyItem(itemGrid, item);

        items.Sort((x, y) => (y.item.CellSizeWidth * y.item.CellSizeHeight).CompareTo(x.item.CellSizeWidth * x.item.CellSizeHeight));
        //^ сортировка по занимаемой площади https://stackoverflow.com/a/3309292

        foreach (CompactedItem item in items)
        {
            TryCreateAndInsertItem(item.item, item.amount, item.daysBoughtAgo, itemGrid);
        }
    }
    public InventoryItem TryCreateAndInsertItem(Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst = true, ItemGrid itemGrid = null, bool forcedPlacement = false)
    {
        if (amount <= 0)
            return null;
        if (itemGrid == null)
            itemGrid = Player.Instance.BaseItemGrid;
        ItemGrid additionalItemGrid = Player.Instance.QuestItemGrid;

        if (item.IsQuestItem)
        {
            InventoryItem result = TryCreateInsertTypeSelection(additionalItemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
            if (result == null)
                Debug.LogError($"Не удалось поместить квестовый предмет {item.Name} в доп.инвентарь!");
            return result;
        }
        else
        {
            InventoryItem result = TryCreateInsertTypeSelection(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
            if (result == null && forcedPlacement)
            {
                result = TryCreateInsertTypeSelection(additionalItemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
                if (result == null)
                    Debug.LogError($"{item.Name} не квестовый, но должен был быть помещен в дополнительный инвентарь, чего сделать не удалось!");
            }
            return result;
        }
    }
    private InventoryItem TryCreateInsertTypeSelection(ItemGrid itemGrid, Item item, int amount, float daysBoughtAgo, bool isFillingStackFirst)
    {
        InventoryItem result;
        result = TryCreateAndInsertItemUnrotated(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
        if (result == null)
            result = TryCreateAndInsertItemRotated(itemGrid, item, amount, daysBoughtAgo, isFillingStackFirst);
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
        //Не получилось поместить
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
        //Не получилось поместить
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
        _itemPickedUpFromPosition = new Vector2Int(CurrentSelectedItem.XPositionOnTheGrid, CurrentSelectedItem.YPositionOnTheGrid);
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
    private InventoryItem CreateItem(Item item, int amount, float boughtDaysAgo)
    {
        InventoryItem spawnedItem = Instantiate(_itemPrefab, _canvasTransform).GetComponent<InventoryItem>();
        spawnedItem.SetItemFromData(item);
        CurrentSelectedItem = spawnedItem;

        spawnedItem.CurrentItemsInAStack = amount;
        spawnedItem.BoughtDaysAgo = boughtDaysAgo;

        _rectTransform = spawnedItem.GetComponent<RectTransform>();
        _rectTransform.SetAsLastSibling();

        CurrentSelectedItem.transform.localScale = Vector2.one;

        return spawnedItem;
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
    public InventoryItem TryPickUpRotateInsert(InventoryItem itemInInventory, ItemGrid itemGrid)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;
        SelectedItemGrid = itemGrid;

        Vector2Int itemPosition = new(itemInInventory.XPositionOnTheGrid, itemInInventory.YPositionOnTheGrid);
        PickUp(itemPosition);
        InventoryItem pickedUpItem = CurrentSelectedItem;
        if (itemInInventory.IsRotated)
        {
            InventoryItem result = TryCreateAndInsertItem(itemInInventory.ItemData, itemInInventory.CurrentItemsInAStack, itemInInventory.BoughtDaysAgo, itemGrid);
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
    public InventoryItem MoveFromGridToGrid(ItemGrid source, ItemGrid destination, InventoryItem item)
    {
        ItemGrid initialItemGridState = SelectedItemGrid;
        SelectedItemGrid = source;

        Vector2Int itemPosition = new(item.XPositionOnTheGrid, item.YPositionOnTheGrid);
        PickUp(itemPosition);
        InventoryItem pickedUpItem = CurrentSelectedItem;
        InventoryItem result = TryCreateAndInsertItem(item.ItemData, item.CurrentItemsInAStack, item.BoughtDaysAgo, destination);
        if (result != null)
        {
            Destroy(pickedUpItem.gameObject);
            SelectedItemGrid = initialItemGridState;
        }
        return result;
    }
    private InventoryItem TryInsertItem(InventoryItem itemToInsert, bool isFillingStackFirst)
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
