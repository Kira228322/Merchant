using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellPanel : MonoBehaviour
{
    //TODO : Тут много дорабатывать, во первых, нужен способ получать и изменять текущие деньги торговца.
    // Во вторых, нужно, чтобы правильно рассчитывалась новая цена на основе всех коэффициентов продажи
    // В третьих, добавлять игроку деньги и убирать их у торговца.
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
        // показать окно с предупреждением
        //"Правда хотите продать, даже что у торговца не хватает денег?"
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
        _currentValueText.text = "Сумма сделки: " +  _currentSellingValue;
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
