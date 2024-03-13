using System.Collections.Generic;

public class EventPost : EventInTravel
{
    private bool contrabandSpotted = false;
    private List<InventoryItem> contrabandItems = new();
    private int avgPriceOfContraband = 0;

    private int probabilityCheapOffer = 50;
    private int probabilityExpensiveOffer = 80;

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
            {
                ButtonsLabel.Add($"���������� {avgPriceOfContraband} ������ � �������� ������");
                SetInfoButton($"���������� {avgPriceOfContraband} ������ � �������� ������ - ���� ������ " +
                      $"{TravelEventHandler.GetProbability(probabilityCheapOffer, Player.Instance.Statistics.Diplomacy)}% \n" +
                      $"���� ������ ������� �� ����� ����������.");
                if (Player.Instance.Money >= avgPriceOfContraband * 1.5)
                {
                    ButtonsLabel.Add($"���������� {avgPriceOfContraband * 1.5} ������ � �������� ������");
                    SetInfoButton($"���������� {avgPriceOfContraband} ������ � �������� ������ - ���� ������ " +
                                  $"{TravelEventHandler.GetProbability(probabilityCheapOffer, Player.Instance.Statistics.Diplomacy)}% \n" +
                                  $"���������� {avgPriceOfContraband * 1.5} ������ � �������� ������ - ���� ������ " +
                                  $"{TravelEventHandler.GetProbability(probabilityExpensiveOffer, Player.Instance.Statistics.Diplomacy)}% \n" +
                                  $"����� ������ ������� �� ����� ����������.");
                }
            }
            else SetInfoButton("");

        }
        else
        {
            ButtonsLabel.Add("������ ��������");
            SetInfoButton("");
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
                    _eventWindow.ChangeDescription("�� �������� ��� ����������� ���������� �������");
                    break;
                case 1:
                    //���������� ������� ������, 50% ����
                    Player.Instance.Money -= avgPriceOfContraband;
                    if (!TravelEventHandler.EventFire(probabilityCheapOffer, true, Player.Instance.Statistics.Diplomacy))
                    {
                        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                            Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                        _eventWindow.ChangeDescription("�������. �������� ���� ������, �� �� ����� ������ �����������");
                    }
                    else
                        _eventWindow.ChangeDescription("��� ������� ������������ �� ����������");
                    break;
                case 2:
                    //���������� ����� ������, 80% ����
                    Player.Instance.Money -= (int)(avgPriceOfContraband * 1.5);
                    if (!TravelEventHandler.EventFire(probabilityExpensiveOffer, true, Player.Instance.Statistics.Diplomacy))
                    {
                        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                            Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                        _eventWindow.ChangeDescription("�������. �������� ���� ������, �� �� ����� ������ �����������");
                    }
                    else
                        _eventWindow.ChangeDescription("��� ������� ������������ �� ����������");
                    break;
            }
        }
        //else: contrabandSpotted == false. ������ �������� � ����� ������. ������ �� ����������
        else _eventWindow.ChangeDescription("� ��� ��� �����. ������ ��������� ��� ��������.");
    }

}
