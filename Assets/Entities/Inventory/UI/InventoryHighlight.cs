using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform _highlighter;

    public void Show(bool b)
    {
        _highlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new()
        {
            x = targetItem.Width * ItemGrid.TileSizeWidth,
            y = targetItem.Height * ItemGrid.TileSizeHeight
        };
        _highlighter.sizeDelta = size;
    }

    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) { return; }
        _highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        Vector2 position = targetGrid.CalculatePositionOnTheGrid(targetItem, targetItem.XPositionOnTheGrid, targetItem.YPositionOnTheGrid);

        _highlighter.localPosition = position;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int positionX, int positionY)
    {
        Vector2 position = targetGrid.CalculatePositionOnTheGrid(targetItem, positionX, positionY);

        _highlighter.localPosition = position;
    }
}
