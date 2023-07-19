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
            ButtonsLabel.Add("Отдать всю контрабанду");
            if (Player.Instance.Money >= avgPriceOfContraband)
                ButtonsLabel.Add($"Предложить {avgPriceOfContraband} золота в качестве взятки");
            if (Player.Instance.Money >= avgPriceOfContraband * 1.5)
                ButtonsLabel.Add($"Предложить {avgPriceOfContraband * 1.5} золота в качестве взятки");
        }
        else
        {
            ButtonsLabel.Add("Пройти проверку");
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
                    foreach (InventoryItem item in contrabandItems)
                        Player.Instance.Inventory.RemoveItemInInventory(item);
                    break;
                case 1:
                    //Предложить немного золота, 50% шанс
                    Player.Instance.Money -= avgPriceOfContraband;
                    if (!TravelEventHandler.EventFire(50f, true, TravelEventHandler.EventMultiplierType.Diplomacy))
                    {
                        foreach (InventoryItem item in contrabandItems)
                            Player.Instance.Inventory.RemoveItemInInventory(item);
                        canvasWarningGenerator.CreateWarning("Неудача", "Стражник взял взятку, но всё равно забрал контрабанду");
                    }
                    break;
                case 2:
                    //Предложить много золота, 80% шанс
                    Player.Instance.Money -= (int)(avgPriceOfContraband * 1.5);
                    if (!TravelEventHandler.EventFire(80f, true, TravelEventHandler.EventMultiplierType.Diplomacy))
                    {
                        foreach (InventoryItem item in contrabandItems)
                            Player.Instance.Inventory.RemoveItemInInventory(item);
                        canvasWarningGenerator.CreateWarning("Неудача", "Стражник взял взятку, но всё равно забрал контрабанду");
                    }
                    break;
            }
        }
        //else: contrabandSpotted == false. Пройти проверку и ехать дальше. Ничего не происходит
        _eventHandler.EventEnd();
    }

}
