using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventPost : EventInTravel
{
    private bool contrabandSpotted = false;
    private List<InventoryItem> contrabandItems = new();
    private int avgPriceOfContraband = 0;
    private CanvasWarningGenerator canvasWarningGenerator;
    public override void SetButtons()
    {

        canvasWarningGenerator = FindObjectOfType<CanvasWarningGenerator>();

        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
        {
            contrabandItems.Concat(Player.Instance.Inventory.GetInventoryItemsOfThisData(bannedItem));
        }
        if (contrabandItems.Count > 0) 
            contrabandSpotted = true;

        if (contrabandSpotted)
        {
            foreach (InventoryItem item in contrabandItems)
            {
                avgPriceOfContraband += item.ItemData.Price * item.CurrentItemsInAStack;
            }
            ButtonsLabel.Add("������ ��� �����������");
            if (Player.Instance.Money >= avgPriceOfContraband)
                ButtonsLabel.Add($"���������� {avgPriceOfContraband} ������ � �������� ������");
            if (Player.Instance.Money >= avgPriceOfContraband * 1.5)
                ButtonsLabel.Add($"���������� {avgPriceOfContraband * 1.5} ������ � �������� ������");
        }
        else
        {
            ButtonsLabel.Add("������ ��������");
        }
    }

    public override void OnButtonClick(int n)
    {
        if (contrabandSpotted)
        {
            switch (n)
            {
                case 0:
                    //������ ��� �����������
                    foreach (InventoryItem item in contrabandItems)
                        Player.Instance.Inventory.RemoveItemInInventory(item);
                    break;
                case 1:
                    //���������� ������� ������, 50% ����
                    Player.Instance.Money -= avgPriceOfContraband;
                    if (!TravelEventHandler.EventFire(50f, true, TravelEventHandler.EventMultiplierType.Diplomacy))
                    {
                        foreach (InventoryItem item in contrabandItems)
                            Player.Instance.Inventory.RemoveItemInInventory(item);
                        canvasWarningGenerator.CreateWarning("�������", "�������� ���� ������, �� �� ����� ������ �����������");
                    }
                    break;
                case 2:
                    //���������� ����� ������, 80% ����
                    Player.Instance.Money -= (int)(avgPriceOfContraband * 1.5);
                    if (!TravelEventHandler.EventFire(80f, true, TravelEventHandler.EventMultiplierType.Diplomacy))
                    {
                        foreach (InventoryItem item in contrabandItems)
                            Player.Instance.Inventory.RemoveItemInInventory(item);
                        canvasWarningGenerator.CreateWarning("�������", "�������� ���� ������, �� �� ����� ������ �����������");
                    }
                    break;
            }
        }
        //else: contrabandSpotted == false. ������ �������� � ����� ������. ������ �� ����������
        _eventHandler.EventEnd();
    }

}
