using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableBerryBush : UsableEnvironment
{
    [SerializeField] private Item _berry;
    protected override bool IsFunctionalComplete()
    {
        // TODO ��������� ���� �� ����� ��������� � ��������� � �������� ����� �����-������
        if (InventoryController.Instance.TryCreateAndInsertItem
                (Player.Instance.ItemGrid, _berry, 1, 0, true) != null) 
            return true;
        return false;
    }
}
