using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrid : MonoBehaviour
{

    #region ����, �������� � �������

    [Serializable]
    private struct InventoryRow
    {
        public int rowLength;
        public InventoryItem[] itemArray;

        public InventoryRow(int rowLength)
        {
            this.rowLength = rowLength;
            itemArray = new InventoryItem[rowLength];
        }
    }

    public const float TileSizeWidth = 147;
    public const float TileSizeHeight = 147;

    [SerializeField] private int _gridSizeWidth;
    [SerializeField] private int _gridSizeHeight;
   
    private List<InventoryRow> _storedInventoryItems;
    private RectTransform _rectTransform;
    private Vector2 _positionOnTheGrid = new();
    private Vector2Int _tileGridPosition = new();

    public int GridSizeHeight => _gridSizeHeight;

    public event UnityAction<InventoryItem> ItemPlacedInTheGrid;
    public event UnityAction<InventoryItem, int> ItemUpdated; //���������� ���-�� ��������� � �����
    public event UnityAction<InventoryItem> ItemRemovedFromTheGrid;

    #endregion
    #region ������ �������������

    public void Init()
    {
        _rectTransform = GetComponent<RectTransform>();
        _storedInventoryItems = new List<InventoryRow>();

        for (int i = 0; i < _gridSizeHeight; i++)
        {
            InventoryRow inventoryRow = new(_gridSizeWidth);
            _storedInventoryItems.Add(inventoryRow);
        }

        Vector2 size = new(_gridSizeWidth * TileSizeWidth, _gridSizeHeight * TileSizeHeight);
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
                if(_storedInventoryItems[positionY + y].itemArray[positionX + x] != null)
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
                if (_storedInventoryItems[positionY + y].itemArray[positionX + x] != null)
                {
                    if (_storedInventoryItems[positionY + y].itemArray[positionX + x].ItemData.name == item.ItemData.name)
                    {
                        itemInInventory = _storedInventoryItems[positionY + y].itemArray[positionX + x];
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
    private bool IsRotDifferenceBetweenTwoItemsLessThanOne(InventoryItem firstItem, InventoryItem secondItem)
    {
        if (firstItem.ItemData.IsPerishable && secondItem.ItemData.IsPerishable)
        {
            if (Math.Abs(firstItem.BoughtDaysAgo - secondItem.BoughtDaysAgo) < 1)
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
        InventoryItem itemInInventory = _storedInventoryItems[positionY].itemArray[positionX];
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
        return _storedInventoryItems[y].itemArray[x];
    }
    [System.Obsolete] public List<InventoryItem> GetAllItemsInTheGrid()
    {
        //������� �����, �� ������ ������ ����� �� ������������, �� ����� ���� ����� � �������.
        //�� ����, ��� ���������� � ��� ��������� PlayersInventory, ����� �� ����� �������� � ����� ������ (� �����������),
        //�� ����� ����� ����� ���� �����, ���� ����� �������� ��� �������� � ������ ����� ��������� (��������� �� ������)
        //����� ����� ������ ������ ��� � ��� �����, � �����, ���� �� ������ ��� ����������, ����� �����.

        List<InventoryItem> result = new();
        for (int i = 0; i < _storedInventoryItems.Count; i++)
        {
            for (int j = 0; j < _storedInventoryItems[i].itemArray.Length; j++)
            {
                if (_storedInventoryItems[i].itemArray[j] != null && !result.Contains(_storedInventoryItems[i].itemArray[j]))
                {
                    result.Add(_storedInventoryItems[i].itemArray[j]);
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
        //������: ������ ���������� ������� ����� ������������ ���� ����� � false, � ��������� �������� � ��������� � true.

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
        InventoryItem itemToReturn = _storedInventoryItems[positionY].itemArray[positionX];

        if (itemToReturn == null) { return null; }

        CleanGridReference(itemToReturn);
        ItemRemovedFromTheGrid?.Invoke(itemToReturn);
        return itemToReturn;
    }
    public void DestroyItem(int positionX, int positionY)
    {
        InventoryItem itemToDestroy = _storedInventoryItems[positionY].itemArray[positionX];

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
                _storedInventoryItems[item.YPositionOnTheGrid + y].itemArray[item.XPositionOnTheGrid + x] = null;
            }
        }
    }
    public InventoryItem TryPlaceItem(InventoryItem item, int positionX, int positionY)
    {
        if (!IsOverlapping(positionX, positionY, item.Width, item.Height))
        {
            //IsOverlapping == false, ������������� � ���� ��������� ��� ���������, ����� �������� �������
            //�������� ���� ���������, ���� �� ������ � �������� �����
            if (!IsInCorrectPosition(positionX, positionY, item.Width, item.Height))
            {
                return null;
            }
            PlaceItem(item, positionX, positionY);

            ItemPlacedInTheGrid?.Invoke(item);
            return item;
        }
        if (!IsOverlappingWithTheSameItemType(item, positionX, positionY, item.Width, item.Height, out InventoryItem itemInInventory))
            { return null; }
        if (!IsRotDifferenceBetweenTwoItemsLessThanOne(item, itemInInventory))
            { return null; }
        
        //��� ������� ������������ ���������, ����� �����������

        InventoryItem result = TryPlaceItemInAStack(item, itemInInventory, out InventoryItem leftoverItem, out int howManyWerePlaced);
        if (leftoverItem != null)
        {
            //��������, ��� �������-�� �����������, �� �� ���� ���� �������
            ItemUpdated?.Invoke(itemInInventory, howManyWerePlaced);
            return null;
        }
        else
        {
            //��������, ��� ���������� ���� ���� �������
            ItemUpdated?.Invoke(itemInInventory, item.CurrentItemsInAStack);
            return result;
        }
    }

    private void PlaceItem(InventoryItem item, int positionX, int positionY)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(_rectTransform);

        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                _storedInventoryItems[positionY + y].itemArray[positionX + x] = item;
            }
        }

        item.XPositionOnTheGrid = positionX;
        item.YPositionOnTheGrid = positionY;
        Vector2 position = CalculatePositionOnTheGrid(item, positionX, positionY);

        rectTransform.localPosition = position;
    }
    public InventoryItem TryPlaceItemInAStack(InventoryItem itemToPlace, InventoryItem itemToReceive, out InventoryItem leftoverItem, out int howManyWerePlaced)
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
            int amountInserted = itemToReceive.ItemData.MaxItemsInAStack - itemToReceive.CurrentItemsInAStack;
            itemToPlace.CurrentItemsInAStack -= amountInserted;
            itemToReceive.CurrentItemsInAStack = itemToReceive.ItemData.MaxItemsInAStack;
            leftoverItem = itemToPlace;
            howManyWerePlaced = amountInserted;
            return itemToReceive;
        }
        //...���, �.�. ���������� ���������, ���������� ������� ����� ����������.
        itemToReceive.CurrentItemsInAStack += itemToPlace.CurrentItemsInAStack;
        howManyWerePlaced = itemToPlace.CurrentItemsInAStack;
        Destroy(itemToPlace.gameObject);
        leftoverItem = null;
        return itemToReceive;
    }
    public void RemoveItemsFromAStack(InventoryItem itemToTruncate, int amount)
    {
        itemToTruncate.CurrentItemsInAStack -= amount;
        ItemUpdated?.Invoke(itemToTruncate, -amount);
        if (itemToTruncate.CurrentItemsInAStack <= 0)
        {
            DestroyItem(itemToTruncate);
        }
    }

    #endregion
    #region ������ �������� � ��������� (�������� ������ � ������-������ ������)
    public void AddRowsToInventory(int numberOfRowsToAdd) 
    {
        if (numberOfRowsToAdd <= 0)  return; 
        for (int i = 0; i < numberOfRowsToAdd; i++)
        {
            InventoryRow newInventoryRow = new(_gridSizeWidth);
            _storedInventoryItems.Add(newInventoryRow);
        }
        _gridSizeHeight += numberOfRowsToAdd;

        Vector2 size = new(_gridSizeWidth * TileSizeWidth, _gridSizeHeight * TileSizeHeight);
        _rectTransform.sizeDelta = size;

    }
    #endregion


}
