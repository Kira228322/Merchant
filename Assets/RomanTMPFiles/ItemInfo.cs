using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] Image _itemIcon;

    private InventoryItem _currentItemSelected;

    public void OnRotateButtonPressed()
    {
        _currentItemSelected.Rotate();
        gameObject.SetActive(false);
    }
    public void OnSplitButtonPressed()
    {
        //��� ���� ����: �������� ������� ��������� �������� � ������ ���� (������� �� 1 �� �������� ���������� ��������� � �����)
    }

    public void Show(InventoryItem item)
    {
        Debug.Log("������� �������� �������");
        _currentItemSelected = item;

        //���������� ������ � ������
        _itemIcon.sprite = item.ItemData.Icon;

        gameObject.SetActive(true);
    }
}
