using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    #region ����
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _splitButton;
    [SerializeField] private Button _rotateButton;
    [SerializeField] private Button _eatButton;
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
    [SerializeField] private TMP_Text _foodValueText;

    private Player _player;
    private InventoryItem _currentItemSelected;
    private ItemGrid _lastItemGridSelected;
    #endregion
    #region ������ �������������
    private void Start()
    {
        _player = Player.Singleton;
    }

    public void Initialize(InventoryItem item, ItemGrid itemGrid)
    {
        _currentItemSelected = item;
        _lastItemGridSelected = itemGrid;

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
        if (item.ItemData.IsEdible)
        {
            _foodValueText.alpha = 1;
            _foodValueText.text = $"+{item.ItemData.FoodValue} �������";
            _eatButton.gameObject.SetActive(true);
        }
        else
        {
            _foodValueText.alpha = 0;
            _eatButton.interactable = false;
            _eatButton.gameObject.SetActive(false);
        }
    }
    #endregion
    #region ������ ������ � ��������
    public void OnEatButtonPressed()
    {
        _player.Needs.RestoreHunger(_currentItemSelected.ItemData.FoodValue);
        _currentItemSelected.CurrentItemsInAStack--;
        _quantityText.text = "����������: " +_currentItemSelected.CurrentItemsInAStack.ToString();
        if (_currentItemSelected.CurrentItemsInAStack == 0)
        {
            _lastItemGridSelected.DestroyItem(_currentItemSelected);
            Destroy(gameObject);
        }
    }
    public void OnRotateButtonPressed()
    {
        if (_currentItemSelected.ItemData.CellSizeHeight != _currentItemSelected.ItemData.CellSizeWidth)
        {
            InventoryItem item = InventoryController.Instance.TryPickUpRotateInsert(_currentItemSelected, _lastItemGridSelected);
            if (item != null)
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

    public void OnDestroyButtonPressed()
    {
        _lastItemGridSelected.DestroyItem(_currentItemSelected);
        Destroy(gameObject);
    }
    #endregion
    public void Split(int amountToSplit) //�� ����������� ��� � InventoryController?
    {
        _lastItemGridSelected.RemoveItemsFromAStack(_currentItemSelected, amountToSplit); //����� �� ������������� �� ������ ���� ����������� �������� ����� ������ ������� � �������� ������
        InventoryItem item = InventoryController.Instance.TryCreateAndInsertItem(_lastItemGridSelected, _currentItemSelected.ItemData, amountToSplit, _currentItemSelected.BoughtDaysAgo, isFillingStackFirst: false);
        if (item != null)
        {
            Destroy(gameObject);
        }

    }

}
