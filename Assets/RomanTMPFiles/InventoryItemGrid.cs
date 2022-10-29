using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemGrid : MonoBehaviour
{
    private const float _tileSizeWidth = 32;
    private const float _tileSizeHeight = 32;

    [SerializeField] private int _gridSizeWidth;
    [SerializeField] private int _gridSizeHeight;

    private Item[,] _storedInventoryItems;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int gridPositionInTiles = new Vector2Int();
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Initialize(_gridSizeWidth, _gridSizeHeight);
    }
    public Vector2Int GetTileGridPosition(Vector2 mouseposition)
    {
        positionOnTheGrid.x = mouseposition.x - rectTransform.position.x;
        positionOnTheGrid.y = mouseposition.y - rectTransform.position.y;

        gridPositionInTiles.x = (int)(positionOnTheGrid.x / _tileSizeWidth);
        gridPositionInTiles.y = (int)(positionOnTheGrid.y / _tileSizeHeight);

        return gridPositionInTiles;
    }

    private void Initialize(int width, int height)
    {
        _storedInventoryItems = new Item[width, height];
        rectTransform.sizeDelta = new Vector2(width * _tileSizeWidth, height * _tileSizeHeight);
    }

    private void PlaceItem(Roman.Item item, int gridPositionX, int gridPositionY)
    {

    }
}
