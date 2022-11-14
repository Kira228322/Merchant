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
        //Ещё одно меню: выберите сколько предметов отделить в другой стак (слайдер от 1 до текущего количества предметов в стаке)
    }

    public void Show(InventoryItem item)
    {
        _currentItemSelected = item;

        //присвоение текста и иконок
        _itemIcon.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.Name;

        _weightText.text = $"Вес: {item.ItemData.Weight}";
        _averagePriceText.text = $"Средняя цена: {item.ItemData.Price}";
        _maxItemsInAStackText.text = $"Макс. количество предметов в стаке: {item.ItemData.MaxItemsInAStack}";
        if (item.ItemData.IsPerishable)
        {
            _isPerishableText.text = "Портится? Да";
            _daysToHalfSpoilText.alpha = 1;
            _daysToSpoilText.alpha = 1;
            _daysToHalfSpoilText.text = $"Количество дней до потери свежести: {item.ItemData._daysToHalfSpoil}"; 
            //Вопросы к тому кто писал изначальный Item.cs - почему эти поля публичные и начинаются с '_'? Так необходимо для инспектора или это просто нарушение условий именования?
            _daysToSpoilText.text = $"Количество дней до порчи: {item.ItemData._daysToSpoil}";
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
