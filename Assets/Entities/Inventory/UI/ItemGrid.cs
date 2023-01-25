using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrid : MonoBehaviour
{

    #region ����, �������� � �������

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
    #region ������ �������������

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
    #region ������ �������� ��������� ������ (��������� ����, ��������� �� ������ ������ � ��.)
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
    private bool IsOverlapping(int positionX, int positionY, int width, int height) //����������� �� ���� ������� � ������� ����������, ������ ������������ � ��� ������?
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
        return true; //���� �������� ��� ����, ������� �� ��������, �� ������ true. �� ����, item1 � item2 - ������ ���� ������ ���� 
    }
    private bool AnyUnfilledStack(int positionX, int positionY, InventoryItem targetItem)
    {
        //���� ��� ��������, ������� � ��������� ����� ��, ��� �������, ������� �� �������� ���������� �
        //� ���� ����� �������� ��������, �.� ����� �������� � ���� ��� ������� ���������, ������� � ���������� �����, �� true.
        //� ������
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
    #region ������ ��������� ���������� �� ���������
    public Vector2Int GetTileGridPosition(Vector2 mousePosition, Vector2 canvasLocalScale) //�������� ������� � ������� �� ������� �����, �.� (0;1) ��� (3;4)
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
        //������� �����, �� ������ ������ ����� �� ������������, �� ����� ���� ����� � �������.
        //�� ����, ��� ���������� � ��� ��������� PlayersInventory, ����� �� ����� �������� � ����� ������ (� �����������),
        //�� ����� ����� ����� ���� �����, ���� ����� �������� ��� �������� � ������ ����� ��������� (��������� �� ������)
        //����� ����� ������ ������ ��� � ��� �����, � �����, ���� �� ������ ��� ����������, ����� �����.
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

    public Vector2 CalculatePositionOnTheGrid(InventoryItem item, int positionX, int positionY) // ������, ��� ��������� ����������� ������� (����� ��� ���������, ������� ��� 1x1)
    {
        Vector2 position = new();

        position.x = positionX * TileSizeWidth + TileSizeWidth * item.Width / 2;
        position.y = -(positionY * TileSizeHeight + TileSizeHeight * item.Height / 2);
        return position;
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
    #region ������ �������� � ������� (�������, ���������� � ��.)
    public InventoryItem PickUpItem(int positionX, int positionY) //������ ����� �� ����� � return ���, ����� ����� � "����"
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
    private void CleanGridReference(InventoryItem item) //�������� �����, ������ �������
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
                    //ItemRemovedFromTheGrid?.Invoke(item); 16.12.22: � �� ����, ����� ��� ����� ���� �����: ������� ��������� 
                    //�� ����� ��� � ������ ��� ��������. �������� � � ����, � ��� ����� ��������.
                    //���� ���-�� ���������, �� ����� �������� �������� ����
                    //������������ ����� ���������, ���� ����� ��������� � ���� ������� �� ������ �����, �� ������ ����� � ���� ���� ���
                    //UPD 24.12.22: ����� ����, ���� ���������� ������ � ����, ���� ������ ��� ����� ��� ���������?
                    if (!TryPlaceItemInAStack(item, itemInInventory, out int amountInserted))
                    {
                        //��������, ��� �������-�� �����������, �� �� ���� ���� �������
                        ItemPlacedInTheStack?.Invoke(itemInInventory, amountInserted);
                        Debug.LogWarning("�� ���� ���� ���������");
                        return false; 
                    }
                    else
                    {
                        //��������, ��� ���������� ���� ���� �������
                        ItemPlacedInTheStack?.Invoke(itemInInventory, amountInserted);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        //IsOverlapping == false, ������������� � ���� ��������� ��� ���������, ����� �������� �������
        //�������� ���� ���������, ���� �� ������ � �������� �����
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
        //���� ��������� ���������� ������� � ���������� � ���������� ������� � ���������, �� ��������� ������, ��� ����.����?
        if (itemToReceive.CurrentItemsInAStack + itemToPlace.CurrentItemsInAStack > itemToReceive.ItemData.MaxItemsInAStack)
        {
            //...��, �.�. ���������� �� ���������, ������ ���� ������ ���������������� �� ���������,
            //� �����-�� ���������� ��������� ������ ��������� �������. ����� �����, ������� ��������� ������ ��������� �������,
            //������� ����.���� ����� ������� ��� ����� -> ������� ������� �� �������.
            //������� �� ������� ������� �� �������� ���������� � ����� � ��������� �� ����� ������� (return false)

            //� �������, ��� ��� ���������� ��� 4 ������,
            //�� ��������� ���������� ����� ������� � ��� ����� � ��� ����������� � ���, ��� ����� �����, ���� � ������� ����� ����� �������.
            amountInserted = itemToReceive.ItemData.MaxItemsInAStack - itemToReceive.CurrentItemsInAStack;
            itemToPlace.CurrentItemsInAStack -= amountInserted;
            itemToReceive.CurrentItemsInAStack = itemToReceive.ItemData.MaxItemsInAStack;
            return false;
        }
        //...���, �.�. ���������� ���������, ���������� ������� ����� ����������.
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
