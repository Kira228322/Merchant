using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] Image _itemIcon;

    private InventoryItem _currentItemSelected;

    public void OnRotateButtonPressed()
    {
        _currentItemSelected.Rotate();
        gameObject.SetActive(false);
    }
    public void OnSplitButtonPressed()
    {
        //≈щЄ одно меню: выберите сколько предметов отделить в другой стак (слайдер от 1 до текущего количества предметов в стаке)
    }

    public void Show(InventoryItem item)
    {
        Debug.Log("ѕытаюсь показать предмет");
        _currentItemSelected = item;

        //присвоение текста и иконок
        _itemIcon.sprite = item.ItemData.Icon;

        gameObject.SetActive(true);
    }
}
