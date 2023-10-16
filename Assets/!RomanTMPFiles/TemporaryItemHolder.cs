using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(ItemGrid))]
public class TemporaryItemHolder : MonoBehaviour
{
    //Должен динамически расширяться и сужаться
    //Можно забрать, но нельзя положить (только если с возврата, что ты достал а положить себе не можешь)
    //Если insert не сработал в мейн, то отправляется сюда
    //Не является основным инвентарем, но учитывается в has_items(questItem)
    //Предметы внутри не портятся (?)

    private ItemGrid _itemGrid;

    private int _defaultRowCount;

    private void OnEnable()
    {
        _itemGrid.ItemPlacedInTheGrid += OnItemPlaced;
        _itemGrid.ItemRemovedFromTheGrid += OnItemRemoved;
    }
    private void OnDisable()
    {
        _itemGrid.ItemPlacedInTheGrid -= OnItemPlaced;
        _itemGrid.ItemRemovedFromTheGrid -= OnItemRemoved;
    }
    private void Awake()
    {
        _defaultRowCount = _itemGrid.GridSizeHeight;
    }

    private void OnItemPlaced(InventoryItem item) 
    {
        //Добавлять пустые роу так, чтобы их стало 3. Сортировать после постановки айтема сюда (пустые роу должны быть в конце чтобы норм убирались)
        int emptyRowsCount = _itemGrid.GetEmptyRowsCount();
        if (emptyRowsCount < 3)
        {
            _itemGrid.AddRowsToInventory(3 - emptyRowsCount);
        }
        InventoryController.Instance.Sort(_itemGrid);
    }
    private void OnItemRemoved(InventoryItem item)
    {
        //Убирать лишние роу так, чтобы стало 3 пустых роу. Но не менее чем _defaultRowsCount (~4)
        //Не сортировать, иначе предмету сложно будет вернуться на место если что-то пойдет не так
        int emptyRowsCount = _itemGrid.GetEmptyRowsCount();
        if (emptyRowsCount > 3)
        {
            if (_itemGrid.GridSizeHeight - (emptyRowsCount - 3) < _defaultRowCount)
            {
                _itemGrid.RemoveRowsFromGrid(emptyRowsCount - _defaultRowCount);
            }
            else
                _itemGrid.RemoveRowsFromGrid(emptyRowsCount - 3);
        }
    }
}
