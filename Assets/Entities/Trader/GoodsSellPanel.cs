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
    private Trader _trader;

    public void Init(Trader trader, InventoryItem itemToSell, ItemGrid playerInventoryItemGrid)
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
                Color yellow = new Color(178f / 255, 179f / 255, 73f / 255);
                _spoilSlider.SetColour(yellow);
            }
        }
        Refresh();
    }
    public void Refresh()
    {
        if (_trader.CurrentCountToBuy[(int)_item.ItemData.TypeOfItem] == 0)
        {
            _sellButton.interactable = false;
        }
        _countText.text = _currentCount.ToString();
    }

    public void OnSellButtonClick()
    {
        //TODO начислять денежек за айтем, списывать деньги у торговца

        _item.CurrentItemsInAStack--;
        _currentCount--;

        _trader.CurrentCountToBuy[(int)_item.ItemData.TypeOfItem]--;
        GoodsBuyPanel panel = TradeManager.Singleton.BuyPanelContent.GetComponentsInChildren<GoodsBuyPanel>().FirstOrDefault(i => i.Item == _item.ItemData && i.IsOriginatedFromTrader == false);
        if (panel != null)
        {
            panel.CurrentCount++;
        }
        else
        {
            GameObject tradersGoods = Instantiate(TradeManager.Singleton.GoodsBuyPanelPrefab.gameObject, TradeManager.Singleton.BuyPanelContent);
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
}
