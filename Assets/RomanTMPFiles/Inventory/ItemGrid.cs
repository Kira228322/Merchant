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
    [SerializeField] Sprite _inventoryTileSprite;

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
        if (itemToReceive.CurrentItemsInAStack + itemToPlace.CurrentItemsInAStack > itemToReceive.ItemData.MaxItemsInAStack)
        {
            itemToPlace.CurrentItemsInAStack -= (itemToReceive.ItemData.MaxItemsInAStack - itemToReceive.CurrentItemsInAStack);
            itemToReceive.CurrentItemsInAStack = itemToReceive.ItemData.MaxItemsInAStack;
            return false;
        }
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

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
        //Алгоритм поиска свободного места для автоматического добавления предметов
    {
        for (int y = 0; y < _gridSizeHeight - itemToInsert.Height + 1; y++)
        {
            for (int x = 0; x < _gridSizeWidth - itemToInsert.Width + 1; x++)
            {
                if (AnyAvailableSpace(x,y,itemToInsert.Width, itemToInsert.Height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
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
    private bool AnyAvailableSpace(int positionX, int positionY, int width, int height)
        //Присутствует ли где-то в инвентаре свободное место чтобы поместить предмет размера {width} x {height}?
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
}
