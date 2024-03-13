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
            ButtonsLabel.Add("Отдать всю контрабанду");
            if (Player.Instance.Money >= avgPriceOfContraband)
            {
                ButtonsLabel.Add($"Предложить {avgPriceOfContraband} золота в качестве взятки");
                SetInfoButton($"Предложить {avgPriceOfContraband} золота в качестве взятки - шанс успеха " +
                      $"{TravelEventHandler.GetProbability(probabilityCheapOffer, Player.Instance.Statistics.Diplomacy)}% \n" +
                      $"Шанс успеха зависит от вашей дипломатии.");
                if (Player.Instance.Money >= avgPriceOfContraband * 1.5)
                {
                    ButtonsLabel.Add($"Предложить {avgPriceOfContraband * 1.5} золота в качестве взятки");
                    SetInfoButton($"Предложить {avgPriceOfContraband} золота в качестве взятки - шанс успеха " +
                                  $"{TravelEventHandler.GetProbability(probabilityCheapOffer, Player.Instance.Statistics.Diplomacy)}% \n" +
                                  $"Предложить {avgPriceOfContraband * 1.5} золота в качестве взятки - шанс успеха " +
                                  $"{TravelEventHandler.GetProbability(probabilityExpensiveOffer, Player.Instance.Statistics.Diplomacy)}% \n" +
                                  $"Шансы успеха зависят от вашей дипломатии.");
                }
            }
            else SetInfoButton("");

        }
        else
        {
            ButtonsLabel.Add("Пройти проверку");
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
                    //Отдать всю контрабанду
                    foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                        Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                    _eventWindow.ChangeDescription("Вы передали всю контрабанду блюстителю порядка");
                    break;
                case 1:
                    //Предложить немного золота, 50% шанс
                    Player.Instance.Money -= avgPriceOfContraband;
                    if (!TravelEventHandler.EventFire(probabilityCheapOffer, true, Player.Instance.Statistics.Diplomacy))
                    {
                        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                            Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                        _eventWindow.ChangeDescription("Неудача. Стражник взял взятку, но всё равно забрал контрабанду");
                    }
                    else
                        _eventWindow.ChangeDescription("Вам удалось договориться со стражником");
                    break;
                case 2:
                    //Предложить много золота, 80% шанс
                    Player.Instance.Money -= (int)(avgPriceOfContraband * 1.5);
                    if (!TravelEventHandler.EventFire(probabilityExpensiveOffer, true, Player.Instance.Statistics.Diplomacy))
                    {
                        foreach (Item bannedItem in BannedItemsHandler.Instance.BannedItems)
                            Player.Instance.Inventory.RemoveAllItemsOfThisItemData(bannedItem);
                        _eventWindow.ChangeDescription("Неудача. Стражник взял взятку, но всё равно забрал контрабанду");
                    }
                    else
                        _eventWindow.ChangeDescription("Вам удалось договориться со стражником");
                    break;
            }
        }
        //else: contrabandSpotted == false. Пройти проверку и ехать дальше. Ничего не происходит
        else _eventWindow.ChangeDescription("У вас все чисто. Охрана позволяет вам проехать.");
    }

}
