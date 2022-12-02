using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentItemsInAStackText;
    [SerializeField] private TMP_Text _sellValueText;
    [SerializeField] private SlidersController _spoilSlider;

    private int _currentItemsInAStack;

    
    public Item ItemData;
    public float BoughtDaysAgo;


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

    public void ShowSellValue(bool b)
    {
        _sellValueText.gameObject.SetActive(b);
    }
    public void SetItemFromData(Item itemData) //Присвоение значений из SO. 
    {
        Image image = GetComponent<Image>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        ItemData = itemData;

        image.sprite = ItemData.Icon;
        _sellValueText.text = itemData.Price.ToString(); //Вот и нихуя, цена должна показываться та, которая при продаже будет у торговца. Как это сделать - пока хз. (27.11.22)

        Vector2 size = new();
        size.x = ItemData.CellSizeWidth * ItemGrid.TileSizeWidth;
        size.y = ItemData.CellSizeHeight * ItemGrid.TileSizeHeight;
        rectTransform.sizeDelta = size;
        if (ItemData.IsPerishable)
        {
            RectTransform spoilSliderRectTransform = _spoilSlider.GetComponent<RectTransform>();
            _spoilSlider.gameObject.SetActive(true);
            spoilSliderRectTransform.localPosition = new(spoilSliderRectTransform.localPosition.x - 5, spoilSliderRectTransform.localPosition.y);
            _spoilSlider.GetComponent<RectTransform>().sizeDelta = new(Width * ItemGrid.TileSizeWidth - 10, spoilSliderRectTransform.sizeDelta.y);
            //Wtf is sizeDelta?? -> https://stackoverflow.com/questions/44471568/how-to-calculate-sizedelta-in-recttransform
            RefreshSliderValue();
        }
    }

    public void Rotate()
    {
        IsRotated = !IsRotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransform sliderRectTransform = _spoilSlider.GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, IsRotated == true ? 90f : 0f);

        _currentItemsInAStackText.rectTransform.anchorMin = new Vector2(IsRotated == true ? 0 : 1, 0);
        _currentItemsInAStackText.rectTransform.anchorMax = new Vector2(IsRotated == true ? 0 : 1, 0);
        _currentItemsInAStackText.rectTransform.localRotation = Quaternion.Euler(0, 0, IsRotated == true ? 270f : 0f);

        _sellValueText.rectTransform.anchorMin = new Vector2(IsRotated == true ? 1 : 0, 1);
        _sellValueText.rectTransform.anchorMax = new Vector2(IsRotated == true ? 1 : 0, 1);
        _sellValueText.rectTransform.localRotation = Quaternion.Euler(0, 0, IsRotated == true ? 270f : 0f);

        sliderRectTransform.anchorMin = new Vector2(IsRotated == true ? 0 : 1, 0);
        sliderRectTransform.anchorMax = new Vector2(IsRotated == true ? 0 : 1, 0);
        sliderRectTransform.localRotation = Quaternion.Euler(0, 0, IsRotated == true ? 270f : 0f);
        sliderRectTransform.sizeDelta = new Vector2(Width * ItemGrid.TileSizeWidth, sliderRectTransform.sizeDelta.y);
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
