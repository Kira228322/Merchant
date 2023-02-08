using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body", menuName = "WagonParts/Body")]
public class Body : WagonPart
{
    [SerializeField] [Header("������� ����� ������ ���� ����� � ���������")] private int _inventoryRows;
    public int InventoryRows => _inventoryRows;
}
