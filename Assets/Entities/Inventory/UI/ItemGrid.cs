using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrid : MonoBehaviour
{

    #region Поля, свойства и события

    public const float TileSizeWidth = 160;
    public const float TileSizeHeight = 160;

    [SerializeField] private int _gridSizeWidth;
    [SerializeField] private int _gridSizeHeight;

    private List<InventoryItem[]> _storedInventoryItems;
    private RectTransform _rectTransform;
    private Vector2 _positionOnTheGrid = new();
    private Vector2Int _tileGridPosition = new();

    public event UnityAction<InventoryItem> ItemPlacedInTheGrid;
    public event UnityAction<InventoryItem, int> ItemPlacedInTheStack;
    public event UnityAction<InventoryItem> ItemRemovedFromTheGrid;
    public event UnityAction<InventoryItem, int> ItemsRemovedFromTheStack;

    #endregion
    #region Методы инициализации

    private void Start()
    {

        _rectTransform = GetComponent<RectTransform>();
        Init(_gridSizeWidth, _gridSizeHeight);

    }

    private void Init(int width, int height)
    {
        _storedInventoryItems = new List<InventoryItem[]>();

        for (int i = 0; i < height; i++)
        {
            InventoryItem[] rowArray = new InventoryItem[width];
            _storedInventoryItems.Add(rowArray);
        }

        Vector2 size = new(width * TileSizeWidth, height * TileSizeHeight);
        _rectTransform.sizeDelta = size;
    }
    #endregion
    #region Методы проверки положения айтема (свободный стак, находится ли внутри границ и др.)
    private bool IsInBounds(int positionX, int positionY) //Находится ли внутри границ сетки?
    {
        if (positionX < 0 || positionY < 0)
        {
            return false;
        }

        if (positionX >= _gridSizeWidth || positionY >= _gridSizeHeight)
        {
            return false;
        }
        return true;
    }
    private bool IsOverlapping(int positionX, int positionY, int width, int height) //Пересечётся ли этот предмет с другими предметами, будучи поставленным в эту клетку?
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(_storedInventoryItems[positionY + y][positionX + x] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public bool IsOverlappingWithTheSameItemType(InventoryItem item, int positionX, int positionY, int width, int height, out InventoryItem itemInInventory)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_storedInventoryItems[positionY + y][positionX + x] != null)
                {
                    if (_storedInventoryItems[positionY + y][positionX + x].ItemData.name == item.ItemData.name)
                    {
                        itemInInventory = _storedInventoryItems[positionY + y][positionX + x];
                        return true;
                    }
                }
            }
        }

        itemInInventory = null;
        return false;
    }
    public bool IsInCorrectPosition(int positionX, int positionY, int width, int height)
    {
        if (IsInBounds(positionX, positionY) == false)
        {
            Debug.Log($"Not in the correct position, because starting point {positionX},{positionY} is not in grid");
            return false;
        }

        positionX += width - 1;
        positionY += height - 1;

        if (IsInBounds(positionX, positionY) == false)
        {
            Debug.Log($"Not in the correct position, because ending point (BR) {positionX},{positionY} is not in grid");
            return false;
        }

        return true;
    }
    private bool IsRotDifferenceBetweenTwoItemsLessThanOne(InventoryItem item1, InventoryItem item2)
    {
        if (item1.ItemData.IsPerishable && item2.ItemData.IsPerishable)
        {
            if (Math.Abs(item1.BoughtDaysAgo - item2.BoughtDaysAgo) < 1)
            {
                return true;
            }
            return false;
        }
        return true; //Если сравнены две вещи, которые не портятся, то всегда true. По сути, item1 и item2 - всегда вещи одного типа 
    }
    private bool AnyUnfilledStack(int positionX, int positionY, InventoryItem targetItem)
    {
        //Если тип предмета, лежащий в инвентаре такой же, как предмет, который мы пытаемся установить И
        //И стак этого предмета неполный, т.е может вместить в себя ещё столько предметов, сколько в помещаемом стаке, то true.
        //И должен
        InventoryItem itemInInventory = _storedInventoryItems[positionY][positionX];
        if (itemInInventory != null)
        {
            if (itemInInventory.ItemData.name == targetItem.ItemData.name)
            {
                if ((itemInInventory.ItemData.MaxItemsInAStack - itemInInventory.CurrentItemsInAStack) >= targetItem.CurrentItemsInAStack)
                {
                    if ((itemInInventory.BoughtDaysAgo - targetItem.BoughtDaysAgo) < 1)
                    return true;
                }
            }
        }
        return false;
    }

    #endregion
    #region Методы получения информации об инвентаре
    public Vector2Int GetTileGridPosition(Vector2 mousePosition, Vector2 canvasLocalScale) //Получить позицию в клетках из позиции мышки, т.е (0;1) или (3;4)
    {
        _positionOnTheGrid.x = mousePosition.x - _rectTransform.position.x;
        _positionOnTheGrid.y = _rectTransform.position.y - mousePosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / TileSizeWidth / canvasLocalScale.x);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / TileSizeHeight / canvasLocalScale.y);

        return _tileGridPosition;
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _storedInventoryItems[y][x];
    }
    public List<InventoryItem> GetAllItemsInTheGrid()
        //Дорогой метод, на данный момент нигде не используется, но может быть нужен в будущем.
        //По сути, его функционал и так выполняет PlayersInventory, следя за всеми айтемами в любой момент (и эффективнее),
        //но такой метод может быть нужен, если нужно получить все предметы в другой сетке инвентаря (инвентаря не игрока)
        //может какой нибудь сундук или я хуй знает, в общем, если не найдет своё применение, потом уберу.
    {
        List<InventoryItem> result = new();
        for (int i = 0; i < _storedInventoryItems.Count; i++)
        {
            for (int j = 0; j < _storedInventoryItems[i].Length; j++)
            {
                if (_storedInventoryItems[i][j] != null && !result.Contains(_storedInventoryItems[i][j]))
                {
                    result.Add(_storedInventoryItems[i][j]);
                }
            }
        }
        return result;
    }

    public Vector2 CalculatePositionOnTheGrid(InventoryItem item, int positionX, int positionY) // Понять, где визуально расположить предмет (нужно для предметов, больших чем 1x1)
    {
        Vector2 position = new();

        position.x = positionX * TileSizeWidth + TileSizeWidth * item.Width / 2;
        position.y = -(positionY * TileSizeHeight + TileSizeHeight * item.Height / 2);
        return position;
    }

    public Vector2Int? FindSpaceForItemInsertion(InventoryItem itemToInsert, bool isFillingStackFirst)
    {
        //Буль определяет порядок заполнения - если true, сначала будет искать незаполненные стаки и заполнять их (как в майнкрафте)
        //Если false, сначала будет создавать новые стаки, а если места не останется, заполнять имеющиеся.
        //Такая неоднозначность нужна поскольку сейчас, когда я это пишу, я планирую использовать это для создания
        //кнопки разделения айтемов:
        //Очевидно, обычным режимом работы должно быть сперва заполнение стаков, но при разделении ведь нужно обязательно создавать
        //новый стак, потому что если просто делать InsertItem() по нажатию кнопки разделения, то предмет просто вернется в стак,
        //из которого его разделили 
        //
        //Поэтому кнопка разделения айтемов будет использовать этот метод с false, а обычное поведение заполнения с true.
        if (isFillingStackFirst)
        {
            Vector2Int? result = FindUnfilledStackOfSameItems(itemToInsert);
            if (result == null)
            {
                result = FindAvailableSpaceForNewStack(itemToInsert);
            }
            return result;
        }
        else
        {
            Vector2Int? result = FindAvailableSpaceForNewStack(itemToInsert);
            if (result == null)
            {
                result = FindUnfilledStackOfSameItems(itemToInsert);
            }
            return result;
        }
    }
    private Vector2Int? FindAvailableSpaceForNewStack(InventoryItem itemToInsert)
    {
        for (int y = 0; y < _gridSizeHeight - itemToInsert.Height + 1; y++)
        {
            for (int x = 0; x < _gridSizeWidth - itemToInsert.Width + 1; x++)
            {
                if (!IsOverlapping(x, y, itemToInsert.Width, itemToInsert.Height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }
    private Vector2Int? FindUnfilledStackOfSameItems(InventoryItem itemToInsert)
    {
        for (int y = 0; y < _gridSizeHeight; y++)
        {
            for (int x = 0; x < _gridSizeWidth; x++)
            {
                if (AnyUnfilledStack(x, y, itemToInsert))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }

    #endregion
    #region Методы действий с айтемом (поднять, уничтожить и др.)
    public InventoryItem PickUpItem(int positionX, int positionY) //Убрать айтем из ячеек и return его, чтобы взять в "руку"
    {
        InventoryItem itemToReturn = _storedInventoryItems[positionY][positionX];

        if (itemToReturn == null) { return null; }

        CleanGridReference(itemToReturn);
        ItemRemovedFromTheGrid?.Invoke(itemToReturn);
        return itemToReturn;
    }
    public void DestroyItem(int positionX, int positionY)
    {
        InventoryItem itemToDestroy = _storedInventoryItems[positionY][positionX];

        if (itemToDestroy == null) { return; }

        CleanGridReference(itemToDestroy);
        ItemRemovedFromTheGrid?.Invoke(itemToDestroy);
        Destroy(itemToDestroy.gameObject);
    }
    public void DestroyItem(InventoryItem itemToDestroy)
    {
        CleanGridReference(itemToDestroy);
        ItemRemovedFromTheGrid?.Invoke(itemToDestroy);
        Destroy(itemToDestroy.gameObject);
    }
    private void CleanGridReference(InventoryItem item) //Очистить сетку, убрать предмет
    {
        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                _storedInventoryItems[item.YPositionOnTheGrid + y][item.XPositionOnTheGrid + x] = null;
            }
        }
    }
    public bool TryPlaceItem(InventoryItem item, int positionX, int positionY)
    {
        if (IsOverlapping(positionX, positionY, item.Width, item.Height))
        {
            if (IsOverlappingWithTheSameItemType(item, positionX, positionY, item.Width, item.Height, out InventoryItem itemInInventory))
            {
                if (IsRotDifferenceBetweenTwoItemsLessThanOne(item, itemInInventory))
                {
                    //ItemRemovedFromTheGrid?.Invoke(item); 16.12.22: Я не знаю, зачем это здесь было нужно: предмет удаляется 
                    //из сетки ещё в момент его поднятия. Работало и с этим, и щас вроде работает.
                    //Если что-то сломается, то стоит обратить внимание сюда
                    //Потенциально может сломаться, если игрок переносит в стек предмет из другой сетки, но другой сетки в игре пока нет
                    //UPD 24.12.22: Может быть, если происходит инсерт в стак, если больше нет места для предметов?
                    if (!TryPlaceItemInAStack(item, itemInInventory, out int amountInserted))
                    {
                        //Означает, что сколько-то поместилось, но не весь стак целиком
                        ItemPlacedInTheStack?.Invoke(itemInInventory, amountInserted);
                        Debug.LogWarning("Не весь стак поставлен");
                        return false; 
                    }
                    else
                    {
                        //Означает, что поместился весь стак целиком
                        ItemPlacedInTheStack?.Invoke(itemInInventory, amountInserted);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        //IsOverlapping == false, следовательно в этих клеточках нет предметов, можно спокойно ставить
        //Осталось лишь проверить, если он вообще в границах сетки
        if (!IsInCorrectPosition(positionX, positionY, item.Width, item.Height))
        {
            return false;
        }
        PlaceItem(item, positionX, positionY);
        ItemPlacedInTheGrid?.Invoke(item);
        return true;
    }

    private void PlaceItem(InventoryItem item, int positionX, int positionY)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(_rectTransform);

        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                _storedInventoryItems[positionY + y][positionX + x] = item;
            }
        }

        item.XPositionOnTheGrid = positionX;
        item.YPositionOnTheGrid = positionY;
        Vector2 position = CalculatePositionOnTheGrid(item, positionX, positionY);

        rectTransform.localPosition = position;
    }
    public bool TryPlaceItemInAStack(InventoryItem itemToPlace, InventoryItem itemToReceive, out int amountInserted)
    {
        //Если прибавить количество айтемов в помещаемом к количеству айтемов в имеющемся, то получится больше, чем макс.стак?
        if (itemToReceive.CurrentItemsInAStack + itemToPlace.CurrentItemsInAStack > itemToReceive.ItemData.MaxItemsInAStack)
        {
            //...Да, т.е. поместится не полностью, значит стак должен укомплектоваться до максимума,
            //а какое-то количество предметов должно вернуться обратно. Чтобы найти, сколько предметов должно вернуться обратно,
            //отнимаю Макс.стак минус сколько уже лежит -> получаю сколько не хватает.
            //сколько не хватает вычитаю из текущего количества в стаке и возвращаю на место остатки (return false)

            //Я понимаю, что это математика для 4 класса,
            //но подробные объяснения лучше помогут в том числе и мне разобраться в том, что здесь будет, если в будущем нужна будет отладка.
            amountInserted = itemToReceive.ItemData.MaxItemsInAStack - itemToReceive.CurrentItemsInAStack;
            itemToPlace.CurrentItemsInAStack -= amountInserted;
            itemToReceive.CurrentItemsInAStack = itemToReceive.ItemData.MaxItemsInAStack;
            return false;
        }
        //...Нет, т.е. поместится полностью, помещаемый предмет можно уничтожать.
        itemToReceive.CurrentItemsInAStack += itemToPlace.CurrentItemsInAStack;
        amountInserted = itemToPlace.CurrentItemsInAStack;
        Destroy(itemToPlace.gameObject);
        return true;
    }
    public void RemoveItemsFromAStack(InventoryItem itemToTruncate, int amount)
    {
        itemToTruncate.CurrentItemsInAStack -= amount;
        ItemsRemovedFromTheStack?.Invoke(itemToTruncate, amount);
        if (itemToTruncate.CurrentItemsInAStack <= 0)
        {
            DestroyItem(itemToTruncate);
        }
    }

    #endregion
    public void AddRowsToInventory(int numberOfRowsToAdd) 
    {
        for (int i = 0; i < numberOfRowsToAdd; i++)
        {
            InventoryItem[] rowArray = new InventoryItem[_gridSizeWidth];
            _storedInventoryItems.Add(rowArray);
        }
        _gridSizeHeight += numberOfRowsToAdd;

        Vector2 size = new(_gridSizeWidth * TileSizeWidth, _gridSizeHeight * TileSizeHeight);
        _rectTransform.sizeDelta = size;

    }
}
