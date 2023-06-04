using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GoodsBuyPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemName;
    public int CurrentCount
    {
        get { return _currentCount; }
        set 
        {
            _currentCount = value;
            _countText.text = _currentCount.ToString();
        }
    }
    private int _currentCount;
    private NpcTrader.TraderGood _item;
    private NpcTrader _trader;
    private float _boughtDaysAgo;
    public bool IsOriginatedFromTrader;
    public NpcTrader.TraderGood Item => _item;
    public void Init(NpcTrader trader, Item goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count)
    {
        _trader = trader;
        _item = new NpcTrader.TraderGood(goods.Name, 1,1, goods.Price);
        _boughtDaysAgo = boughtDaysAgo;
        IsOriginatedFromTrader = isOriginatedFromTrader;
        CurrentCount = count;
        _cost.text = goods.Price.ToString();
        _icon.sprite = _item.Good.Icon;
        _itemName.text = goods.Name;
    }

    public void Init(NpcTrader trader, NpcTrader.TraderGood goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count)
    {
        _trader = trader;
        _item = goods;
        _boughtDaysAgo = boughtDaysAgo;
        IsOriginatedFromTrader = isOriginatedFromTrader;
        CurrentCount = count;
        _cost.text = goods.CurrentPrice.ToString();
        _icon.sprite = _item.Good.Icon;
        _itemName.text = goods.Good.Name;
    }

    public void OnBuyButtonClick()
    {
        if (CurrentCount > 0)
        {
            if (!IsOriginatedFromTrader) 
            {
                //увеличить BuyCoefficient.CountToBuy с таким же типом как _item.TypeOfItem
                _trader.BuyCoefficients.FirstOrDefault(x => x.itemType == _item.Good.TypeOfItem).CountToBuy++;
            }
            InventoryItem boughtItem = InventoryController.Instance.TryCreateAndInsertItem(Player.Instance.Inventory.ItemGrid, _item.Good, 1, _boughtDaysAgo, true);
            if (boughtItem == null) //не было места поместить вещь
            {
                return;
            }

            if (Player.Instance.Money < CalculatePrice(_item))
            {
                return;
            }

            Player.Instance.Money -= CalculatePrice(_item);
            // TODO Дать деньги трейдеру
            CurrentCount--;
            _trader.SellItem(_item.Good);

            if (CurrentCount <= 0)
            {
                Destroy(gameObject);
            }

            //Пересасывание SellPanel заменено на обновление только одной панели / создание новой
            foreach (GoodsSellPanel sellPanel in TradeManager.Instance.SellPanelContent.GetComponentsInChildren<GoodsSellPanel>())
            {
                if (sellPanel.Item == boughtItem)
                {
                    sellPanel.Refresh();
                    return;
                }
            }
            //Такой селлПанели не обнаружено:
            GameObject tradersGoods = Instantiate(TradeManager.Instance.GoodsSellPanelPrefab.gameObject, TradeManager.Instance.SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(_trader, boughtItem, Player.Instance.Inventory.ItemGrid);

        }
    }

    private int CalculatePrice(NpcTrader.TraderGood item)
    {
        int currentQuantity = 0; // TODO считать сколько на локации продукта текущего
        float locationCoef = MapManager.CurrentLocation.Region.CalculatePriceCoef(currentQuantity, item.Good.Price, 
                MapManager.CurrentLocation.ItemEconomyParams[item.Good.Name][0],
                MapManager.CurrentLocation.ItemEconomyParams[item.Good.Name][1],
                MapManager.CurrentLocation.ItemEconomyParams[item.Good.Name][2]);
        
        float regionCoef = MapManager.CurrentLocation.Region.CalculatePriceCoef(currentQuantity, item.Good.Price, 
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Good.Name][0],
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Good.Name][1],
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Good.Name][2]);

        float itemTypeCoef = MapManager.CurrentLocation.Region._coefsForItemTypes[item.Good.TypeOfItem];
        return Convert.ToInt32(Math.Round(item.CurrentPrice * locationCoef * regionCoef * itemTypeCoef));
    }
}
