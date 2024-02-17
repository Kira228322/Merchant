using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Serialization;

public class GoodsBuyPanel : MonoBehaviour
{
    [SerializeField] private GameObject _itemInfoPanel;
    [FormerlySerializedAs("_cost")] [SerializeField] private TMP_Text _costText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemName;
    private int _cost;
    private int _currentCount;
    private float _boughtDaysAgo;
    private NpcTrader.TraderGood _item;
    private NpcTrader _trader;
    public bool IsOriginatedFromTrader;
    public NpcTrader Trader => _trader;
    public NpcTrader.TraderGood Item => _item;
    public int Cost => _cost;
    public int CurrentCount
    {
        get { return _currentCount; }
        set 
        {
            _currentCount = value;
            _countText.text = _currentCount.ToString();
        }
    }

    public void OnIconClick()
    {
        ItemInfo itemInfoPanel = Instantiate(_itemInfoPanel, MapManager.Canvas.transform).GetComponent<ItemInfo>();
        itemInfoPanel.Initialize(_item.Good);
    }
    
    public void Init(NpcTrader trader, Item goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count)
    {
        TradeManager.Instance.GoodsBuyPanels.Add(this);
        _trader = trader;
        _item = new NpcTrader.TraderGood(goods.Name, 1,1, goods.Price); 
        _boughtDaysAgo = boughtDaysAgo;
        IsOriginatedFromTrader = isOriginatedFromTrader;
        CurrentCount = count;
        _cost = CalculatePrice(_item);
        ChangeNameColor();
        _costText.text = _cost.ToString();
        _icon.sprite = _item.Good.Icon;
        _itemName.text = goods.Name;
    }
    public void Init(NpcTrader trader, Item goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count, int price)
    {
        //ѕерегрузка, в которой цена предмета не считаетс€ на основе экономики, а остаетс€ такой как при продаже.
        //Ќужно дл€ того, чтобы создавать новую GoodsBuyPanel из GoodsSellPanel. „тобы игрок мог "выкупить" предмет
        //по той цене, по которой продал до закрыти€ окна торговли.
        _trader = trader;
        _item = new NpcTrader.TraderGood(goods.Name, 1, 1, goods.Price); 
        _boughtDaysAgo = boughtDaysAgo;
        IsOriginatedFromTrader = isOriginatedFromTrader;
        CurrentCount = count;
        _cost = price;
        _costText.text = _cost.ToString();
        _icon.sprite = _item.Good.Icon;
        _itemName.text = goods.Name;
    }
    public void Init(NpcTrader trader, NpcTrader.TraderGood goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count)
    {
        TradeManager.Instance.GoodsBuyPanels.Add(this);
        _trader = trader;
        _item = goods;
        _boughtDaysAgo = boughtDaysAgo;
        IsOriginatedFromTrader = isOriginatedFromTrader;
        CurrentCount = count;
        _cost = CalculatePrice(_item);
        ChangeNameColor();
        _costText.text = _cost.ToString();
        _icon.sprite = _item.Good.Icon;
        _itemName.text = goods.Good.Name;
    }

    public void OnBuyButtonClick()
    {
        if (CurrentCount > 0)
        {
            
            if (Player.Instance.Money < _cost)
            {
                TradeManager.Instance.NotEnoughPlayerMoney();
                return;
            }
            
            InventoryItem boughtItem = InventoryController.Instance.TryCreateAndInsertItem(_item.Good, 1, _boughtDaysAgo);
            if (boughtItem == null) //не было места поместить вещь
            {
                TradeManager.Instance.NotEnoughSpace();
                return;
            }
            
            if (!IsOriginatedFromTrader) 
            {
                //увеличить BuyCoefficient.CountToBuy с таким же типом как _item.TypeOfItem
                _trader.BuyCoefficients.FirstOrDefault(x => x.ItemType == _item.Good.TypeOfItem).CountToBuy++;
            }

            TradeManager.Instance.PlayerGoldDecrease(_cost);
            Player.Instance.Money -= _cost;
            _trader.NpcData.CurrentMoney += _cost;
            TradeManager.Instance.ChangeTraderMoneyText(_trader.NpcData.CurrentMoney);
            if (_item.Good.TypeOfItem != global::Item.ItemType.Null)
                MapManager.CurrentLocation.ChangeCountOfCurrentItemOnScene(_item.Good.Name, -1);
            TradeManager.Instance.RefreshPriceOfThisGood(_item.Good.Name);
            CurrentCount--;
            
            
            if (IsOriginatedFromTrader)
                _trader.SellItem(_item.Good);

            if (CurrentCount <= 0)
            {
                Destroy(gameObject);
            }
            
            
            //ѕересасывание SellPanel заменено на обновление только одной панели / создание новой
            foreach (GoodsSellPanel sellPanel in TradeManager.Instance.SellPanelContent.GetComponentsInChildren<GoodsSellPanel>())
            {
                if (sellPanel.Item == boughtItem)
                {
                    sellPanel.Refresh();
                    return;
                }
            }
            //“акой селлѕанели не обнаружено:
            GameObject tradersGoods = Instantiate(TradeManager.Instance.GoodsSellPanelPrefab.gameObject, TradeManager.Instance.SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(_trader, boughtItem, Player.Instance.Inventory.BaseItemGrid);

            
        }
    }
    
    public void RefreshPrice()
    {
        _cost = CalculatePrice(_item);
        ChangeNameColor();
        _costText.text = _cost.ToString(); 
    }

    private void ChangeNameColor()
    {
        if (_cost < 0.855f * _item.Good.Price) // 1/1.17
        {
            _itemName.color = new Color(62 / 255f, 188 / 255f, 0 );
        }
        else if (_cost > 1.17f * _item.Good.Price)
        {
            _itemName.color = new Color(174/255f,32/255f,14/255f);
        }
        else
        {
            _itemName.color = new Color(35/255f,50/255f,55/255f);
        }
    }

    private int CalculatePrice(NpcTrader.TraderGood item)
    {
        if (item.Good.IsQuestItem)
            return item.Good.Price;
        
        int currentQuantityLoca = MapManager.CurrentLocation.CountOfEachItem[item.Good.Name];
        int currentQuantityReg = MapManager.CurrentLocation.Region.CountOfEachItem[item.Good.Name];
        
        float locationCoef = MapManager.CurrentLocation.Region.CalculatePriceCoefLocation(currentQuantityLoca, item.Good.Price, 
                MapManager.CurrentLocation.ItemEconomyParams[item.Good.Name][0],
                MapManager.CurrentLocation.ItemEconomyParams[item.Good.Name][1],
                MapManager.CurrentLocation.ItemEconomyParams[item.Good.Name][2]);
        
        float regionCoef = MapManager.CurrentLocation.Region.CalculatePriceCoefRegion(currentQuantityReg, item.Good.Price, 
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Good.Name][0],
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Good.Name][1],
            MapManager.CurrentLocation.Region.ItemEconomyParams[item.Good.Name][2]);

        int bannedItem = 1;
        if (BannedItemsHandler.Instance.IsItemBanned(item.Good))
            bannedItem = 2;
        
        float itemTypeCoef = MapManager.CurrentLocation.Region.CoefsForItemTypes[item.Good.TypeOfItem];
        return Convert.ToInt32(Math.Round(item.CurrentPrice * locationCoef * regionCoef * itemTypeCoef * bannedItem));
    }
}
