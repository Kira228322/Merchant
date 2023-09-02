using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GoodsSellPanel : MonoBehaviour
{
    [FormerlySerializedAs("_cost")] [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private SlidersController _spoilSlider;
    [SerializeField] private Button _sellButton;
    private int _cost;
    private int _currentCount;
    private ItemGrid _playerInventoryItemGrid;
    private InventoryItem _item;
    private NpcTrader _trader;

    public InventoryItem Item { get => _item; }

    public void Init(NpcTrader trader, InventoryItem itemToSell, ItemGrid playerInventoryItemGrid)
    {
        TradeManager.Instance.GoodsSellPanels.Add(this);
        _playerInventoryItemGrid = playerInventoryItemGrid;
        _trader = trader;
        _item = itemToSell;
        _currentCount = itemToSell.CurrentItemsInAStack;
        // TODO Посмотреть, не оказывается ли, что трейдер продает такой же предмет и у него цена продажи меньше, чем игрок ему продает
        _cost = CalculatePrice(_item.ItemData);
        _costText.text = _cost.ToString(); 
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
        NpcTrader.BuyCoefficient buyCoefficient = _trader.BuyCoefficients.FirstOrDefault(x => x.ItemType == _item.ItemData.TypeOfItem);
        if (buyCoefficient == null || buyCoefficient.CountToBuy <= 0)
        {
            _sellButton.interactable = false;
        }

        NpcTraderData traderData = (NpcTraderData)_trader.NpcData;
        if (!traderData.IsBlackMarket && BannedItemsHandler.Instance.IsItemBanned(_item.ItemData))
            _sellButton.interactable = false;
        
        _currentCount = _item.CurrentItemsInAStack;
        _countText.text = _currentCount.ToString();
    }

    public void OnSellButtonClick()
    {
        if (_trader.NpcData.CurrentMoney < _cost)
        {
            TradeManager.Instance.NotEnoughTraderMoney();
            return;
        }
        
        TradeManager.Instance.PlayerGoldIncrease(_cost);
        Player.Instance.Money += _cost;
        _trader.NpcData.CurrentMoney -= _cost;
        TradeManager.Instance.ChangeTraderMoneyText(_trader.NpcData.CurrentMoney);
        MapManager.CurrentLocation.ChangeCountOfCurrentItemOnScene(_item.ItemData.Name, 1);
        TradeManager.Instance.RefreshPriceOnThisGood(_item.ItemData.Name);
        
        _playerInventoryItemGrid.RemoveItemsFromAStack(_item, 1);
        _currentCount--;
        //Уменьшить CountToBuy у коэффициента с этим типом товара
        _trader.BuyCoefficients.FirstOrDefault(x => x.ItemType == _item.ItemData.TypeOfItem).CountToBuy--;
        GoodsBuyPanel panel = TradeManager.Instance.BuyPanelContent.GetComponentsInChildren<GoodsBuyPanel>().FirstOrDefault(i => i.Item.Good == _item.ItemData && i.IsOriginatedFromTrader == false);
        if (panel != null)
        {
            panel.CurrentCount++;
        }
        else
        {
            GameObject tradersGoods = Instantiate(TradeManager.Instance.GoodsBuyPanelPrefab.gameObject, TradeManager.Instance.BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(_trader, _item.ItemData, _item.BoughtDaysAgo, false, 1, _cost);
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

    public void RefreshPrice()
    {
        _cost = CalculatePrice(_item.ItemData);
        _costText.text = _cost.ToString(); 
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
        float itemTypeCoef = MapManager.CurrentLocation.Region.CoefsForItemTypes[item.TypeOfItem];

        float traderTypeCoef = 0;
        for (int i = 0; i < _trader.BuyCoefficients.Count; i++)
        {
            if (_trader.BuyCoefficients[i].ItemType == item.TypeOfItem)
            {
                traderTypeCoef = _trader.BuyCoefficients[i].Coefficient;
                break;
            }
        }

        int bannedItem = 1;
        if (BannedItemsHandler.Instance.IsItemBanned(item))
            bannedItem = 2;
        return Convert.ToInt32(Math.Round(item.Price * locationCoef * regionCoef * itemTypeCoef * traderTypeCoef * bannedItem));
    }
}
