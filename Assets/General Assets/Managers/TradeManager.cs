
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance;
    [SerializeField] private CanvasGroup _playerBlock;
    [SerializeField] private GameObject _closeTradeButton;
    [SerializeField] private GameObject _BuyPanel;
    public CanvasGroup PlayerBlock => _playerBlock;
    public GameObject BuyPanel => _BuyPanel;
    [SerializeField] private GameObject _sellPanel;
    public GameObject SellPanel => _sellPanel;
    [SerializeField] private Transform _buyPanelContent;
    public Transform BuyPanelContent => _buyPanelContent;
    [SerializeField] private GoodsBuyPanel _goodsBuyPanelPrefab;
    public GoodsBuyPanel GoodsBuyPanelPrefab => _goodsBuyPanelPrefab;
    [SerializeField] private GoodsSellPanel _goodsSellPanelPrefab;
    public GoodsSellPanel GoodsSellPanelPrefab => _goodsSellPanelPrefab;
    [SerializeField] private Transform _sellPanelContent;
    public Transform SellPanelContent => _sellPanelContent;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void OpenTradeWindow(NpcTrader trader)
    {
        InventoryController.Instance.enabled = false;
        _playerBlock.alpha = 0;
        _closeTradeButton.SetActive(true);
        OpenBuyPanel(trader);
        OpenSellPanel(trader);
        OpenInventory();
    }
    public void CloseTradeWindow()
    {
        List<GoodsBuyPanel> goodsBuyPanels = BuyPanelContent.GetComponentsInChildren<GoodsBuyPanel>().
            Where(panel => panel.IsOriginatedFromTrader == false).ToList();
        
        foreach (var panel in goodsBuyPanels)
        {
            NpcTrader.TraderGood traderGood =
                panel.Trader.Goods.FirstOrDefault(item => item.Good.Name == panel.Item.Good.Name);
            if ( traderGood != null)
            {
                traderGood.CurrentCount += panel.CurrentCount;
                continue;
            }

            traderGood = panel.Trader.AdditiveGoods.FirstOrDefault(item => item.Good.Name == panel.Item.Good.Name);
            if ( traderGood != null)
            {
                traderGood.CurrentCount += panel.CurrentCount;
                continue;
            }

            int newPrice = panel.Item.CurrentPrice;
            if (newPrice < panel.Item.Good.Price)
                newPrice = panel.Item.Good.Price + Random.Range(1, panel.Item.Good.Price / 12 + 2);
            
            panel.Trader.AdditiveGoods.Add(new NpcTrader.TraderGood
                (panel.Item.Good.Name, panel.CurrentCount, panel.Item.CurrentCount, newPrice));
        }
            
        InventoryController.Instance.enabled = true;
        _playerBlock.alpha = 1;
        _closeTradeButton.SetActive(false);
        BuyPanel.SetActive(false);
        SellPanel.SetActive(false);
        Player.Instance.Inventory.InventoryPanel.SetActive(false);
    }
    private void OpenBuyPanel(NpcTrader trader)
    {
        BuyPanel.SetActive(true);
        for (int i = BuyPanelContent.childCount - 1; i >= 0; i--)
            Destroy(BuyPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < trader.Goods.Count; i++)
        {
            if (trader.Goods[i].CurrentCount <= 0) continue;
            GameObject tradersGoods = Instantiate(GoodsBuyPanelPrefab.gameObject, BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(trader, trader.Goods[i], 0f, true, trader.Goods[i].CurrentCount);
        }

        for (int i = 0; i < trader.AdditiveGoods.Count; i++)
        {
            if (trader.AdditiveGoods[i].CurrentCount <= 0) continue;
            GameObject tradersGoods = Instantiate(GoodsBuyPanelPrefab.gameObject, BuyPanelContent);
            tradersGoods.GetComponent<GoodsBuyPanel>().Init(trader, trader.AdditiveGoods[i], 0f, true, trader.AdditiveGoods[i].CurrentCount);
        }

    }
    private void OpenSellPanel(NpcTrader trader)
    {
        SellPanel.SetActive(true);

        for (int i = SellPanelContent.childCount - 1; i >= 0; i--)
            Destroy(SellPanelContent.GetChild(i).gameObject);

        for (int i = 0; i < Player.Instance.Inventory.ItemList.Count; i++)
        {
            GameObject tradersGoods = Instantiate(GoodsSellPanelPrefab.gameObject, SellPanelContent);
            tradersGoods.GetComponent<GoodsSellPanel>().Init(trader, Player.Instance.Inventory.ItemList[i], Player.Instance.Inventory.ItemGrid);
        }
    }
    private void OpenInventory()
    {
        Player.Instance.Inventory.InventoryPanel.SetActive(true);
    }
}
