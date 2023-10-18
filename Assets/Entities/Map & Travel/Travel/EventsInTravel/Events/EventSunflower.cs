using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSunflower : EventInTravel
{
    public override void SetButtons()
    {
        ButtonsLabel.Add("������� ����������"); 
        ButtonsLabel.Add("����� ������");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                if (InventoryController.Instance.TryCreateAndInsertItem(Player.Instance.Inventory.ItemGrid, ItemDatabase.GetItem("���������"), 2, 0))
                {
                    int skippedMinutes = Random.Range(1, 20);
                    _eventWindow.ChangeDescription($"�� ������� ���� ����������� � ��������� �� ��� 1 ��� {skippedMinutes} �����");
                    GameTime.TimeSkip(0,1,skippedMinutes);
                }
                else
                {
                    _eventWindow.ChangeDescription("� ��� �� ���� ����� � ���������, � �� ������� ������");
                }
                
                break;
            case 1:
                _eventWindow.ChangeDescription("�� ������ �� ������������� � ����� ����");
                break;
        }
        
    }
}
