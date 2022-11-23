using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrid : MonoBehaviour
{

    public const float TileSizeWidth = 160;
    public const float TileSizeHeight = 160;

    [SerializeField] private int _gridSizeWidth;
    [SerializeField] private int _gridSizeHeight;

    private InventoryItem[,] _storedInventoryItems; //������, �������� ���������� � ���� ��������� � ����� � ��������� � ���
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

    public void AddRowsToInventory(int numberOfRowsToAdd)
    {
        InventoryItem[,] newArray = new InventoryItem[numberOfRowsToAdd + _gridSizeHeight, _gridSizeWidth];

        Array.Copy(_storedInventoryItems, newArray, _storedInventoryItems.Length);

        _storedInventoryItems = newArray;
        _gridSizeHeight += numberOfRowsToAdd;

        Vector2 size = new(_gridSizeWidth * TileSizeWidth, _gridSizeHeight * TileSizeHeight);
        _rectTransform.sizeDelta = size;

    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition, Vector2 canvasLocalScale) //�������� ������� � ������� �� ������� �����, �.� (0;1) ��� (3;4)
    {
        _positionOnTheGrid.x = mousePosition.x - _rectTransform.position.x;
        _positionOnTheGrid.y = _rectTransform.position.y - mousePosition.y;

        _tileGridPosition.x = (int)(_positionOnTheGrid.x / TileSizeWidth / canvasLocalScale.x);
        _tileGridPosition.y = (int)(_positionOnTheGrid.y / TileSizeHeight / canvasLocalScale.y);

        return _tileGridPosition;
    }

    public InventoryItem PickUpItem(int positionX, int positionY) //������ ����� �� ����� � return ���, ����� ����� � "����"
    {
        InventoryItem itemToReturn = _storedInventoryItems[positionX, positionY];

        if (itemToReturn == null) { return null; }

        CleanGridReference(itemToReturn);
        TradeManager.PlayersInventory.RemoveItemInInventory(itemToReturn);
        return itemToReturn;
    }

    public void DestroyItem(int positionX, int positionY)
    {
        InventoryItem itemToDestroy = _storedInventoryItems[positionX, positionY];

        if (itemToDestroy == null) { return; }

        CleanGridReference(itemToDestroy);
        TradeManager.PlayersInventory.RemoveItemInInventory(itemToDestroy);
        Destroy(itemToDestroy.gameObject);
    }
    public void DestroyItem(InventoryItem itemToDestroy)
    {
        CleanGridReference(itemToDestroy);
        TradeManager.PlayersInventory.RemoveItemInInventory(itemToDestroy);
        Destroy(itemToDestroy.gameObject);
    }

    private void CleanGridReference(InventoryItem item) //�������� �����, ������ �������
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
                if (IsRotDifferenceBetweenTwoItemsLessThanOne(item, itemInInventory))
                {
                    if (TryPlaceItemInAStack(item, itemInInventory))
                    {
                        TradeManager.PlayersInventory.RemoveItemInInventory(item);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        PlaceItem(item, positionX, positionY);
        TradeManager.PlayersInventory.AddItemInInventory(item);
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
        //���� ��������� ���������� ������� � ���������� � ���������� ������� � ���������, �� ��������� ������, ��� ����.����?
        if (itemToReceive.CurrentItemsInAStack + itemToPlace.CurrentItemsInAStack > itemToReceive.ItemData.MaxItemsInAStack)
        {
            //...��, �.�. ���������� �� ���������, ������ ���� ������ ���������������� �� ���������,
            //� �����-�� ���������� ��������� ������ ��������� �������. ����� �����, ������� ��������� ������ ��������� �������,
            //������� ����.���� ����� ������� ��� ����� -> ������� ������� �� �������.
            //������� �� ������� ������� �� �������� ���������� � ����� � ��������� �� ����� ������� (return false)

            //� �������, ��� ��� ���������� ��� 4 ������,
            //�� ��������� ���������� ����� ������� � ��� ����� � ��� ����������� � ���, ��� ����� �����, ���� � ������� ����� ����� �������.
            itemToPlace.CurrentItemsInAStack -= (itemToReceive.ItemData.MaxItemsInAStack - itemToReceive.CurrentItemsInAStack);
            itemToReceive.CurrentItemsInAStack = itemToReceive.ItemData.MaxItemsInAStack;
            return false;
        }
        //...���, �.�. ���������� ���������, ���������� ������� ����� ����������.
        itemToReceive.CurrentItemsInAStack += itemToPlace.CurrentItemsInAStack;
        Destroy(itemToPlace.gameObject);
        return true;
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _storedInventoryItems[x, y];
    }

    public Vector2 CalculatePositionOnTheGrid(InventoryItem item, int positionX, int positionY) // ������, ��� ��������� ����������� ������� (����� ��� ���������, ������� ��� 1x1)
    {
        Vector2 position = new();

        position.x = positionX * TileSizeWidth + TileSizeWidth * item.Width / 2;
        position.y = -(positionY * TileSizeHeight + TileSizeHeight * item.Height / 2);
        return position;
    }

    private bool IsInBounds(int positionX, int positionY) //��������� �� ������ ������ �����?
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
        //���� ���������� ������� ���������� - ���� true, ������� ����� ������ ������������� ����� � ��������� �� (��� � ����������)
        //���� false, ������� ����� ��������� ����� �����, � ���� ����� �� ���������, ��������� ���������.
        //����� ��������������� ����� ��������� ������, ����� � ��� ����, � �������� ������������ ��� ��� ��������
        //������ ���������� �������:
        //��������, ������� ������� ������ ������ ���� ������ ���������� ������, �� ��� ���������� ���� ����� ����������� ���������
        //����� ����, ������ ��� ���� ������ ������ InsertItem() �� ������� ������ ����������, �� ������� ������ �������� � ����,
        //�� �������� ��� ��������� 
        //
        //������� ������ ���������� ������� ����� ������������ ���� ����� � false, � ������� ��������� ���������� � true.
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
    private bool IsRotDifferenceBetweenTwoItemsLessThanOne(InventoryItem item1, InventoryItem item2)
    {
        if (item1.ItemData.IsPerishable || item2.ItemData.IsPerishable)
        {
            if (Math.Abs(item1.BoughtDaysAgo - item2.BoughtDaysAgo) < 1)
            {
                return true;
            }
            return false;
        }
        return true; //���� �������� ��� ����, ������� �� ��������, �� ������ true. �� ����, item1 � item2 - ������ ���� ������ ���� 
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
        //������������ �� �� ���� ����� � ��������� ��������� ����� ����� ��������� ������� ������� {width} x {height}?
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
        //���� ��� ��������, ������� � ��������� ����� ��, ��� �������, ������� �� �������� ���������� �
        //� ���� ����� �������� ��������, �.� ����� �������� � ���� ��� ������� ���������, ������� � ���������� �����, �� true.
        //� ������
        InventoryItem itemInInventory = _storedInventoryItems[positionX, positionY];
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
}
