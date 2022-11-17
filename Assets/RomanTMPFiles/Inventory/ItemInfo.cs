using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private Button _splitButton;
    [SerializeField] private Button _rotateButton;
    [SerializeField] private ItemInfoSplitSlider _splitSliderPanel;

    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private TMP_Text _averagePriceText;
    [SerializeField] private TMP_Text _maxItemsInAStackText;
    [SerializeField] private TMP_Text _isPerishableText;
    [SerializeField] private TMP_Text _daysToHalfSpoilText;
    [SerializeField] private TMP_Text _daysToSpoilText;

    private InventoryItem _currentItemSelected;
    private ItemGrid _lastItemGridSelected;

    public void OnRotateButtonPressed()
    {
        if (_currentItemSelected.ItemData.CellSizeHeight != _currentItemSelected.ItemData.CellSizeWidth)
        {
            _inventoryController.PickUpRotateInsert(_currentItemSelected, _lastItemGridSelected);
        }
        gameObject.SetActive(false);
    }
    public void OnSplitButtonPressed()
    {
        _splitSliderPanel.Show(_currentItemSelected);
    }
    public void Split(int amountToSplit)
    {
        _currentItemSelected.CurrentItemsInAStack -= amountToSplit; //����� �� ������������� �� ������ ���� ����������� �������� ����� ������ ������� � �������� ������
        _inventoryController.TryCreateAndInsertItem(_lastItemGridSelected, _currentItemSelected.ItemData, amountToSplit, isFillingStackFirst: false);
    }

    public void Show(InventoryItem item, ItemGrid itemGrid)
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

        _weightText.text = $"���: {item.ItemData.Weight}";
        _averagePriceText.text = $"������� ����: {item.ItemData.Price}";
        _maxItemsInAStackText.text = $"����. ���������� ��������� � �����: {item.ItemData.MaxItemsInAStack}";
        if (item.ItemData.IsPerishable)
        {
            _isPerishableText.text = "��������? ��";
            _daysToHalfSpoilText.alpha = 1;
            _daysToSpoilText.alpha = 1;
            _daysToHalfSpoilText.text = $"���� �� ������ ��������: {item.ItemData._daysToHalfSpoil}"; 
            //������� � ���� ��� ����� ����������� Item.cs - ������ ��� ���� ��������� � ���������� � '_'? ��� ���������� ��� ���������� ��� ��� ������ ��������� ������� ����������?
            _daysToSpoilText.text = $"���� �� �����: {item.ItemData._daysToSpoil}";
        }
        else
        {
            _isPerishableText.text = "��������? ���";
            _daysToHalfSpoilText.alpha = 0;
            _daysToSpoilText.alpha = 0;
        }

        gameObject.SetActive(true);
    }
}
