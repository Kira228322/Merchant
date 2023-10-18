using System.Linq;
using UnityEngine;

public class QuestItemHolder : MonoBehaviour
{
    //������ ����������� ����������� � ��������
    //����� �������, �� ������ �������� (������ ���� � ��������, ��� �� ������ � �������� ���� �� ������)
    //���� insert �� �������� � ����, �� ������������ ���� (������ ��� ��������)
    //�� �������� �������� ����������, �� ����������� � has_items(questItem). ������, ������������ ������ ��� �������� ��������� ���������
    //�������� ������ ��������, ����� � ��������.

    //�� �� ����� �������������, ��� ��� �������� ����� ���������.
    //�� �� ����� �������������, ��� ��� ��������� �������� ������ ��� �����. 
    //�� ����� �� �� ��������� ��������? ��� ��� �������� ������

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
        //��������� ������ ��� ���, ����� �� ����� 3. ����������� ����� ���������� ������ ���� (������ ��� ������ ���� � ����� ����� ���� ���������)

        int emptyRowsCount = _itemGrid.GetEmptyRowsCount();
        if (emptyRowsCount < 3)
        {
            _itemGrid.AddRowsToInventory(3 - emptyRowsCount);
        }
        if (!_isSortingInProgress) 
            //.Sort() ����� ��� ��������� ��� �������� ���� ����� ItemPlaced,       
            //�.� ����� �������� �������� � StackOverflowException � �������� ����� bool ��������
        {
            _isSortingInProgress = true;
            InventoryController.Instance.Sort(_itemGrid);
            _isSortingInProgress = false;
        }
    }
    private void OnItemRemoved(InventoryItem item)
    {
        //������� ������ ��� ���, ����� ����� 3 ������ ���. �� �� ����� ��� _defaultRowsCount (~4)
        //�� �����������, ����� �������� ������ ����� ��������� �� ����� ���� ���-�� ������ �� ���
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
