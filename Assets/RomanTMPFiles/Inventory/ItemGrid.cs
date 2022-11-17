using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{

    public const float TileSizeWidth = 160;
    public const float TileSizeHeight = 160;

    [SerializeField] private int _gridSizeWidth;
    [SerializeField] private int _gridSizeHeight;

    private InventoryItem[,] _storedInventoryItems; //Массив, хранящий информацию о всех клеточках в сетке и предметах в них
    private RectTransform _rectTransform;
    private Vector2 _positionOnTheGrid = new();
    private Vector2Int _tileGridPosition = new();

    private void Start()
    {

        _rectTransform = GetComponent<RectTransform>();
        Init(_gridSizeWidth, _gridSizeHeight);

    }

    private void Init(int width, int height)
    {
        _storedInventoryItems = new InventoryItem[width, height];
        Vector2 size = new(width * TileSizeWidth, height * TileSizeHeight);
        _rectTransform.sizeDelta = size;
    }
    public Vector2Int GetTileGridPosition(Vector2 mousePosition, Vector2 canvasLocalScale) //Получить позицию в клетках из позиции мышки, т.е (0;1) или (3;4)
    {
        _positionOnTheGrid.x = mousePosition.x - _rectTransform.position.x;
        _positionOnTheGrid.y = _rectTransform.position.y - mousePosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / TileSizeWidth / canvasLocalScale.x);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / TileSizeHeight / canvasLocalScale.y);

        return _tileGridPosition;
    }

    public InventoryItem PickUpItem(int positionX, int positionY) //Убрать айтем из ячеек
    {
        InventoryItem itemToReturn = _storedInventoryItems[positionX, positionY];

        if (itemToReturn == null) { return null; }

        CleanGridReference(itemToReturn);
        return itemToReturn;
    }

    private void CleanGridReference(InventoryItem item)
        //Очистить сетку, убрать предмет
    {
        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                _storedInventoryItems[item.XPositionOnTheGrid + x, item.YPositionOnTheGrid + y] = null;
            }
        }
    }

    public bool TryPlaceItem(InventoryItem item, int positionX, int positionY)
    {
        if (IsInCorrectPosition(positionX, positionY, item.Width, item.Height) == false)
        {
            return false;
        }

        if (IsNotOverlapping(positionX, positionY, item.Width, item.Height) == false)
        {
            if (IsOverlappingWithTheSameItemType(item, positionX, positionY, item.Width, item.Height, out InventoryItem itemInInventory))
            {
                if (TryPlaceItemInAStack(item, itemInInventory))
                {
                return true;
                }
                return false;
            }
            return false;
        }

        PlaceItem(item, positionX, positionY);
        return true;
    }

    public void PlaceItem(InventoryItem item, int positionX, int positionY)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(_rectTransform);

        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                _storedInventoryItems[positionX + x, positionY + y] = item;

            }
        }

        item.XPositionOnTheGrid = positionX;
        item.YPositionOnTheGrid = positionY;
        Vector2 position = CalculatePositionOnTheGrid(item, positionX, positionY);

        rectTransform.localPosition = position;
    }
    public bool TryPlaceItemInAStack(InventoryItem itemToPlace, InventoryItem itemToReceive)
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
            itemToPlace.CurrentItemsInAStack -= (itemToReceive.ItemData.MaxItemsInAStack - itemToReceive.CurrentItemsInAStack);
            itemToReceive.CurrentItemsInAStack = itemToReceive.ItemData.MaxItemsInAStack;
            return false;
        }
        //...Нет, т.е. поместится полностью, помещаемый предмет можно уничтожать.
        itemToReceive.CurrentItemsInAStack += itemToPlace.CurrentItemsInAStack;
        Destroy(itemToPlace.gameObject);
        return true;
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _storedInventoryItems[x, y];
    }

    public Vector2 CalculatePositionOnTheGrid(InventoryItem item, int positionX, int positionY)
        // Понять, где визуально расположить предмет (нужно для предметов, больших чем 1x1)
    {
        Vector2 position = new();

        position.x = positionX * TileSizeWidth + TileSizeWidth * item.Width / 2;
        position.y = -(positionY * TileSizeHeight + TileSizeHeight * item.Height / 2);
        return position;
    }

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


    private bool IsNotOverlapping(int positionX, int positionY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(_storedInventoryItems[positionX + x, positionY + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsOverlappingWithTheSameItemType(InventoryItem item, int positionX, int positionY, int width, int height, out InventoryItem itemInInventory)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_storedInventoryItems[positionX + x, positionY + y] != null)
                {
                    if (_storedInventoryItems[positionX + x, positionY + y].ItemData.name == item.ItemData.name)
                    {
                        itemInInventory = _storedInventoryItems[positionX + x, positionY + y];
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
            return false;
        }

        positionX += width - 1;
        positionY += height - 1;

        if (IsInBounds(positionX, positionY) == false)
        {
            return false;
        }

        return true;
    }
    private Vector2Int? FindAvailableSpaceForNewStack(InventoryItem itemToInsert)
    {
        for (int y = 0; y < _gridSizeHeight - itemToInsert.Height + 1; y++)
        {
            for (int x = 0; x < _gridSizeWidth - itemToInsert.Width + 1; x++)
            {
                if (AnyAvailableSpace(x, y, itemToInsert.Width, itemToInsert.Height))
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
    private bool AnyAvailableSpace(int positionX, int positionY, int width, int height)
        //Присутствует ли на этом месте в инвентаре свободное место чтобы поместить предмет размера {width} x {height}?
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_storedInventoryItems[positionX + x, positionY + y] != null)
                {

                    return false;
                }
            }
        }

        return true;
    }
    private bool AnyUnfilledStack(int positionX, int positionY, InventoryItem targetItem)
    {
        //Если тип предмета, лежащий в инвентаре такой же, как предмет, который мы пытаемся установить И
        //И стак этого предмета неполный, т.е может вместить в себя ещё столько предметов, сколько в помещаемом стаке, то true.
        if (_storedInventoryItems[positionX, positionY] != null)
        {
            if (_storedInventoryItems[positionX, positionY].ItemData.name == targetItem.ItemData.name)
            {
                if ((_storedInventoryItems[positionX, positionY].ItemData.MaxItemsInAStack -
                _storedInventoryItems[positionX, positionY].CurrentItemsInAStack) >= targetItem.CurrentItemsInAStack)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
