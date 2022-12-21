using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Item _item;
    private InventoryController _inventoryController;
    private Trader _trader;
    private float _boughtDaysAgo;
    public bool IsOriginatedFromTrader;
    public Item Item => _item;

    private void Start()
    {
        _inventoryController = InventoryController.Instance;
    }
    public void Init(Trader trader, Item goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count)
    {
        _trader = trader;
        _item = goods;
        _boughtDaysAgo = boughtDaysAgo;
        IsOriginatedFromTrader = isOriginatedFromTrader;
        CurrentCount = count;
        _cost.text = goods.Price.ToString();
        _icon.sprite = _item.Icon;
        _itemName.text = goods.Name;
    }

    public void OnBuyButtonClick()
    {
        if (CurrentCount > 0)
        {
            if (!_inventoryController.TryCreateAndInsertItem(Player.Singleton.Inventory.GetComponent<ItemGrid>(), _item, 1, _boughtDaysAgo, true))
            {
                if (!_inventoryController.TryCreateAndInsertItemRotated(Player.Singleton.Inventory.GetComponent<ItemGrid>(), _item, 1, _boughtDaysAgo, true))
                {
                    return;
                }
            }
            CurrentCount--;
            _trader.SellItem(_item);

            
            GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsSellPanelPrefab.gameObject, TradeManager.Singleton.SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(_trader, Player.Singleton.Inventory.ItemList[i], Player.Singleton.Inventory.GetComponent<ItemGrid>());


        }
    }
}
