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
        _currentItemSelected.CurrentItemsInAStack -= amountToSplit; //вроде бы предусмотрено на случай всех невозможных ситуаций через другие скрипты и свойства кнопок
        _inventoryController.TryCreateAndInsertItem(_lastItemGridSelected, _currentItemSelected.ItemData, amountToSplit, isFillingStackFirst: false);
    }

    public void Show(InventoryItem item, ItemGrid itemGrid)
    {
        _currentItemSelected = item;
        _lastItemGridSelected = itemGrid;

        //присвоение текста и иконок
        _itemIcon.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.Name;
        _itemDescription.text = "Описание: " + item.ItemData.Description;

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

        _weightText.text = $"Вес: {item.ItemData.Weight}";
        _averagePriceText.text = $"Средняя цена: {item.ItemData.Price}";
        _maxItemsInAStackText.text = $"Макс. количество предметов в стаке: {item.ItemData.MaxItemsInAStack}";
        if (item.ItemData.IsPerishable)
        {
            _isPerishableText.text = "Портится? Да";
            _daysToHalfSpoilText.alpha = 1;
            _daysToSpoilText.alpha = 1;
            _daysToHalfSpoilText.text = $"Дней до потери свежести: {item.ItemData._daysToHalfSpoil}"; 
            //Вопросы к тому кто писал изначальный Item.cs - почему эти поля публичные и начинаются с '_'? Так необходимо для инспектора или это просто нарушение условий именования?
            _daysToSpoilText.text = $"Дней до порчи: {item.ItemData._daysToSpoil}";
        }
        else
        {
            _isPerishableText.text = "Портится? Нет";
            _daysToHalfSpoilText.alpha = 0;
            _daysToSpoilText.alpha = 0;
        }

        gameObject.SetActive(true);
    }
}
