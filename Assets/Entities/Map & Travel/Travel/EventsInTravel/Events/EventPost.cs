using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventPost : EventInTravel
{
    private bool contrabandSpotted = false;
    private List<InventoryItem> contrabandItems = new();
    private int avgPriceOfContraband = 0;
    public override void SetButtons()
    {
        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
        {
            contrabandItems.AddRange(Player.Instance.Inventory.GetInventoryItemsOfThisData(bannedItem));
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
                ButtonsLabel.Add($"���������� {avgPriceOfContraband} ������ � �������� ������ (50% ���� ������)");
            if (Player.Instance.Money >= avgPriceOfContraband * 1.5)
                ButtonsLabel.Add($"���������� {avgPriceOfContraband * 1.5} ������ � �������� ������ (80% ���� ������)");
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
                    foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                        Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                    if (contrabandSpotted)
                        _eventWindow.ChangeDescription("�� �������� ��� ����������� ���������� �������");
                    else
                        _eventWindow.ChangeDescription("� ��� ��� �����. ������ ��������� ��� ��������.");
                    break;
                case 1:
                    //���������� ������� ������, 50% ����
                    Player.Instance.Money -= avgPriceOfContraband;
                    if (!TravelEventHandler.EventFire(50f, true, Player.Instance.Statistics.Diplomacy))
                    {
                        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                            Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                        _eventWindow.ChangeDescription("�������. �������� ���� ������, �� �� ����� ������ �����������");
                    }
                    _eventWindow.ChangeDescription("��� ������� ������������ �� ����������");
                    break;
                case 2:
                    //���������� ����� ������, 80% ����
                    Player.Instance.Money -= (int)(avgPriceOfContraband * 1.5);
                    if (!TravelEventHandler.EventFire(80f, true, Player.Instance.Statistics.Diplomacy))
                    {
                        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                            Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                        _eventWindow.ChangeDescription("�������. �������� ���� ������, �� �� ����� ������ �����������");
                    }
                    _eventWindow.ChangeDescription("��� ������� ������������ �� ����������");
                    break;
            }
        }
        //else: contrabandSpotted == false. ������ �������� � ����� ������. ������ �� ����������
    }

}
