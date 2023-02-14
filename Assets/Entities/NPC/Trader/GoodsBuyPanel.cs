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
    private Item _item;
    private NPCTrader _trader;
    private float _boughtDaysAgo;
    public bool IsOriginatedFromTrader;
    public Item Item => _item;
    public void Init(NPCTrader trader, Item goods, float boughtDaysAgo, bool isOriginatedFromTrader, int count)
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
            if (!IsOriginatedFromTrader) 
            {
                _trader.CurrentCountToBuy[(int)_item.TypeOfItem]++; 
            }
            InventoryItem boughtItem = InventoryController.Instance.TryCreateAndInsertItem(Player.Singleton.Inventory.ItemGrid, _item, 1, _boughtDaysAgo, true);
            if (boughtItem == null) //не было места поместить вещь
            {
                return;
            }
            CurrentCount--;
            _trader.SellItem(_item);

            if (CurrentCount <= 0)
            {
                Destroy(gameObject);
            }

            //Пересасывание SellPanel заменено на обновление только одной панели / создание новой
            foreach (GoodsSellPanel sellPanel in TradeManager.Singleton.SellPanelContent.GetComponentsInChildren<GoodsSellPanel>())
            {
                if (sellPanel.Item == boughtItem)
                {
                    sellPanel.Refresh();
                    return;
                }
            }
            //Такой селлПанели не обнаружено:
            GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsSellPanelPrefab.gameObject, TradeManager.Singleton.SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(_trader, boughtItem, Player.Singleton.Inventory.ItemGrid);

        }
    }
}
