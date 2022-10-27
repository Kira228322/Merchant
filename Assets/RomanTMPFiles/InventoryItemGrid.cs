using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemGrid : MonoBehaviour
{
    const float tileSizeWidth = 32;
    const float tileSizeHeight = 32;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int gridPositionInTiles = new Vector2Int();
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public Vector2Int GetTileGridPosition(Vector2 mouseposition)
    {
        positionOnTheGrid.x = mouseposition.x - rectTransform.position.x;
        positionOnTheGrid.y = mouseposition.y - rectTransform.position.y;

        gridPositionInTiles.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        gridPositionInTiles.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return gridPositionInTiles;
    }
}
