using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Roman.Item ItemData;

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

    public bool IsRotated = false;

    public void SetItemFromData(Roman.Item itemData) //Присвоение значений из SO. 
        //Когда буду делать менюшку, нужно будет добавить остальную информацию (стоп нахуя, здесь же нужна только картинка?)
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
    }
}
