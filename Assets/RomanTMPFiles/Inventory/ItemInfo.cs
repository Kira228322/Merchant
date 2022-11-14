using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemName;

    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private TMP_Text _averagePriceText;
    [SerializeField] private TMP_Text _maxItemsInAStackText;
    [SerializeField] private TMP_Text _isPerishableText;
    [SerializeField] private TMP_Text _daysToHalfSpoilText;
    [SerializeField] private TMP_Text _daysToSpoilText;

    private InventoryItem _currentItemSelected;

    public void OnRotateButtonPressed()
    {
        if (_currentItemSelected.ItemData.CellSizeHeight != _currentItemSelected.ItemData.CellSizeWidth)
        {
            _currentItemSelected.Rotate();
        }
        gameObject.SetActive(false);
    }
    public void OnSplitButtonPressed()
    {
        //��� ���� ����: �������� ������� ��������� �������� � ������ ���� (������� �� 1 �� �������� ���������� ��������� � �����)
    }

    public void Show(InventoryItem item)
    {
        _currentItemSelected = item;

        //���������� ������ � ������
        _itemIcon.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.Name;

        _weightText.text = $"���: {item.ItemData.Weight}";
        _averagePriceText.text = $"������� ����: {item.ItemData.Price}";
        _maxItemsInAStackText.text = $"����. ���������� ��������� � �����: {item.ItemData.MaxItemsInAStack}";
        if (item.ItemData.IsPerishable)
        {
            _isPerishableText.text = "��������? ��";
            _daysToHalfSpoilText.alpha = 1;
            _daysToSpoilText.alpha = 1;
            _daysToHalfSpoilText.text = $"���������� ���� �� ������ ��������: {item.ItemData._daysToHalfSpoil}"; 
            //������� � ���� ��� ����� ����������� Item.cs - ������ ��� ���� ��������� � ���������� � '_'? ��� ���������� ��� ���������� ��� ��� ������ ��������� ������� ����������?
            _daysToSpoilText.text = $"���������� ���� �� �����: {item.ItemData._daysToSpoil}";
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
