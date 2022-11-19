using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _currentItemsInAStackText;
    [SerializeField] private SlidersController _spoilSlider;
    public Item ItemData;
    public float BoughtDaysAgo;

    private int _currentItemsInAStack;

    public int Width
    {
        get
        {
            if (!IsRotated)
            {
                return ItemData.CellSizeWidth;
            }
            return ItemData.CellSizeHeight;
        }
    }
    public int Height
    {
        get
        {
            if (!IsRotated)
            {
                return ItemData.CellSizeHeight;
            }
            return ItemData.CellSizeWidth;
        }
    }

    public int XPositionOnTheGrid;
    public int YPositionOnTheGrid;

    public int CurrentItemsInAStack 
    {
        get { return _currentItemsInAStack; }
        set
        {
            _currentItemsInAStack = value;
            _currentItemsInAStackText.text = _currentItemsInAStack.ToString();
        }
    }

    public bool IsRotated = false;

    public void SetItemFromData(Item itemData) //Присвоение значений из SO. 
    {
        ItemData = itemData;

        GetComponent<Image>().sprite = itemData.Icon;

        Vector2 size = new();
        size.x = itemData.CellSizeWidth * ItemGrid.TileSizeWidth;
        size.y = itemData.CellSizeHeight * ItemGrid.TileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    public void Rotate()
    {
        IsRotated = !IsRotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, IsRotated == true ? 90f : 0f); //нихуя умный способ задавать значения, запомню

        _currentItemsInAStackText.rectTransform.anchorMin = new Vector2(IsRotated == true ? 0 : 1, 0);
        _currentItemsInAStackText.rectTransform.anchorMax = new Vector2(IsRotated == true ? 0 : 1, 0);
        _currentItemsInAStackText.rectTransform.localRotation = Quaternion.Euler(0, 0, IsRotated == true ? 270f : 0f);
    }

    public void RefreshSliderValue()
    {
        _spoilSlider.SetValue(ItemData.DaysToSpoil - BoughtDaysAgo, ItemData.DaysToSpoil);
        if (BoughtDaysAgo > ItemData.DaysToHalfSpoil)
        {
            Color yellow = new(178f/255, 179f/255, 73f/255);
            _spoilSlider.SetColour(yellow);
        }
    }
}
