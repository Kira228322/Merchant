using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTraveler : EventInTravel
{
    private int _countOfEdibleItems;
    private InventoryItem _first;
    private InventoryItem _second;
    public override void SetButtons()
    {
        List<InventoryItem> inventory = Player.Instance.Inventory.ItemList;
        List<InventoryItem> edibleItems = new List<InventoryItem>();
        
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].ItemData is UsableItem)
                if (inventory[i].BoughtDaysAgo < inventory[i].ItemData.DaysToHalfSpoil)
                {
                    UsableItem item = (UsableItem)inventory[i].ItemData;
                    if (item.UsableItemType == UsableItem.UsableType.Edible)
                        edibleItems.Add(inventory[i]);
                }
            
        }

        _countOfEdibleItems = edibleItems.Count;
        
        switch (_countOfEdibleItems)
        {
            case 0:
                break;
            case 1:
                ButtonsLabel.Add($"Дать {edibleItems[0].ItemData.Name}");
                _first = edibleItems[0];
                break;
            case 2:
                ButtonsLabel.Add($"Дать {edibleItems[0].ItemData.Name}");
                _first = edibleItems[0];
                ButtonsLabel.Add($"Дать {edibleItems[1].ItemData.Name}");
                _second = edibleItems[1];
                break;
            default:
                InventoryItem min = edibleItems[0];
                InventoryItem max = edibleItems[1];
                for (int i = 0; i < edibleItems.Count; i++)
                {
                    if (edibleItems[i].ItemData.Price > max.ItemData.Price)
                        max = edibleItems[i];
                    else if (edibleItems[i].ItemData.Price < min.ItemData.Price)
                        min = edibleItems[i];
                }
                ButtonsLabel.Add($"Дать {max.ItemData.Name}");
                _first = max;
                ButtonsLabel.Add($"Дать {min.ItemData.Name}");
                _second = min;
                break;
        }
        ButtonsLabel.Add("Не помогать");
        SetInfoButton("Вы можете помочь путнику, если дадите ему немного еды.\nВы получите опыт, зависящий от стоимости отданного вами продукта.");
    }

    public override void OnButtonClick(int n)
    {
        bool exist = false;
        switch (_countOfEdibleItems)
        {
            case 0:
                _eventWindow.ChangeDescription("Вы понадеялись, что путнику поможет кто-то другой...");
                break;
            case 1:
                switch (n)
                {
                    case 0:
                        foreach (var item in Player.Instance.Inventory.ItemList)
                        {
                            if (_first == item)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist)
                        {
                            Player.Instance.Inventory.RemoveItemsOfThisItemData(_first.ItemData, 1);
                            
                            Player.Instance.Experience.AddExperience(_first.ItemData.Price/4 + 1);
                            _eventWindow.ChangeDescription($"Путник был рад принять в дар {_first.ItemData.Name}. Он поблагодарил вас и отправился дальше. Вы получили {_first.ItemData.Price/4 + 1} опыта.");
                        }
                        else
                        {
                            _eventWindow.ChangeDescription($"Вы пообещали дать путнику {_first.ItemData.Name}, но вместо этого съедаете это лакомство сами. Он, еле сдерживая слезы, пошёл прочь.");
                        }
                        break;
                    case 1:
                        _eventWindow.ChangeDescription("Вы понадеялись, что путнику поможет кто-то другой...");
                        break;
                }
                break;
            default:
                switch (n)
                {
                    case 0:
                        foreach (var item in Player.Instance.Inventory.ItemList)
                        {
                            if (_first == item)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist)
                        {
                            Player.Instance.Inventory.RemoveItemsOfThisItemData(_first.ItemData, 1);
                            
                            Player.Instance.Experience.AddExperience(_first.ItemData.Price/4 + 1);
                            _eventWindow.ChangeDescription($"Путник был рад принять в дар {_first.ItemData.Name}. Он поблагодарил вас и отправился дальше. Вы получили {_first.ItemData.Price/4 + 1} опыта.");
                        }
                        else
                        {
                            _eventWindow.ChangeDescription($"Вы пообещали дать путнику {_first.ItemData.Name}, но вместо этого съедаете это лакомство сами. Он, еле сдерживая слезы, пошел прочь.");
                        }
                        break;
                    case 1:
                        foreach (var item in Player.Instance.Inventory.ItemList)
                        {
                            if (_second == item)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist)
                        {
                            Player.Instance.Inventory.RemoveItemsOfThisItemData(_second.ItemData, 1);
                            
                            Player.Instance.Experience.AddExperience(_second.ItemData.Price/4 + 1);
                            _eventWindow.ChangeDescription($"Путник был рад принять в дар {_second.ItemData.Name}. Он поблагодарил вас и отправился дальше. Вы получили {_second.ItemData.Price/4 + 1} опыта.");
                        }
                        else
                        {
                            _eventWindow.ChangeDescription($"Вы пообещали дать путнику {_second.ItemData.Name}, но вместо этого съедаете это лакомство сами. Он, еле сдерживая слезы, пошел прочь.");
                        }
                        break;
                    case 2:
                        _eventWindow.ChangeDescription("Вы понадеялись, что путнику поможет кто-то другой...");
                        break;
                }
                break;
        }
        
        
    }
}
