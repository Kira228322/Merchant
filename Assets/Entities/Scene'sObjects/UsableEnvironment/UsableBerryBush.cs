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
                (_berry, 1, 0) != null) 
            return true;
        return false;
    }
}
