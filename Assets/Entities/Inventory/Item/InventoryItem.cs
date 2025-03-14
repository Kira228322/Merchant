using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentItemsInAStackText;
    [SerializeField] private SlidersController _spoilSlider;

    private int _currentItemsInAStack;

    public Item ItemData;
    public float BoughtDaysAgo;
    public int TotalPrice => ItemData.Price * CurrentItemsInAStack;

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
    public void SetItemFromData(Item itemData) //���������� �������� �� SO. 
    {
        Image image = GetComponent<Image>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        ItemData = itemData;

        image.sprite = ItemData.Icon;

        Vector2 size = new()
        {
            x = ItemData.CellSizeWidth * ItemGrid.TileSizeWidth * 0.9f,
            y = ItemData.CellSizeHeight * ItemGrid.TileSizeHeight * 0.9f
        };
        rectTransform.sizeDelta = size;
        if (ItemData.IsPerishable)
        {
            RectTransform spoilSliderRectTransform = _spoilSlider.GetComponent<RectTransform>();
            _spoilSlider.gameObject.SetActive(true);
            spoilSliderRectTransform.localPosition = new(spoilSliderRectTransform.localPosition.x - Width * 5, spoilSliderRectTransform.localPosition.y);
            _spoilSlider.GetComponent<RectTransform>().sizeDelta = new(Width * (ItemGrid.TileSizeWidth * 0.9f - 10), spoilSliderRectTransform.sizeDelta.y);
            //Wtf is sizeDelta?? -> https://stackoverflow.com/questions/44471568/how-to-calculate-sizedelta-in-recttransform
            //spoilSliderRectTransform.localPosition.x - 5 � TileSizeWidth - 10:
            //-5 � -10 ��� ����, ����� ������� ������� �� ������, �� �� ��� �������� �� ��� ������������� ������,
            //����� �������� ����� ����������� ��������� �� ��������� � ���� ���������

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

        sliderRectTransform.anchorMin = new Vector2(IsRotated == true ? 0 : 1, 0);
        sliderRectTransform.anchorMax = new Vector2(IsRotated == true ? 0 : 1, 0);
        sliderRectTransform.pivot = new Vector2(IsRotated == true ? 0 : 1, 0);
        sliderRectTransform.localPosition = new(sliderRectTransform.localPosition.x, sliderRectTransform.localPosition.y + Width * 5);
        sliderRectTransform.localRotation = Quaternion.Euler(0, IsRotated == true ? 180f : 0f, IsRotated == true ? 90f : 0f);
        sliderRectTransform.sizeDelta = new(Width * (ItemGrid.TileSizeWidth * 0.9f - 10), sliderRectTransform.sizeDelta.y);
    }

    public void RefreshSliderValue()
    {
        _spoilSlider.SetValue(ItemData.DaysToSpoil - BoughtDaysAgo, ItemData.DaysToSpoil);
        if (BoughtDaysAgo > ItemData.DaysToHalfSpoil)
        {
            Color yellow = new(178f / 255, 179f / 255, 73f / 255);
            _spoilSlider.SetColour(yellow);
        }
    }
}
