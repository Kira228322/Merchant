using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableWell : UsableEnvironment
{
    protected override bool IsFunctionalComplete()
    {
        foreach (var item in Player.Singleton.Inventory.ItemList)
        {
            if (item.ItemData.Name == "Empty bottle")
            {
                // TODO �������� ������� "Empty bottle" �� "Water bottle" ����� �� 
                
                return true;
            }
        }

        return false;
    }
}
