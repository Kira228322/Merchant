using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellPanel : MonoBehaviour
{
    //TODO : ��� ����� ������������, �� ������, ����� ������ �������� � �������� ������� ������ ��������.
    // �� ������, �����, ����� ��������� �������������� ����� ���� �� ������ ���� ������������� �������
    // � �������, ��������� ������ ������ � ������� �� � ��������.
    [SerializeField] private ItemGrid _sellItemGrid;
    [SerializeField] private TMP_Text _currentValueText;
    [SerializeField] private TMP_Text _traderTotalMoneyText;

    private int _currentSellingValue;

    private List<InventoryItem> _currentItemsInGrid = new();

    private void OnEnable()
    {
        _sellItemGrid.ItemPlacedInTheGrid += AddItemToList;
        _sellItemGrid.ItemRemovedFromTheGrid += RemoveItemFromList;
    }
    private void OnDisable()
    {
        _sellItemGrid.ItemPlacedInTheGrid -= AddItemToList;
        _sellItemGrid.ItemRemovedFromTheGrid -= RemoveItemFromList;
    }

    private void AddItemToList(InventoryItem item)
    {
        _currentItemsInGrid.Add(item);
        Refresh();
    }
    private void RemoveItemFromList(InventoryItem item)
    {
        _currentItemsInGrid.Remove(item);
        Refresh();
    }
    public void OnDoneButtonPressed()
    {
        //if(Torgovec.Money < _currentSellingValue)
        //{
        // �������� ���� � ���������������
        //"������ ������ �������, ���� ��� � �������� �� ������� �����?"
        //if (da) CommenceSellingAllItems();
        //}
        CommenceSellingAllItems();
    }
    private void Refresh()
    {
        _currentSellingValue = 0;
        foreach (InventoryItem item in _currentItemsInGrid)
        {
            _currentSellingValue += item.CurrentItemsInAStack * item.ItemData.Price;
        }
        _currentValueText.text = "����� ������: " +  _currentSellingValue;
    }
    private void CommenceSellingAllItems()
    {
        for (int i = _currentItemsInGrid.Count; i > 0; i--)
        {
            _sellItemGrid.DestroyItem(_currentItemsInGrid[i - 1]);
        }
        //Player.AddMoney(_currentSellingValue);
        //Torgovec.RemoveMoney(_currentSellingValue);
        Refresh();
    }
}
