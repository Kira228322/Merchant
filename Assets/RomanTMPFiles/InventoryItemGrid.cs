using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemGrid : MonoBehaviour
{
    public const float TileSizeWidth = 32;
    public const float TileSizeHeight = 32;

    [SerializeField] private int _gridSizeWidth;
    [SerializeField] private int _gridSizeHeight;

    private InventoryItem[,] _storedInventoryItems;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid = new();
    Vector2Int gridPositionInTiles = new();
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Initialize(_gridSizeWidth, _gridSizeHeight);
    }
    public Vector2Int GetTileGridPosition(Vector2 mouseposition)
    {
        positionOnTheGrid.x = mouseposition.x - rectTransform.position.x;
        positionOnTheGrid.y = mouseposition.y - rectTransform.position.y;

        gridPositionInTiles.x = (int)(positionOnTheGrid.x / TileSizeWidth);
        gridPositionInTiles.y = (int)(positionOnTheGrid.y / TileSizeHeight);

        return gridPositionInTiles;
    }

    private void Initialize(int width, int height)
    {
        _storedInventoryItems = new InventoryItem[width, height];
        rectTransform.sizeDelta = new Vector2(width * TileSizeWidth, height * TileSizeHeight);
    }

    public void PlaceItemInGrid(InventoryItem item, Vector2Int gridPosition)
    {
        _storedInventoryItems[gridPosition.x, gridPosition.y] = item;
    }
    public void RemoveItemFromGrid(Vector2Int gridPosition)
    {
        _storedInventoryItems[gridPosition.x, gridPosition.y] = null;
    }
}
