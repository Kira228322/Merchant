using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{

    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _splitButton;
    [SerializeField] private Button _rotateButton;
    [SerializeField] private ItemInfoSplitSlider _splitSliderPanel;

    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private TMP_Text _totalWeightText;
    [SerializeField] private TMP_Text _maxItemsInAStackText;
    [SerializeField] private TMP_Text _fragilityText;
    [SerializeField] private TMP_Text _averagePriceText;
    [SerializeField] private TMP_Text _daysToHalfSpoilText;
    [SerializeField] private TMP_Text _daysToSpoilText;
    [SerializeField] private TMP_Text _boughtDaysAgoText;

    private InventoryItem _currentItemSelected;
    private ItemGrid _lastItemGridSelected;
    private InventoryController _inventoryController;

    public void OnRotateButtonPressed()
    {
        if (_currentItemSelected.ItemData.CellSizeHeight != _currentItemSelected.ItemData.CellSizeWidth)
        {
            if (_inventoryController.PickUpRotateInsert(_currentItemSelected, _lastItemGridSelected))
            {
                Destroy(gameObject);
            }
        }
    }
    public void OnSplitButtonPressed()
    {
        _splitSliderPanel.Show(_currentItemSelected);
    }

    public void OnExitButtonPressed()
    {
        Destroy(gameObject);
    }
    public void Split(int amountToSplit)
    {
        _currentItemSelected.CurrentItemsInAStack -= amountToSplit; //����� �� ������������� �� ������ ���� ����������� �������� ����� ������ ������� � �������� ������
        if(_inventoryController.TryCreateAndInsertItem(_lastItemGridSelected, _currentItemSelected.ItemData, amountToSplit, _currentItemSelected.BoughtDaysAgo, isFillingStackFirst: false))
        {
            Destroy(gameObject);
        }

    }

    public void Initialize(InventoryItem item, ItemGrid itemGrid, InventoryController inventoryController)
    {
        _currentItemSelected = item;
        _lastItemGridSelected = itemGrid;
        _inventoryController = inventoryController;

        //���������� ������ � ������
        _itemIcon.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.Name;
        _itemDescription.text = "��������: " + item.ItemData.Description;

        if (item.CurrentItemsInAStack == 1)
        {
            _splitButton.interactable = false;
        }
        else _splitButton.interactable = true;

        if (item.ItemData.CellSizeWidth == item.ItemData.CellSizeHeight)
        {
            _rotateButton.interactable = false;
        }
        else _rotateButton.interactable = true;

        _quantityText.text = $"����������: {item.CurrentItemsInAStack}";
        _weightText.text = $"���: {item.ItemData.Weight}";
        _totalWeightText.text = $"����� ���: {item.ItemData.Weight * item.CurrentItemsInAStack}";
        _maxItemsInAStackText.text = $"����. ����������: {item.ItemData.MaxItemsInAStack}";
        _fragilityText.text = $"���������: {item.ItemData.Fragility}";
        _averagePriceText.text = $"������� ����: {item.ItemData.Price}";
        
        if (item.ItemData.IsPerishable)
        {
            _daysToHalfSpoilText.alpha = 1;
            _daysToSpoilText.alpha = 1;
            _boughtDaysAgoText.alpha = 1;
            _daysToHalfSpoilText.text = $"���� �� ������ ��������: {item.ItemData.DaysToHalfSpoil}";
            _daysToSpoilText.text = $"���� �� �����: {item.ItemData.DaysToSpoil}";
            _boughtDaysAgoText.text = "�������� ���: " + Math.Round(item.BoughtDaysAgo, 1);
        }
        else
        {
            _daysToHalfSpoilText.alpha = 0;
            _daysToSpoilText.alpha = 0;
            _boughtDaysAgoText.alpha = 0;
        }
    }
}
