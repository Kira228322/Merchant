using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(ItemGrid))]
public class TemporaryItemHolder : MonoBehaviour
{
    //������ ����������� ����������� � ��������
    //����� �������, �� ������ �������� (������ ���� � ��������, ��� �� ������ � �������� ���� �� ������)
    //���� insert �� �������� � ����, �� ������������ ����
    //�� �������� �������� ����������, �� ����������� � has_items(questItem)
    //�������� ������ �� �������� (?)

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
        //��������� ������ ��� ���, ����� �� ����� 3. ����������� ����� ���������� ������ ���� (������ ��� ������ ���� � ����� ����� ���� ���������)
        int emptyRowsCount = _itemGrid.GetEmptyRowsCount();
        if (emptyRowsCount < 3)
        {
            _itemGrid.AddRowsToInventory(3 - emptyRowsCount);
        }
        InventoryController.Instance.Sort(_itemGrid);
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
                _itemGrid.RemoveRowsFromGrid(emptyRowsCount - 3);
        }
    }
}
