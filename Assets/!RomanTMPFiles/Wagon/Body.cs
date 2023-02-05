using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : WagonPart
{
    [SerializeField] [Header("������� ����� ������ ���� ����� � ���������")] private int _inventoryRows;
    public int InventoryRows => _inventoryRows;
    public override void Replace(WagonPart wagonPart)
    {
        base.Replace(wagonPart);
        Body body = wagonPart as Body;
        _inventoryRows = body.InventoryRows;
    }
}
