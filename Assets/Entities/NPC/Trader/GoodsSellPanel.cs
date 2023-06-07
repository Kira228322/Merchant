using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsSellPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private SlidersController _spoilSlider;
    [SerializeField] private Button _sellButton;
    private int _currentCount;
    private ItemGrid _playerInventoryItemGrid;
    private InventoryItem _item;
    private NpcTrader _trader;

    public InventoryItem Item { get => _item; }

    public void Init(NpcTrader trader, InventoryItem itemToSell, ItemGrid playerInventoryItemGrid)
    {
        _playerInventoryItemGrid = playerInventoryItemGrid;
        _trader = trader;
        _item = itemToSell;
        _currentCount = itemToSell.CurrentItemsInAStack;
        _cost.text = _item.ItemData.Price.ToString(); //Тут бы высчитывать цену для этого конкретного торговца
        _countText.text = _currentCount.ToString();
        _icon.sprite = _item.ItemData.Icon;
        _itemName.text = _item.ItemData.Name;


        if (itemToSell.ItemData.IsPerishable)
        {
            _spoilSlider.gameObject.SetActive(true);
            _spoilSlider.SetValue(_item.ItemData.DaysToSpoil - _item.BoughtDaysAgo, _item.ItemData.DaysToSpoil);
            if (_item.BoughtDaysAgo > _item.ItemData.DaysToHalfSpoil)
            {
                Color yellow = new(178f / 255, 179f / 255, 73f / 255);
                _spoilSlider.SetColour(yellow);
            }
        }
        Refresh();
    }
    public void Refresh()
    {
        NpcTrader.BuyCoefficient buyCoefficient = _trader.BuyCoefficients.FirstOrDefault(x => x.itemType == _item.ItemData.TypeOfItem);
        if (buyCoefficient == null || buyCoefficient.CountToBuy <= 0)
        {
            _sellButton.interactable = false;
        }
        _currentCount = _item.CurrentItemsInAStack;
        _countText.text = _currentCount.ToString();
    }

    public void OnSellButtonClick()
    {
        //TODO начислять денежек за айтем, списывать деньги у торговца
        // TODO доделать
        // if (_trader.money < CalculatePrice(_item.ItemData))
        // {
        //     Player.Instance.Money += CalculatePrice(_item.ItemData);
        //     _trader.money -= CalculatePrice(_item.ItemData);
        // }
        
        _playerInventoryItemGrid.RemoveItemsFromAStack(_item, 1);
        _currentCount--;
        //Уменьшить CountToBuy у коэффициента с этим типом товара
        _trader.BuyCoefficients.FirstOrDefault(x => x.itemType == _item.ItemData.TypeOfItem).CountToBuy--;
        GoodsBuyPanel panel = TradeManager.Instance.BuyPanelContent.GetComponentsInChildren<GoodsBuyPanel>().FirstOrDefault(i => i.Item.Good == _item.ItemData && i.IsOriginatedFromTrader == false);
        if (panel != null)
        {
            panel.CurrentCount++;
        }
        else
        {
            GameObject tradersGoods = Instantiate(TradeManager.Instance.GoodsBuyPanelPrefab.gameObject, TradeManager.Instance.BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(_trader, _item.ItemData, _item.BoughtDaysAgo, false, 1);
        }
        if (_currentCount <= 0)
        {
            InventoryController.Instance.DestroyItem(_playerInventoryItemGrid, _item);
            Destroy(gameObject);
        }

        //рефрешить нужно все панельки в СеллПанели
        foreach (GoodsSellPanel sellPanel in transform.parent.GetComponentsInChildren<GoodsSellPanel>())
        {
            sellPanel.Refresh();
        }
    }

    private int CalculatePrice(Item item)
    {
        int currentQuantityLoca = MapManager.CurrentLocation.CountOfEachItem[item.Name];
        int currentQuantityReg = MapManager.CurrentLocation.Region.CountOfEachItem[item.Name];
        
        float locationCoef = MapManager.CurrentLocation.Region.CalculatePriceCoef(currentQuantityLoca, item.Price, 
            MapManager.CurrentLocation.ItemEconomyParams[item.Name][0],
            MapManager.CurrentLocation.ItemEconomyParams[item.Name][1],
            MapManager.CurrentLocation.ItemEconomyParams[item.Name][2]);
        
        float regionCoef = MapManager.CurrentLocation.Region.CalculatePriceCoef(currentQuantityReg, item.Price, 
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Name][0],
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Name][1],
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Name][2]);
        float itemTypeCoef = MapManager.CurrentLocation.Region._coefsForItemTypes[item.TypeOfItem];

        float traderTypeCoef = 0;
        for (int i = 0; i < _trader.BuyCoefficients.Count; i++)
        {
            if (_trader.BuyCoefficients[i].itemType == item.TypeOfItem)
            {
                traderTypeCoef = _trader.BuyCoefficients[i].Coefficient;
                break;
            }
        }

        return Convert.ToInt32(Math.Round(item.Price * locationCoef * regionCoef * itemTypeCoef * traderTypeCoef));
    }
}
