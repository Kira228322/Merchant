using System.Linq;
using UnityEngine;

public class QuestItemHolder : MonoBehaviour
{
    //Должен динамически расширяться и сужаться
    //Можно забрать, но нельзя положить (только если с возврата, что ты достал а положить себе не можешь)
    //Если insert не сработал в мейн, то отправляется сюда (только для диалогов)
    //Не является основным инвентарем, но учитывается в has_items(questItem). Вообще, предназначен только для хранения квестовых предметов
    //Предметы внутри портятся, весят и ломаются.

    //Мы не можем гарантировать, что все предметы будут вставлены.
    //Но мы можем гарантировать, что все квестовые предметы найдут своё место. 
    //Не похуй ли на остальные предметы? Это уже проблемы игрока

    [SerializeField] private ItemGrid _itemGrid;

    public ItemGrid ItemGrid => _itemGrid;

    private int _defaultRowCount;
    private bool _isSortingInProgress;

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
        _itemGrid.Init();
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
        if (!_isSortingInProgress) 
            //.Sort() будет сам несколько раз вызывать этот ивент ItemPlaced,       
            //т.е чтобы избежать рекурсии и StackOverflowException я добавляю такую bool проверку
        {
            _isSortingInProgress = true;
            InventoryController.Instance.Sort(_itemGrid);
            _isSortingInProgress = false;
        }
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
            {
                _itemGrid.RemoveRowsFromGrid(emptyRowsCount - 3);
            }
        }
    }
}
